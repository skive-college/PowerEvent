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
    public class OpretEventModel : PageModel
    {
        public Login CurrentLogin { get; set; }

        [BindProperty]
        public int SelectedEvent { get; set; }

        public string TxtEvent { get; set; }

        public string ValgtGuiElemement { get; set; }

        public List<Event> EventList { get; set; }



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
                loadEventList();

                checkScript();

                if (ValgtGuiElemement == "CmdGemEvent")
                {
                    gemEvent();
                }
                else if (ValgtGuiElemement == "CmdSletEvent")
                {
                    sletEvent();
                }
            }
            return this.Page();
        }

        public void sletEvent()
        {
            if (SelectedEvent != -1)
            {
                DBAdapter.deleteEvent(SelectedEvent);
                loadEventList();
                SelectedEvent = -1;
            }
        }

        public void gemEvent()
        {
            if (TxtEvent != "")
            {
                DBAdapter.addEvent(TxtEvent);
                loadEventList();
            }
        }



        private void loadEventList()
        {
            EventList = DBAdapter.getEvent();
        }


        private void checkScript()
        {
            try
            {
                TxtEvent = Request.Query["TxtEvent"];
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


            ValgtGuiElemement = Request.Query["ValgtGuiElemement"];

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