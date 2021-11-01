using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Client.Logic
{ 
    public class Playing
    {
        private MediaPlayer m_MediaPlayer = new MediaPlayer();
        private Music m_Music;

        public event EventHandler MediaOpened;

        public double Volume
        {
            get
            {
                return m_MediaPlayer.Volume;
            }
            set
            {
                m_MediaPlayer.Volume = value;
            }
        }

        public TimeSpan Position => m_MediaPlayer.Position;

        public long MaximumTicks => m_Music.MaximumTicks;

        public Playing()
        {

        }

        public void SetPos(long pos)
        {
            m_MediaPlayer.Position = new TimeSpan(pos);
        }

        public void Open(Music music, double volume)
        {
            m_Music = music;

            m_MediaPlayer.Close();

            if (!File.Exists(music.Uri?.LocalPath))
            {
                music.Uri = null;
            }

            if (music.Uri == null && music is OnlineMusic)
            {
                m_Music.Uri = QueryMaster.GetMusicUrl(m_Music.Id);
            }

            m_MediaPlayer.Open(m_Music.Uri);
            m_MediaPlayer.Play();
            m_MediaPlayer.Volume = volume;
        }

        public void Play()
        {
            m_MediaPlayer.Play();
        }

        public void Pause()
        {
            m_MediaPlayer.Pause();
        }

        public void OpenedEvent(EventHandler handler)
        {
            m_MediaPlayer.MediaOpened += handler;
        }
    }
}
