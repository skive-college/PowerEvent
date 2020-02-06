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
        public int Skift { get; set; }
        
        [BindProperty]
        public int SelectedEvent { get; set; }

        [BindProperty]
        public int SelectedHoldList { get; set; }
        
        [BindProperty]
        public int SelectedAktivitet { get; set; }

        public List<Hold> HoldList { get; set; }

        public List<Deltager> DeltagerList { get; set; }

        public List<Event> EventList { get; set; }
        
        public List<Aktivitet> AktivitetList { get; set; }

        public void OnGet()
        {
            SelectedEvent = -1;
            SelectedHoldList = -1;
            SelectedAktivitet = -1;
            AktivitetList = new List<Aktivitet>();
            HoldList = new List<Hold>();
            DeltagerList = new List<Deltager>();
            EventList = DBAdapter.getEvent();
            checkListScript();
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
                }
                else
                {
                }
            }
            else if (navn == "HoldListe2")
            {
                if (SelectedHoldList != -1)
                {
                    //DeltagerList = DBAdapter.getDeltagere();
                }
                else
                {
                }
            }
            else if (navn == "HoldListe")
            {
                if (SelectedHoldList != -1)
                {
                    //DeltagerList = DBAdapter.getDeltagere();
                }
                else
                {
                }
            }
        }

    }
}