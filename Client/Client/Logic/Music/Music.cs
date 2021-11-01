using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Client.Logic
{
    public class Music
    {
        private Uri m_Uri;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string Duration { get; set; }
        public int TagSize { get; set; }
        public long MaximumTicks { get; set; }
        public int FrameRate { get; set; }
        public int SampleRate { get; set; }
        public virtual string Image { get; set; }        

        public Uri Uri
        {
            get
            {
                return m_Uri;
            }
            set
            {
                m_Uri = value;
            }
        }

        

        public Music()
        {

        }
    }
}
