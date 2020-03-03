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

        [BindProperty]
        public int DeltagerID { get; set; }

        [BindProperty]
        public int HoldID { get; set; }

        [TempData]
        public int SelectedEvent { get; set; }

        public List<Event> EventList { get; set; }

        public List<Deltager> DeltagerList { get; set; }

        public List<Hold> HoldList { get; set; }


        public void OnGet()
        {
            if (Request.Query.ContainsKey("Event"))
            {
                SelectedEvent = int.Parse(Request.Query["Event"]);
                DeltagerList = DBAdapter.getDeltagere(SelectedEvent);
                HoldList = DBAdapter.getHold();
            }
            else
            {
                if (TempData.Peek("SelectedEvent") != null)
                {
                    DeltagerList = DBAdapter.getDeltagere(SelectedEvent);
                    HoldList = DBAdapter.getHold();
                }
                else
                {
                    SelectedEvent = -1;
                }
            }
            
            EventList = DBAdapter.getEvent();
        }

        public void OnPostCmdSubmitDeltagerNavn()
        {
            TempData.Keep("SelectedEvent");
            DBAdapter.addDeltager(DeltagerNavn, SelectedEvent);
            OnGet();
        }

        public void OnPostCmdRemoveDeltager()
        {
            TempData.Keep("SelectedEvent");
            DBAdapter.deleteDeltager(DeltagerID);
            OnGet();
        }

        public void OnPostCmdAddDeltagerToHold()
        {
            TempData.Keep("SelectedEvent");
            DBAdapter.updateDeltager(DeltagerID, HoldID);
            OnGet();
        }
    }
}