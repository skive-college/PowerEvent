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
        public int? TxtScore { get; set; }

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


        private Aktivitet valgtAktivitet;

        public Aktivitet ValgtAktivitet
        {
            get 
            {
                if (valgtAktivitet == null)
                {
                    if (AktivitetList.Count == 0)
                    {
                        if (SelectedAktivitet != -1)
                        {
                            valgtAktivitet = DBAdapter.getAktivitet(SelectedEvent, SelectedAktivitet)[0];
                        }
                    }
                    else
                    {
                        valgtAktivitet = AktivitetList.Where(i => i.Id == SelectedAktivitet).FirstOrDefault();
                    }
                }
                return valgtAktivitet;
            }
            set 
            { 
                valgtAktivitet = value; 
            }
        }

        private Deltager valgtDeltager;

        public Deltager ValgtDeltager
        {
            get
            {
                if (valgtDeltager == null)
                {
                    if (DeltagerList.Count == 0)
                    {
                        valgtDeltager = DBAdapter.getDeltagere(SelectedEvent, SelectedAktivitet, SelectedHold, SelectedDeltager)[0];
                    }
                    else
                    {
                        valgtDeltager = DeltagerList.Where(i => i.Id == SelectedDeltager).FirstOrDefault();
                    }
                }
                return valgtDeltager;
            }
            set
            {
                valgtDeltager = value;
            }
        }

        public List<Hold> HoldList { get; set; }

        public List<Deltager> DeltagerList { get; set; }

        public List<Event> EventList { get; set; }
        
        public List<Aktivitet> AktivitetList { get; set; }

        public List<int> OrderList { get; set; }

        public void OnGet()
        {
            SelectedEvent = -1;
            SelectedAktivitet = -1;
            SelectedOrder = -1;
            TxtScore = null;
            guiSelectedListReset();
            AktivitetList = new List<Aktivitet>();
            HoldList = new List<Hold>();
            DeltagerList = new List<Deltager>();
            EventList = DBAdapter.getEvent();
            OrderList = new List<int>();


            checkListScript();
            if (SelectedEvent != -1)
            {
                loadTempDataEvent();
                if (SelectedEvent != -1)
                {
                    AktivitetList = DBAdapter.getAktivitet(SelectedEvent);
                }
                if (AktivitetList.Count == 0)
                {
                    AktivitetList = DBAdapter.getAktivitet(SelectedEvent);
                }
                if (SelectedAktivitet != -1)
                {
                    OrderList = DBAdapter.getHoldOrder(SelectedEvent, SelectedAktivitet);
                    if (SelectedOrder != -1)
                    {
                        HoldList = DBAdapter.getHold(SelectedEvent, SelectedOrder, SelectedAktivitet);
                        HoldList = DBAdapter.getHoldAktivitet(HoldList, SelectedEvent, SelectedOrder, SelectedAktivitet);
                        HoldList = DBAdapter.getHoldAktivitetScores(HoldList, SelectedEvent, SelectedOrder, SelectedAktivitet);
                        if (SelectedHold != -1)
                        {
                            if (ValgtAktivitet.HoldSport == 1)
                            {
                                DeltagerList = DBAdapter.getDeltagere(SelectedEvent, SelectedAktivitet, SelectedHold);
                            }
                        }
                    }
                }
            }
        }

        public void OnPost()
        {

        }

        public void OnPostCmdRemoveHold()
        {

        }

        public void OnPostCmdAddPoint()
        {
            OnGet();
            if (ValgtAktivitet.HoldSport == 0)
            {
                //HoldSport Add HOLD score 
                if (TxtScore != null)
                {
                    DBAdapter.addHoldScore(SelectedEvent, SelectedAktivitet, SelectedOrder, SelectedHold, TxtScore.Value);
                }
            }
            else if (ValgtAktivitet.HoldSport == 1)
            {
                //HoldSport Add DELTAGER score
            }
        }

        public void OnPostCmdDeletePoint()
        {
            if (ValgtAktivitet.HoldSport == 0)
            {
                //HoldSport Delete HOLD score
                if (SelectedPoint != -1)
                {
                    DBAdapter.deleteHoldScore(SelectedPoint);
                }
            }
            else if (ValgtAktivitet.HoldSport == 1)
            {
                //HoldSport Deltete DELTAGER score

            }
        }


        private void checkListScript()
        {
            //on click for select element script. navn = select elementets "navn"
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
            try
            {
                SelectedPoint = int.Parse(Request.Query["PointList"]);
            }
            catch
            {
            }



            if (navn == "EventList")
            {
                if (SelectedEvent != -1)
                {
                    guiSelectedListReset();
                    saveTempDataEvent();
                    SelectedAktivitet = -1;
                }
            }
            else if (navn == "AktivitetList")
            {
                if (SelectedAktivitet != -1 && SelectedEvent != -1)
                {
                    guiSelectedListReset();
                }
            }
            else if (navn == "OrderList")
            {
                if (SelectedOrder != -1 && SelectedAktivitet != -1 && SelectedEvent != -1)
                {
                    guiSelectedListReset();
                }
            }
            else if (navn == "HoldList")
            {
                if (SelectedHold != -1)
                {
                    SelectedDeltager = -1;
                    SelectedPoint = -1;
                }
            }
            else if (navn == "DeltagerList")
            {
                if (SelectedDeltager != -1)
                {
                    SelectedPoint = -1;
                }
            }
            else if (navn == "PointList")
            {
                if (SelectedDeltager != -1)
                {
                    
                }
            }
        }

        private void guiSelectedListReset()
        {
            SelectedHold = -1;
            SelectedDeltager = -1;
            SelectedPoint = -1;
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