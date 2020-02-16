using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseClassLibrary.Models
{
    public class EventAktivitetHoldScore
    {
        public int Id { get; set; }

        public int EventAktivitetHoldId { get; set; }

        public int HoldScore { get; set; }
    }
}
