using SharedData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;


namespace Client.Logic
{
    public class InterprList
    {
        public string SongWords { get; set; }

        public ObservableCollection<Interpretation> Data { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public InterprList()
        {
            Data = new ObservableCollection<Interpretation>();
        }

        public void Update(int id)
        {
            Data.Clear();
            SetData(id);
        }

        private void SetData(int id)
        {
            var data = QueryMaster.GetInterpretation(id);

            if (data != null)
            {
                foreach (var elem in data)
                {
                    var inter = new Interpretation(elem.Id, elem.Start, elem.End, elem.Words, elem.Date, elem.User, elem.IdMusic);
                    Data.Add(inter);
                    inter.WordsLoop = SongWords.Substring(elem.Start, elem.End - elem.Start);
                }
            }
        }

        public bool AddInterpretation(int start, int end, string words, string user, int music, string wordloop)
        {
            var com = new Interpretation(0, start, end, words, DateTime.Now, user, music, wordloop);

            var result = QueryMaster.AddInterpretation(com);

            if (result)
            {
                Data.Add(com);
            }

            return result;
        }
    }
}
