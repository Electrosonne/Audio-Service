using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData
{
    public class Playlist
    {
        public Playlist()
        {

        }

        public Playlist(int id, string name, string author = "", byte[] data = null)
        {
            Id = id;
            Name = name;
            Author = author;
            DataImage = data;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public byte[] DataImage { get; set; }
    }
}
