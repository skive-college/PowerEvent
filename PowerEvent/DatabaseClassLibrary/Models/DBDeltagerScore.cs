using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseClassLibrary.Models
{
    public class DBDeltagerScore
    {
        public int Id { get; set; }

        public int DeltagerId { get; set; }

        public int EventAktivitetId { get; set; }

        public int? Score { get; set; }
    }
}
