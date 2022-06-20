using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopRu.Data.FileManager;
using VikopRu.Data.Repository;
using VikopRu.Models;
using VikopRu.ViewModels;

namespace VikopRu.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IFileManager fileManager, UserManager<ApplicationUser> userManager, IRepository repository)
            : base(userManager, fileManager, repository) { }

        public IActionResult Index() 
        {
            var viewModel = new MainPageViewModel
            {
                Findings = _repository.GetFindings().Select(finding => new FindingViewModel
                {
                    Creator = _repository.GetUser(finding.CreatorId),
                    Description = finding.Description,
                    Title = finding.Title,
                    ImageName = finding.Image,
                    Link = finding.Link,
                    Diggs = _repository.GetDiggs(finding.Id),
                    Buries = _repository.GetBuries(finding.Id),
                    FindingId = finding.Id,
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpGet("/ProfilePicture/{image}")]
        public IActionResult ProfilePicture(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.ProfilePictureStream(image), $"image/{mime}");
        }

        [HttpGet("/Findings/{image}")]
        public IActionResult FindingPicture(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.FindingPictureStream(image), $"image/{mime}");
        }
        [HttpGet("/PostImages/{image}")]
        public IActionResult PostImage(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.PostPictureStream(image), $"image/{mime}");
        }

        [HttpGet]
        public IActionResult AddFinding() => View(new FindingViewModel());

        [HttpPost]
        public async Task<IActionResult> AddFinding(FindingViewModel viewModel)
        {
            if(!ModelState.IsValid)
                return RedirectToAction("AddFinding");

            var newFinding = new Finding
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                Link = viewModel.Link.Replace("https://", ""),
                CreatorId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(HttpContext.User))
            };

            if (viewModel.Image != null)
                newFinding.Image = await _fileManager.SaveFindingPicture(viewModel.Image);

            _repository.AddFinding(newFinding);

            if(await _repository.SaveChanges())
                return RedirectToAction("Index");

            return RedirectToAction("AddFinding");
        }

        [HttpGet]
        public IActionResult Finding(int id)
        {
            var finding = _repository.GetFindings().First(finding => finding.Id == id);

            var viewModel = new FindingViewModel
            {
                Creator = _repository.GetUser(finding.CreatorId),
                Description = finding.Description,
                Title = finding.Title,
                ImageName = finding.Image,
                Link = finding.Link,
                Diggs = _repository.GetDiggs(finding.Id),
                Buries = _repository.GetBuries(finding.Id),
                FindingId = finding.Id,
                Comments = _repository.GetFindingComments(finding).
                Select(comment => _repository.GetComment(comment.CommentId)).
                Select(comment => new CommentViewModel
                {
                    CommentId = comment.Id,
                    Creator = _repository.GetUser(comment.PosterId),
                    Content = comment.Content,
                    ImageName = comment.Image,
                    FindingId = finding.Id,
                    SubComments = _repository.GetSubComments(comment).Select(subcomment => new SubCommentViewModel
                    {
                        MainCommentId = comment.Id,
                        ImageName = subcomment.Image,
                        Content = subcomment.Content,
                        Creator = _repository.GetUser(subcomment.PosterId),
                        FindingId = finding.Id
                    }).ToList()
                })
                .ToList(),
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Comment() => View(new CommentViewModel());

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel viewModel)
        {
            var comment = await AddComment(viewModel);

            _repository.AddFindingComment(new FindingComment
            {
                CommentId = comment.Id,
                FindingId = viewModel.FindingId,
            });

            await _repository.SaveChanges();

            return RedirectToAction("Finding", "Home", new { id = viewModel.FindingId} );
        }

        [HttpGet]
        public IActionResult SubComment() => View(new SubCommentViewModel());

        [HttpPost]
        public async Task<IActionResult> SubComment(SubCommentViewModel viewModel)
        {
            var subcomment = new SubComment
            {
                MainCommentId = viewModel.MainCommentId,
                Content = viewModel.Content,
                PosterId = (await _userManager.GetUserAsync(HttpContext.User)).Id,
            };

            if(viewModel.Image != null)
                subcomment.Image = await _fileManager.SavePostPicture(viewModel.Image);

            _repository.AddSubComment(subcomment);

            await _repository.SaveChanges();

            return RedirectToAction("Finding", "Home", new { id = viewModel.FindingId });
        }

        [HttpPost]
        public async Task<IActionResult> FindingAction(int id, bool dig)
        {
            var newAction = new FindingAction
            {
                FindingId = id,
                UserId = (await _userManager.GetUserAsync(HttpContext.User)).Id,
                IsDig = dig
            };

            if (_repository.GetAllActions().Any(action => action.FindingId == id && action.UserId == newAction.UserId))
                _repository.RemoveAction(_repository.GetAllActions().
                    First(action => action.FindingId == newAction.FindingId && 
                                    action.UserId == newAction.UserId).Id);

            _repository.AddAction(newAction);

            await _repository.SaveChanges();

            return RedirectToAction("Finding", "Home", new { id = id });
        }
    }
}
