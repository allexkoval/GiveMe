using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiveMe.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        public IActionResult Roles()
        {
            return View();
        }
    }
}