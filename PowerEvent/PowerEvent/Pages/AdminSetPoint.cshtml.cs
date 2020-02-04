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

        public List<Hold> HoldList { get; set; }

        public List<Deltager> DeltagerList { get; set; }

        public List<Event> EventList { get; set; }
        
        public List<Aktivitet> AktivitetList { get; set; }

        public void OnGet()
        {
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
            if (navn == "EventList")
            {
                int i = -1;
                try
                {
                    i = int.Parse(Request.Query["id"]);
                }
                catch
                {
                }
                if (i != -1)
                {
                    AktivitetList = DBAdapter.getAktivitet(i);
                }
                else
                {
                    EventList = DBAdapter.getEvent();
                }
                SelectedEvent = i;
            }
            else if (navn == "AktivitetList")
            {
                int i = -1;
                try
                {
                    i = int.Parse(Request.Query["id"]);
                }
                catch
                {
                }
                if (i != -1)
                {
                }
                else
                {
                }
            }
            else if (navn == "HoldListe2")
            {
                int i = -1;
                try
                {
                    i = int.Parse(Request.Query["id"]);
                }
                catch
                {   
                }
                if (i != -1)
                {
                    //DeltagerList = DBAdapter.getDeltagere();
                }
                else
                {
                }
            }
            else if (navn == "HoldListe")
            {
                int i = -1;
                try
                {
                    i = int.Parse(Request.Query["id"]);
                }
                catch
                {   
                }
                if (i != -1)
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