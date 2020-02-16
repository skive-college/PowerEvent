using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerEvent.Models
{
    public class EventAktivitetHold
    {
        public int Id { get; set; }

        public Aktivitet EventAktivitet { get; set; }

        public int? Point { get; set; }

        public int HoldOrder { get; set; }

        public List<EventAktivitetHoldScore> HoldScores { get; set; }

    }
}
