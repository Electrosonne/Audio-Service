using Client.Logic;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для UploadPage.xaml
    /// </summary>
    public partial class UploadPage : UserControl
    {
        public MusicManager m_MusicManager;

        public UploadPage(MusicManager MusicManager)
        {
            InitializeComponent();
            m_MusicManager = MusicManager;

            Binding binding = new Binding();
            binding.Source = m_MusicManager;
            binding.Path = new PropertyPath("Image");
            binding.Mode = BindingMode.OneWay;
            UploadMusicImage.SetBinding(Image.SourceProperty, binding);

            binding = new Binding();
            binding.Source = m_MusicManager;
            binding.Path = new PropertyPath("Author");
            binding.Mode = BindingMode.OneWay;
            UploadMusicAuthor.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding();
            binding.Source = m_MusicManager;
            binding.Path = new PropertyPath("Name");
            binding.Mode = BindingMode.OneWay;
            UploadMusicName.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding();
            binding.Source = m_MusicManager;
            binding.Path = new PropertyPath("Description");
            binding.Mode = BindingMode.OneWay;
            UploadMusicDescr.SetBinding(TextBox.TextProperty, binding);
        }

        private void ButtonFileImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Audio files|*.png;*.jpg";

            if (dialog.ShowDialog() == true)
            {
                var bi = new BitmapImage(new Uri(dialog.FileName));
                bi.CacheOption = BitmapCacheOption.OnLoad;

                UploadMusicImage.Source = bi;
            }
        }

        private void UploadMusicClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int? id = m_MusicManager.ID;
                string name = UploadMusicName.Text;
                string author = UploadMusicAuthor.Text;
                string descr = UploadMusicDescr.Text;
                string text = new TextRange(UploadMusicText.Document.ContentStart, UploadMusicText.Document.ContentEnd).Text;
                byte[] dataImage = Utils.ImageToByte(UploadMusicImage.Source);
                byte[] data = System.IO.File.ReadAllBytes(m_MusicManager.ActiveMusic.Uri.LocalPath);

                var music = new SharedData.Music(name, author, descr, text, data, dataImage, id);
                QueryMaster.UploadMusic(music);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка загрузки");
            }
        }
    }
}
