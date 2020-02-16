using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerEvent.Models
{
    public class HoldEventAktivitet
    {
        public int Id { get; set; }

        public Aktivitet Aktivitet { get; set; }

        public int? Point { get; set; }

        public int HoldOrder { get; set; }

        public List<HoldEventAktivitetScore> HoldScores { get; set; }

    }
}
