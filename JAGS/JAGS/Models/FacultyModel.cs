using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace JAGS.Models
{
    public class FacultyModel
    {
        public List<string> facultylist { get; set; }
        public string selectedfac { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string Facultyfname { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string Facultylname { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Facultytitle { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public int Facultytype { get; set; }

        public bool IsValid(string _Facultyfname, string _Facultylname, string _Facultytitle, int _Facultytype)
        {
            if (_Facultyfname != null && _Facultylname != null && Facultytitle != null && Facultytype != null)
            {
                return true;
            }
            else
            {
                //[ErrorMessage = "Login or Password is incorrect"]
                return false;
            }

        }
    }
}
