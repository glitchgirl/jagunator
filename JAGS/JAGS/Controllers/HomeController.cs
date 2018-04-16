using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
//using System.Web.Script.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using JAGS.Models;
using Microsoft.AspNetCore.Razor;
using System.Web.Optimization;
using Newtonsoft.Json;
//using System.Web
//using static System.Web.Mvc.SelectListItem;
//using System.Web.Hosting;
//using Environment;
using static Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Session;

namespace JAGS.Controllers
{
    public class ReturnData
    {
        public string Success { get; set; }
        public List<EventObject> Data { get; set; }
    }
    public class EventObject
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string title { get; set; }
        public string name { get; set; }
    }


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
            ViewBag.sessiontype = "none";
            HttpContext.Session.SetString(SessionUserType, "");
            return RedirectToAction("", "");
        }

        /*------------------------------------------------------------------------------------------------------------------*/

        public IActionResult HomePage()
        {
            return View();
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
            String filepathusers = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Users/";  //get file path for users folder
            string[] fileEntries = Directory.GetFiles(filepathusers);  //get array of files in user directory
            int pos = filepathusers.LastIndexOf("/") + 1;  //get position of last slash
            var listofusers = fileEntries.Select((r, index) => new System.Web.Mvc.SelectListItem { Text = r.Substring(pos, r.Length - pos - 4), Value = "user" }).ToList();  //populate drop down with list that automatically strips out .csv and the leading directories
            ViewBag.listusers = listofusers;
            //ViewBag.debugtext = "Entered createedituser after button";
            switch (CreateEdit)
            {   
                case "Create/Edit":
                    if (model.IsValid(model.Username,model.Password))
                    {
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
                        if (model.Password == null)
                        {
                            model.Password = "";
                        }
                        var csv = model.Username.ToString() + "," + model.Password.ToString() + "," + usertype;  //create csv string to write out
                        System.IO.File.WriteAllText(filepath, csv.ToString());   //write csv file

                        filepathusers = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Users/";  //get file path for users folder
                        fileEntries = Directory.GetFiles(filepathusers);  //get array of files in user directory
                        pos = filepathusers.LastIndexOf("/") + 1;  //get position of last slash
                        listofusers = fileEntries.Select((r, index) => new System.Web.Mvc.SelectListItem { Text = r.Substring(pos, r.Length - pos - 4), Value = "user" }).ToList();  //populate drop down with list that automatically strips out .csv and the leading directories
                        ViewBag.listusers = listofusers;
                        return View("CreateEditUser");
                    }
                    break;

                case "Delete":
                    //ViewBag.debugtext = "entered delete case";
                    ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
                    ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session

                    filepathusers = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Users/" + model.Username + ".csv";  //get absolute file path for possible user file
                    if (System.IO.File.Exists(filepathusers))   //check if user csv file exists
                    {
                        System.IO.File.Delete(filepathusers);   //delete user file if it exists
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
            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);
            return View();

        }

        /*------------------------------------------------------------------------------------------------------------------*/

        //[HttpPost("Home/CreateEditFaculty")]
        [HttpPost]
        //[HttpGet("[controller]/[action]")]
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

        public ActionResult CreateEditFaculty()
        {
            //if (HttpContext.Session.GetString(SessionUserType) != "Admin" || HttpContext.Session.GetString(SessionUserType) != "Editor")
            //{
            //    return View("Index");
            //}
            //var model = new FacultyModel();
            ViewBag.debugtext = "create edit faculty";
            ViewBag.Jsonstr = "";
            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session

            String filepathfac = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Faculty/";  //get file path for users folder
            string[] fileEntries = Directory.GetFiles(filepathfac);  //get array of files in user directory
            int pos = filepathfac.LastIndexOf("/") + 1;  //get position of last slash
            var listoffac = fileEntries.Select((r, index) => new System.Web.Mvc.SelectListItem { Text = r.Substring(pos, r.Length - pos - 4), Value = "fac" }).ToList();  //populate drop down with list that automatically strips out .csv and the leading directories
            ViewBag.listfac = listoffac;
            return View();
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
        public ActionResult GetSectionValues(string val)
        {
            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Schedules/" + val + "/";
            var files = System.IO.Directory.GetFiles(filepath).Select(Path.GetFileNameWithoutExtension);
            if (files.Count() > 0)   //check if user csv file exists
            {
                ViewBag.sections = files;
                var sections = JsonConvert.SerializeObject(files);
                return Json(new { Success = "true", Data = sections });
            }
            else
            {
                return Json(new { Success = "false" });
            }
            //ViewBag.sections = files;
            return Json(new { Success = "true" });
        }


        /*------------------------------------------------------------------------------------------------------------------*/

        [HttpPost]
        public ActionResult ExportCalendar(string val)
        {
            //string filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Semesters/";
            //Debug.WriteLine(val);
            //var semesterevents = JsonConvert.DeserializeObject<List<EventObject>>(val);
            //Debug.WriteLine(semesterevents[0].name);
            //filepath = filepath + val + ".csv";
            string filepath = "Export/" + val + ".csv";
            //if (System.IO.File.Exists(filepath))   //check if user csv file exists
            //{
            //    var mimeType = "text/csv";
            //    FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            //    return File(fileStream, mimeType, "Export.csv");   //delete user file if it exists
            //}
            //var retfilepath = JsonConvert.DeserializeObject(filepath);
            var returnvalue = Json(new { Success = "true", Filepath = JsonConvert.SerializeObject(filepath) });
            return Json(new { Success = "true", Filepath = filepath });
        }


        /*------------------------------------------------------------------------------------------------------------------*/

        [HttpPost]
        public ActionResult SaveSemesterValues(string val)
        {
            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Semesters/";
            //Debug.WriteLine(val);
            var semesterevents = JsonConvert.DeserializeObject<List<EventObject>>(val);
            //Debug.WriteLine(semesterevents[0].name);
            if (semesterevents.Count > 0)
            {
                filepath = filepath + semesterevents[0].name + ".csv";
                if (System.IO.File.Exists(filepath))   //check if user csv file exists
                {
                    System.IO.File.Delete(filepath);   //delete user file if it exists
                }
                System.IO.File.WriteAllText(filepath, val);   //write csv file

                //create export file (csv that opens in excel)
                List<string> export = new List<string>();
                export.Add("Subject,ID,Section,Title,Credit Hours,Professor,Campus,Type,Size,Classroom Type,Crosslist,Day of Week, Start Time of Day, End Time of Day,");
                String current_input = "";
                var expfilepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "wwwroot/Export/" + semesterevents[0].name + ".csv";
                if (System.IO.File.Exists(expfilepath))   //check if user csv file exists
                {
                    System.IO.File.Delete(expfilepath);   //delete user file if it exists
                }
                for (int i = 0; i < semesterevents.Count; i++)
                {
                    String current_file_path = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Schedules/" + semesterevents[0].name + "/" + semesterevents[i].title + ".csv";
                    if (System.IO.File.Exists(current_file_path))
                    {
                        StreamReader readFile = new StreamReader(current_file_path);
                        current_input = readFile.ReadLine() + "," + semesterevents[i].start.DayOfWeek + "," + semesterevents[i].start.TimeOfDay + "," + semesterevents[i].end.TimeOfDay + ",";
                        export.Add(current_input);
                    }
                }
                System.IO.File.WriteAllLines(expfilepath, export);   //write csv file
            }
            return Json(new { Success = "true" });
        }


        /*------------------------------------------------------------------------------------------------------------------*/

        [HttpPost]
        public ActionResult GetSemesterEvents(string val)
        {
            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Semesters/" + val + ".csv";
            Debug.WriteLine(val);
            String line = "";
            if (System.IO.File.Exists(filepath))   //check if user csv file exists
            {
                StreamReader readFile = new StreamReader(filepath);
                line = readFile.ReadLine();
            }
            else
            {
                return Json(new { Success = "false" });
            }
            var retevents = JsonConvert.DeserializeObject<List<EventObject>>(line);
            //var semesterevents = JsonConvert.DeserializeObject<List<EventObject>>(val);
            Debug.WriteLine(retevents[0].name);
            //filepath = filepath + semesterevents[0].name + ".csv";
            //System.IO.File.WriteAllText(filepath, val);   //write csv file

            return Json(new { Success = "true" , Data = retevents});
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
                model.CampusNames.Add(new System.Web.Mvc.SelectListItem { Value = counter.ToString(), Text = s });
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
                model.ScheduleType.Add(new System.Web.Mvc.SelectListItem { Value = counter.ToString(), Text = s });
                counter++;
            }

            //Semester Load into model
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Schedules/";
            directories = Directory.GetDirectories(filepath);
            counter = 0;
            foreach (string s in directories)
            {
                string[] tmp = s.Split("Schedules/");
                model.Semester.Add(new System.Web.Mvc.SelectListItem { Value = counter.ToString(), Text = tmp[1] });
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
                //model.CourseList.Add(new ListOfCourses { CourseNumberID = counter, CourseNameFromFile = path });//CourseNameFromFile = s.Remove(s.Length-4)});
                                                                                                                //>>>>>>> J-branch
            }

            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);
            return View(model);


        }//createeditschedule

    }//class
}//namespace
