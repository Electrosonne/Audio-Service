using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Logic
{
    public class OnlineMusic : Music
    {
        public override string Image
        {
            get
            {
                return ConfigurationMaster.GetServerImage(Id);
            }
            set
            {
                base.Image = value;
            }
        }

        public OnlineMusic(int id, string name, string author, string descr,
               string text, string duration, int framerate, int samplerate, int tagsize, long maxtick)
        {
            Id = id;
            Name = name ??= "";
            Author = author ??= "";
            Description = descr ??= "";
            Text = text ??= "";
            Duration = duration ??= "";
            //FrameRate = framerate;
            //SampleRate = samplerate;
            //TagSize = tagsize;
            MaximumTicks = maxtick;
        }
    }
}
