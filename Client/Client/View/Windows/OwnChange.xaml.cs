using Client.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client.View
{
    /// <summary>
    /// Логика взаимодействия для OwnChange.xaml
    /// </summary>
    public partial class OwnChange : Window
    {
        public MusicManager m_MusicManager;

        public OwnChange(MusicManager MusicManager)
        {
            InitializeComponent();

            m_MusicManager = MusicManager;

            Binding binding = new Binding();
            binding.Source = m_MusicManager.Playlist;
            binding.Path = new PropertyPath("Data");
            PlaylistsList.SetBinding(ListBox.ItemsSourceProperty, binding);
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
