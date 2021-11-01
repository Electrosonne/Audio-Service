using Client.Logic;
using SharedData;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для ForumPage.xaml
    /// </summary>
    public partial class ForumPage : UserControl
    {
        public MusicManager m_MusicManager;
        public UserProfile m_UserProfile;

        public ForumPage(MusicManager MusicManager, UserProfile UserProfile)
        {
            InitializeComponent();
            m_MusicManager = MusicManager;
            m_UserProfile = UserProfile;

            Binding binding = new Binding();
            binding.Source = m_MusicManager.Forum;
            binding.Path = new PropertyPath("Data");
            CommentaryList.SetBinding(ListBox.ItemsSourceProperty, binding);
        }

        private void CommentaryClick(object sender, RoutedEventArgs e)
        {
            if (m_UserProfile.User == null)
            {
                MessageBox.Show("Please, Logining");
                return;
            }

            string text = new TextRange(UsersCommentary.Document.ContentStart, UsersCommentary.Document.ContentEnd).Text;

            if (text == "")
            {
                return;
            }

            int prev = -1;

            if (AnswerAppeal != null && text.Substring(0, AnswerAppeal.Length).Equals(AnswerAppeal))
            {
                prev = IdAppeal;
            }

            if (m_MusicManager.ActiveMusic is OnlineMusic)
            {
                if (m_MusicManager.Forum.AddCommentary(text,
                m_UserProfile.User.Login, m_MusicManager.ID,
                prev, SelectedIdCollection))
                {
                    UsersCommentary.Document = new FlowDocument();
                }
            }                
        }

        string AnswerAppeal { get; set; }
        int IdAppeal { get; set; }
        int SelectedIdCollection { get; set; }

        private void ReplyCommentaryClick(object sender, RoutedEventArgs e)
        {
            var forum = ((Forum)((Button)sender).DataContext);
            SelectedIdCollection = m_MusicManager.Forum.Data.IndexOf(forum);
            IdAppeal = forum.Id;
            AnswerAppeal = forum.User + ",";

            var document = new FlowDocument();
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run(AnswerAppeal));

            document.Blocks.Add(paragraph);
            UsersCommentary.Document = document;
        }
    }

    public class ThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new Thickness((int)value, 0, -(int)value, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
