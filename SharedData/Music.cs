namespace SharedData
{
    public class Music
    {
        public Music()
        {

        }

        public Music(int id, string name, string path, string author, string descr, 
            string text, string duration, int framerate, int samplerate, int tagsize, long maxtick)
        {
            Id = id;
            Name = name;
            Author = author;
            Description = descr;
            Text = text;
            Path = path;
            Duration = duration;
            FrameRate = framerate;
            SampleRate = samplerate;
            TagSize = tagsize;
            MaximumTicks = maxtick;
        }

        public Music(string name, string author, string descr, string text, byte[] data, byte[] dataImage, int? id = null, int rating = 0)
        {        
            Name = name;
            Author = author;
            Description = descr;
            Text = text;           
            Data = data;
            DataImage = dataImage;
            Id = id;
        }

        public int? Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string Path { get; set; }
        public string Duration { get; set; }       
        public int TagSize { get; set; }
        public long MaximumTicks { get; set; }
        public int FrameRate { get; set; }
        public int SampleRate { get; set; }
        public byte[] DataImage { get; set; }
        public byte[] Data { get; set; }
    }
}
