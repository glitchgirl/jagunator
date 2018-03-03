using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JAGS.Models;
using System.Web.Mvc;

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
                "1  - 30",
                "30 - 45",
                "46 + "
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
    }
}
