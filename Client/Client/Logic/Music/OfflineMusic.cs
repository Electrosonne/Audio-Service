using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Client.Logic
{
    public class OfflineMusic : Music
    {
        private string m_Path;

        public override string Image
        {
            get
            {
                return ConfigurationMaster.LocalImage(Name, m_Path);
            }
            set
            {
                base.Image = value;
            }
        }

        /// <summary>
        /// Локальный файл
        /// </summary>
        public OfflineMusic(string path)
        {
            AsyncInitialize(path);
        }

        private async void AsyncInitialize(string path)
        {
            m_Path = path;

            TagLib.File musicData = TagLib.File.Create(path);

            Description = musicData.Tag.Description ??= "";
            Author = musicData.Tag.Artists.Length > 0 ? musicData.Tag.Artists[0] : "";
            Name = musicData.Tag.Title ??= "";

            var span = musicData.Properties.Duration;
            Duration = Utils.SpanToString(span);
            //FrameRate = musicData.Properties.AudioSampleRate;
            //SampleRate = musicData.Properties.AudioSampleRate;
            //TagSize = (int)musicData.InvariantStartPosition;
            MaximumTicks = span.Ticks;

            if (musicData.Tag.Pictures.Length > 0)
            {
                var image = Name;
                Image = ConfigurationMaster.CachedImage(image, musicData.Tag.Pictures[0].Data.Data);
            }
            else
            {
                Image = ConfigurationMaster.StandartImage;
            }

            Uri = new Uri(path);          
        }
    }
}
