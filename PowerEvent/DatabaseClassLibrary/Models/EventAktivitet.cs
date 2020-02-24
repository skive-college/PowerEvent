using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseClassLibrary.Models
{
    public class EventAktivitet
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int AktivitetId { get; set; }
    }
}
