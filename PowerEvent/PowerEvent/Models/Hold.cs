using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerEvent.Models
{
    public class Hold
    {
        public int Id { get; set; }

        public string Navn { get; set; }

        public string Farve { get; set; }

        public List<EventAktivitetHold> HoldAktiviteter { get; set; }

    }
}
