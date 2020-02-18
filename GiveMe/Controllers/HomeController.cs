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
using GiveMe.Data.User;

namespace GiveMe.Controllers
{
    public class HomeController : Controller
    {
        
        private IRepo _repo;

        IRepository _context;

        public HomeController(IRepo repo, IRepository context)
        {
            _repo = repo;
            _context = context;
        }

        public IActionResult Index()
        {
          return View();
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
            if(id == null)
                return View(new Post());
            else
            {
                var post = _repo.GetPost((int)id);
                return View(post);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Post post)
        {
            if (post.Id > 0)
                _repo.UpdatePost(post);
            else
            _repo.AddPost(post);
            if (await _repo.SaveChangesAsync())
                return RedirectToAction("Posts");
            else return View(post);
        }

        public IActionResult Posts()
        {
            var posts = _repo.GetAllPosts();
            return View(posts);
        }

        public IActionResult Post(int id)
        {
            var post = _repo.GetPost(id);
            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            _repo.RemovePost(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("Posts");
        }

        public IActionResult UserPage()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
    }
}
