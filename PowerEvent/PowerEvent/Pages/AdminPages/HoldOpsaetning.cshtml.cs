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
                    TempData.Keep("SelectedEvent");
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
            DBAdapter.addDeltager(DeltagerNavn, SelectedEvent);
            OnGet();
        }

        public void OnPostCmdRemoveDeltager()
        {
            DBAdapter.deleteDeltager(DeltagerID);
            OnGet();
        }

        public void OnPostCmdAddDeltagerToHold()
        {
            if(HoldID != -1)
            {
                DBAdapter.updateDeltager(DeltagerID, HoldID);
            }
            else
            {
                DBAdapter.updateDeltager(DeltagerID, null);
            }
            OnGet();
        }
    }
}