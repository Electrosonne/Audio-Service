using Client.Logic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace Client
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : UserControl
    {
        public MusicManager m_MusicManager;
        public UserProfile m_UserProfile;

        public delegate void ToRegistRedirect();
        public event ToRegistRedirect ToRegistRedirectEvent;

        public AuthorizationPage(MusicManager MusicManager, UserProfile UserProfile)
        {
            InitializeComponent();
            m_MusicManager = MusicManager;
            m_UserProfile = UserProfile;

            Binding binding = new Binding();
            binding.Source = m_UserProfile;
            binding.Path = new PropertyPath("AuthoErrorLogin");
            AuthoLogErr.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding();
            binding.Source = m_UserProfile;
            binding.Path = new PropertyPath("AuthoErrorPassword");
            AuthoPassErr.SetBinding(TextBlock.TextProperty, binding);
        }

        private void AuthorizationClick(object sender, RoutedEventArgs e)
        {
            m_UserProfile.Authorization(AuthoLogin.Text, AuthoPassword.Password);
        }

        private void AuthoToRegistRedirect(object sender, RoutedEventArgs e)
        {
            ToRegistRedirectEvent.Invoke();
        }
    }
}
