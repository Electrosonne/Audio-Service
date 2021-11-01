using Newtonsoft.Json;
using SharedData;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client.Logic
{
    public class UserProfile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private User m_User;

        public User User 
        {
            get
            {
                return m_User;
            }
            set
            {
                m_User = value;
                if(value != null)
                {
                    Greetings = "Hello, " + value.Login + "!";
                }                         
            }
        }

        private string m_Greetings;

        public string Greetings
        {
            get
            {
                return m_Greetings;
            }
            set
            {
                m_Greetings = value;
                OnPropertyChanged("Greetings");
            }
        }
        
        private string m_AuthoErrorLogin;

        public string AuthoErrorLogin
        {
            get
            {
                return m_AuthoErrorLogin;
            }
            set
            {
                m_AuthoErrorLogin = value;
                OnPropertyChanged("AuthoErrorLogin");
            }
        }

        private string m_AuthoErrorPassword;

        public string AuthoErrorPassword
        {
            get
            {
                return m_AuthoErrorPassword;
            }
            set
            {
                m_AuthoErrorPassword = value;
                OnPropertyChanged("AuthoErrorPassword");
            }
        }
        
        private string m_RegisErrorLogin;

        public string RegisErrorLogin
        {
            get
            {
                return m_RegisErrorLogin;
            }
            set
            {
                m_RegisErrorLogin = value;
                OnPropertyChanged("RegisErrorLogin");
            }
        }

        private string m_RegisErrorPassword;

        public string RegisErrorPassword
        {
            get
            {
                return m_RegisErrorPassword;
            }
            set
            {
                m_RegisErrorPassword = value;
                OnPropertyChanged("RegisErrorPassword");
            }
        }

        public bool IsAuthorized { get; set; } = false;

        public UserProfile()
        {
            try
            {              
                Greetings = "Hello!";
                AsyncInitialize();
            }
            catch
            {
            }          
        }

        private async void AsyncInitialize()
        {
            await Task.Run(() =>
            {
                try
                {
                    User = JsonConvert.DeserializeObject<User>(File.ReadAllText(ConfigurationMaster.AudioConfig));
                }
                catch
                {
                    User = null;
                }
            });
        }

        public void Authorization(string login, string password)
        {
            string result = QueryMaster.Authorization(login, password);
            
            switch(result)
            {
                case User.SuccessRequest:
                {                   
                    User = new User(login, password);
                    ChangeUserConfigAsync();
                    IsAuthorized = true;
                    
                    AuthoErrorLogin = "";
                    AuthoErrorPassword = "";                   
                    break;
                }
                case User.ErrLoginRequest:
                {
                    AuthoErrorPassword = "";
                    AuthoErrorLogin = User.ErrLoginReturn;
                    break;
                }
                case User.ErrPasswordRequest:
                {
                    AuthoErrorPassword = User.ErrPasswordReturn;
                    AuthoErrorLogin = "";
                    break;
                }
                default:
                {
                    AuthoErrorLogin = result;
                    AuthoErrorPassword = result;
                    break;
                }
            }
        }

        public void Registration(string login, string password1, string password2)
        {
            RegisErrorLogin = "";
            RegisErrorPassword = "";

            if (login.Length < 3 || login.Length > 20)
            {
                RegisErrorLogin = "Length Must Be 3-20";
                return;
            }

            if (password1.Length < 3 || password1.Length > 20)
            {
                RegisErrorPassword = "Length Must Be 3-20";
                return;
            }

            if(!password1.Equals(password2))
            {
                RegisErrorPassword = "Passwords Are Not Equals";
                return;
            }

            string pattern = "[A-Z]";
            if(!Regex.Match(password1, pattern).Success)
            {
                RegisErrorPassword = "Passwords Must Containc A-Z";
                return;
            }

            pattern = "[a-z]";
            if (!Regex.Match(password1, pattern).Success)
            {
                RegisErrorPassword = "Passwords Must Containc a-z";
                return;
            }

            pattern = "[0-9]";
            if (!Regex.Match(password1, pattern).Success)
            {
                RegisErrorPassword = "Passwords Must Containc 0-9";
                return;
            }

            string result = QueryMaster.Registration(login, password1);

            switch (result)
            {
                case User.SuccessRequest:
                    {
                        User = new User(login, password1);
                        ChangeUserConfigAsync();
                        IsAuthorized = true;
                        RegisErrorLogin = "";
                        RegisErrorPassword = "";        
                        break;
                    }
                case User.ErrLoginNoFreeRequest:
                    {
                        RegisErrorPassword = "";
                        RegisErrorLogin = User.ErrLoginNoFreeReturn;
                        break;
                    }
                default:
                    {
                        RegisErrorLogin = result;
                        RegisErrorPassword = result;
                        break;
                    }
            }
        }

        public async Task ChangeUserConfigAsync()
        {
            using FileStream createStream = File.Create(ConfigurationMaster.AudioConfig);
            await System.Text.Json.JsonSerializer.SerializeAsync(createStream, User);
        }
    }
}
