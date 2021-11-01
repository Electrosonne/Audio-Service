using System;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SharedData;
using System.Windows;
using System.Threading;

namespace Client.Logic
{
    public class MusicManager : INotifyPropertyChanged
    {
        private const int m_DelayTime = 500;
        private Playing m_Playing = new Playing();
        private string m_Image;        
        private string m_Description;
        private string m_Author;
        private string m_Name;
        private string m_MusicTime;
        private string m_CurrentTime;
        private string m_Words;
        private string m_Search;
        private long m_Maximum;
        private double m_CurrentTicks;
        public double m_Volume;
        private PackIconKind m_KindPlay;
        private PackIconKind m_KindRepeat;

        public int ID { get; set; }

        private bool m_IsPaused = true;

        public bool IsRepeat = false;

        public Music ActiveMusic = null;

        public string Image
        {
            get
            {
                return m_Image;
            }
            set
            {
                m_Image = value;
                OnPropertyChanged("Image");
            }
        }

        public string Description
        {
            get
            {
                return m_Description;
            }
            set
            {
                m_Description = value;
                OnPropertyChanged("Description");
            }
        }
        
        public string Author
        {
            get
            {
                return m_Author;
            }
            set
            {
                m_Author = value;
                OnPropertyChanged("Author");
            }
        }

        public string Words
        {
            get
            {
                return m_Words;
            }
            set
            {
                m_Words = value;
                Interpretation.SongWords = value;
                NewWordsEvent.Invoke(value);
            }
        }

