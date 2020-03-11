using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PowerEvent.Helpers;
using PowerEvent.Models;
using System.Collections.Generic;
using System.Linq;

namespace PowerEvent.Pages
{
    public class opretAktivit : PageModel
    {
        public Login CurrentLogin { get; set; }

        [BindProperty]
        public int SelectedPointType { get; set; }
        
        [BindProperty]
        public int SelectedHoldSport { get; set; }

        [BindProperty]
        public int SelectedAktivitet { get; set; }

        [BindProperty]
        public int SelectedEventAktivitet { get; set; }

        [BindProperty]
        public int SelectedEvent { get; set; }
        
        [BindProperty]
        public int SelectedOpretEvent { get; set; }

        public string TxtAktivitet { get; set; }

        public string TxtEvent { get; set; }

        public string TxtSletEvent { get; set; }

        public string ValgtGuiElemement { get; set; }

        public List<Event> EventList { get; set; }

        public List<Event> OpretEventList { get; set; }

        public List<SelectListItem> GuiAktivitetList { get; set; }        

        public List<Aktivitet> AktivitetList { get; set; }

        public List<EventAktivitet> EventAktivitetList { get; set; }

        public List<SelectListItem> PointTypeList { get; set; }

        public List<SelectListItem> TempPointTypeList { get; set; }

