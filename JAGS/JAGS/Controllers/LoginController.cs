using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JAGS.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JAGS.Controllers
{
    public class LoginController : Controller
    {
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            Console.Write("We have pressed the login button!");
            if (ModelState.IsValid)
            {
                return RedirectToPage("_Index", new HomeModel());
            }

            return View("_Index", model);
        }
    }
}
