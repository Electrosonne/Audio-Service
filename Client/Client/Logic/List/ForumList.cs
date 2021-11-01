using SharedData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Client.Logic
{
    public class ForumList
    {
        public List<int> ConvertPrevToPos { get; private set; }

        public ObservableCollection<Forum> Data { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ForumList()
        {
            ConvertPrevToPos = new List<int>();
            Data = new ObservableCollection<Forum>();
        }

        public void Update(int id)
        {
            ConvertPrevToPos = new List<int>();
            Data.Clear();
            SetData(id);
        }

        private void SetData(int id)
        {
            var data = QueryMaster.GetForum(id);

            if (data != null)
            {
                for (var i = 0; i < data.Count; i++)
                {
                    ConvertPrevToPos.Add(data[i].Id);

                    data[i].Deep = 0;
                    int prev = data[i].IdPrevious;
                    if (prev != data[i].Id)
                    {
                        data[i].Deep = Data[ConvertPrevToPos.IndexOf(prev)].Deep + 50;
                    }

                    Data.Add(data[i]);

                    if (prev != data[i].Id && prev + 1 != i)
                    {
                        Swap(i, ConvertPrevToPos.IndexOf(prev) + 1);
                    }
                }
            }
        }

        public bool AddCommentary(string text, string user, int music, int prev = -1, int placeInCollection = 0, int rate = 10)
        {
            int deep = 0;
            if (prev != -1)
            {
                deep = Data[placeInCollection].Deep + 50;
            }

            var com = new Forum(Data.Count, prev, deep, DateTime.Now, text, rate, user, music);

            var result = QueryMaster.AddCommentary(com);

            if (result)
            {
                Data.Add(com);
                ConvertPrevToPos.Add(Data.Count);

                if (Data.Count > 1)
                {
                    if(prev != - 1)
                    {
                        Swap(Data.Count - 1, placeInCollection + 1);
                    }                    
                }                
            }       
            
            return result;
        }

        public void Swap(int posDel, int newPos)
        {
            Data.Move(posDel, newPos);
            var value = ConvertPrevToPos[posDel];
            ConvertPrevToPos.RemoveAt(posDel);
            ConvertPrevToPos.Insert(newPos, value);
        }
    }
}
