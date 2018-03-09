using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JAGS.Models
{
    public class CourseInfo
    {
        [Required]
        [Display(Name = "Instructor Name")]
        public string IntructorName { get; set; }

        [Required]
        [Display(Name ="Course Name")]
        public string CourseName { get; set; }

        [Required]
        [Display(Name ="Course ID")]
        public string CourseID { get; set; }

        [Required]
        [Display(Name ="Campus Name")]
        public string CampusLocation { get; set; }

        [Required]
        [Display(Name ="ClassSize")]
        public string ClassSize { get; set; }

        public List<CampusLocation> CampusNames { get; set; }
        public List<ClassroomSize> ClassroomStudentSize { get; set; }
        public List<ListOfCourses> CourseList { get; set; }
        //public List<String>Semester { get; set; }

        public CourseInfo()
        {
            ClassroomStudentSize = new List<ClassroomSize>();
            CampusNames = new List<CampusLocation>();
            CourseList = new List<ListOfCourses>();
            //Semester = new List<String>();
        }

    }
}
