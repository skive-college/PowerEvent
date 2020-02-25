using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseClassLibrary.Models
{
    public class Bruger
    {
        public int Id { get; set; }

        public string Brugernavn { get; set; }

        public string Kodeord { get; set; }

        public int AdminType { get; set; }
    }
}
