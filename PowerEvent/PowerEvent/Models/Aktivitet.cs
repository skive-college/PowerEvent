using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerEvent.Models
{
    public class Aktivitet
    {
        public int Id { get; set; }

        public string Navn { get; set; }

        public int PointType { get; set; }

        public int HoldSport { get; set; }
    }
}
