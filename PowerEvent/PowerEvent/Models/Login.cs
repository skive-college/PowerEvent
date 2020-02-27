using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerEvent.Models
{
    public class Login
    {
        public int Id { get; set; }

        public string Brugernavn { get; set; }

        public string Kodeord { get; set; }

        public int AdminType { get; set; }

        public int? EventId { get; set; }

        public int? HoldId { get; set; }
    }
}
