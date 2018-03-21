using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace JAGS.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    //[Serializable]
    public class UserModel
    {
        public List<string> userlist { get; set; }
        public string selecteduser { get; set; }

        [Required(ErrorMessage = "Login is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public int Type { get; set; }

        public bool IsValid(string _Username, string _Password)
        {
            if (_Username != null && _Password != null)
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


    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class UserModelExtensions
    {
        public static IApplicationBuilder UseMiddlewareClassTemplate(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserModel>();
        }
    }
}
