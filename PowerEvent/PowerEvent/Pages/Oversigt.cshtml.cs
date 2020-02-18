using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DatabaseClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PowerEvent.Models;
using PowerEvent.Helpers;

namespace PowerEvent.Pages
{
    public class Oversigt : PageModel
    {
        //value på det valgte element i "select" list i GUI. skal være en "BindProperty".
        [BindProperty]
        public int SelectedEvent { get; set; }

        public List<Event> EventList { get; set; }

        public List<Hold> HoldListe { get; set; }

        public List<Deltager> DeltagerList { get; set; }


        public void OnGet()
        {
            SelectedEvent = -1;
            DeltagerList = new List<Deltager>();
            HoldListe = new List<Hold>();
            EventList = DBAdapter.getEvent();
            checkListScript();
            if (DeltagerList.Count != 0)
            {
                HoldListe = DBAdapter.getHold(SelectedEvent);
            }
        }

        public void OnPost()
        {
            EventList = DBAdapter.getEvent();
            checkListScript();
        }



        //on click for select element script. navn = select elementets "navn"
        private void checkListScript()
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
                    DeltagerList = DBAdapter.getDeltagere(SelectedEvent);
                }
            }

            string navn = Request.Query["navn"];
            if (navn == "EventList")
            {
                if (SelectedEvent != -1)
                {
                    saveTempDataEvent();
                    DeltagerList = DBAdapter.getDeltagere(SelectedEvent);
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
