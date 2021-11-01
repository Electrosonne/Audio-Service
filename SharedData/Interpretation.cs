using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData
{
    public class Interpretation
    {
        public Interpretation()
        {

        }

        public Interpretation(int id, int start, int end, string words, DateTime date, string user, int music, string wordloop = "")
        {
            Id = id;
            Start = start;
            End = end;
            Words = words;
            Date = date;
            User = user;
            IdMusic = music;
            WordsLoop = wordloop;
        }

        public int Id { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string Words { get; set; }
        public DateTime Date { get; set; }
        public string User { get; set; }
        public int IdMusic { get; set; }
        public string WordsLoop { get; set; }
    }
}
