using System;
namespace JAGS.Models
{
    public class LoginModel
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public bool IsValid(string _login, string _password)
        {
            Console.Write("We have reached IsValid");
            if (_login == "test" && _password == "test")
            {
                Console.Write("Returned True");
                return true;

            }
            else
            {
                Console.Write("Returned False");
                return false;

            }

        }
    }
}