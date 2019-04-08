using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginReg.Models;

namespace LoginReg.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
                return RedirectToAction("Success", user);
            else
                return View("Index");
        }
        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginUser user)
        {
            if(ModelState.IsValid)
                return RedirectToAction("Success", user);
            else
                return View("Index");
        }

        [Route("success")]
        [HttpGet]
        public IActionResult Success()
        {
            return View("Success");
        }

    }
}
