using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GiveMe.Models;
using GiveMe.Data;
using GiveMe.Data.Repository;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using GiveMe.ViewModels;
using GiveMe.CloudStorage;
using System.IO;

namespace GiveMe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICloudStorage _cloudStorage;
        private IRepo _repo;
        IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        IRepository _context;

        public HomeController(ICloudStorage cloudStorage, IRepo repo, IRepository context, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _cloudStorage = cloudStorage;
            _repo = repo;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var posts = _repo.GetAllPosts();
            return View(posts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var adminRole = _httpContextAccessor.HttpContext.User.IsInRole("admin");
            var developerRole = _httpContextAccessor.HttpContext.User.IsInRole("developer");

            bool inRole = false;

            if (adminRole || developerRole)
            {
                inRole = true;
            }

            if (id == null)
                return View(new EditViewModel());
            else
            {
            var editViewModel = new EditViewModel
            {
                IsinRole = inRole,
                UserProject = _repo.GetPost((int)id)
            };

                //var post = _repo.GetPost((int)id);
                return View(editViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel post)
        {
            if (post.UserProject.Id > 0)
            {
                if(post.UserProject.ImageFile != null)
                {

                if(post.UserProject.ImageStorageName != null)
                {
                    await _cloudStorage.DeleteFileAsync(post.UserProject.ImageStorageName);
                }
                    await UploadFile(post.UserProject);
                }

                _repo.UpdatePost(post.UserProject);
            }
            else
            {
                if(post.UserProject.ImageFile != null)
                {
                    await UploadFile(post.UserProject);
                }
                post.UserProject.UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _repo.AddPost(post.UserProject);
            }
            if (await _repo.SaveChangesAsync())
                return RedirectToAction("Projects");
            else return View(post);
        }

        public IActionResult Projects()
        {
            var posts = _repo.GetAllPosts();
            ViewBag.user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View(posts);
        }

        public ActionResult _UserProjects()
        {
            var posts = _repo.GetAllPosts();
            ViewBag.user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View(posts);
        }


        public async Task<IActionResult> Project(int id)
        {
            var adminRole = _httpContextAccessor.HttpContext.User.IsInRole("admin");
            var developerRole = _httpContextAccessor.HttpContext.User.IsInRole("developer");
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var post = _repo.GetPost(id);
            bool inRole = false;

            if (adminRole || developerRole)
            {
                inRole = true;
            }

            var projectViewModel = new ProjectViewModel
            {
                Project =post,
                AdminRole = inRole,
                UserId = userId,
                CreatedBy = post.UserId
            };

            return View(projectViewModel);
        }

        public IActionResult ProjectInformation()
        {
            return View();
        }

        public IActionResult Comments()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Donate(int id)
        {
            var project = _repo.GetPost(id);
            var donateViewModel = new DonateViewModel
            {
                UserProject = project
            };
            return View(donateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Donate(DonateViewModel donateViewModel)
        {
            donateViewModel.UserProject.AlreadyFunded += donateViewModel.UserCash;
            _repo.UpdatePost(donateViewModel.UserProject);

            if (await _repo.SaveChangesAsync())
                return RedirectToAction("Projects");
            else return View(donateViewModel.UserProject);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            var project = await _context.Posts.FindAsync(id);
            if (project.ImageStorageName != null)
            {
                await _cloudStorage.DeleteFileAsync(project.ImageStorageName);
            }
            
            _repo.RemovePost(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("Projects");
        }

        public IActionResult UserPage()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.FirstOrDefault(p => p.Id == userId);
            return View(user);
        }

        private async Task UploadFile(Project project)
        {
            string fileNameForStorage = FormFileName(project.Title, project.ImageFile.FileName);
            project.ImageUrl = await _cloudStorage.UploadFileAsync(project.ImageFile, fileNameForStorage);
            project.ImageStorageName = fileNameForStorage;
        }

        private static string FormFileName(string title, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameForStorage = $"{title}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
            return fileNameForStorage;
        }
    }
}
