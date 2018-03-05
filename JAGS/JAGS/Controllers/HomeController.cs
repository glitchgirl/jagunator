using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using JAGS.Models;
using Microsoft.AspNetCore.Razor;
using System.Web.Optimization;
//using System.Web
//using static System.Web.Mvc.SelectListItem;
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

        public IActionResult Logout()
        {
            return View("Index");
        }


        public IActionResult CreateEditSchedule()
        {
            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);
            return View();
        }

        /*-----------------------------------------------*/
        [HttpGet]
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

            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Courses/";
            int fileCounter = 0;
            foreach (string s in Directory.GetFiles(filepath))
            {
                var path = Path.GetFileNameWithoutExtension(s);
                model.CourseList.Add(new ListOfCourses { CourseNumberID = fileCounter, CourseNameFromFile = path});//CourseNameFromFile = s.Remove(s.Length-4)});
            }


            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);

            return View(model);
        }


        [HttpPost]
        public ActionResult GetUserValues(string val)
        {
            String[] row;
            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Users/" + val + ".csv";
            if (System.IO.File.Exists(filepath))   //check if user csv file exists
            {
                StreamReader readFile = new StreamReader(filepath);
                String line = readFile.ReadLine();
                row = line.Split(',');
            }
            else
            {
                return Json(new { Success = "false" });
            }

            if (row[2] == "Admin")
            {
                return Json(new { Success = "true", Data = new { usertype = 1 } });
            }
            else
            {
                return Json(new { Success = "true", Data = new { usertype = 0 } });
            }
        }


        /*-----------------------------------------------*/

        [HttpPost]
        public ActionResult GetCourseValues(string val)
        {
            String[] row;
            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Courses/" + val + ".csv";
            if (System.IO.File.Exists(filepath))   //check if user csv file exists
            {
                StreamReader readFile = new StreamReader(filepath);
                String line = readFile.ReadLine();
                row = line.Split(',');
            }
            else
            {
                return Json(new { Success = "false" });
            }

            //ViewBag.reached = 1;
            return Json(new
            {
                Success = "true",
                Data = new
                {
                    InstructorName = row[0],
                    CourseName = row[1],
                    CourseID = row[2],
                    CampusName = row[3],
                    ClassroomSize = row[4]
                }
            });
            
        }

        /*-----------------------------------------------*/

        public IActionResult CreateEditUser()
        {
            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session

            String filepathusers = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Users/";  //get file path for users folder
            string[] fileEntries = Directory.GetFiles(filepathusers);  //get array of files in user directory
            int pos = filepathusers.LastIndexOf("/") + 1;  //get position of last slash
            var listofusers = fileEntries.Select((r, index) => new System.Web.Mvc.SelectListItem { Text = r.Substring(pos, r.Length - pos - 4), Value = "admin" }).ToList();  //populate drop down with list that automatically strips out .csv and the leading directories
            ViewBag.listusers = listofusers;
            return View("CreateEditUser");
        }

        /*-----------------------------------------------*/

        [HttpPost("CreateEditUser")]
        public ActionResult CreateEditUser(UserModel model)
        {
            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session
            var usertype = "";

            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Users/" + model.Username + ".csv";  //get absolute file path for possible user file
            if (System.IO.File.Exists(filepath))   //check if user csv file exists
            {
                System.IO.File.Delete(filepath);   //delete user file if it exists
            }
            if (model.Type == true)  //get text value of user type
            {
                usertype = "Admin";
            }
            else
            {
                usertype = "Viewer";
            }
            var csv = model.Username.ToString() + "," + model.Password.ToString() + "," + usertype.ToString();  //create csv string to write out
            System.IO.File.WriteAllText(filepath, csv.ToString());   //write csv file

            String filepathusers = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Users/";  //get file path for users folder
            string[] fileEntries = Directory.GetFiles(filepathusers);  //get array of files in user directory
            int pos = filepathusers.LastIndexOf("/") + 1;  //get position of last slash
            var listofusers = fileEntries.Select((r, index) => new System.Web.Mvc.SelectListItem { Text = r.Substring(pos, r.Length - pos - 4), Value = "admin" }).ToList();  //populate drop down with list that automatically strips out .csv and the leading directories
            ViewBag.listusers = listofusers;
            return View("CreateEditUser");

        }

        
        [HttpPost]
        public ActionResult CreateEditCourse(CourseInfo courseInfo)
        {
            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session

            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Courses/" + courseInfo.CourseID.Replace(" ","") + ".csv";
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }

            var csv = courseInfo.IntructorName.ToString() + "," + courseInfo.CourseName.ToString() + "," + courseInfo.CourseID.ToString() + "," + courseInfo.CampusLocation + "," + courseInfo.ClassSize;  //create csv string to write out
            System.IO.File.WriteAllText(filepath, csv.ToString());   //write csv file
           
            return View("CreateEditCourse");
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
                    ViewBag.loginname = row[0];
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
                    String filepathusers = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Users/";  //get file path for users folder
                    ViewBag.filepathdir = filepathusers;
                    string[] fileEntries = Directory.GetFiles(filepathusers);  //get array of files in user directory
                    int pos = filepathusers.LastIndexOf("/") + 1;  //get position of last slash
                    var listofusers = fileEntries.Select((r, index) => new System.Web.Mvc.SelectListItem { Text = r.Substring(pos, r.Length - pos - 4), Value = row[2].ToLower() }).ToList();  //populate drop down with list that automatically strips out .csv and the leading directories
                    ViewBag.listusers = listofusers;
                    return View("CreateEditUser", new UserModel());
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
