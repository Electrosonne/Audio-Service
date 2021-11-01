using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Client.Logic
{
    public class Playlist
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Author { get; set; }

        public List<string> Songs { get; set; } = new List<string>();

        public Playlist()
        {

        }

        /// <summary>
        /// Загрузка с сервера
        /// </summary>
        public Playlist(int? id, string name, string pathImage, string author = "")
        {
            Id = id;
            Name = name;
            Author = author;

            Image = pathImage;
        }
    }
}
