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
    public class AdminSetPointModel : PageModel
    {
        [BindProperty]
        public int SelectedEvent { get; set; }

        [BindProperty]
        public int SelectedHold { get; set; }
        
        [BindProperty]
        public int SelectedAktivitet { get; set; }

        [BindProperty]
        public int SelectedDeltager { get; set; }

        [BindProperty]
        public int SelectedOrder { get; set; }

        [BindProperty]
        public int SelectedPoint { get; set; }

        public Aktivitet valgtAktivitet { get; set; }

        public List<Hold> HoldList { get; set; }

        public List<Deltager> DeltagerList { get; set; }

        public List<Event> EventList { get; set; }
        
        public List<Aktivitet> AktivitetList { get; set; }

        public void OnGet()
        {
            SelectedEvent = -1;
            SelectedAktivitet = -1;
            SelectedOrder = -1;
            SelectedHold = -1;
            SelectedDeltager = -1;
            SelectedPoint = -1;
            AktivitetList = new List<Aktivitet>();
            HoldList = new List<Hold>();
            DeltagerList = new List<Deltager>();
            EventList = DBAdapter.getEvent();
            checkListScript();
            if (SelectedAktivitet != -1)
            {
                AktivitetList = DBAdapter.getAktivitet(SelectedEvent);
            }
        }
        public void OnPost()
        {

        }
        public void OnPostCmdSkift()
        {
            //AktivitetList = DBAdapter.getAktivitet();
            //EventList = DBAdapter.getEvent();
            //HoldList = DBAdapter.getHold();
        }
        public void OnPostCmdRemoveHold()
        {

        }
        public void OnPostCmdAddHoldPoint()
        {

        }
        public void OnPostCmdDeleteDHoldPoint()
        {

        }
        public void OnPostCmdAddDeltagerPoint()
        {

        }
        public void OnPostCmdDeleteDeltagerPoint()
        {

        }


        private void loadHoldList()
        {
            HoldList = new List<Hold>();
            HoldList = DBAdapter.getHold();
        }


        private void checkListScript()
        {
            //on click for select element script. navn = select elementets "id"
            string navn = Request.Query["navn"];

            try
            {
                SelectedEvent = int.Parse(Request.Query["EventList"]);
            }
            catch
            {
            }
            try
            {
                SelectedAktivitet = int.Parse(Request.Query["AktivitetList"]);
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
            try
            {
                SelectedHold = int.Parse(Request.Query["HoldList"]);
            }
            catch
            {
            }
            try
            {
                SelectedDeltager = int.Parse(Request.Query["DeltagerList"]);
            }
            catch
            {
            }


            if (navn == "EventList")
            {
                if (SelectedEvent != -1)
                {
                    AktivitetList = DBAdapter.getAktivitet(SelectedEvent);
                }
            }
            else if (navn == "AktivitetList")
            {
                if (SelectedAktivitet != -1)
                {
                    Aktivitet tempAktivitet = new Aktivitet();
                    tempAktivitet = AktivitetList.Where(i => i.Id == SelectedAktivitet).FirstOrDefault();
                    if (tempAktivitet != null)
                    {
                        valgtAktivitet = tempAktivitet;
                    }
                }
            }
            else if (navn == "OrderList")
            {
                if (SelectedDeltager != -1)
                {
                    DeltagerList = DBAdapter.getDeltagere(SelectedEvent, SelectedAktivitet, SelectedHold);
                }
            }
            else if (navn == "HoldList")
            {
                if (SelectedHold != -1)
                {
                    //DeltagerList = DBAdapter.getDeltagere();
                }
            }
            else if (navn == "DeltagerList")
            {
                if (SelectedDeltager != -1)
                {
                    DeltagerList = DBAdapter.getDeltagere(SelectedEvent, SelectedAktivitet, SelectedHold);
                }
            }
            else if (navn == "DeltagerList")
            {
                if (SelectedDeltager != -1)
                {
                    DeltagerList = DBAdapter.getDeltagere(SelectedEvent, SelectedAktivitet, SelectedHold);
                }
            }
        }

    }
}