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
    public class ManageLoginModel : PageModel
    {
        public Login CurrentLogin { get; set; }

        [BindProperty]
        public int SelectedLogin { get; set; }

        [BindProperty]
        public int SelectedAdminType { get; set; }


        [BindProperty]
        public int SelectedEvent { get; set; }

        [BindProperty]
        public int SelectedHold { get; set; }

        [BindProperty]
        public string TxtBrugernavn { get; set; }

        [BindProperty]
        public string TxtKodeord { get; set; }

        public string TxtKodeordRepeat { get; set; }

        public string ValgtGuiElemement { get; set; }

        public List<Event> EventList { get; set; }

        public List<Hold> HoldList { get; set; }

        public List<SelectListItem> AdminTypeList { get; set; }

        public List<Login> LoginList { get; set; }


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
                SelectedLogin = -1;
                SelectedEvent = -1;
                SelectedHold = -1;
                SelectedAdminType = -1;
                EventList = new List<Event>();
                LoginList = new List<Login>();
                HoldList = new List<Hold>();

                checkScript();

                EventList = DBAdapter.getEvent();

                AdminTypeList = new List<SelectListItem>()
                {
                new SelectListItem { Value = "0", Text = "Bruger" },
                new SelectListItem { Value = "1", Text = "Admin" },
                new SelectListItem { Value = "2", Text = "SuperAdmin" }
                };

                loadLoginList();
                if (SelectedAdminType == 0)
                {
                    if (SelectedEvent != -1)
                    {
                        HoldList = DBAdapter.getHold(SelectedEvent);
                    }
                }
                

                if (ValgtGuiElemement == "CmdOpretLogin")
                {
                    CmdSaveLogin();
                }
                else if (ValgtGuiElemement == "CmdSletLogin")
                {
                    CmdDeleteLogin();
                }
                else if (ValgtGuiElemement == "CmdRndKode")
                {
                    string tempKode = "";
                    for (int i = 0; i < 8; i++)
                    {
                        tempKode += GetLetter();
                    }
                    TxtKodeord = tempKode;
                    TxtKodeordRepeat = tempKode;
                }
                return this.Page();
            }
        }
        
        public char GetLetter()
        {
            string chars = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random rand = new Random();
            int num = rand.Next(0, chars.Length - 1);
            return chars[num];
        }

        public void CmdDeleteLogin()
        {
            if (SelectedLogin != -1)
            {
                if (LoginList.Where(i => i.Id == SelectedLogin).FirstOrDefault().AdminType <= CurrentLogin.AdminType)
                {
                    DBAdapter.deleteLogin(SelectedLogin);
                    loadLoginList();
                    SelectedLogin = -1;
                }
            }
        }

        public void CmdSaveLogin()
        {
            if (TxtBrugernavn != "" && TxtKodeord != "" && TxtKodeord == TxtKodeordRepeat && SelectedAdminType <= CurrentLogin.AdminType)
            {
                if (SelectedAdminType == 0 && SelectedEvent != -1 && SelectedHold != -1 || SelectedAdminType > 0)
                {
                    int? tempEventId = null;
                    if (SelectedEvent != -1)
                    {
                        tempEventId = SelectedEvent;
                    }
                    int? tempHoldId = null;
                    if (SelectedHold != -1)
                    {
                        tempHoldId = SelectedHold;
                    }
                    DBAdapter.addLogin(TxtBrugernavn, TxtKodeord, SelectedAdminType, tempEventId, tempHoldId);
                    loadLoginList();
                }
            }
        }


        private void loadLoginList()
        {
            int? tempEventId = null;
            if (SelectedEvent != -1 && SelectedEvent != 0)
            {
                tempEventId = SelectedEvent;
            }
            LoginList = DBAdapter.getLogin(tempEventId);
        }



        private void checkScript()
        {
            try
            {
                TxtBrugernavn = Request.Query["TxtBrugernavn"];
            }
            catch
            {
            }
            try
            {
                TxtKodeord = Request.Query["TxtKodeord"];
            }
            catch
            {
            }
            try
            {
                TxtKodeordRepeat = Request.Query["TxtKodeordRepeat"];
            }
            catch
            {
            }
            try
            {
                SelectedAdminType = int.Parse(Request.Query["AdminTypeList"]);
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
                SelectedLogin = int.Parse(Request.Query["LoginList"]);
            }
            catch
            {
            }

            if (SelectedEvent == -1)
            {
                loadTempDataEvent();
            }

            ValgtGuiElemement = Request.Query["ValgtGuiElemement"];

            if (ValgtGuiElemement == "EventList")
            {
                try
                {
                    SelectedEvent = int.Parse(Request.Query["EventList"]);
                }
                catch
                {
                }
                if (SelectedEvent != -1)
                {
                    saveTempDataEvent();
                }
                SelectedLogin = -1;
            }
            else if (ValgtGuiElemement == "OpretLoginEventList")
            {
                try
                {
                    SelectedEvent = int.Parse(Request.Query["OpretLoginEventList"]);
                }
                catch
                {
                }
                if (SelectedEvent != -1)
                {
                    saveTempDataEvent();
                }
                SelectedLogin = -1;
            }
        }


        private void saveTempDataEvent()
        {
            List<int> tempEventList = new List<int>();
            if (ValgtGuiElemement == "EventList" || ValgtGuiElemement == "OpretLoginEventList")
            {
                tempEventList.Add(SelectedEvent);
            }
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