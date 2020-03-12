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
    public class HoldOpsaetningModel : PageModel
    {
        public Login CurrentLogin { get; set; }

        [BindProperty]
        public string DeltagerNavn { get; set; }

        [BindProperty]
        public int DeltagerID { get; set; }

        [BindProperty]
        public int HoldID { get; set; }

        [TempData]
        public int SelectedEvent { get; set; }

        public List<Event> EventList { get; set; }

        public List<Deltager> DeltagerList { get; set; }

        public List<Hold> HoldList { get; set; }


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
                if (Request.Query.ContainsKey("Event"))
                {
                    SelectedEvent = int.Parse(Request.Query["Event"]);
                    DeltagerList = DBAdapter.getDeltagere(SelectedEvent);
                    HoldList = DBAdapter.getHold();
                }
                else
                {
                    if (TempData.Peek("SelectedEvent") != null)
                    {
                        TempData.Keep("SelectedEvent");
                        DeltagerList = DBAdapter.getDeltagere(SelectedEvent);
                        HoldList = DBAdapter.getHold();
                    }
                    else
                    {
                        SelectedEvent = -1;
                    }
                }
                EventList = DBAdapter.getEvent();

                return this.Page();
            }
        }

        public void OnPostCmdSubmitDeltagerNavn()
        {
            DBAdapter.addDeltager(DeltagerNavn, SelectedEvent);
            OnGet();
        }

        public void OnPostCmdRemoveDeltager()
        {
            DBAdapter.deleteDeltager(DeltagerID);
            OnGet();
        }

        public void OnPostCmdAddDeltagerToHold()
        {
            if(HoldID != 0)
            {
                DBAdapter.updateDeltager(DeltagerID, HoldID);
            }
            else
            {
                DBAdapter.updateDeltager(DeltagerID, null);
            }
            OnGet();
        }

        public void OnPostRandomTeams()
        {
            OnGet();
            List<Deltager> temp = new List<Deltager>();
            Dictionary<int, int> pairs = new Dictionary<int, int>();
            foreach (Deltager deltager in DeltagerList)
            {
                if (deltager.HoldId == null)
                {
                    temp.Add(deltager);
                }
                else
                {
                    if (pairs.ContainsKey(deltager.HoldId ?? -1))
                    {
                        pairs[deltager.HoldId ?? -1]++;
                    }
                    else
                    {
                        pairs.Add(deltager.HoldId ?? -1, 1);
                    }
                }
            }
            pairs = (Dictionary<int, int>) pairs.OrderBy(x => x.Value);
            List<int> temphold = new List<int>();
            int h = 0;
            while (true)
            {
                if(pairs.Values.GetEnumerator().Current > 0)
                {
                    if (h == 0)
                    {
                        h = pairs.Values.GetEnumerator().Current;
                    }
                    if (pairs.Values.GetEnumerator().Current > h)
                    {
                        break;
                    }
                    else
                    {
                        temphold.Add(pairs.Keys.GetEnumerator().Current);
                    }
                }

                pairs.Values.GetEnumerator().MoveNext();
                pairs.Keys.GetEnumerator().MoveNext();
            }
            Random rand = new Random();
            int hindex = rand.Next(0, temphold.Count);
            int dindex = rand.Next(0, temp.Count);
            DBAdapter.updateDeltager(temp[dindex].Id, temphold[hindex]);
        }
        public Dictionary<string, int> teamCount()
        {
            List<Deltager> temp = new List<Deltager>();
            Dictionary<int, int> pairs = new Dictionary<int, int>();
            foreach (Deltager deltager in DeltagerList)
            {
                if (deltager.HoldId == null)
                {
                    temp.Add(deltager);
                }
                else
                {
                    if (pairs.ContainsKey(deltager.HoldId ?? -1))
                    {
                        pairs[deltager.HoldId ?? -1]++;
                    }
                    else
                    {
                        pairs.Add(deltager.HoldId ?? -1, 1);
                    }
                }
            }
            IOrderedEnumerable<KeyValuePair<int, int>> pairs2 = pairs.OrderByDescending(x => x.Value);
            pairs = pairs2.ToDictionary<KeyValuePair<int, int>, int, int>(x => x.Key, x => x.Value);
            Dictionary<string, int> teamcount = new Dictionary<string, int>();
            foreach (KeyValuePair<int, int> item in pairs)
            {
                string name = HoldList.Find(x => x.Id == item.Key).Navn;
                teamcount.Add(name, item.Value);
            }
            //for (int i = 0; i < pairs.Count; i++)
            //{
            //    pairs.GetEnumerator().MoveNext();
            //    keypair = pairs2.GetEnumerator().Current;
            //    string name = HoldList.Find(x => x.Id == pairs.Keys.GetEnumerator().Current).Navn;
            //    teamcount.Add(name, pairs.Values.GetEnumerator().Current);
            //    string name = HoldList.Find(x => x.Id == keypair.Key).Navn;
            //    teamcount.Add(name, keypair.Value);
            //    pairs.GetEnumerator().MoveNext();
            //}
            return teamcount;
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