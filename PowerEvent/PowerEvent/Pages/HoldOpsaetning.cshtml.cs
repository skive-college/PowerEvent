using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PowerEvent.Helpers;
using PowerEvent.Models;

namespace PowerEvent
{
    public class HoldOpsaetningModel : PageModel
    {
        public List<Event> EventList { get; set;
        }
        public List<Deltager> DeltagerList { get; set; }

        public List<Hold> HoldList { get; set; }


        public void OnGet()
        {
            EventList = DBAdapter.getEvent();
            DeltagerList = DBAdapter.getDeltagere(1);
            HoldList = DBAdapter.getHold();
        }

        public void OnPostCmdSubmitNavn()
        {

            OnGet();
        }
    }
}