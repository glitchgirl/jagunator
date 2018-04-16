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
        //[Required]
        [Display(Name = "Instructor Name")]
        public string InstructorName { get; set; }
        public List<SelectListItem> ListOfInstructors { get; set; }

        [Required(ErrorMessage = "*Required*")]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }


        [Required(ErrorMessage = "*Required*")]
        [Display(Name = "Course Subject")]
        public string CourseSubject { get; set; }
        public List<SelectListItem> SubjectList { get; set; }

        [Required(ErrorMessage = "*Required*")]
        [Display(Name = "Course ID")]
        public string CourseID { get; set; }
        public string CourseIDDummy { get; set; }
        public List<SelectListItem> CourseList { get; set; }

        [Required(ErrorMessage = "*Required*")]
        [Display(Name ="Credit Hours")]
        public string CreditHours { get; set; }
        public List<SelectListItem> CourseCreditList { get; set; }

        //[Required]
        [Display(Name = "Course Section")]
        public string CourseSection { get; set; }
        public List<SelectListItem> SectionList { get; set; }

        //[Required]
        [Display(Name = "Campus Name")]
        public string CampusLocation { get; set; }
        public List<SelectListItem> CampusNames { get; set; }

        [Display(Name = "Course Semester")]
        public string sectionSemester { get; set; }
        public List<SelectListItem> Semester { get; set; }

        [Display(Name ="Schedule Type")]
        public string ScheduleAtt { get; set; }
        public List<SelectListItem> ScheduleType { get; set; }

        [Display(Name ="Cross-List")]
        public string CrossListWith { get; set; }

        [Display(Name ="Classroom Size")]
        public string ClassroomSize { get; set; }
        public List<SelectListItem> ClassroomSizeList { get; set; }

        [Display(Name = "Classroom Type")]
        public string ClassroomType { get; set; }
        public List<SelectListItem> ClassroomTypeList { get; set; }

        //[Display(Name = "Crosslist")]
        //public string CrossList_With { get; set; }


        //Since no math is required for this and it is only for a visual representation, 
        //Credits are being made into a string so nop converstions are requried

        public List<SelectListItem> Subject { get; set; }
        public List<SelectListItem> CourseSectionList { get; set; }

        public CourseInfo()
        {
            CourseName = "";
            ClassroomSize = "";
            ClassroomType = "";
            CrossListWith = "";
            sectionSemester = "";
            CampusLocation = "";
            CourseSection = "";
            CreditHours = "";
            CourseSubject = "";
            InstructorName = "";

            CourseCreditList = new List<SelectListItem>();
            CourseList = new List<SelectListItem> ();
            ScheduleType = new List<SelectListItem>();
            Semester = new List<SelectListItem>();
            Subject = new List<SelectListItem>();
            ListOfInstructors = new List<SelectListItem>();
            ClassroomSizeList = new List<SelectListItem>();
            ClassroomTypeList = new List<SelectListItem>();
            CampusNames = new List<SelectListItem>();
            CourseSectionList = new List<SelectListItem>();
        }

    }
}
