﻿using System;
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

        [Required(ErrorMessage = "Name is required")]
        public string Facultyname { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Facultytitle { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public int Facultytype { get; set; }
    }
}
