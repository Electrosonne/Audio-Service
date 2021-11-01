using Client.Logic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для InterpretationPage.xaml
    /// </summary>
    public partial class InterpretationPage : UserControl
    {
        public MusicManager m_MusicManager;
        public UserProfile m_UserProfile;

        public InterpretationPage(MusicManager MusicManager, UserProfile UserProfile)
        {
            InitializeComponent();
            m_MusicManager = MusicManager;
            m_UserProfile = UserProfile;

            Binding binding = new Binding();
            binding.Source = m_MusicManager.Interpretation;
            binding.Path = new PropertyPath("Data");
            InterpretationList.SetBinding(ListBox.ItemsSourceProperty, binding);

            m_MusicManager.NewWordsEvent += M_MusicManager_NewWordsEvent;
        }

        private void M_MusicManager_NewWordsEvent(string words)
        {
            var document = new FlowDocument();
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Bold(new Run(words)));

            document.Blocks.Add(paragraph);
            SongText.Document = document;
        }

        private void InterprClick(object sender, RoutedEventArgs e)
        {
            if (m_UserProfile.User == null)
            {
                MessageBox.Show("Please, Logining");
                return;
            }

            string text = new TextRange(UsersInterpr.Document.ContentStart, UsersInterpr.Document.ContentEnd).Text;

            if (text == "")
            {
                return;
            }

            var start = m_MusicManager.Words.IndexOf(LyricsWordInterpr.Text);
            var end = start + LyricsWordInterpr.Text.Length;

            if (m_MusicManager.ActiveMusic is OnlineMusic)
            {
                if (m_MusicManager.Interpretation.AddInterpretation(start, end, text,
                m_UserProfile.User.Login, m_MusicManager.ID, SongText.Selection.Text))
                {
                    UsersInterpr.Document = new FlowDocument();
                }
            }
        }

        private void SelectionLyrics(object sender, RoutedEventArgs e)
        {
            LyricsWordInterpr.Text = SongText.Selection.Text;
        }
    }
}
