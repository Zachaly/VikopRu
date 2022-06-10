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
    public class HomeController : Controller
    {
        private IFileManager _fileManager;
        private UserManager<ApplicationUser> _userManager;
        private IRepository _repository;

        public HomeController(IFileManager fileManager, UserManager<ApplicationUser> userManager, IRepository repository)
        {
            _fileManager = fileManager;
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<IActionResult> Index() 
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.ProfilePicture = (await _userManager.FindByNameAsync(User.Identity.Name)).ProfilePicture;
            }

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
            return new FileStreamResult(_fileManager.ProfilePictureStream(image), $"image/{mime}");
        }
    }
}
