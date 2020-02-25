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
        public int? TxtScore { get; set; }

        [BindProperty]
        public int SelectedEvent { get; set; }

        [BindProperty]
        public int SelectedHold { get; set; }
        
        [BindProperty]
        public int SelectedEventAktivitet { get; set; }

        [BindProperty]
        public int SelectedDeltager { get; set; }

        [BindProperty]
        public int SelectedOrder { get; set; }

        [BindProperty]
        public int SelectedPoint { get; set; }

        public string ValgtGuiElemement { get; set; }


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
                            if (SelectedEventAktivitet != -1)
                            {
                                return DBAdapter.getAktivitet(SelectedEvent).Where(i => i.Id == EventAktivitetList.Where(i => i.Id == SelectedEventAktivitet).FirstOrDefault().AktivitetId).FirstOrDefault();
                            }
                        }
                        else
                        {
                            if (SelectedEventAktivitet != -1)
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

        private Deltager valgtDeltager;

        public Deltager ValgtDeltager
        {
            get
            {
                if (valgtDeltager == null)
                {
                    if (DeltagerList.Count == 0)
                    {
                        valgtDeltager = DBAdapter.getDeltagere(SelectedEvent, SelectedEventAktivitet, SelectedHold, SelectedDeltager)[0];
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

        public List<EventAktivitet> EventAktivitetList { get; set; }

        public List<int> OrderList { get; set; }


        public void OnGet()
        {
            SelectedEvent = -1;
            SelectedEventAktivitet = -1;
            SelectedOrder = -1;
            TxtScore = null;
            guiSelectedListReset();
            AktivitetList = new List<Aktivitet>();
            EventAktivitetList = new List<EventAktivitet>();
            HoldList = new List<Hold>();
            DeltagerList = new List<Deltager>();
            EventList = DBAdapter.getEvent();
            OrderList = new List<int>();

            checkScript();
            if (SelectedEvent != -1)
            {
                loadTempDataEvent();
                if (SelectedEvent != -1)
                {
                    EventAktivitetList = DBAdapter.getEventAktivitet(SelectedEvent);
                    AktivitetList = DBAdapter.getAktivitet(SelectedEvent);
                }
                if (EventAktivitetList.Count == 0)
                {
                    EventAktivitetList = DBAdapter.getEventAktivitet(SelectedEvent);
                    AktivitetList = DBAdapter.getAktivitet(SelectedEvent);
                }
                if (SelectedEventAktivitet != -1)
                {
                    OrderList = DBAdapter.getHoldOrder(SelectedEvent, SelectedEventAktivitet);
                    if (SelectedOrder != -1)
                    {
                        HoldList = DBAdapter.getHold(SelectedEvent, SelectedOrder, SelectedEventAktivitet);
                        HoldList = DBAdapter.getHoldAktivitet(HoldList, SelectedEvent, SelectedOrder, SelectedEventAktivitet);
                        HoldList = DBAdapter.getHoldAktivitetScores(HoldList, SelectedEvent, SelectedOrder, SelectedEventAktivitet);
                        if (SelectedHold != -1)
                        {
                            if (ValgtAktivitet.HoldSport == 1)
                            {
                                DeltagerList = DBAdapter.getDeltagere(SelectedEvent, SelectedEventAktivitet, SelectedHold);
                            }
                        }
                    }
                }
                if (ValgtGuiElemement == "CmdAddPoint" || ValgtGuiElemement == "CmdDeletePoint")
                {
                    if (ValgtGuiElemement == "CmdAddPoint")
                    {
                        CmdAddPoint();
                    }
                    else if (ValgtGuiElemement == "CmdDeletePoint")
                    {
                        CmdDeletePoint();
                    }
                    if (ValgtAktivitet.HoldSport == 0)
                    {
                        HoldList = DBAdapter.getHold(SelectedEvent, SelectedOrder, SelectedEventAktivitet);
                        HoldList = DBAdapter.getHoldAktivitet(HoldList, SelectedEvent, SelectedOrder, SelectedEventAktivitet);
                        HoldList = DBAdapter.getHoldAktivitetScores(HoldList, SelectedEvent, SelectedOrder, SelectedEventAktivitet);

                    }
                    else if (ValgtAktivitet.HoldSport == 1)
                    {
                         DeltagerList = DBAdapter.getDeltagere(SelectedEvent, SelectedEventAktivitet, SelectedHold);
                    }
                }
            }
        }

        public void OnPost()
        {

        }

        public void CmdAddPoint()
        {
            if (ValgtAktivitet.HoldSport == 0)
            {
                //HoldSport Add HOLD score 
                if (TxtScore != null)
                {
                    DBAdapter.addHoldScore(SelectedEvent, SelectedEventAktivitet, SelectedOrder, SelectedHold, TxtScore.Value);
                    HoldList = DBAdapter.getHold(SelectedEvent, SelectedOrder, SelectedEventAktivitet);
                    HoldList = DBAdapter.getHoldAktivitet(HoldList, SelectedEvent, SelectedOrder, SelectedEventAktivitet);
                    HoldList = DBAdapter.getHoldAktivitetScores(HoldList, SelectedEvent, SelectedOrder, SelectedEventAktivitet);
                }
            }
            else if (ValgtAktivitet.HoldSport == 1)
            {
                if (TxtScore != null)
                {
                    DBAdapter.addDeltagerScore(SelectedEvent, SelectedEventAktivitet, SelectedHold, SelectedDeltager, TxtScore.Value);
                    DeltagerList = DBAdapter.getDeltagere(SelectedEvent, SelectedEventAktivitet, SelectedHold);
                }
                //HoldSport Add DELTAGER score
            }
        }

        public void CmdDeletePoint()
        {
            if (ValgtAktivitet.HoldSport == 0)
            {
                //HoldSport Delete HOLD score
                if (SelectedPoint != -1)
                {
                    DBAdapter.deleteHoldScore(SelectedPoint);
                    SelectedPoint = -1;
                }
            }
            else if (ValgtAktivitet.HoldSport == 1)
            {
                if (SelectedPoint != -1)
                {
                    //HoldSport Deltete DELTAGER score
                    DBAdapter.deleteDeltagerScore(SelectedPoint);
                    SelectedPoint = -1;
                }
                
            }
        }


        private void checkScript()
        {
            //on click for select element script. navn = select elementets "navn"
            ValgtGuiElemement = Request.Query["ValgtGuiElemement"];

            try
            {
                SelectedEvent = int.Parse(Request.Query["EventList"]);
            }
            catch
            {
            }
            try
            {
                SelectedEventAktivitet = int.Parse(Request.Query["AktivitetList"]);
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
            try
            {
                TxtScore = int.Parse(Request.Query["txtScore"]);
            }
            catch
            {
            }


            if (SelectedEvent == -1)
            {
                loadTempDataEvent();
            }


            if (ValgtGuiElemement == "EventList")
            {
                if (SelectedEvent != -1)
                {
                    guiSelectedListReset();
                    saveTempDataEvent();
                    SelectedEventAktivitet = -1;
                }
            }
            else if (ValgtGuiElemement == "AktivitetList")
            {
                if (SelectedEventAktivitet != -1 && SelectedEvent != -1)
                {
                    guiSelectedListReset();
                }
            }
            else if (ValgtGuiElemement == "OrderList")
            {
                if (SelectedOrder != -1 && SelectedEventAktivitet != -1 && SelectedEvent != -1)
                {
                    guiSelectedListReset();
                }
            }
            else if (ValgtGuiElemement == "HoldList")
            {
                if (SelectedHold != -1)
                {
                    SelectedDeltager = -1;
                    SelectedPoint = -1;
                }
            }
            else if (ValgtGuiElemement == "DeltagerList")
            {
                if (SelectedDeltager != -1)
                {
                    SelectedPoint = -1;
                }
            }
            else if (ValgtGuiElemement == "PointList")
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