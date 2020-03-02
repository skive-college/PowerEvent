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
    public class AdminSlutResultatModel : PageModel
    {
        [BindProperty]
        public int SelectedEvent { get; set; }

        public int Vis { get; set; }

        public string ValgtGuiElemement { get; set; }

        public List<Event> EventList { get; set; }

        public List<Hold> HoldList{ get; set; }

        public List<EventAktivitetHoldScore> ScoreList{ get; set; }
        
        public List<EventAktivitetHold> EventAktivitetHoldList { get; set; }

        public List<Aktivitet> AktivitetList { get; set; }

        public List<EventAktivitet> EventAktivitetList { get; set; }




        public void OnGet()
        {
            SelectedEvent = -1;
            Vis = 1;
            AktivitetList = new List<Aktivitet>();
            HoldList = new List<Hold>();
            ScoreList= new List<EventAktivitetHoldScore>();
            EventAktivitetHoldList = new List<EventAktivitetHold>();
            EventAktivitetList = new List<EventAktivitet>();
            EventList = DBAdapter.getEvent();
            checkScript();

            HoldList = DBAdapter.getHold(SelectedEvent);
            HoldList = DBAdapter.getHoldAktivitet(HoldList, SelectedEvent);
            HoldList = DBAdapter.getHoldAktivitetScores(HoldList, SelectedEvent);
            AktivitetList = DBAdapter.getAktivitet(SelectedEvent);
            EventAktivitetList = DBAdapter.getEventAktivitet(SelectedEvent);


            if (ValgtGuiElemement == "CmdVis")
            {
                Vis++;
            }
        }

        private void checkScript()
        {
            try
            {
                SelectedEvent = int.Parse(Request.Query["EventList"]);
            }
            catch
            {
            }

            if (SelectedEvent == -1)
            {
                loadTempDataEvent();
                if (SelectedEvent != -1)
                {

                }
            }

            ValgtGuiElemement = Request.Query["ValgtGuiElemement"];
            if (ValgtGuiElemement == "EventList")
            {
                if (SelectedEvent != -1)
                {
                    saveTempDataEvent();
                }
            }
        }


        private void saveTempDataEvent()
        {
            List<int> tempEventList = new List<int>();
            tempEventList.Add(SelectedEvent);
            TempData.Set("SelectedEventId", tempEventList);
        }

        private void loadTempDataEvent()
        {
            List<int> tempEventList = TempData.Peek<List<int>>("SelectedEventId");
            if (tempEventList != null)
            {
                SelectedEvent = tempEventList[0];
            }
        }

    }
}