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
            };

            return View(viewModel);
        }

        //public IActionResult Finding(FindingViewModel viewModel) => View(viewModel);
    }
}
