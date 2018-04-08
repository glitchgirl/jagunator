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
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Session;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JAGS.Controllers
{
    public class CoursesController : Microsoft.AspNetCore.Mvc.Controller
    {
        const string SessionUserName = "_Name";
        const string SessionUserPass = "_Pass";
        const string SessionUserType = "_Type";

        [Microsoft.AspNetCore.Mvc.HttpGet]
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
                model.CampusNames.Add(new SelectListItem { Value = counter.ToString(), Text = s });
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
                model.ScheduleType.Add(new SelectListItem { Value = counter.ToString(), Text = s });
                counter++;
            }

            //Semester Load into model
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Schedules/";
            directories = Directory.GetDirectories(filepath);
            counter = 0;
            foreach (string s in directories)
            {
                string[] tmp = s.Split("Schedules/");
                model.Semester.Add(new SelectListItem { Value = counter.ToString(), Text = tmp[1] });
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

            //THIS IS NEEDED HERE
            model.SubjectList = new List<System.Web.Mvc.SelectListItem>();
            foreach (string s in listDetails)
            {
                //TRYING NEW FOR DYNAMIC LOADING
                model.SubjectList.Add(new System.Web.Mvc.SelectListItem { Value = counter.ToString(), Text = s });


                //THIS IS OLD ONE
                //model.Subject.Add(new CourseSubjectModel { CourseSubjectID = counter, SubjectCode = s });
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
                model.CourseCreditList.Add(new SelectListItem { Value = counter.ToString(), Text = s });
                counter++;
            }

            //Load Classroom Size into Model
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/ClassroomData/ClassroomSize.csv";
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
                model.ClassroomSizeList.Add(new SelectListItem { Value = counter.ToString(), Text = s });
                counter++;
            }

            //Load Classroom Type into Model
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/ClassroomData/ClassroomType.csv";
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
                model.ClassroomTypeList.Add(new SelectListItem { Value = counter.ToString(), Text = s });
                counter++;
            }


            //Load Faculty into Model
            filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Faculty/";
            counter = 0;
            listDetails = Directory.GetFiles(filepath);
            pos = filepath.LastIndexOf("/") + 1;
            foreach (string s in listDetails)
            {
                model.ListOfInstructors.Add(new SelectListItem { Value = counter.ToString(), Text = s.Substring(pos, s.Length - pos - 4) });
                counter++;
            }

            ViewBag.sessiontype = HttpContext.Session.GetString(SessionUserType);
            ViewBag.loginname = HttpContext.Session.GetString(SessionUserName);
            return View(model);
        }





        /*-----------------------------------------------------------------------------------------------------------------*/



        [Microsoft.AspNetCore.Mvc.HttpPost]
        public Microsoft.AspNetCore.Mvc.ActionResult CreateEditCourse(CourseInfo model, string command)
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
                        + model.CourseSubject
                        + "_"
                        + model.CourseID
                        + ".csv";

                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                    var csv = model.CourseSubject.ToString() + "," + model.CourseID.ToString() + "," + model.CourseName.ToString() + "," + model.CreditHours;
                    if (model.CrossListWith.ToString() != null)
                        csv += "," + model.CrossListWith.ToString(); //create csv string to write out
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
                            + model.CourseSubject
                            + "_"
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
                    var csv = "";
                    if (model.CrossListWith == null)
                    {
                            csv = model.CourseSubject.ToString()
                            + "," + model.CourseID.ToString()
                            + "," + model.CourseSection.ToString()
                            + "," + model.CourseName.ToString()
                            + "," + model.CreditHours
                            + "," + model.IntructorName.ToString()
                            + "," + model.CampusLocation.ToString()
                            + "," + model.ScheduleAtt.ToString()
                            + "," + model.ClassroomSize.ToString()
                            + "," + model.ClassroomType.ToString();
                    }

                    else
                    {
                            csv = model.CourseSubject.ToString()
                            + "," + model.CourseID.ToString()
                            + "," + model.CourseSection.ToString()
                            + "," + model.CourseName.ToString()
                            + "," + model.CreditHours
                            + "," + model.IntructorName.ToString()
                            + "," + model.CampusLocation.ToString()
                            + "," + model.ScheduleAtt.ToString()
                            + "," + model.ClassroomSize.ToString()
                            + "," + model.ClassroomType.ToString()
                            + "," + model.CrossListWith.ToString();

                    }

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



        /*-----------------------------------------------------------------------------------------------------------------*/

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public Microsoft.AspNetCore.Mvc.ActionResult GetCourseValues(string val)
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

            if (row.Length == 4)
            {
                return Json(new
                {
                    Success = "true",
                    Data = new
                    {
                        CourseSubject = row[0],
                        CourseID = row[1],
                        CourseName = row[2],
                        CourseCredit = row[3]
                    }
                });
            }
            else
            {
                return Json(new
                {
                    Success = "true",
                    Data = new
                    {
                        CourseSubject = row[0],
                        CourseID = row[1],
                        CourseName = row[2],
                        CourseCredit = row[3],
                        CrossList = row[4]
                    }
                });
            }

        }


        /*-----------------------------------------------------------------------------------------------------------------*/

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public Microsoft.AspNetCore.Mvc.ActionResult GetSectionValues(string val)
        {
            String[] row;
            StreamReader readFile;
            string line;
            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24) + "Data/Schedules/" + val + ".csv";
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

            if (row.Length == 10)
            {
                return Json(new
                {
                    Success = "true",
                    Data = new
                    {
                        CourseSubject = row[0],
                        CourseID = row[1],
                        CourseSection = row[2],
                        CourseName = row[3],
                        CourseCredit = row[4],
                        SectionInstructor = row[5],
                        SectionCampus = row[6],
                        SectionScheduleType = row[7],
                        SectionClassroomSize = row[8],
                        SectionClassroomType = row[9]
                    }
                });
            }
            else
            {
                return Json(new
                {
                    Success = "true",
                    Data = new
                    {
                        CourseSubject = row[0],
                        CourseID = row[1],
                        CourseSection = row[2],
                        CourseName = row[3],
                        CourseCredit = row[4],
                        SectionInstructor = row[5],
                        SectionCampus = row[6],
                        SectionScheduleType = row[7],
                        SectionClassroomSize = row[8],
                        SectionClassroomType = row[9],
                        CrossList = row[10]
                    }
                });
            }

        }


        /*-----------------------------------------------------------------------------------------------------------------*/


        [Microsoft.AspNetCore.Mvc.HttpPost]
        public Microsoft.AspNetCore.Mvc.ActionResult SaveSection(CourseInfo model)
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
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public Microsoft.AspNetCore.Mvc.ActionResult DeleteSection(CourseInfo model)
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

        public Microsoft.AspNetCore.Mvc.ActionResult GetCourses(string id)
        {
            List<System.Web.Mvc.SelectListItem> items = new List<System.Web.Mvc.SelectListItem>();
            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24)
                + "/Data/Courses/";
            string[] list = Directory.GetFiles(filepath);
            int pos = filepath.LastIndexOf("/") + 1;
            int counter = 0;
            foreach (string s in list)
            {
                if (s.Contains(id))
                {
                    pos = s.LastIndexOf("_") + 1;
                    items.Add(new System.Web.Mvc.SelectListItem() { Text = s.Substring(pos, s.Length - pos-4), Value = counter.ToString() });
                    counter++;
                }
            }

            //return new System.Web.Mvc.JsonResult() {Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };   



            return Json(new
            {
                Success = "true",
                Data = new
                {
                    items
                },
                JsonRequestBehavior.AllowGet
            });
        }

        public Microsoft.AspNetCore.Mvc.ActionResult GetSections(string id)
        {
            List<System.Web.Mvc.SelectListItem> items = new List<System.Web.Mvc.SelectListItem>();
            string[] parseID = id.Split("?");
            //parseID[0] is the semester, parseID[1] is the Subject and courseID "Subj_CourseID"
            var filepath = ApplicationBasePath.ToString().Substring(0, ApplicationBasePath.ToString().Length - 24)
                + "/Data/Schedules/" + parseID[0] + "/";
            string[] list = Directory.GetFiles(filepath);
            int pos = filepath.LastIndexOf("/") + 1;
            int counter = 0;
            foreach (string s in list)
            {
                if (s.Contains(parseID[1]))
                {
                    pos = s.LastIndexOf("_") + 1;
                    items.Add(new System.Web.Mvc.SelectListItem() { Text = s.Substring(pos, s.Length - pos - 4), Value = counter.ToString() });
                    counter++;
                }
            }

            //return new System.Web.Mvc.JsonResult() {Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };   



            return Json(new
            {
                Success = "true",
                Data = new
                {
                    items
                },
                JsonRequestBehavior.AllowGet
            });
        }

    }
}