        public string Search
        {
            get
            {
                return m_Search;
            }
            set
            {
                m_Search = value;
                OnPropertyChanged("Search");
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string MusicTime
        {
            get
            {
                return m_MusicTime;
            }
            set
            {
                m_MusicTime = value;
                OnPropertyChanged("MusicTime");
            }
        }

        public string CurrentTime
        {
            get
            {
                return m_CurrentTime;
            }
            set
            {
                m_CurrentTime = value;
                OnPropertyChanged("CurrentTime");
            }
        }

        public long Maximum
        {
            get
            {
                return m_Maximum;
            }
            set
            {
                m_Maximum = value;
                OnPropertyChanged("Maximum");
            }
        }

        public double CurrentTicks
        {
            get
            {
                return m_CurrentTicks;
            }
            set
            {
                m_CurrentTicks = value;
                NewPositionEvent.Invoke(value);                
            }
        }

        public double Volume
        {
            get
            {
                return m_Volume;
            }
            set
            {
                m_Volume = value;
                m_Playing.Volume = value;
                OnPropertyChanged("Volume");
            }
        }

        public PackIconKind KindPlay
        {
            get
            {
                return m_KindPlay;
            }
            set
            {
                m_KindPlay = value;
                OnPropertyChanged("KindPlay");
            }
        }

        public PackIconKind KindRepeat
        {
            get
            {
                return m_KindRepeat;
            }
            set
            {
                m_KindRepeat = value;
                OnPropertyChanged("KindRepeat");
            }
        }

        public MusicList Music {get;set;}
        public PlaylistList Playlist { get; set; }
        public ForumList Forum { get; set; }
        public InterprList Interpretation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public delegate void NewTimePosition(double pos);
        public event NewTimePosition NewPositionEvent;

        public delegate void NewWords(string words);
        public event NewWords NewWordsEvent;

        private bool m_UpTimeToken;


        public MusicManager()
        {
            string imagePng = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString()}\\Items\\nice.jpg";
            Image = imagePng;
            KindPlay = PackIconKind.Play;
            KindRepeat = PackIconKind.RepeatOff;
            Volume = 0.1;            
            Maximum = 100;
            MusicTime = "0:00";
            CurrentTime = "0:00";
            Description = "";
            Author = "";
            Name = "";
            Search = "";

            Music = new MusicList();
            Playlist = new PlaylistList();
            Forum = new ForumList();
            Interpretation = new InterprList();
            m_UpTimeToken = true;
        }

        public void Play()
        {
            m_IsPaused = !m_IsPaused;

            if (!m_IsPaused)
            {
                m_Playing.Play();
                KindPlay = PackIconKind.Pause;

                if(m_UpTimeToken == false)
                {
                    //Контроль, чтобы асинхронный метод исполнялся только в одном потоке
                    Thread.Sleep(m_DelayTime);
                }

                m_UpTimeToken = true;
                RunPeriodicSave();
            }
            else
            {
                m_Playing.Pause();
                KindPlay = PackIconKind.Play;
                m_UpTimeToken = false;
            }            
        }

        /// <summary>
        /// Асинхронный метод, обновляющий ползунок и текущее проигрываемое время
        /// </summary>
        async void RunPeriodicSave()
        {
            while (m_UpTimeToken)
            {
                CurrentTime = Utils.SpanToString(m_Playing.Position);
                CurrentTicks = m_Playing.Position.Ticks;

                try 
                { 
                    if (m_Playing.Position.Ticks == m_Playing.MaximumTicks)
                    {
                        if (IsRepeat)
                        {
                            NewCompositionSelected(ActiveMusic);
                        }
                        else
                        {
                            PlayNext();
                        }
                    }
                }
                catch { }
                finally
                {
                    await Task.Delay(m_DelayTime);
                }        
            }
        }

        public void NewVolume(double volume)
        {
            Volume = volume;
        }

        public void NewPosition(double newTick)
        {
            m_Playing.SetPos((long)newTick);
            CurrentTicks = newTick;
        }

        public void PlayNext()
        {
            if(ActiveMusic != null)
            {
                var now = Music.Data.IndexOf(ActiveMusic);
                now++;

                if (now == Music.Data.Count)
                {
                    now = 0;
                }

                NewCompositionSelected(Music.Data[now]);
            }
        }

        public void PlayPrevious()
        {
            if (ActiveMusic != null)
            {
                var now = Music.Data.IndexOf(ActiveMusic);
                now--;

                if (now == -1)
                {
                    now = Music.Data.Count - 1;
                }

                NewCompositionSelected(Music.Data[now]);
            }
        }

        public void PlayRepeat()
        {
            IsRepeat = !IsRepeat;

            if (IsRepeat)
            {
                KindRepeat = PackIconKind.Repeat;
            }
            else
            {
                KindRepeat = PackIconKind.RepeatOff;
            }           
        }

        Random Random = new Random();

        public void PlayRandom()
        {
            int n = Music.Data.Count;

            while (n > 1)
            {
                n--;
                int k = Random.Next(n + 1);
                Music.Data.Move(k, n);
                Music.Data.Move(n - 1, k);
            }

            if(n > 0)
            {
                NewCompositionSelected(Music.Data[0]);
            }            
        }    

        public void OpenMusicFile(string path)
        {
            NewCompositionSelected(Music.AddNewComposition(path));
        }

        public void NewCompositionSelected(Music music)
        {
            m_UpTimeToken = false;

            CurrentTicks = 0;
            m_IsPaused = false;
            KindPlay = PackIconKind.Pause;

            ActiveMusic = music;

            Image = music.Image;
            Author = music.Author;
            Description =  music.Description;
            Name = music.Name;
            Words = music.Name + "\n\r" + music.Text;
            ID = music.Id;
            Maximum = music.MaximumTicks;
            MusicTime = music.Duration;

            m_Playing.Open(music, Volume);
            m_Playing.OpenedEvent(MediaPlayer_MediaOpened);

            if (music is OnlineMusic)
            {
                Forum.Update(ID);
                Interpretation.Update(ID);
            }    
        }

        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            m_UpTimeToken = true;
            RunPeriodicSave();
        }

        public void LoadContentFromServer()
        {
            Music.LoadMore();
            Playlist.Update();
        }
    }
}
