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
                })
                .ToList(),
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Comment() => View(new FindingCommentViewModel());

        [HttpPost]
        public async Task<IActionResult> Comment(FindingCommentViewModel viewModel)
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
    }
}
