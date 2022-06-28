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
    public class MikroblogController : BaseController
    {
        public MikroblogController(UserManager<ApplicationUser> userManager, IFileManager fileManager, IRepository repository)
            : base(userManager, fileManager, repository) { }

        public IActionResult Index()
        {
            var viewmodel = new MikroblogViewModel
            {
                Posts = _repository.GetMikroblogPosts().Select(post => new MikroblogPostViewModel
                {
                    CommentId = post.Id,
                    Content = post.Content,
                    Created = post.Created,
                    Creator = _repository.GetUser(post.PosterId),
                    Pluses = _repository.GetPostReactions(post.Id),
                    ImageName = post.Image,
                    SubComments = _repository.GetSubComments(post).Select(comment => new MikroblogCommentViewModel
                    {
                        MainCommentId = comment.MainCommentId,
                        CommentId = comment.Id,
                        Content = comment.Content,
                        ImageName = comment.Image,
                        Creator = _repository.GetUser(comment.PosterId),
                        Pluses = _repository.GetPostReactions(comment.Id),
                        Created = comment.Created
                    }).ToList(),
                }).ToList()
            };

            return View(viewmodel);
        }
        [HttpGet]
        public IActionResult Post() => View(new MikroblogPostViewModel());

        [HttpPost]
        public async Task<IActionResult> Post(MikroblogPostViewModel viewModel)
        {
            var newPost = new MikroblogPost
            {
                Content = viewModel.Content,
                PosterId = await GetCurrentUserId()
            };

            if (viewModel.Image != null)
                newPost.Image = await _fileManager.SavePostPicture(viewModel.Image);

            _repository.AddPost(newPost);

            await _repository.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Comment() => View(new MikroblogCommentViewModel());

        [HttpPost]
        public async Task<IActionResult> Comment(MikroblogCommentViewModel viewModel)
        {
            var comment = new SubComment
            {
                Content = viewModel.Content,
                PosterId = await GetCurrentUserId(),
                MainCommentId = viewModel.MainCommentId,
            };

            if (viewModel.Image != null)
                comment.Image = await _fileManager.SavePostPicture(viewModel.Image);

            _repository.AddSubComment(comment);

            await _repository.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Plus(int postId)
        {
            var newReaction = new MikroblogPostReaction
            {
                PostId = postId,
                UserId = await GetCurrentUserId()
            };

            if (_repository.GetPostReactions(postId).
                Any(reaction => reaction.PostId == postId && reaction.UserId == newReaction.UserId))
            {
                _repository.RemovePostReaction(_repository.GetPostReactions(postId).
                    First(reaction => reaction.PostId == postId && reaction.UserId == newReaction.UserId).Id);
                await _repository.SaveChanges();
                return RedirectToAction("Index");
            }

            _repository.AddPostReaction(newReaction);

            await _repository.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
