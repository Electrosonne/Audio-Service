using Client.Logic;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {
        MainWindow m_MainWindow;
        MusicManager m_MusicManager;
        double m_LastPosByManager = 0;
        //Блокирует обновление позиции курсора при работе пользователя с ползунком
        bool m_LockTimerSlider = true;
        bool VolumeSwitch = true;

        public Player(MainWindow main, MusicManager MusicManager)
        {
            m_MainWindow = main;

            InitializeComponent();
            m_MusicManager = MusicManager;

            Binding binding = new Binding();
            binding.Source = m_MusicManager;
            binding.Path = new PropertyPath("Image");
            binding.Converter = new ImageSourceConverter();
            MusicPng.SetBinding(Image.SourceProperty, binding);

            binding = new Binding();
            binding.Source = m_MusicManager;
            binding.Path = new PropertyPath("Author");
            TextAuthor.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding();
            binding.Source = m_MusicManager;
            binding.Path = new PropertyPath("Name");
            TextSongName.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding();
            binding.Source = m_MusicManager;
            binding.Path = new PropertyPath("MusicTime");
            TextMusicTime.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding();
            binding.Source = m_MusicManager;
            binding.Path = new PropertyPath("CurrentTime");
            TextCurrentTime.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding();
            binding.Source = m_MusicManager;
            binding.Path = new PropertyPath("Maximum");
            TimerSlider.SetBinding(Slider.MaximumProperty, binding);

            binding = new Binding();
            binding.Source = m_MusicManager;
            binding.Path = new PropertyPath("KindPlay");
            ButtonPlayDesign.SetBinding(PackIcon.KindProperty, binding);

            binding = new Binding();
            binding.Source = m_MusicManager;
            binding.Path = new PropertyPath("KindRepeat");
            ButtonRepeatDesign.SetBinding(PackIcon.KindProperty, binding);

            m_MusicManager.NewPositionEvent += M_MusicManager_Notify;
        }

        private void M_MusicManager_Notify(double pos)
        {
            if (m_LockTimerSlider)
            {
                m_LastPosByManager = pos;
                TimerSlider.Value = pos;
            }
        }

        private void TimeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (m_LastPosByManager != e.NewValue)
            {
                m_LockTimerSlider = false;

                m_MusicManager.NewPosition(TimerSlider.Value);

                m_LockTimerSlider = true;
            }
        }

        private void ButtonVolumeSwitch_Click(object sender, RoutedEventArgs e)
        {
            VolumeSwitch = !VolumeSwitch;

            if (!VolumeSwitch)
            {
                ButtonVolumeDesign.Kind = PackIconKind.AudioOff;
                m_MusicManager.Volume = 0;
            }
            else
            {
                ButtonVolumeDesign.Kind = PackIconKind.Audio;
                m_MusicManager.Volume = VolumeSlider.Value / 100;
            }
        }

        private void Volume_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VolumeSwitch = true;

            m_MusicManager?.NewVolume(e.NewValue / 100);
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            m_MusicManager.Play();
        }

        private void ButtonRandom_Click(object sender, RoutedEventArgs e)
        {
            m_MusicManager.PlayRandom();
        }

        private void ButtonPrev_Click(object sender, RoutedEventArgs e)
        {
            m_MusicManager.PlayPrevious();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            m_MusicManager.PlayNext();
        }

        private void ButtonRepeat_Click(object sender, RoutedEventArgs e)
        {
            m_MusicManager.PlayRepeat();
        }

        private void ButtonFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Audio files|*.mp3";
            dialog.FilterIndex = 2;

            if (dialog.ShowDialog() == true)
            {
                m_MusicManager.OpenMusicFile(dialog.FileName);
            }
        }

        private bool IsCollapsed { get; set; } = false;
        private double LastWidth { get; set; } = 0;

        private void ButtonCollapse_Click(object sender, RoutedEventArgs e)
        {
            if(IsCollapsed)
            {
                m_MainWindow.HeaderCollapse(Visibility.Visible);

                var widthAnimation = new DoubleAnimation
                {
                    From = m_MainWindow.Width,
                    To = LastWidth,
                    Duration = TimeSpan.FromSeconds(1)
                };

                m_MainWindow.BeginAnimation(WidthProperty, widthAnimation);
            }
            else
            {
                m_MainWindow.HeaderCollapse(Visibility.Hidden);

                var widthAnimation = new DoubleAnimation
                {
                    From = m_MainWindow.Width,
                    To = this.ActualWidth,
                    Duration = TimeSpan.FromSeconds(1)
                };

                m_MainWindow.BeginAnimation(WidthProperty, widthAnimation);

                LastWidth = m_MainWindow.Width;          
            }
            IsCollapsed = !IsCollapsed;        
        }
    }

    public class ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var res = new BitmapImage(new Uri(value.ToString()));
            res.CacheOption = BitmapCacheOption.OnLoad;
            res.DecodePixelHeight = 200;
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
