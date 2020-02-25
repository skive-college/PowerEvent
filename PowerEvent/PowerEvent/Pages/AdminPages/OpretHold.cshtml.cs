using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PowerEvent.Helpers;
using PowerEvent.Models;

namespace PowerEvent
{
    public class OpretHoldModel : PageModel
    {

        [BindProperty]
        public int SelectedHold { get; set; }

        [BindProperty]
        public int SelectedAktivitet { get; set; }

        [BindProperty]
        public int SelectedEventAktivitet { get; set; }

        [BindProperty]
        public int SelectedEvent { get; set; }

        public string TxtHold { get; set; }

        public string TxtFarve { get; set; }

        public string ValgtGuiElemement { get; set; }

        public List<Event> EventList { get; set; }

        public List<Hold> HoldList { get; set; }

        public List<EventAktivitet> EventAktivitetList { get; set; }


        private Aktivitet valgtAktivitet;

        public Aktivitet ValgtAktivitet
        {
            get
            {
                if (valgtAktivitet == null)
                {
                    if (EventAktivitetList.Count != 0)
                    {
                        if (HoldList.Count == 0)
                        {
                            if (SelectedAktivitet != -1)
                            {
                                return DBAdapter.getAktivitet(SelectedEvent).Where(i => i.Id == EventAktivitetList.Where(i => i.Id == SelectedEventAktivitet).FirstOrDefault().AktivitetId).FirstOrDefault();
                            }
                        }
                        else
                        {
                            if (SelectedAktivitet != -1)
                            {
                                //return HoldList.Where(i => i.Id == EventAktivitetList.Where(i => i.Id == SelectedEventAktivitet).FirstOrDefault().AktivitetId).FirstOrDefault();
                            }
                        }
                    }
                }
                return new Aktivitet();
            }
            set
            {
                valgtAktivitet = value;
            }
        }

        public void OnGet()
        {
            SelectedHold = -1;
            SelectedAktivitet = -1;
            SelectedEvent = -1;
            SelectedEventAktivitet = -1;
            HoldList = new List<Hold>();
            EventAktivitetList = new List<EventAktivitet>();
            EventList = DBAdapter.getEvent();
            loadHoldList();
            checkScript();

            if (SelectedEvent != -1)
            {
                HoldList = DBAdapter.getHold();
                EventAktivitetList = DBAdapter.getEventAktivitet(SelectedEvent);
            }

            if (ValgtGuiElemement == "CmdGemHold")
            {
                CmdSaveHold();
            }
            else if (ValgtGuiElemement == "CmdSletAktivitet")
            {
                CmdDeleteAktivitet();
            }
            else if (ValgtGuiElemement == "CmdAddEventAktivitet")
            {
                CmdAddEventAktivitet();
            }
            else if (ValgtGuiElemement == "CmdSletEventAktivitet")
            {
                CmdSletEventAktivitet();
            }
        }

        public void OnPost()
        {

        }

        public void CmdDeleteAktivitet()
        {
            if (SelectedAktivitet != -1)
            {
                DBAdapter.deleteAktivitet(SelectedAktivitet);
                loadHoldList();
                SelectedAktivitet = -1;
            }
        }

        public void CmdSaveHold()
        {
            if (TxtHold != "" && TxtFarve != "")
            {
                DBAdapter.addHold(TxtHold, TxtFarve);
                loadHoldList();
            }
        }

        public void CmdAddEventAktivitet()
        {
            if (SelectedAktivitet != -1 && SelectedEvent != -1)
            {
                DBAdapter.addEventAktivitet(SelectedEvent, SelectedAktivitet);
                loadHoldList();
                loadEventAktivitetList();
            }
        }
        public void CmdSletEventAktivitet()
        {
            if (SelectedEventAktivitet != -1)
            {
                DBAdapter.deleteEventAktivitet(SelectedEventAktivitet);
                loadHoldList();
                loadEventAktivitetList();
                SelectedEventAktivitet = -1;
            }
        }


        private void loadHoldList()
        {
            HoldList = DBAdapter.getHold();
        }

        private void loadEventAktivitetList()
        {
            //SKAL LAVES
            EventAktivitetList = DBAdapter.getEventAktivitet(SelectedEvent);
        }


        private void checkScript()
        {
            try
            {
                SelectedAktivitet = int.Parse(Request.Query["AktivitetList"]);
            }
            catch
            {
            }
            try
            {
                TxtHold = Request.Query["TxtHold"];
            }
            catch
            {
            }
            try
            {
                SelectedEvent = int.Parse(Request.Query["EventList"]);
            }
            catch
            {
            }
            try
            {
                SelectedEventAktivitet = int.Parse(Request.Query["EventAktivitetList"]);
            }
            catch
            {
            }


            ValgtGuiElemement = Request.Query["ValgtGuiElemement"];

            if (SelectedEvent == -1)
            {
                loadTempDataEvent();
                if (SelectedEvent != -1)
                {

                }
            }

            if (ValgtGuiElemement == "AktivitetList")
            {

            }
            else if (ValgtGuiElemement == "EventList")
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