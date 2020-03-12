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


        //giver point til hver aktivitet efter deres stilling
        private void udregnPoint()
        {
            if (Vis > 0)
            {
                for (int _vis = 0; _vis < Vis; _vis++)
                {
                    List<Hold> HoldVindere = new List<Hold>();
                    List<Hold> HoldTabere = new List<Hold>();
                    List<Hold> HoldStodLige = new List<Hold>();
                    List<Hold> HoldResterende = new List<Hold>();
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
                            int antalLige = 0;
                            List<Hold> tempHoldList = new List<Hold>();

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
                            if (_aktivitet.HoldSport == 0)
                            {
                                tempHoldList = HoldList.Where(i => i.HoldAktiviteter.Where(i => i.HoldOrder == _eventAktivitetHold.HoldOrder && i.EventAktivitetId == _eventAktivitetHold.EventAktivitetId).ToList().Count != 0).ToList();
                            }
                            for (int i = 0; i < HoldList.Count; i++)
                            {
                                if (HoldList[i] != _hold)
                                {
                                    if (tempHoldList == null || tempHoldList.Count == 0 || tempHoldList != null && tempHoldList.Count != 0 && tempHoldList.Contains(HoldList[i]))
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
                                                if (tempHoldList != null && tempHoldList.Count != 0 && totalScore == modstanderTotalScore)
                                                {
                                                    antalLige++;
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
                                            if (totalScore > modstanderTotalScore && _aktivitet.PointType != 1 && _aktivitet.PointType != 3 || totalScore < modstanderTotalScore && _aktivitet.PointType != 0 && _aktivitet.PointType != 2)
                                            {
                                                antalForan++;
                                            }
                                        }
                                    }
                                }
                            }
                            if (_aktivitet.HoldSport == 0 && tempHoldList != null && tempHoldList.Count != 0)
                            {
                                if (antalLige == tempHoldList.Count - 1)
                                {
                                    HoldStodLige.Add(_hold);
                                }
                                else if (antalForan == 0)
                                {
                                    HoldVindere.Add(_hold);
                                }
                                else
                                {
                                    HoldTabere.Add(_hold);
                                }
                            }
                            else
                            {
                                _eventAktivitetHold.Point = 10 * (HoldList.Count() - antalForan);
                            }
                        }
                        else
                        {
                            HoldResterende.Add(_hold);
                        }
                    }

                    //hvis det er hold mod hold og point gives til vinderen af de individuelle hold mod hold.
                    if (HoldVindere.Count != 0 && HoldTabere.Count != 0 || HoldStodLige.Count != 0)
                    {
                        List<List<Hold>> listerAfHold = new List<List<Hold>>();
                        if (HoldVindere.Count != 0 && HoldTabere.Count != 0)
                        {
                            listerAfHold.Add(HoldVindere);
                            listerAfHold.Add(HoldTabere);
                        }
                        if (HoldStodLige.Count != 0)
                        {
                            listerAfHold.Add(HoldStodLige);
                        }
                        HoldList.Clear();
                        foreach (List<Hold> _holdlist in listerAfHold)
                        {
                            //hvor mange hold der er i de andre lister før den liste som den nuværende hold er i
                            int holdlistenummer = 0;
                            if (_holdlist == HoldStodLige || _holdlist == HoldTabere)
                            {
                                if (HoldVindere != null)
                                {
                                    holdlistenummer += HoldVindere.Count;
                                }
                                if (_holdlist == HoldTabere)
                                {
                                    if (HoldStodLige != null)
                                    {
                                        holdlistenummer += HoldStodLige.Count;
                                    }
                                }
                            }
                            
                            foreach (Hold _hold in _holdlist)
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
                                    if (antalScores != 0 && _aktivitet.HoldSport == 1)
                                    {
                                        totalScore /= antalScores;
                                    }
                                    for (int i = 0; i < _holdlist.Count; i++)
                                    {
                                        if (_holdlist[i] != _hold)
                                        {
                                            modstanderTotalScore = 0;
                                            modstanderAntalScores = 0;
                                            EventAktivitetHold _modstanderEventAktivitetHold = _holdlist[i].HoldAktiviteter.Where(i => i.EventAktivitetId == EventAktivitetList[_vis].Id).FirstOrDefault();
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
                                                }
                                                else if (_aktivitet.HoldSport == 1)
                                                {
                                                    foreach (var _deltager in DeltagerList.Where(d => d.HoldId == _holdlist[i].Id).ToList())
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
                                                if (totalScore > modstanderTotalScore && _aktivitet.PointType != 1 && _aktivitet.PointType != 3 || totalScore < modstanderTotalScore && _aktivitet.PointType != 0 && _aktivitet.PointType != 2)
                                                {
                                                    antalForan++;
                                                }
                                            }
                                        }
                                    }
                                    _eventAktivitetHold.Point = 10 * (HoldVindere.Count + HoldTabere.Count + HoldStodLige.Count - antalForan - holdlistenummer);
                                }
                            }
                            HoldList.AddRange(_holdlist);
                        }
                        HoldList.AddRange(HoldResterende);
                    }
                }
            }
            HoldList = HoldList.OrderByDescending(i => i.HoldAktiviteter.Sum(i => i.Point)).ToList();
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