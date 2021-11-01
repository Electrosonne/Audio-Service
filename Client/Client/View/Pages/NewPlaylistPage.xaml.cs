using Client.Logic;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для NewPlaylistPage.xaml
    /// </summary>
    public partial class NewPlaylistPage : UserControl
    {
        public MusicManager m_MusicManager;

        public NewPlaylistPage(MusicManager MusicManager)
        {
            InitializeComponent();

            m_MusicManager = MusicManager;
        }

        private void AddPlaylistClick(object sender, RoutedEventArgs e)
        {
            string name = NewPlaylistName.Text;
            string author = NewPlaylistAuthor.Text;

            if (name.Length < 1 || author.Length < 1)
            {
                return;
            }

            var data = Utils.ImageToByte(NewPlaylistMusic.Source);
            var playlist = new SharedData.Playlist(0, name, author, data);

            m_MusicManager.Playlist.Add(playlist.ConvertShD());

            Task.Run(() =>
            {
                QueryMaster.AddPlaylist(playlist);
            });
        }

        private void ButtonFileImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Audio files|*.png;*.jpg";

            if (dialog.ShowDialog() == true)
            {
                var bi = new BitmapImage(new Uri(dialog.FileName));
                bi.CacheOption = BitmapCacheOption.OnLoad;

                NewPlaylistMusic.Source = bi;
            }
        }
    }
}
