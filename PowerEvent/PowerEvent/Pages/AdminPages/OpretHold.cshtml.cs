using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PowerEvent.Helpers;
using PowerEvent.Models;
using System.Collections.Generic;
using System.Linq;

namespace PowerEvent
{
    public class OpretHoldModel : PageModel
    {
        public Login CurrentLogin { get; set; }

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

        public int txtHoldOrder { get; set; }

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

        public IActionResult OnGet()
        {
            loadTempDataLogin();
            if (CurrentLogin != null)
            {
                CurrentLogin = DBAdapter.verifyLogin(CurrentLogin.Brugernavn, CurrentLogin.Kodeord);
            }
            if (CurrentLogin == null || CurrentLogin.Id == 0 || CurrentLogin.AdminType == 0)
            {
                return Redirect("/Index");
            }
            else
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
                else if (ValgtGuiElemement == "CmdSletHold")
                {
                    CmdDeleteHold();
                }
                else if (ValgtGuiElemement == "CmdAddEventAktivitetHold")
                {
                    CmdAddEventAktivitetHold();
                }
                else if (ValgtGuiElemement == "CmdDeleteEventAktivitetHold")
                {
                    CmdSletEventAktivitetHold();
                }
            }
            return this.Page();
        }


        public void CmdDeleteHold()
        {
            if (SelectedHold != -1)
            {
                DBAdapter.deleteHold(SelectedHold);
                loadHoldList();
                SelectedHold = -1;
                SelectedHoldAktivitet = -1;
            }
        }

        public void CmdSaveHold()
        {
            if (TxtHold != "" && TxtFarve != "")
            {
                if (!TxtFarve.StartsWith("#"))
                {
                    TxtFarve = "#" + TxtFarve;
                }

                DBAdapter.addHold(TxtHold, TxtFarve);
                loadHoldList();
            }
            else if (TxtHold != "" && TxtFarve == "")
            {
                TxtFarve = "#fff";
                DBAdapter.addHold(TxtHold, TxtFarve);
                loadHoldList();
            }
        }

        public void CmdAddEventAktivitetHold()
        {
            if (SelectedEventAktivitet != -1 && txtHoldOrder != 0)
            {
                DBAdapter.addEventAktivitetHold(SelectedEventAktivitet, SelectedHold, txtHoldOrder);
                loadHoldAktivitetList();
            }
        }
        public void CmdSletEventAktivitetHold()
        {
            if (SelectedEventAktivitet != -1)
            {
                DBAdapter.deleteEventAktivitetHold(SelectedHoldAktivitet);
                loadHoldAktivitetList();
                SelectedHold = -1;
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
            foreach (var _hold in HoldAktivitetList)
            {
                _hold.HoldAktiviteter = _hold.HoldAktiviteter.Where(i => i.EventAktivitetId == SelectedEventAktivitet).ToList();
            }
            List<Hold> tempHoldList = new List<Hold>();
            foreach (var _hold in HoldAktivitetList)
            {
                foreach (var _aktivitet in _hold.HoldAktiviteter)
                {
                    Hold h = _hold;
                    h.HoldAktiviteter = new List<EventAktivitetHold>();
                    h.HoldAktiviteter.Add(_aktivitet);
                    tempHoldList.Add(h);
                }
            }
            tempHoldList = tempHoldList.OrderBy(i => i.HoldAktiviteter[0].HoldOrder).ThenBy(i => i.Navn).ToList();
            HoldAktivitetList = tempHoldList;
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
                txtHoldOrder = int.Parse(Request.Query["TxtHoldOrder"]);
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
            }

            else if (ValgtGuiElemement == "EventList")
            {
                if (SelectedEvent != -1)
                {
                    saveTempDataEvent();
                    SelectedEventAktivitet = -1;
                    SelectedHoldAktivitet = -1;
                }
            }
            else if (ValgtGuiElemement == "EventAktivitetList")
            {
                if (SelectedEventAktivitet != -1)
                {
                    SelectedHoldAktivitet = -1;
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

        private void loadTempDataLogin()
        {
            Login tempLogin = TempData.Peek<Login>("CurrentLogin");
            if (tempLogin != null)
            {
                CurrentLogin = tempLogin;
            }
        }

    }
}