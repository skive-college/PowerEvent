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
        [BindProperty]
        public string DeltagerNavn { get; set; }

        public int EventID { get; set; }

        public List<Event> EventList { get; set; }

        public List<Deltager> DeltagerList { get; set; }

        public List<Hold> HoldList { get; set; }


        public void OnGet()
        {
            EventID = 0;
            if(Request.Query.Count != 0)
            {
                EventID = int.Parse(Request.Query["Event"]);
                DeltagerList = DBAdapter.getDeltagere(EventID);
                HoldList = DBAdapter.getHold();
            }
            EventList = DBAdapter.getEvent();
            
        }

        public void OnPostCmdSubmitNavn()
        {
            DBAdapter.addDeltager(DeltagerNavn, 0, EventID);
            //OnGet();
        }
    }
}