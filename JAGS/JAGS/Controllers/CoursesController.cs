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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JAGS.Controllers
{
    public class CoursesController : HomeController
    {
        const string SessionUserName = "_Name";
        const string SessionUserPass = "_Pass";
        const string SessionUserType = "_Type";

        [HttpGet]
        public IActionResult CreateEditCourse()
        {

            string[] listDetails;
            string data;
            string[] directories;
            int counter = 0;
            int pos;
            StreamReader readFile;

            //CampusLocation Load into model
            var model = new CourseInfo();
            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/ClassroomData/CampusLocations.csv";
            if (System.IO.File.Exists(filepath))
            {
                readFile = new StreamReader(filepath);
                data = readFile.ReadLine();
                if (data != null && data.Contains(','))
                    listDetails = data.Split(',');
                else
                    listDetails = new string[0];
                readFile.Close();
            }
            else
            {
                listDetails = new string[0];
            }

            foreach (string s in listDetails)
            {
                model.CampusNames.Add(new CourseCampusLocation { CampusID = counter, CampusName = s });
                counter++;
            }

            //ScheduleType Load into model
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/ClassroomData/ScheduleType.csv";
            counter = 0;
            if (System.IO.File.Exists(filepath))
            {
                readFile = new StreamReader(filepath);
                data = readFile.ReadLine();
                if (data != null && data.Contains(','))
                    listDetails = data.Split(',');
                else
                    listDetails = new string[0];
                readFile.Close();

            }
            else
            {
                listDetails = new string[0];
            }

            foreach (string s in listDetails)
            {
                model.ScheduleType.Add(new CourseScheduleTypeList { ScheduleTypeID = counter, ScheduleTypeName = s });
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
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/ClassroomData/Subject.csv";
            counter = 0;
            if (System.IO.File.Exists(filepath))
            {
                readFile = new StreamReader(filepath);
                data = readFile.ReadLine();
                if (data != null && data.Contains(','))
                    listDetails = data.Split(',');
                else
                    listDetails = new string[0];
                readFile.Close();
            }
            else
            {
                listDetails = new string[0];
            }
            foreach (string s in listDetails)
            {
                model.Subject.Add(new CourseSubjectModel { CourseSubjectID = counter, SubjectCode = s });
                counter++;
            }


            //Course Credit Hours Load into model
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/ClassroomData/CreditHours.csv";
            counter = 0;
            if (System.IO.File.Exists(filepath))
            {
                readFile = new StreamReader(filepath);
                data = readFile.ReadLine();
                if (data != null && data.Contains(','))
                    listDetails = data.Split(',');
                else
                    listDetails = new string[0];
                readFile.Close();
            }
            else
            {
                listDetails = new string[0];
            }
            foreach (string s in listDetails)
            {
                model.CourseCreditList.Add(new CourseCreditModel { CreditID = counter, CreditAmount = s });
                counter++;
            }

            //Load Faculty into Model
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Faculty/";
            counter = 0;
            listDetails = Directory.GetFiles(filepath);
            pos = filepath.LastIndexOf("/") + 1;
            foreach (string s in listDetails)
            {
                model.ListOfInstructors.Add(new CourseInstructorModel { InstructorListID = counter, InstructorName = s.Substring(pos, s.Length - pos - 4) });
                counter++;
            }


            //Load courses 

            //* Trying a new way to load these,
            //* this list can get very large so I would
            //* rather only load what needs to be loaded
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Courses/";
            counter = 0;
            listDetails = Directory.GetFiles(filepath);
            pos = filepath.LastIndexOf("/") + 1;
            foreach (string s in listDetails)
            {
                model.CourseIDList.Add(new CourseIDModel { CourseListID = counter, CourseIDForSchedule = s.Substring(pos, s.Length - pos - 4) });
                counter++;
            }


            //test
            //model.CourseIDList.Add(new CourseIDModel { CourseListID = 0, CourseIDForSchedule = "" });


            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);
            return View(model);
        }





        /*-----------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------*/



        /*-----------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------*/


        [HttpPost]
        public ActionResult CreateEditCourse(CourseInfo model, string command)
        {
            //IF COURSE SAVE IS PRESSED
            if (command.Equals("Save Course"))
            {
                if (ModelState.IsValid)
                {

                    ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
                    ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session
                    var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24)
                        + "Data/Courses/"
                        + model.CourseID
                        + ".csv";

                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                    var csv = model.CourseSubject.ToString() + "," + model.CourseID.ToString() + "," + model.CourseName.ToString() + "," + model.CreditHours;  //create csv string to write out
                    System.IO.File.WriteAllText(filepath, csv.ToString());   //write csv file
                }
            }
            //ELSE SAVE SECTION IS PRESSED
            else if (command.Equals("Delete Course"))
            {
                ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
                ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session

                var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24)
                            + "Data/Courses/"
                            + model.CourseID
                            + ".csv";

                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            else if (command.Equals("Save Section"))
            {
                if (ModelState.IsValid)
                {

                    ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
                    ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session
                    var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24)
                        + "Data/Schedules/"
                        + model.sectionSemester
                        + "/"
                        + model.CourseSubject
                        + "_"
                        + model.CourseID
                        + "_"
                        + model.CourseSection
                        + ".csv";

                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                    var csv = model.CourseSubject.ToString()
                        + "," + model.CourseID.ToString()
                        + "," + model.CourseSection.ToString()
                        + "," + model.CourseName.ToString()
                        + "," + model.CreditHours
                        + "," + model.IntructorName.ToString()
                        + "," + model.CampusLocation.ToString()
                        + "," + model.ScheduleAtt.ToString();

                    //create csv string to write out
                    System.IO.File.WriteAllText(filepath, csv.ToString());   //write csv file


                }
            }
            else if (command.Equals("Delete Section"))
            {
                ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
                ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session

                var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24)
                        + "Data/Schedules/"
                        + model.sectionSemester
                        + "/"
                        + model.CourseSubject
                        + "_"
                        + model.CourseID
                        + "_"
                        + model.CourseSection
                        + ".csv";

                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            return RedirectToAction("CreateEditCourse", model);
        }

        [HttpPost]
        public ActionResult SaveCourse(CourseInfo model)
        {
            //IF COURSE SAVE IS PRESSED

            if (ModelState.IsValid)
            {

                ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
                ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session
                var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24)
                    + "Data/Courses/"
                    + model.CourseID
                    + ".csv";

                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
                var csv = model.CourseSubject.ToString() + "," + model.CourseID.ToString() + "," + model.CourseName.ToString() + "," + model.CreditHours;  //create csv string to write out
                System.IO.File.WriteAllText(filepath, csv.ToString());   //write csv file


            }

            return this.View(model);
            //return RedirectToAction("CreateEditCourse", model);
        }

        [HttpPost]
        public ActionResult DeleteCourse(CourseInfo model)
        {
            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session

            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24)
                        + "Data/Courses/"
                        + model.CourseID
                        + ".csv";

            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }

            return RedirectToAction("CreateEditCourse", model);
        }

        /*-----------------------------------------------------------------------------------------------------------------*/

        [HttpPost]
        public ActionResult GetCourseValues(string val)
        {
            String[] row;
            StreamReader readFile;
            string line;
            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Courses/" + val + ".csv";
            if (System.IO.File.Exists(filepath))   //check if user csv file exists
            {
                readFile = new StreamReader(filepath);
                line = readFile.ReadLine();
                if (line != null && line.Contains(','))
                    row = line.Split(',');
                else
                    return Json(new { Success = "false" });
                readFile.Close();
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
                    CourseSubject = row[0],
                    CourseID = row[1],
                    CourseName = row[2],
                    CourseCredit = row[3],
                }
            });

        }



        [HttpPost]
        public ActionResult SaveSection(CourseInfo model)
        {
            if (ModelState.IsValid)
            {

                ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
                ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session
                var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24)
                    + "Data/Schedules/"
                    + model.sectionSemester
                    + "/"
                    + model.CourseSubject
                    + "_"
                    + model.CourseID
                    + "_"
                    + model.CourseSection
                    + ".csv";

                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
                var csv = model.CourseSubject.ToString()
                    + "," + model.CourseID.ToString()
                    + "," + model.CourseSection.ToString()
                    + "," + model.CourseName.ToString()
                    + "," + model.CreditHours
                    + "," + model.IntructorName.ToString()
                    + "," + model.CampusLocation.ToString()
                    + "," + model.ScheduleAtt.ToString();
                    
                    //create csv string to write out
                System.IO.File.WriteAllText(filepath, csv.ToString());   //write csv file


            }
            return RedirectToAction("CreateEditCourse", model);
        }

        /*-----------------------------------------------------------------------------------------------------------------*/
        [HttpPost]
        public ActionResult DeleteSection(CourseInfo model)
        {
            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);  //get type of user from session
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);    //get username from session

            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24)
                    + "Data/Schedules/"
                    + model.sectionSemester
                    + "/"
                    + model.CourseSubject
                    + "_"
                    + model.CourseID
                    + "_"
                    + model.CourseSection
                    + ".csv";

            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }

            return RedirectToAction("CreateEditCourse", model);
        }



    }
}
