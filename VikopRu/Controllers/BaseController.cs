using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VikopRu.Data.FileManager;
using VikopRu.Data.Repository;
using VikopRu.Models;
using VikopRu.ViewModels;

namespace VikopRu.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<ApplicationUser> _userManager;
        protected IFileManager _fileManager;
        protected IRepository _repository;

        public BaseController(UserManager<ApplicationUser> userManager, IFileManager fileManager,
            IRepository repository)
        {
            _userManager = userManager;
            _fileManager = fileManager;
            _repository = repository;
        }

        // overriten to add user profile picture to viewbag by default
        public override ViewResult View(object view)
        {
            try
            {
                var userProfileTask = _userManager.GetUserAsync(HttpContext.User);
                userProfileTask.Wait();

                ViewBag.ProfilePicture = userProfileTask.Result.ProfilePicture;
            }
            catch (NullReferenceException)
            {
                return base.View(view);
            }

            return base.View(view);
        }

        public override ViewResult View()
        {
            try
            {
                var userProfileTask = _userManager.GetUserAsync(HttpContext.User);
                userProfileTask.Wait();

                ViewBag.ProfilePicture = userProfileTask.Result.ProfilePicture;
            }
            catch (NullReferenceException)
            {
                return base.View();
            }

            return base.View();
        }

        /// <summary>
        /// Creating a basic comment
        /// </summary>
        protected async Task<Comment> AddComment(CommentViewModel viewModel)
        {
            var comment = new Comment
            {
                PosterId = await GetCurrentUserId(),
                Content = viewModel.Content,
            };

            if (viewModel.Image != null)
                comment.Image = await _fileManager.SavePostPicture(viewModel.Image);

            _repository.AddComment(comment);

            await _repository.SaveChanges();

            return comment;
        }

        protected string GetImageMime(string image) => image.Substring(image.LastIndexOf('.') + 1);

        /// <summary>
        /// Gets id of the current logged user
        /// </summary>
        protected async Task<string> GetCurrentUserId() => (await _userManager.GetUserAsync(HttpContext.User)).Id;
    }
}
