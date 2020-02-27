﻿using System;
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

        public string ValgtGuiElemement { get; set; }

        public List<Event> EventList { get; set; }
        public List<Deltager> DeltagerList { get; set; }

        public List<Hold> HoldList{ get; set; }




        public void OnGet()
        {
            SelectedEvent = -1;
            DeltagerList = new List<Deltager>();
            HoldList = new List<Hold>();
            EventList = DBAdapter.getEvent();

            checkScript();

                HoldList = DBAdapter.getHold(SelectedEvent);
            
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