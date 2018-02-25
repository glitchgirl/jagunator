using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JAGS.Models;

namespace JAGS.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateEdit()
        {
            return View();
        }

        public IActionResult CreateCourse()
        {
            return View();
        }

        public IActionResult EditCourse()
        {
            return View();
        }

        public IActionResult CreateEditUser()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            Console.Write("We have pressed the login button!");
            if (ModelState.IsValid)
            {
                return View("CreateEdit", model);
            }
            else
            {
                return View("About", model);
            }
        }
    }
}
