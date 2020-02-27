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
        public int SelectedHoldAktivitet { get; set; }

        [BindProperty]
        public int SelectedEventAktivitet { get; set; }

        [BindProperty]
        public int SelectedEvent { get; set; }

        public string TxtHold { get; set; }

        public string TxtFarve { get; set; }

        public string ValgtGuiElemement { get; set; }

        public List<Event> EventList { get; set; }

        public List<Hold> HoldList { get; set; }

        public List<Aktivitet> AktivitetList { get; set; }
        
        public List<Hold> HoldAktivitetList { get; set; }

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
                        if (AktivitetList.Count == 0)
                        {
                            if (SelectedEventAktivitet != -1)
                            {
                                return DBAdapter.getAktivitet(SelectedEvent).Where(i => i.Id == EventAktivitetList.Where(i => i.Id == SelectedEventAktivitet).FirstOrDefault().AktivitetId).FirstOrDefault();
                            }
                        }
                        else
                        {
                            if (SelectedEventAktivitet != -1)
                            {
                                return AktivitetList.Where(i => i.Id == EventAktivitetList.Where(i => i.Id == SelectedEventAktivitet).FirstOrDefault().AktivitetId).FirstOrDefault();
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
            SelectedHoldAktivitet = -1;
            SelectedEvent = -1;
            SelectedEventAktivitet = -1;
            HoldList = new List<Hold>();
            EventAktivitetList = new List<EventAktivitet>();
            EventList = DBAdapter.getEvent();
            loadHoldList();
            checkScript();
            


            if (SelectedEvent != -1)
            {
                loadEventAktivitetList();
                if (SelectedEventAktivitet != -1)
                {
                    loadHoldAktivitetList();
                }
                
            }

            if (ValgtGuiElemement == "CmdGemHold")
            {
                CmdSaveHold();
            }
            else if (ValgtGuiElemement == "CmdSletAktivitet")
            {
                CmdDeleteHold();
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

        public void CmdDeleteHold()
        {
            if (SelectedHold != -1)
            {
                DBAdapter.deleteHold(SelectedHold);
                loadHoldList();
                SelectedHold = -1;
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
            if (SelectedEventAktivitet != -1)
            {
                DBAdapter.addEventAktivitet(SelectedEvent, SelectedEventAktivitet);
                loadHoldAktivitetList();
            }
        }
        public void CmdSletEventAktivitet()
        {
            if (SelectedEventAktivitet != -1)
            {
                DBAdapter.deleteEventAktivitet(SelectedEventAktivitet);
                loadHoldAktivitetList();
                SelectedEventAktivitet = -1;
            }
        }


        private void loadHoldList()
        {
            HoldList = DBAdapter.getHold();
        }
        
        private void loadHoldAktivitetList()
        {
            HoldAktivitetList = DBAdapter.getHold(SelectedEvent);
            HoldAktivitetList = DBAdapter.getHoldAktivitet(HoldAktivitetList, SelectedEvent);

            HoldAktivitetList = HoldAktivitetList.Where(i => i.HoldAktiviteter.Where(i => i.EventAktivitetId == SelectedEventAktivitet).FirstOrDefault() != null).ToList();
            //List<Hold> tempHoldList = new List<Hold>();
            //foreach (var item in HoldAktivitetList)
            //{
            //    if (item.HoldAktiviteter.Where(i => i.EventAktivitetId == SelectedEventAktivitet).FirstOrDefault() != null)
            //    {
            //        tempHoldList.Add(item);
            //    }
            //}
            //HoldAktivitetList = tempHoldList;
        }

        private void loadEventAktivitetList()
        {
            AktivitetList = DBAdapter.getAktivitet(SelectedEvent);
            EventAktivitetList = DBAdapter.getEventAktivitet(SelectedEvent);
        }


        private void checkScript()
        {
            try
            {
                SelectedHold = int.Parse(Request.Query["HoldList"]);
            }
            catch
            {
            }
            try
            {
                SelectedHoldAktivitet = int.Parse(Request.Query["HoldAktivitetList"]);
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
                TxtFarve = Request.Query["TxtFarve"];
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