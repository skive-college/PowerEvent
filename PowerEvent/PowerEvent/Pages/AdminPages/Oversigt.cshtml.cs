﻿using System;
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
        public Login CurrentLogin { get; set; }

        //value på det valgte element i "select" list i GUI. skal være en "BindProperty".
        [BindProperty]
        public int SelectedEvent { get; set; }

        public List<Event> EventList { get; set; }

        public List<Hold> HoldListe { get; set; }

        public List<Deltager> DeltagerList { get; set; }

        public string ValgtGuiElemement { get; set; }


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
                SelectedEvent = -1;
                DeltagerList = new List<Deltager>();
                HoldListe = new List<Hold>();
                EventList = DBAdapter.getEvent();

                checkScript();
                if (DeltagerList.Count != 0)
                {
                    HoldListe = DBAdapter.getHold(SelectedEvent);
                }
                return this.Page();
            }
        }


        //on click for select element script. navn = select elementets "navn"
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
                    DeltagerList = DBAdapter.getDeltagere(SelectedEvent);
                }
            }

            ValgtGuiElemement = Request.Query["ValgtGuiElemement"];
            if (ValgtGuiElemement == "EventList")
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
