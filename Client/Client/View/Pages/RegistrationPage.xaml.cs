using Client.Logic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : UserControl
    {
        public MusicManager m_MusicManager;
        public UserProfile m_UserProfile;

        public RegistrationPage(MusicManager MusicManager, UserProfile UserProfile)
        {
            InitializeComponent();
            m_MusicManager = MusicManager;
            m_UserProfile = UserProfile;

            Binding binding = new Binding();
            binding.Source = m_UserProfile;
            binding.Path = new PropertyPath("RegisErrorLogin");
            RegisLogErr.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding();
            binding.Source = m_UserProfile;
            binding.Path = new PropertyPath("RegisErrorPassword");
            RegisPassErr.SetBinding(TextBlock.TextProperty, binding);
        }

        private void RegistrationClick(object sender, RoutedEventArgs e)
        {
            m_UserProfile.Registration(RegisLogin.Text, RegisPassword1.Password, RegisPassword2.Password);
        }
    }
}
