using System;
using System.ComponentModel.DataAnnotations;
namespace JAGS.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsValid(string _login, string _password)
        {
            if (_login != null && _password != null)
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