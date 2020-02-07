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

        // 0 = holdsport, 1 = deltagersport
        public int HoldSport { get; set; }
    }
}
