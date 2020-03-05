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

        [TempData]
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
                    if (Vis < EventAktivitetList.Count)
                    {
                        Vis++;
                    }
                    TempData.Keep("Vis");
                }
                if (Vis > 0)
                {
                    for (int _vis = 0; _vis < Vis; _vis++)
                    {
                        for (int _stop = 0; _stop < HoldList.Count; _stop++)
                        {
                            foreach (Hold _hold in HoldList)
                            {
                                EventAktivitetHold _eventAktivitetHold = _hold.HoldAktiviteter.Where(i => i.EventAktivitetId == EventAktivitetList[_vis].Id).FirstOrDefault();
                                
                                if (_eventAktivitetHold != null)
                                {
                                    Aktivitet _aktivitet = AktivitetList.Where(i => i.Id == EventAktivitetList.Where(i => i.Id == _eventAktivitetHold.EventAktivitetId).FirstOrDefault().AktivitetId).FirstOrDefault();
                                    int? totalScore = 0;
                                    int? antalScores = 0;
                                    int? andetTotalScore = 0;
                                    int? andetAntalScores = 0;
                                    int check = 0;
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
                                            foreach (var _score in _deltager.ScoreList)
                                            {
                                                if (_score.Score != null)
                                                {
                                                    totalScore += _score.Score;
                                                    antalScores++;
                                                }
                                            }
                                        }
                                    }
                                    if (antalScores != 0)
                                    {
                                        totalScore /= antalScores;
                                    }
                                    for (int i = 0; i < HoldList.Count; i++)
                                    {
                                        if (HoldList[i] != _hold)
                                        {
                                            EventAktivitetHold _andetEventAktivitetHold = HoldList[i].HoldAktiviteter.Where(i => i.EventAktivitetId == EventAktivitetList[_vis].Id).FirstOrDefault();
                                            if (_andetEventAktivitetHold != null)
                                            {
                                                if (_aktivitet.HoldSport == 0)
                                                {
                                                    if (_andetEventAktivitetHold.HoldScores.Count != 0)
                                                    {
                                                        foreach (var _score in _andetEventAktivitetHold.HoldScores)
                                                        {
                                                            andetTotalScore += _score.HoldScore;
                                                            andetAntalScores++;
                                                        }
                                                    }
                                                }
                                                else if (_aktivitet.HoldSport == 1)
                                                {
                                                    foreach (var _deltager in DeltagerList.Where(d => d.HoldId == HoldList[i].Id).ToList())
                                                    {
                                                        foreach (var _score in _deltager.ScoreList)
                                                        {
                                                            if (_score.Score != null)
                                                            {
                                                                andetTotalScore += _score.Score;
                                                                andetAntalScores++;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (andetAntalScores != 0)
                                                {
                                                    andetTotalScore /= andetAntalScores;
                                                }

                                                if (totalScore > andetTotalScore && _aktivitet.PointType != 1 && _aktivitet.PointType != 3 || totalScore < andetTotalScore && _aktivitet.PointType != 0 && _aktivitet.PointType != 2)
                                                {
                                                    if (HoldList.IndexOf(_hold) > i)
                                                    {
                                                        HoldList.Remove(_hold);
                                                        HoldList.Insert(i, _hold);
                                                        i++;
                                                        check = 1;
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    if (check == 1)
                                    {
                                        break;
                                    }

                                    else if (_aktivitet.HoldSport == 1)
                                    {

                                    }
                                    
                                }
                            }
                        }
                        
                        foreach (var _hold in HoldList)
                        {
                            foreach (var _eventAktivitetHold in _hold.HoldAktiviteter.Where(i => i.EventAktivitetId == EventAktivitetList[_vis].Id).ToList())
                            {
                                _eventAktivitetHold.Point = 10 * (HoldList.Count() - HoldList.IndexOf(_hold));
                            }
                        }
                    }
                    HoldList = HoldList.OrderByDescending(i => i.HoldAktiviteter.Sum(i => i.Point)).ToList();
                }
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