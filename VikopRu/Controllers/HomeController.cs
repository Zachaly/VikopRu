using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopRu.Data.FileManager;
using VikopRu.Models;

namespace VikopRu.Controllers
{
    public class HomeController : Controller
    {
        private IFileManager _fileManager;
        private UserManager<ApplicationUser> _userManager;

        public HomeController(IFileManager fileManager, UserManager<ApplicationUser> userManager)
        {
            _fileManager = fileManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() 
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.ProfilePicture = (await _userManager.FindByNameAsync(User.Identity.Name)).ProfilePicture;
            }

            return View();
        }

        [HttpGet("/ProfilePicture/{image}")]
        public IActionResult ProfilePicture(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.ProfilePictureStream(image), $"image/{mime}");
        }
    }
}
