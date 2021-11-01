using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Client.Logic;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using SharedData;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MusicManager m_MusicManager;
        UserProfile m_UserProfile;
        Header m_Header;

        public MainWindow()
        {
            InitializeComponent();
           
            m_MusicManager = new MusicManager();
            m_UserProfile = new UserProfile();

            m_Header = new Header(m_MusicManager, m_UserProfile);
            var autho = new AuthorizationPage(m_MusicManager, m_UserProfile);

            Header.Children.Add(m_Header);
            Player.Children.Add(new Player(this, m_MusicManager));
            MusicPage.Children.Add(new MusicPage(m_MusicManager));
            PlaylistPage.Children.Add(new PlaylistPage(m_MusicManager));
            NewPlaylistPage.Children.Add(new NewPlaylistPage(m_MusicManager));
            ForumPage.Children.Add(new ForumPage(m_MusicManager, m_UserProfile));
            InterpretationPage.Children.Add(new InterpretationPage(m_MusicManager, m_UserProfile));
            UploadPage.Children.Add(new UploadPage(m_MusicManager));
            RegistrationPage.Children.Add(new RegistrationPage(m_MusicManager, m_UserProfile));            
            AuthorizationPage.Children.Add(autho);

            autho.ToRegistRedirectEvent += RegistrationRedirect;
            m_Header.ToAuthorizRedirectEvent += AuthorizationRedirect;

            m_Header.ButtonConnect_Click(null, null);        
        }   

        public void HeaderCollapse(Visibility visibility)
        {
            m_Header.Collapse(visibility);
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            finally { }            
        }
        
        private void RegistrationRedirect()
        {
            TabMenu.SelectedValue = Registration;
        }

        private void AuthorizationRedirect()
        {
            TabMenu.SelectedValue = Authorization;
        }
    }
}
