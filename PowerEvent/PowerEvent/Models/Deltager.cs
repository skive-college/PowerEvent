using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerEvent.Models
{
    public class Deltager
    {
        public int Id { get; set; }

        public string Navn { get; set; }

        public int HoldId { get; set; }

        public int EventId { get; set; }

        public List<DeltagerScore> ScoreList { get; set; }


        public int AvgScore 
        {
            get 
            {
                int retur = 0;
                for (int i = 0; i < ScoreList.Count; i++)
                {
                    if (ScoreList[i].Score != null)
                    {
                        retur += (int)ScoreList[i].Score;
                    }
                }
                retur /= ScoreList.Count;
                return retur;
            } 
        }


    }
}
