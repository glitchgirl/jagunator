using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JAGS.Models
{
    public class Section
    {
        [Required]
        [Display(Name = "Instructor Name")]
        public string IntructorName { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }

        [Required]
        [Display(Name = "Course ID")]
        public string CourseID { get; set; }

        [Required]
        [Display(Name ="Section")]
        public char SectionForCourse { get; set; }

        [Required]
        [Display(Name = "Campus Name")]
        public string CampusLocation { get; set; }


        public Section()
        {
            
        }
    }
}
