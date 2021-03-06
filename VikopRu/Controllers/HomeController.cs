using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
                    Created = finding.Created
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpGet("/ProfilePicture/{image}")]
        public IActionResult ProfilePicture(string image) 
            => new FileStreamResult(_fileManager.ProfilePictureStream(image), $"image/{GetImageMime(image)}");
        
        [HttpGet("/Findings/{image}")]
        public IActionResult FindingPicture(string image) 
            => new FileStreamResult(_fileManager.FindingPictureStream(image), $"image/{GetImageMime(image)}");
        
        [HttpGet("/PostImages/{image}")]
        public IActionResult PostImage(string image)
            => new FileStreamResult(_fileManager.PostPictureStream(image), $"image/{GetImageMime(image)}");
        
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
                CreatorId = await GetCurrentUserId()
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
                        CommentId = subcomment.Id,
                        ImageName = subcomment.Image,
                        Content = subcomment.Content,
                        Creator = _repository.GetUser(subcomment.PosterId),
                        FindingId = finding.Id,
                        PositiveReactions = _repository.GetPosiviteCommentReactions(subcomment.Id),
                        NegativeReactions = _repository.GetNegativeCommentReactions(subcomment.Id),
                        Created = subcomment.Created
                    }).ToList(),
                    PositiveReactions = _repository.GetPosiviteCommentReactions(comment.Id),
                    NegativeReactions = _repository.GetNegativeCommentReactions(comment.Id),
                    Created = comment.Created
                })
                .ToList(),
                Created = finding.Created
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
                PosterId = await GetCurrentUserId(),
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
                UserId = await GetCurrentUserId(),
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

        [HttpPost]
        public async Task<IActionResult> CommentReaction(int commentId, int findingId, bool positive)
        {
            var newReaction = new CommentReaction
            {
                CommentId = commentId,
                UserId = await GetCurrentUserId(),
                Positive = positive
            };

            if (_repository.GetAllCommentReactions().
                Any(reaction => reaction.CommentId == newReaction.CommentId
                && reaction.UserId == newReaction.UserId))
            {
                _repository.RemoveCommentReaction(_repository.GetAllCommentReactions().
                    First(reaction => reaction.CommentId == newReaction.CommentId 
                    && reaction.UserId == newReaction.UserId).Id);
            }

            _repository.AddCommentReaction(newReaction);

            await _repository.SaveChanges();

            return RedirectToAction("Finding", "Home", new { id = findingId });
        }
    }
}
