using System;
namespace JAGS.Models
{
    public class LoginModel
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public bool IsValid(string _login, string _password)
        {
            if (_login == "test" && _password == "test")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}