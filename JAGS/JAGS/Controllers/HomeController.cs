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
            ViewBag.ErrorMessage = "";
            return View();
        }

        /*------------------------------------------------------------------------------------------------------------------*/

        public IActionResult Logout()
        {
            return View("Index");
        }

        /*------------------------------------------------------------------------------------------------------------------*/

        public IActionResult HomePage()
        {
            return View();
        }

        /*------------------------------------------------------------------------------------------------------------------*/ 

        [HttpGet]
        public IActionResult CreateEditCourse()
        {
            string[] listDetails;
            string data;
            string[] directories;
            int counter = 0;

            //CampusLocation Load into model
            var model = new CourseInfo();
            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/ClassroomData/CampusLocations.csv";
            if (System.IO.File.Exists(filepath))
            {
                StreamReader readFile = new StreamReader(filepath);
                data = readFile.ReadLine();
                listDetails = data.Split(',');

            }
            else
            {
                listDetails = new string[0];
            }

            foreach (string s in listDetails)
            {
                model.CampusNames.Add(new CampusLocation { CampusID = counter, CampusName = s });
                counter++;
            }
            
            //ScheduleType Load into model
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/ClassroomData/ScheduleType.csv";
            counter = 0;
            if (System.IO.File.Exists(filepath))
            {
                StreamReader readFile = new StreamReader(filepath);
                data = readFile.ReadLine();
                listDetails = data.Split(',');

            }
            else
            {
                listDetails = new string[0];
            }

            foreach (string s in listDetails)
            {
                model.ScheduleType.Add(new ScheduleTypeList { ScheduleTypeID = counter, ScheduleTypeName = s });
                counter++;
            }

            //Semester Load into model
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) +"Data/Schedules/";
            directories = Directory.GetDirectories(filepath);
            counter = 0;
            foreach (string s in directories)
            {
                string[] tmp = s.Split("Schedules/");
                model.Semester.Add(new ListOfSemesters { SemesterID = counter, SemesterNameFromDirectory = tmp[1] });
                counter++;
            }


            //Course Subject Load into model
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Courses/";
            counter = 0;
            foreach (string s in Directory.GetFiles(filepath))
            {
                var path = Path.GetFileNameWithoutExtension(s);
//<<<<<<< HEAD
                //model.CourseList.Add(new ListOfCourses { CourseNumberID = fileCounter, CourseNameFromFile = path });//CourseNameFromFile = s.Remove(s.Length-4)});
//=======
                model.CourseList.Add(new ListOfCourses { CourseNumberID = counter, CourseNameFromFile = path});//CourseNameFromFile = s.Remove(s.Length-4)});
//>>>>>>> J-branch
            }

            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);
            return View(model);
        }

        /*------------------------------------------------------------------------------------------------------------------*/

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
            ViewBag.Jsonstr = Json(new { Success = "true", Data = new { usertype = row[2] } });
            return Json(new { Success = "true", Data = new { usertype = row[2] } });
            //if (row[2] == "Admin")
            //{
            //    return Json(new { Success = "true", Data = new { usertype = 1 } });
            //}
            //else
            //{
            //    return Json(new { Success = "true", Data = new { usertype = 0 } });
            //}
        }


        /*------------------------------------------------------------------------------------------------------------------*/

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
                    CourseName = row[0],
                    CourseID = row[1],
                    CourseSection = row[2],
                    InstructorName = row[3],
                    CampusName = row[4],
                    ClassroomSize = row[5]
                }
            });
            
        }

        /*------------------------------------------------------------------------------------------------------------------*/

        public IActionResult CreateEditUser()
        {
            ViewBag.debugtext = "test";
            ViewBag.Jsonstr = "";
            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session

            String filepathusers = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Users/";  //get file path for users folder
            string[] fileEntries = Directory.GetFiles(filepathusers);  //get array of files in user directory
            int pos = filepathusers.LastIndexOf("/") + 1;  //get position of last slash
            var listofusers = fileEntries.Select((r, index) => new System.Web.Mvc.SelectListItem { Text = r.Substring(pos, r.Length - pos - 4), Value = "user" }).ToList();  //populate drop down with list that automatically strips out .csv and the leading directories
            ViewBag.listusers = listofusers;
            return View("CreateEditUser");
        }

        /*------------------------------------------------------------------------------------------------------------------*/

        [HttpPost("CreateEditUser")]
        public ActionResult CreateEditUser(UserModel model, string CreateEdit)
        {
            //ViewBag.debugtext = "Entered createedituser after button";
            switch (CreateEdit)
            {   
                case "CreateEdit":
                    //ViewBag.debugtext = "entered createedit case";
                    ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
                    ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session
                    var usertype = "";
                            
                    var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Users/" + model.Username + ".csv";  //get absolute file path for possible user file
                    if (System.IO.File.Exists(filepath))   //check if user csv file exists
                    {
                        System.IO.File.Delete(filepath);   //delete user file if it exists
                    }
                    if (model.Type == 0)  //get text value of user type
                    {
                        usertype = "0";
                    }
                    else if (model.Type == 1)
                    {
                        usertype = "1";
                    }
                    else
                    {
                        usertype = "2";
                    }

                    var csv = model.Username.ToString() + "," + model.Password.ToString() + "," + usertype;  //create csv string to write out
                    System.IO.File.WriteAllText(filepath, csv.ToString());   //write csv file
                        
                    String filepathusers = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Users/";  //get file path for users folder
                    string[] fileEntries = Directory.GetFiles(filepathusers);  //get array of files in user directory
                    int pos = filepathusers.LastIndexOf("/") + 1;  //get position of last slash
                    var listofusers = fileEntries.Select((r, index) => new System.Web.Mvc.SelectListItem { Text = r.Substring(pos, r.Length - pos - 4), Value = "user" }).ToList();  //populate drop down with list that automatically strips out .csv and the leading directories
                    ViewBag.listusers = listofusers;
                    return View();

                case "Delete":
                    //ViewBag.debugtext = "entered delete case";
                    ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
                    ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session

                    filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Users/" + model.Username + ".csv";  //get absolute file path for possible user file
                    if (System.IO.File.Exists(filepath))   //check if user csv file exists
                    {
                        System.IO.File.Delete(filepath);   //delete user file if it exists
                    }

                    filepathusers = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Users/";  //get file path for users folder
                    fileEntries = Directory.GetFiles(filepathusers);  //get array of files in user directory
                    pos = filepathusers.LastIndexOf("/") + 1;  //get position of last slash
                    listofusers = fileEntries.Select((r, index) => new System.Web.Mvc.SelectListItem { Text = r.Substring(pos, r.Length - pos - 4), Value = "user" }).ToList();  //populate drop down with list that automatically strips out .csv and the leading directories
                    ViewBag.listusers = listofusers;
                    return View();
                default:
                    //ViewBag.debugtext = "entered default";
                    return View();

             }

        }

        /*------------------------------------------------------------------------------------------------------------------*/

        [HttpPost("CreateEditFaculty")]
        public ActionResult CreateEditFaculty(FacultyModel model, string CreateEdit)
        {
            ViewBag.debugtext = "entered createeditfacutly";
            switch (CreateEdit)
            {
                case "CreateEdit":
                    ViewBag.debugtext = "entered create case";
                    ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
                    ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session
                    var factype = "";

                    var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Faculty/" + model.Facultyname + ".csv";  //get absolute file path for possible user file
                    if (System.IO.File.Exists(filepath))   //check if user csv file exists
                    {
                        System.IO.File.Delete(filepath);   //delete user file if it exists
                    }
                    if (model.Facultytype == 0)  //get text value of user type
                    {
                        factype = "0";
                    }
                    else if (model.Facultytype == 1)
                    {
                        factype = "1";
                    }
                    else
                    {
                        factype = "2";
                    }

                    var csv = model.Facultyname.ToString() + "," + model.Facultytitle.ToString() + "," + factype;  //create csv string to write out
                    System.IO.File.WriteAllText(filepath, csv.ToString());   //write csv file

                    String filepathfac = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Faculty/";  //get file path for faculty folder
                    string[] fileEntries = Directory.GetFiles(filepathfac);  //get array of files in user directory
                    int pos = filepathfac.LastIndexOf("/") + 1;  //get position of last slash
                    var listoffac = fileEntries.Select((r, index) => new System.Web.Mvc.SelectListItem { Text = r.Substring(pos, r.Length - pos - 4), Value = "fac" }).ToList();  //populate drop down with list that automatically strips out .csv and the leading directories
                    ViewBag.listfac = listoffac;
                    return View();
                case "Delete":
                    ViewBag.debugtext = "entered delete case";
                    ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
                    ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session

                    filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Faculty/" + model.Facultyname + ".csv";  //get absolute file path for possible user file
                    if (System.IO.File.Exists(filepath))   //check if user csv file exists
                    {
                        System.IO.File.Delete(filepath);   //delete user file if it exists
                    }

                    filepathfac = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Faculty/";  //get file path for faculty folder
                    fileEntries = Directory.GetFiles(filepathfac);  //get array of files in user directory
                    pos = filepathfac.LastIndexOf("/") + 1;  //get position of last slash
                    listoffac = fileEntries.Select((r, index) => new System.Web.Mvc.SelectListItem { Text = r.Substring(pos, r.Length - pos - 4), Value = "fac" }).ToList();  //populate drop down with list that automatically strips out .csv and the leading directories
                    ViewBag.listfac = listoffac;
                    return View();
                default:
                    ViewBag.debugtext = "entered default case";
                    filepathfac = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Faculty/";  //get file path for faculty folder
                    fileEntries = Directory.GetFiles(filepathfac);  //get array of files in user directory
                    pos = filepathfac.LastIndexOf("/") + 1;  //get position of last slash
                    listoffac = fileEntries.Select((r, index) => new System.Web.Mvc.SelectListItem { Text = r.Substring(pos, r.Length - pos - 4), Value = "fac" }).ToList();  //populate drop down with list that automatically strips out .csv and the leading directories
                    ViewBag.listfac = listoffac;
                    return View();
            }

        }

        /*------------------------------------------------------------------------------------------------------------------*/

        public IActionResult CreateEditFaculty()
        {
            ViewBag.debugtext = "create edit faculty";
            ViewBag.Jsonstr = "";
            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session

            String filepathfac = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Faculty/";  //get file path for users folder
            string[] fileEntries = Directory.GetFiles(filepathfac);  //get array of files in user directory
            int pos = filepathfac.LastIndexOf("/") + 1;  //get position of last slash
            var listoffac = fileEntries.Select((r, index) => new System.Web.Mvc.SelectListItem { Text = r.Substring(pos, r.Length - pos - 4), Value = "fac" }).ToList();  //populate drop down with list that automatically strips out .csv and the leading directories
            ViewBag.listfac = listoffac;
            return View("CreateEditFaculty");
        }

        /*------------------------------------------------------------------------------------------------------------------*/


        [HttpPost]
        public ActionResult GetFacultyValues(string val)
        {
            String[] row;
            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Faculty/" + val + ".csv";
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
            ViewBag.Jsonstr = Json(new { Success = "true", Data = new { factype = row[2] } });
            return Json(new { Success = "true", Data = new { factype = row[2] } });
        }


        /*------------------------------------------------------------------------------------------------------------------*/

        
        [HttpPost]
        public ActionResult CreateEditCourse(CourseInfo courseInfo, FacultyModel fmodel, String semester)
        {
            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session

            //Filename is created as ..../Data/Schedules/CSCI1301A.csv
            //Course Subject = CSCI  |   CourseID = 1301  |  CourseSection = A
            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) 
                + "Data/Schedules/" 
                + semester + "/" 
                + courseInfo.CourseSubject 
                + courseInfo.CourseID 
                + courseInfo.CourseSection + ".csv";

            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }

            var csv = courseInfo.IntructorName.ToString() + "," + courseInfo.CourseName.ToString() + "," + courseInfo.CourseID.ToString() + "," + courseInfo.CampusLocation + "," + courseInfo.ClassSize;  //create csv string to write out
            System.IO.File.WriteAllText(filepath, csv.ToString());   //write csv file
           
            return View("CreateEditCourse", courseInfo);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /*------------------------------------------------------------------------------------------------------------------*/


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
                    if (row[2] == "0")
                    {
                        HttpContext.Session.SetString(SessionUserType, "Admin");
                        HttpContext.Session.SetString(SessionUserName, row[0].ToString());
                    }
                    else if (row[2] == "1")
                    {
                        HttpContext.Session.SetString(SessionUserType, "Editor");
                        HttpContext.Session.SetString(SessionUserName, row[0].ToString());
                    }
                    else if (row[2] == "2")
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
                    return View("HomePage");
                }
                ViewBag.ErrorMessage = "Login or Password incorrect";
                return View("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "Login or Password incorrect";
                return View("Index");
            }
        }

        [HttpGet]
        public IActionResult CreateEditSchedule()
        {
            string[] listDetails;
            string data;
            string[] directories;
            int counter = 0;

            //CampusLocation Load into model
            var model = new CourseInfo();
            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/ClassroomData/CampusLocations.csv";
            if (System.IO.File.Exists(filepath))
            {
                StreamReader readFile = new StreamReader(filepath);
                data = readFile.ReadLine();
                listDetails = data.Split(',');

            }
            else
            {
                listDetails = new string[0];
            }

            foreach (string s in listDetails)
            {
                model.CampusNames.Add(new CampusLocation { CampusID = counter, CampusName = s });
                counter++;
            }

            //ScheduleType Load into model
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/ClassroomData/ScheduleType.csv";
            counter = 0;
            if (System.IO.File.Exists(filepath))
            {
                StreamReader readFile = new StreamReader(filepath);
                data = readFile.ReadLine();
                listDetails = data.Split(',');

            }
            else
            {
                listDetails = new string[0];
            }

            foreach (string s in listDetails)
            {
                model.ScheduleType.Add(new ScheduleTypeList { ScheduleTypeID = counter, ScheduleTypeName = s });
                counter++;
            }

            //Semester Load into model
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Schedules/";
            directories = Directory.GetDirectories(filepath);
            counter = 0;
            foreach (string s in directories)
            {
                string[] tmp = s.Split("Schedules/");
                model.Semester.Add(new ListOfSemesters { SemesterID = counter, SemesterNameFromDirectory = tmp[1] });
                counter++;
            }


            //Course Subject Load into model
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Courses/";
            counter = 0;
            foreach (string s in Directory.GetFiles(filepath))
            {
                var path = Path.GetFileNameWithoutExtension(s);
                //<<<<<<< HEAD
                //model.CourseList.Add(new ListOfCourses { CourseNumberID = fileCounter, CourseNameFromFile = path });//CourseNameFromFile = s.Remove(s.Length-4)});
                //=======
                model.CourseList.Add(new ListOfCourses { CourseNumberID = counter, CourseNameFromFile = path });//CourseNameFromFile = s.Remove(s.Length-4)});
                                                                                                                //>>>>>>> J-branch
            }

            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);
            return View(model);
        }
    }//namespace
}
