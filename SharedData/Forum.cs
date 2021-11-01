using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData
{
    public class Forum
    {
        public int Id { get; set; }
        public int IdPrevious { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public int Rate { get; set; }
        public string User { get; set; }
        public int IdMusic { get; set; }
        public int Deep { get; set; }

        public Forum()
        {

        }

        public Forum(int id, int prev, int deep, DateTime date, string text, int rate, string user, int music)
        {
            Id = id;
            IdPrevious = prev;
            Deep = deep;
            Date = date;
            Text = text;
            Rate = rate;
            User = user;
            IdMusic = music;
        }

    }
}
