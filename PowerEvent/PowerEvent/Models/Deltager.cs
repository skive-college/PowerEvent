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

        public int Score { get; set; }

    }
}
