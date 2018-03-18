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
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }

        [Required]
        [Display(Name = "Course Subject")]
        public string CourseSubject { get; set; }

        [Required]
        [Display(Name = "Course ID")]
        public string CourseID { get; set; }

        [Required]
        [Display(Name = "Course Section")]
        public string CourseSection;

        [Required]
        [Display(Name = "Campus Name")]
        public string CampusLocation { get; set; }

        [Display(Name = "Crosslist")]
        public string CrossList_With { get; set; }


        //Since no math is required for this and it is only for a visual representation, 
        //Credits are being made into a string so nop converstions are requried
        [Required]
        [Display(Name = "Credits")]
        public string Credits { get; set; }





        //[Required]
        //[Display(Name ="ClassSize")]
        //public string ClassSize { get; set; }

        //public List<Section>
        public List<CourseInstructorModel> ListOfInstructors { get; set; }
        public List<CourseCampusLocation> CampusNames { get; set; }
        public List<ClassroomSize> ClassroomStudentSize { get; set; }
        public List<ListOfCourses> CourseList { get; set; }
        public List<ListOfSemesters> Semester { get; set; }
        public List<CourseSubjectModel> Subject { get; set; }
        public List<CourseScheduleTypeList> ScheduleType { get; set; }
        public List<CourseIDModel>CourseIDList { get; set; }

        public CourseInfo()
        {
            CourseIDList = new List<CourseIDModel>();
            ScheduleType = new List<CourseScheduleTypeList>();
            Semester = new List<ListOfSemesters>();
            Subject = new List<CourseSubjectModel>();
            ListOfInstructors = new List<CourseInstructorModel>();
            ClassroomStudentSize = new List<ClassroomSize>();
            CampusNames = new List<CourseCampusLocation>();
            CourseList = new List<ListOfCourses>();
        }

    }
}
