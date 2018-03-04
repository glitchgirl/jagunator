using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace JAGS.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    //[Serializable]
    public class UserModel
    {
        public List<string> userlist { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Type { get; set; }
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
