using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JAGS.Models;
//using System.Web.Mvc;
//using System.Web.Hosting;
//using Environment;
using static Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment;

namespace JAGS.Controllers
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult CreateEdit()
        {
            return View();
        }

        /*-----------------------------------------------*/
        public IActionResult CreateEditCourse()
        {
            string[] CampusNames = 
            {
                "Summerville",
                "Riverfront",
                "Health Science",
                "Forest Hills"
            };
            string[] clSize =
            {
                "1  - 24",
                "25 - 39",
                "40 + "
            };
            var model = new CourseInfo();
            for(int i = 0; i < CampusNames.Length; i++)
            {
                model.CampusNames.Add(new CampusLocation { CampusID = i, CampusName = CampusNames[i] });
            }
            for (int i = 0; i < clSize.Length; i++)
            {
                model.ClassroomStudentSize.Add(new ClassroomSize { ClassroomID = i, ClassSize = clSize[i] });
            }

            return View(model);
        }


        /*-----------------------------------------------*/

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
            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Users/" + model.Login + ".csv";
            if (System.IO.File.Exists(filepath))   //check if user csv file exists
            {
                return View("CreateEditUser", model);
            }
            Console.Write("We have pressed the login button!");
            else if (model.Login == "Admin")
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
