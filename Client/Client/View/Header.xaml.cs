using Client.Logic;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для Header.xaml
    /// </summary>
    public partial class Header : UserControl
    {
        MusicManager m_MusicManager;
        UserProfile m_UserProfile;

        public delegate void ToAuthorizRedirect();
        public event ToAuthorizRedirect ToAuthorizRedirectEvent;

        public Header(MusicManager MusicManager, UserProfile UserProfile)
        {
            InitializeComponent();
            m_MusicManager = MusicManager;
            m_UserProfile = UserProfile;

            Binding binding = new Binding();
            binding.Source = m_UserProfile;
            binding.Path = new PropertyPath("Greetings");
            TextHello.SetBinding(TextBlock.TextProperty, binding);

            ConnectionStatus.Kind = PackIconKind.LanDisconnect;
            TextConnect.Text = "Offline Mode";
            QueryMaster.Connection = false;
        }

        public void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            if (QueryMaster.Connection)
            {
                return;
            }

            TryConnect();
        }

        public async void TryConnect()
        {
            bool result = false;

            await Task.Run(() =>
            {
                result = QueryMaster.SuccessConnect();

                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (result)
                    {
                        ConnectionStatus.Kind = PackIconKind.Connection;
                        TextConnect.Text = "Online Mode";
                        QueryMaster.Connection = true;
                        m_MusicManager.LoadContentFromServer();
                    }
                    else
                    {
                        ConnectionStatus.Kind = PackIconKind.LanDisconnect;
                        TextConnect.Text = "Offline Mode";
                        QueryMaster.Connection = false;
                    }
                });
            });            
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CacheClear_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationMaster.ClearCache();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            ToAuthorizRedirectEvent.Invoke();
        }

        private void WindowMinimaze_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        public void Collapse(Visibility visibility)
        {
            if (visibility.Equals(Visibility.Hidden))
            {
                TextHello.HorizontalAlignment = HorizontalAlignment.Left;
                Grid0.Width = new GridLength(0);
                Grid1.Width = new GridLength(0);
            }
            else
            {
                TextHello.HorizontalAlignment = HorizontalAlignment.Right;
                Grid0.Width = new GridLength(0, GridUnitType.Auto);
                Grid1.Width = new GridLength(0, GridUnitType.Auto);
            }
            ButtonConnect.Visibility = visibility;
            ButtonLogin.Visibility = visibility;
            ButtonCacheClear.Visibility = visibility;
            ConnectionStatus.Visibility = visibility;
            TextConnect.Visibility = visibility;
        }
    }
}
