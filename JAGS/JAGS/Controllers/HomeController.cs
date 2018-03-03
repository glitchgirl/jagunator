using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using JAGS.Models;
//using System.Web.Mvc;
//using System.Web.Hosting;
//using Environment;
using static Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment;
using System.IO;
using Microsoft.AspNetCore.Session;

namespace JAGS.Controllers
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        const string SessionUserName = "_Name";
        const string SessionUserPass = "_Pass";
        const string SessionUserType = "_Type";
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult CreateEditSchedule()
        {
            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);
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
            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);

            return View(model);
        }


        /*-----------------------------------------------*/

        public IActionResult CreateEditUser()
        {
            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);
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
                StreamReader readFile = new StreamReader(filepath);
                String line = readFile.ReadLine();
                String[] row = line.Split(',');
                if (row[1] == model.Password)   //if password is correct
                {
                    //ViewBag.SessionUserType = SessionUserType;
                    ViewBag.loginname = row[0];
                    //ViewBag.pass = row[1];
                    //ViewBag.type = row[2];
                    //ViewBag.sessiontype = "Admin";
                    if (row[2] == "Admin")
                    {
                        HttpContext.Session.SetString(SessionUserType, "Admin");
                        HttpContext.Session.SetString(SessionUserName, row[0].ToString());
                    }
                    else
                    {
                        HttpContext.Session.SetString(SessionUserType, "Viewer");
                        HttpContext.Session.SetString(SessionUserName, row[0].ToString());
                    }

                    ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);
                    return View("CreateEditUser");
                }

                return View("About", model);
            }
            else
            {
                return View("About", model);
            }
        }
    }
}
