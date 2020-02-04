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
        public int Skift { get; set; }

        [BindProperty]
        public int SelectedHoldList { get; set; }

        public List<Hold> HoldList { get; set; }

        public void OnGet()
        {
            int.TryParse(Request.Query["id"], out int i);

            if (i != 0)
            {
                ;
            }

        }
        public void OnPost()
        {

        }
        public void OnPostCmdSkift()
        {
            loadTempDataHoldList();
            saveTempDataHoldList();
        }
        public void OnPostCmdRemoveHold()
        {
            loadTempDataHoldList();
            saveTempDataHoldList();
        }
        public void OnPostCmdAddHoldPoint()
        {
            loadTempDataHoldList();
            saveTempDataHoldList();
        }
        public void OnPostCmdDeleteDHoldPoint()
        {
            loadTempDataHoldList();
            saveTempDataHoldList();
        }
        public void OnPostCmdAddDeltagerPoint()
        {
            loadTempDataHoldList();
            saveTempDataHoldList();
        }
        public void OnPostCmdDeleteDeltagerPoint()
        {
            loadTempDataHoldList();
            saveTempDataHoldList();
        }


        private void loadHoldList()
        {
            HoldList = DBAdapter.getHold();
        }

        //private void setHoldList()
        //{
        //    List<SelectListItem> temp = new List<SelectListItem>();
        //    int i = 0;
        //    foreach (Hold item in h)
        //    {              
        //        temp.Add(new SelectListItem { Value = item.Id + "", Text = item.Navn});
        //        i++;
        //    }
        //    //HoldList = temp;
        //}

        private void saveTempDataHoldList()
        {
            int i = 0;
            foreach (Hold _sli in HoldList)
            {
                TempData.Set("Hold" + i, _sli);
                i++;
            }
        }
        private void loadTempDataHoldList()
        {
            HoldList = new List<Hold>();
            for (int i = 0; i != -1; i++)
            {
                Hold _a = TempData.Get<Hold>("Hold" + i);
                if (_a != null)
                {
                    HoldList.Add(_a);
                }
                else
                {
                    break;
                }
            }
        }private void saveTempDataHoldpScoreList()
        {
            int i = 0;
            foreach (Hold _sli in HoldList)
            {
                TempData.Set("Hold" + i, _sli);
                i++;
            }
        }
        private void loadTempDataHoldScoreList()
        {
            HoldList = new List<Hold>();
            for (int i = 0; i != -1; i++)
            {
                Hold _a = TempData.Get<Hold>("Hold" + i);
                if (_a != null)
                {
                    HoldList.Add(_a);
                }
                else
                {
                    break;
                }
            }
        }
    }
}