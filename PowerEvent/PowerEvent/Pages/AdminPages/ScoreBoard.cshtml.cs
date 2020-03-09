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
    public class ScoreBoardModel : PageModel
    {
        public Login CurrentLogin { get; set; }

        [BindProperty]
        public int SelectedEvent { get; set; }

        [BindProperty]
        public int SelectedOrder { get; set; }

        [BindProperty]
        public int SelectedEventAktivitet { get; set; }

        public List<int> OrderList { get; set; }

        public List<Aktivitet> AktivitetList { get; set; }

        public List<EventAktivitet> EventAktivitetList { get; set; }

        public List<Hold> HoldList { get; set; }

        public List<Deltager> DeltagerList { get; set; }

        public string ValgtGuiElemement { get; set; }

        public List<Event> EventList { get; set; }

        public string EventName { get; set; }

        public string TeamEt { get; set; }

        public string TeamTo { get; set; }


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
                EventName = "test";
                TeamEt = "teamet";
                TeamTo = "teamto";
                SelectedEvent = -1;

                HoldList = new List<Hold>();
                DeltagerList = new List<Deltager>();
                EventList = DBAdapter.getEvent();


                checkScript();
                if (SelectedEvent != -1)
                {
                    EventAktivitetList = DBAdapter.getEventAktivitet(SelectedEvent);
                    AktivitetList = DBAdapter.getAktivitet(SelectedEvent);
                    
                    if (SelectedEventAktivitet != -1)
                    {
                        OrderList = DBAdapter.getHoldOrder(SelectedEvent, SelectedEventAktivitet);
                        if (SelectedOrder != -1)
                        {
                            HoldList = DBAdapter.getHold(SelectedEvent, SelectedOrder, SelectedEventAktivitet);
                            HoldList = DBAdapter.getHoldAktivitet(HoldList, SelectedEvent, SelectedOrder, SelectedEventAktivitet);
                            HoldList = DBAdapter.getHoldAktivitetScores(HoldList, SelectedEvent, SelectedOrder, SelectedEventAktivitet);
                            DeltagerList= DBAdapter.getDeltagere(SelectedEvent, SelectedOrder, SelectedEventAktivitet);
                        }
                    }
                }
            }
            return this.Page();
        }

        private void checkScript()
        {
            //on click for select element script. navn = select elementets "navn"
            ValgtGuiElemement = Request.Query["ValgtGuiElemement"];

            try
            {
                SelectedEvent = int.Parse(Request.Query["EventList"]);
            }
            catch
            {
            }
            try
            {
                SelectedEventAktivitet = int.Parse(Request.Query["AktivitetList"]);
            }
            catch
            {
            }
            try
            {
                SelectedOrder = int.Parse(Request.Query["OrderList"]);
            }
            catch
            {
            }

            if (SelectedEvent == -1)
            {
                loadTempDataEvent();
            }

            if (ValgtGuiElemement == "EventList")
            {
                if (SelectedEvent != -1)
                {
                    saveTempDataEvent();
                    SelectedEventAktivitet = -1;
                }
            }
            if (ValgtGuiElemement == "AktivitetList")
            {
                SelectedOrder = -1;
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