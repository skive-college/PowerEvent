using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PowerEvent.Helpers;
using PowerEvent.Models;
using System.Collections.Generic;

namespace PowerEvent.Pages
{
    public class opretAktivit : PageModel
    {
        [BindProperty]
        public int SelectedPointType { get; set; }
        
        [BindProperty]
        public int SelectedHoldSport { get; set; }

        [BindProperty]
        public int SelectedAktivitetList { get; set; }

        public int TempSelectedInfoId { get; set; }

        [BindProperty]
        public string Aktivitet { get; set; }

        public string TempAktivitet { get; set; }

        
        public List<SelectListItem> AktivitetList { get; set; }

        public List<Aktivitet> TempAktivitetList { get; set; }

        public List<SelectListItem> PointTypeList { get; set; }

        public List<SelectListItem> TempPointTypeList { get; set; }

        public List<SelectListItem> HoldSportList { get; set; }

        public List<SelectListItem> TempHoldSportList { get; set; }

        public void OnGet()
        {
            TempAktivitetList = new List<Aktivitet>();
            loadTempDataTempPointTypeList();
            loadTempDataTempHoldSportList();
            if (TempAktivitetList == null || TempAktivitetList.Count == 0)
            {
                loadTempAktivitetList();
                setAktivitetList();
            }
            else
            {
                setAktivitetList();
            }
            //---------------------------------------------------------
            if (TempPointTypeList.Count == 0)
            {
                PointTypeList = new List<SelectListItem>()
                {
                new SelectListItem { Value = "3", Text = "MaxSec" },
                new SelectListItem { Value = "2", Text = "MinSec" },
                new SelectListItem { Value = "1", Text = "MaxPoint" },
                new SelectListItem { Value = "0", Text = "MinPoint" }
                };
                saveTempDataPointType();
            }
            else
            {
                PointTypeList = TempPointTypeList;
            }
            //-----------------------------------------------------------
            if (TempHoldSportList.Count == 0)
            {
                HoldSportList = new List<SelectListItem>()
                {
                new SelectListItem { Value = "0", Text = "Hold point" },
                new SelectListItem { Value = "1", Text = "Deltager point" },
                };
                saveTempDataHoldSport();
            }
            else
            {
                HoldSportList = TempHoldSportList;
            }
        }
        
        public void OnPost()
        {

        }

        public void OnPostCmdDelete()
        {
            DBAdapter.deleteAktivitet(SelectedAktivitetList);
            loadTempAktivitetList();
            setAktivitetList();
            loadTempDataTempPointTypeList();
            loadTempDataTempHoldSportList();
            PointTypeList = new List<SelectListItem>();
            PointTypeList = TempPointTypeList;
            HoldSportList = new List<SelectListItem>();
            HoldSportList = TempHoldSportList;
        }

        public void OnPostCmdSubmit()
        {
            if (Aktivitet != null && Aktivitet != "")
            {
                DBAdapter.addAktivitet(Aktivitet, SelectedPointType, SelectedHoldSport);
            }
            loadTempAktivitetList();
            setAktivitetList();
            loadTempDataTempPointTypeList();
            loadTempDataTempHoldSportList();
            PointTypeList = new List<SelectListItem>();
            PointTypeList = TempPointTypeList;
            HoldSportList = new List<SelectListItem>();
            HoldSportList = TempHoldSportList;
        }

        private void setAktivitetList()
        {
            List<SelectListItem> temp = new List<SelectListItem>();
            int i = 0;
            foreach (Aktivitet item in TempAktivitetList)
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
            AktivitetList = temp;
        }

        private void loadTempAktivitetList()
        {
            TempAktivitetList = DBAdapter.getAktivitet();
        }








        private void saveTemp()
        {
            saveTempDataAktivitet();
            saveTempDataPointType();
        }

        private void loadTemp()
        {
            loadTempDataAktivitet();
            loadTempDataTempPointTypeList();
        }

        private void saveTempDataAktivitet()
        {
            int i = 0;
            foreach (Aktivitet _sli in TempAktivitetList)
            {
                TempData.Set("Aktivitet" + i, _sli);
                i++;
            }
        }

        private void loadTempDataAktivitet()
        {
            TempAktivitetList = new List<Aktivitet>();
            for (int i = 0; i != -1; i++)
            {
                Aktivitet _a = TempData.Get<Aktivitet>("Aktivitet" + i);
                if (_a != null)
                {
                    TempAktivitetList.Add(_a);
                }
                else
                {
                    break;
                }
            }
        }

        
        private void saveTempDataPointType()
        {
            int i = 0;
            foreach (SelectListItem _sli in PointTypeList)
            {
                TempData.Set("PointType" + i, _sli);
                i++;
            }
        }

        private void loadTempDataTempPointTypeList()
        {
            TempPointTypeList = new List<SelectListItem>();
            for (int i = 0; i != -1; i++)
            {
                SelectListItem _sli = TempData.Peek<SelectListItem>("PointType" + i);
                if (_sli != null)
                {
                    TempPointTypeList.Add(_sli);
                }
                else
                {
                    break;
                }
            }
        }

        private void saveTempDataHoldSport()
        {
            int i = 0;
            foreach (SelectListItem _sli in HoldSportList)
            {
                TempData.Set("HoldSport" + i, _sli);
                i++;
            }
        }

        private void loadTempDataTempHoldSportList()
        {
            TempHoldSportList = new List<SelectListItem>();
            for (int i = 0; i != -1; i++)
            {
                SelectListItem _sli = TempData.Peek<SelectListItem>("HoldSport" + i);
                if (_sli != null)
                {
                    TempHoldSportList.Add(_sli);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
