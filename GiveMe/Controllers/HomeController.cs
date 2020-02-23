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

namespace GiveMe.Controllers
{
    public class HomeController : Controller
    {

        private IRepo _repo;
        IHttpContextAccessor _httpContextAccessor;
        IRepository _context;

        public HomeController(IRepo repo, IRepository context, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
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
            if (id == null)
                return View(new Project());
            else
            {
                var post = _repo.GetPost((int)id);
                return View(post);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Project post)
        {
            if (post.Id > 0)
                _repo.UpdatePost(post);
            else
            {
                post.UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _repo.AddPost(post);
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


        public IActionResult Project(int id)
        {
            var post = _repo.GetPost(id);
            return View(post);
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
        public async Task<IActionResult> Remove(int id)
        {
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
    }
}
