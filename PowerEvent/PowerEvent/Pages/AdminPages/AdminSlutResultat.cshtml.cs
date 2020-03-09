using System;
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
        public Login CurrentLogin { get; set; }

        [BindProperty]
        public int SelectedEvent { get; set; }

        public int Vis { get; set; }

        public string ValgtGuiElemement { get; set; }

        public List<Event> EventList { get; set; }

        public List<Hold> HoldList{ get; set; }

        public List<EventAktivitetHoldScore> ScoreList{ get; set; }
        
        public List<EventAktivitetHold> EventAktivitetHoldList { get; set; }

        public List<Aktivitet> AktivitetList { get; set; }

        public List<EventAktivitet> EventAktivitetList { get; set; }

        public List<Deltager> DeltagerList { get; set; }


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
                AktivitetList = new List<Aktivitet>();
                HoldList = new List<Hold>();
                ScoreList = new List<EventAktivitetHoldScore>();
                EventAktivitetHoldList = new List<EventAktivitetHold>();
                EventAktivitetList = new List<EventAktivitet>();
                DeltagerList = new List<Deltager>();
                EventList = DBAdapter.getEvent();
                checkScript();

                HoldList = DBAdapter.getHold(SelectedEvent);
                HoldList = DBAdapter.getHoldAktivitet(HoldList, SelectedEvent);
                HoldList = DBAdapter.getHoldAktivitetScores(HoldList, SelectedEvent);

                DeltagerList = DBAdapter.getDeltagere(SelectedEvent);
                AktivitetList = DBAdapter.getAktivitet(SelectedEvent);
                EventAktivitetList = DBAdapter.getEventAktivitet(SelectedEvent);


                if (ValgtGuiElemement == "CmdVis")
                {
                    loadTempDataVis();
                    if (Vis < EventAktivitetList.Count)
                    {
                        Vis++;
                    }
                    saveTempDataVis();
                }
                else
                {
                    Vis = 0;
                    saveTempDataVis();
                }

                udregnPoint();

                return this.Page();
            }
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
            }

            ValgtGuiElemement = Request.Query["ValgtGuiElemement"];
            if (ValgtGuiElemement == "EventList")
            {
                if (SelectedEvent != -1)
                {
                    saveTempDataEvent();
                }
                Vis = 0;
            }
        }


        private void udregnPoint()
        {
            if (Vis > 0)
            {
                for (int _vis = 0; _vis < Vis; _vis++)
                {
                    //sorterer efter stilling
                    foreach (Hold _hold in HoldList)
                    {
                        EventAktivitetHold _eventAktivitetHold = _hold.HoldAktiviteter.Where(i => i.EventAktivitetId == EventAktivitetList[_vis].Id).FirstOrDefault();

                        if (_eventAktivitetHold != null)
                        {
                            Aktivitet _aktivitet = AktivitetList.Where(i => i.Id == EventAktivitetList.Where(i => i.Id == _eventAktivitetHold.EventAktivitetId).FirstOrDefault().AktivitetId).FirstOrDefault();
                            int? totalScore = 0;
                            int? antalScores = 0;
                            int? modstanderTotalScore = 0;
                            int? modstanderAntalScores = 0;
                            int antalForan = 0;
                            if (_aktivitet.HoldSport == 0)
                            {
                                if (_eventAktivitetHold.HoldScores.Count != 0)
                                {
                                    foreach (var _score in _eventAktivitetHold.HoldScores)
                                    {
                                        totalScore += _score.HoldScore;
                                        antalScores++;
                                    }
                                }
                            }
                            else if (_aktivitet.HoldSport == 1)
                            {
                                foreach (var _deltager in DeltagerList.Where(d => d.HoldId == _hold.Id).ToList())
                                {
                                    foreach (var _score in _deltager.ScoreList.Where(i => i.EventAktivitetId == EventAktivitetList[_vis].Id).ToList())
                                    {
                                        if (_score.Score != null && _score.EventAktivitetId == _eventAktivitetHold.EventAktivitetId)
                                        {
                                            totalScore += _score.Score;
                                            antalScores++;
                                        }
                                    }
                                }
                            }
                            if (antalScores != 0  && _aktivitet.HoldSport == 1)
                            {
                                totalScore /= antalScores;
                            }
                            for (int i = 0; i < HoldList.Count; i++)
                            {
                                if (HoldList[i] != _hold)
                                {
                                    modstanderTotalScore = 0;
                                    modstanderAntalScores = 0;
                                    EventAktivitetHold _modstanderEventAktivitetHold = HoldList[i].HoldAktiviteter.Where(i => i.EventAktivitetId == EventAktivitetList[_vis].Id).FirstOrDefault();
                                    if (_modstanderEventAktivitetHold != null)
                                    {
                                        if (_aktivitet.HoldSport == 0)
                                        {
                                            if (_modstanderEventAktivitetHold.HoldScores.Count != 0)
                                            {
                                                foreach (var _score in _modstanderEventAktivitetHold.HoldScores)
                                                {
                                                    modstanderTotalScore += _score.HoldScore;
                                                    modstanderAntalScores++;
                                                }
                                            }
                                            else
                                            {
                                                modstanderTotalScore = 0;
                                            }
                                            if (_modstanderEventAktivitetHold.HoldOrder == _eventAktivitetHold.HoldOrder)
                                            {
                                                if (totalScore > modstanderTotalScore)
                                                {
                                                        
                                                }
                                            }
                                        }
                                        else if (_aktivitet.HoldSport == 1)
                                        {
                                            foreach (var _deltager in DeltagerList.Where(d => d.HoldId == HoldList[i].Id).ToList())
                                            {
                                                foreach (var _score in _deltager.ScoreList.Where(i => i.EventAktivitetId == EventAktivitetList[_vis].Id).ToList())
                                                {
                                                    if (_score.Score != null)
                                                    {
                                                        modstanderTotalScore += _score.Score;
                                                        modstanderAntalScores++;
                                                    }
                                                }
                                            }
                                        }
                                        if (modstanderAntalScores != 0 && _aktivitet.HoldSport == 1)
                                        {
                                            modstanderTotalScore /= modstanderAntalScores;
                                        }
                                    }
                                    if (totalScore > modstanderTotalScore && _aktivitet.PointType != 1 && _aktivitet.PointType != 3 && _modstanderEventAktivitetHold != null || totalScore < modstanderTotalScore && _aktivitet.PointType != 0 && _aktivitet.PointType != 2 && _modstanderEventAktivitetHold != null)
                                    {
                                        antalForan++;
                                    }
                                }
                            }

                            if (_aktivitet.HoldSport == 1)
                            {

                            }
                            _eventAktivitetHold.Point = 10 * (HoldList.Count() - antalForan);
                        }
                    }
                    
                }
                HoldList = HoldList.OrderByDescending(i => i.HoldAktiviteter.Sum(i => i.Point)).ToList();
            }
        }

        //original point udregning:
        //if (Vis > 0)
        //{
        //    for (int _vis = 0; _vis < Vis; _vis++)
        //    {
        //        for (int _stop = 0; _stop < HoldList.Count; _stop++)
        //        {//sorterer efter stilling
        //            foreach (Hold _hold in HoldList)
        //            {
        //                EventAktivitetHold _eventAktivitetHold = _hold.HoldAktiviteter.Where(i => i.EventAktivitetId == EventAktivitetList[_vis].Id).FirstOrDefault();

        //                if (_eventAktivitetHold != null)
        //                {
        //                    Aktivitet _aktivitet = AktivitetList.Where(i => i.Id == EventAktivitetList.Where(i => i.Id == _eventAktivitetHold.EventAktivitetId).FirstOrDefault().AktivitetId).FirstOrDefault();
        //                    int? totalScore = 0;
        //                    int? antalScores = 0;
        //                    int? andetTotalScore = 0;
        //                    int? andetAntalScores = 0;
        //                    int check = 0;
        //                    if (_aktivitet.HoldSport == 0)
        //                    {
        //                        if (_eventAktivitetHold.HoldScores.Count != 0)
        //                        {
        //                            foreach (var _score in _eventAktivitetHold.HoldScores)
        //                            {
        //                                totalScore += _score.HoldScore;
        //                                antalScores++;
        //                            }
        //                        }
        //                    }
        //                    else if (_aktivitet.HoldSport == 1)
        //                    {
        //                        foreach (var _deltager in DeltagerList.Where(d => d.HoldId == _hold.Id).ToList())
        //                        {
        //                            foreach (var _score in _deltager.ScoreList)
        //                            {
        //                                if (_score.Score != null && _score.EventAktivitetId == _eventAktivitetHold.EventAktivitetId)
        //                                {
        //                                    totalScore += _score.Score;
        //                                    antalScores++;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (antalScores != 0)
        //                    {
        //                        totalScore /= antalScores;
        //                    }
        //                    for (int i = 0; i < HoldList.Count; i++)
        //                    {
        //                        if (HoldList[i] != _hold)
        //                        {
        //                            EventAktivitetHold _andetEventAktivitetHold = HoldList[i].HoldAktiviteter.Where(i => i.EventAktivitetId == EventAktivitetList[_vis].Id).FirstOrDefault();
        //                            if (_andetEventAktivitetHold != null)
        //                            {
        //                                if (_aktivitet.HoldSport == 0)
        //                                {
        //                                    if (_andetEventAktivitetHold.HoldScores.Count != 0)
        //                                    {
        //                                        foreach (var _score in _andetEventAktivitetHold.HoldScores)
        //                                        {
        //                                            andetTotalScore += _score.HoldScore;
        //                                            andetAntalScores++;
        //                                        }
        //                                    }
        //                                }
        //                                else if (_aktivitet.HoldSport == 1)
        //                                {
        //                                    foreach (var _deltager in DeltagerList.Where(d => d.HoldId == HoldList[i].Id).ToList())
        //                                    {
        //                                        foreach (var _score in _deltager.ScoreList)
        //                                        {
        //                                            if (_score.Score != null)
        //                                            {
        //                                                andetTotalScore += _score.Score;
        //                                                andetAntalScores++;
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                if (andetAntalScores != 0)
        //                                {
        //                                    andetTotalScore /= andetAntalScores;
        //                                }
        //                            }
        //                            if (_andetEventAktivitetHold == null || totalScore < andetTotalScore && _aktivitet.PointType != 1 && _aktivitet.PointType != 3 && _andetEventAktivitetHold != null || totalScore > andetTotalScore && _aktivitet.PointType != 0 && _aktivitet.PointType != 2 && _andetEventAktivitetHold != null)
        //                            {
        //                                if (HoldList.IndexOf(_hold) > i)
        //                                {
        //                                    HoldList.Remove(_hold);
        //                                    HoldList.Insert(i, _hold);
        //                                    i++;
        //                                    check = 1;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (check == 1)
        //                    {
        //                        break;
        //                    }

        //                    else if (_aktivitet.HoldSport == 1)
        //                    {

        //                    }

        //                }
        //            }

        //            //giver point
        //            foreach (var _hold in HoldList)
        //            {
        //                if (_vis == 4)
        //                {

        //                }
        //                EventAktivitetHold _eventAktivitetHold = _hold.HoldAktiviteter.Where(i => i.EventAktivitetId == EventAktivitetList[_vis].Id).FirstOrDefault();
        //                if (_eventAktivitetHold != null)
        //                {
        //                    int plads = HoldList.IndexOf(_hold);
        //                    int antalForan = 0;
        //                    Aktivitet _aktivitet = AktivitetList.Where(i => i.Id == EventAktivitetList.Where(i => i.Id == _eventAktivitetHold.EventAktivitetId).FirstOrDefault().AktivitetId).FirstOrDefault();
        //                    int? totalScore = 0;
        //                    int? antalScores = 0;
        //                    if (_aktivitet.HoldSport == 0)
        //                    {
        //                        if (_eventAktivitetHold.HoldScores.Count != 0)
        //                        {
        //                            foreach (var _score in _eventAktivitetHold.HoldScores)
        //                            {
        //                                totalScore += _score.HoldScore;
        //                                antalScores++;
        //                            }
        //                        }
        //                    }
        //                    else if (_aktivitet.HoldSport == 1)
        //                    {
        //                        foreach (var _deltager in DeltagerList.Where(d => d.HoldId == _hold.Id).ToList())
        //                        {
        //                            foreach (var _score in _deltager.ScoreList)
        //                            {
        //                                if (_score.Score != null && _score.EventAktivitetId == _eventAktivitetHold.EventAktivitetId)
        //                                {
        //                                    totalScore += _score.Score;
        //                                    antalScores++;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (antalScores != 0)
        //                    {
        //                        totalScore /= antalScores;
        //                    }

        //                    if (plads != 0)
        //                    {
        //                        for (int i = plads - 1; i > -1; i--)
        //                        {
        //                            EventAktivitetHold _eventAktivitetModstanderHold = HoldList[i].HoldAktiviteter.Where(i => i.EventAktivitetId == EventAktivitetList[_vis].Id).FirstOrDefault();
        //                            if (_eventAktivitetModstanderHold != null)
        //                            {
        //                                int? andetTotalScore = 0;
        //                                int? andetAntalScores = 0;
        //                                if (_aktivitet.HoldSport == 0)
        //                                {
        //                                    if (_eventAktivitetModstanderHold.HoldScores.Count != 0)
        //                                    {
        //                                        foreach (var _score in _eventAktivitetModstanderHold.HoldScores)
        //                                        {
        //                                            andetTotalScore += _score.HoldScore;
        //                                            andetAntalScores++;
        //                                        }
        //                                    }
        //                                }
        //                                else if (_aktivitet.HoldSport == 1)
        //                                {
        //                                    foreach (var _deltager in DeltagerList.Where(d => d.HoldId == HoldList[i].Id).ToList())
        //                                    {
        //                                        foreach (var _score in _deltager.ScoreList)
        //                                        {
        //                                            if (_score.Score != null)
        //                                            {
        //                                                andetTotalScore += _score.Score;
        //                                                andetAntalScores++;
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                if (andetAntalScores != 0)
        //                                {
        //                                    andetTotalScore /= andetAntalScores;
        //                                }

        //                                if (andetTotalScore == totalScore)
        //                                {
        //                                    antalForan++;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    _eventAktivitetHold.Point = 10 * (HoldList.Count() - plads + antalForan);
        //                }
        //            }
        //        }
        //    }
        //    HoldList = HoldList.OrderByDescending(i => i.HoldAktiviteter.Sum(i => i.Point)).ToList();
        //}


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

        private void loadTempDataVis()
        {
            int.TryParse(TempData.Get<string>("Vis"), out int tempVis);
            Vis = tempVis;
        }

        private void saveTempDataVis()
        {
            List<int> tempEventList = new List<int>();
            tempEventList.Add(SelectedEvent);
            TempData.Set("Vis", Vis.ToString());
        }

    }
}