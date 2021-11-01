using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData
{
    public class User
    {
        public const string SuccessRequest = "\"Success\"";
        public const string SuccessReturn = "Success";

        public const string ErrPasswordRequest = "\"Wrong Password\"";
        public const string ErrPasswordReturn = "Wrong Password";

        public const string ErrLoginRequest = "\"Wrong Login\"";
        public const string ErrLoginReturn = "Wrong Login";

        public const string ErrLoginNoFreeRequest = "\"Login No Free\"";
        public const string ErrLoginNoFreeReturn = "Login No Free";

        public string Login { get; set; }
        public string Password { get; set; }

        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