        public List<SelectListItem> HoldSportList { get; set; }

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
                            if (SelectedAktivitet != -1)
                            {
                                return DBAdapter.getAktivitet(SelectedEvent).Where(i => i.Id == EventAktivitetList.Where(i => i.Id == SelectedEventAktivitet).FirstOrDefault().AktivitetId).FirstOrDefault();
                            }
                        }
                        else
                        {
                            if (SelectedAktivitet != -1)
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
                SelectedPointType = -1;
                SelectedHoldSport = -1;
                SelectedAktivitet = -1;
                SelectedEvent = -1;
                SelectedEventAktivitet = -1;
                SelectedOpretEvent = -1;
                AktivitetList = new List<Aktivitet>();
                EventAktivitetList = new List<EventAktivitet>();
                loadEventLister();

                //loadTempDataTempPointTypeList();
                //loadTempDataTempHoldSportList();
                loadAktivitetList();
                setAktivitetList();

                //---------------------------------------------------------

                PointTypeList = new List<SelectListItem>()
            {
            new SelectListItem { Value = "3", Text = "MaxSec" },
            new SelectListItem { Value = "2", Text = "MinSec" },
            new SelectListItem { Value = "1", Text = "MaxPoint" },
            new SelectListItem { Value = "0", Text = "MinPoint" }
            };
                //saveTempDataPointType();

                //-----------------------------------------------------------

                HoldSportList = new List<SelectListItem>()
            {
            new SelectListItem { Value = "0", Text = "Hold point" },
            new SelectListItem { Value = "1", Text = "Deltager point" },
            };
                //saveTempDataHoldSport();

                checkScript();
                if (ValgtGuiElemement == "CmdGemEvent")
                {
                    gemEvent();
                }
                else if (ValgtGuiElemement == "CmdSletEvent")
                {
                    sletEvent();
                }

                if (SelectedEvent != -1)
                {
                    AktivitetList = DBAdapter.getAktivitet(SelectedEvent);
                    EventAktivitetList = DBAdapter.getEventAktivitet(SelectedEvent);
                }

                if (ValgtGuiElemement == "CmdGemAktivitet")
                {
                    CmdGemAktivitet();
                }
                else if (ValgtGuiElemement == "CmdDeleteAktivitet")
                {
                    CmdDeleteAktivitet();
                }
                else if (ValgtGuiElemement == "CmdAddEventAktivitet")
                {
                    CmdAddEventAktivitet();
                }
                else if (ValgtGuiElemement == "CmdDeleteEventAktivitet")
                {
                    CmdSletEventAktivitet();
                }
            }
            return this.Page();
        }

        public void sletEvent()
        {
            if (SelectedOpretEvent != -1 && CurrentLogin.AdminType == 2 && TxtSletEvent == OpretEventList.Where(i => i.Id == SelectedOpretEvent).FirstOrDefault().Navn)
            {
                DBAdapter.deleteAllEvent(SelectedOpretEvent);
                loadEventLister();
                SelectedOpretEvent = -1;
            }
        }

        public void gemEvent()
        {
            if (TxtEvent != "")
            {
                DBAdapter.addEvent(TxtEvent);
                loadEventLister();
            }
        }

        private void loadEventLister()
        {
            EventList = DBAdapter.getEvent();
            OpretEventList = DBAdapter.getEvent();
        }

        public void CmdDeleteAktivitet()
        {
            if (SelectedAktivitet != -1)
            {
                DBAdapter.deleteAktivitet(SelectedAktivitet);
                loadAktivitetList();
                setAktivitetList();
                SelectedAktivitet = -1;
            }
        }

        public void CmdGemAktivitet()
        {
            if (TxtAktivitet != "")
            {
                DBAdapter.addAktivitet(TxtAktivitet, SelectedPointType, SelectedHoldSport);
                loadAktivitetList();
                setAktivitetList();
            }
        }
        
        public void CmdAddEventAktivitet()
        {
            if (SelectedAktivitet != -1 && SelectedEvent != -1)
            {
                DBAdapter.addEventAktivitet(SelectedEvent, SelectedAktivitet);
                loadAktivitetList();
                loadEventAktivitetList();
            }
        }
        public void CmdSletEventAktivitet()
        {
            if (SelectedEventAktivitet != -1)
            {
                DBAdapter.deleteEventAktivitet(SelectedEventAktivitet);
                loadAktivitetList();
                loadEventAktivitetList();
                SelectedEventAktivitet = -1;
            }
        }

        private void setAktivitetList()
        {
            List<SelectListItem> temp = new List<SelectListItem>();
            int i = 0;
            foreach (Aktivitet item in AktivitetList)
            {
                string pointTxt = "";
                string holdSportTxt = "";

                if (item.PointType == 0)
                {
                    pointTxt = "MinScore";
                }
                else if (item.PointType == 1)
                {
                    pointTxt = "MaxScore";
                }
                else if (item.PointType == 2)
                {
                    pointTxt = "MinSec";
                }
                else if (item.PointType == 3)
                {
                    pointTxt = "MaxSec";
                }
                //----------------------------------------------
                if (item.HoldSport == 0)
                {
                    holdSportTxt = "HoldScore";
                }
                else if (item.HoldSport == 1)
                {
                    holdSportTxt = "DeltagerScore";
                }
                temp.Add(new SelectListItem { Value = item.Id + "", Text = "Aktivitet: " + item.Navn + ". PointType: " + pointTxt + ", " + holdSportTxt});
                i++;
            }
            GuiAktivitetList = temp;
        }

        private void loadAktivitetList()
        {
            AktivitetList = DBAdapter.getAktivitet();
        }
        
        private void loadEventAktivitetList()
        {
            EventAktivitetList = DBAdapter.getEventAktivitet(SelectedEvent);
        }


        private void checkScript()
        {
            try
            {
                SelectedPointType = int.Parse(Request.Query["PointTypeList"]);
            }
            catch
            {
            }
            try
            {
                SelectedHoldSport = int.Parse(Request.Query["HoldSportList"]);
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
                TxtEvent = Request.Query["TxtEvent"];
            }
            catch
            {
            }
            try
            {
                TxtAktivitet = Request.Query["TxtAktivitet"];
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
                SelectedOpretEvent = int.Parse(Request.Query["OpretEventList"]);
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
            try
            {
                TxtSletEvent = Request.Query["TxtSletEvent"];
            }
            catch
            {
            }


            ValgtGuiElemement = Request.Query["ValgtGuiElemement"];

            if (SelectedEvent == -1)
            {
                loadTempDataEvent();
                if (SelectedEvent != -1)
                {

                }
            }

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

        private void loadTempDataLogin()
        {
            Login tempLogin = TempData.Peek<Login>("CurrentLogin");
            if (tempLogin != null)
            {
                CurrentLogin = tempLogin;
            }
        }


        //eksempel på tempdata↓↓↓↓↓________________________________________________

        //private void saveTemp()
        //{
        //    saveTempDataAktivitet();
        //    saveTempDataPointType();
        //}

        //private void loadTemp()
        //{
        //    loadTempDataAktivitet();
        //    loadTempDataTempPointTypeList();
        //}

        //private void saveTempDataAktivitet()
        //{
        //    int i = 0;
        //    foreach (Aktivitet _sli in TempAktivitetList)
        //    {
        //        TempData.Set("Aktivitet" + i, _sli);
        //        i++;
        //    }
        //}

        //private void loadTempDataAktivitet()
        //{
        //    TempAktivitetList = new List<Aktivitet>();
        //    for (int i = 0; i != -1; i++)
        //    {
        //        Aktivitet _a = TempData.Get<Aktivitet>("Aktivitet" + i);
        //        if (_a != null)
        //        {
        //            TempAktivitetList.Add(_a);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //}


        //private void saveTempDataPointType()
        //{
        //    int i = 0;
        //    foreach (SelectListItem _sli in PointTypeList)
        //    {
        //        TempData.Set("PointType" + i, _sli);
        //        i++;
        //    }
        //}

        //private void loadTempDataTempPointTypeList()
        //{
        //    TempPointTypeList = new List<SelectListItem>();
        //    for (int i = 0; i != -1; i++)
        //    {
        //        SelectListItem _sli = TempData.Peek<SelectListItem>("PointType" + i);
        //        if (_sli != null)
        //        {
        //            TempPointTypeList.Add(_sli);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //}

        //private void saveTempDataHoldSport()
        //{
        //    int i = 0;
        //    foreach (SelectListItem _sli in HoldSportList)
        //    {
        //        TempData.Set("HoldSport" + i, _sli);
        //        i++;
        //    }
        //}

        //private void loadTempDataTempHoldSportList()
        //{
        //    TempHoldSportList = new List<SelectListItem>();
        //    for (int i = 0; i != -1; i++)
        //    {
        //        SelectListItem _sli = TempData.Peek<SelectListItem>("HoldSport" + i);
        //        if (_sli != null)
        //        {
        //            TempHoldSportList.Add(_sli);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //}

    }
}
