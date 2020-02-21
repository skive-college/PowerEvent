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


        public string TxtAktivitet { get; set; }

        public string ValgtGuiElemement { get; set; }


        public List<SelectListItem> AktivitetList { get; set; }

        public List<Aktivitet> TempAktivitetList { get; set; }

        public List<SelectListItem> PointTypeList { get; set; }

        public List<SelectListItem> TempPointTypeList { get; set; }

        public List<SelectListItem> HoldSportList { get; set; }

        public List<SelectListItem> TempHoldSportList { get; set; }

        public void OnGet()
        {
            SelectedPointType = -1;
            SelectedHoldSport = -1;
            SelectedAktivitetList = -1;
            TempAktivitetList = new List<Aktivitet>();
            
            //loadTempDataTempPointTypeList();
            //loadTempDataTempHoldSportList();
            loadTempAktivitetList();
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


            checkListScript();

            if (ValgtGuiElemement == "CmdGemAktivitet")
            {
                CmdSaveAktivitet();
            }
            else if (ValgtGuiElemement == "CmdSletAktivitet")
            {
                CmdDeleteAktivitet();
            }

        }
        
        public void OnPost()
        {

        }

        public void CmdDeleteAktivitet()
        {
            if (SelectedAktivitetList != -1)
            {
                DBAdapter.deleteAktivitet(SelectedAktivitetList);
                loadTempAktivitetList();
                setAktivitetList();
                SelectedAktivitetList = -1;
            }
        }

        public void CmdSaveAktivitet()
        {
            if (TxtAktivitet != "" && SelectedPointType != -1 && SelectedHoldSport != -1)
            {
                DBAdapter.addAktivitet(TxtAktivitet, SelectedPointType, SelectedHoldSport);
                loadTempAktivitetList();
                setAktivitetList();
            }
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


        private void checkListScript()
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
                SelectedAktivitetList = int.Parse(Request.Query["AktivitetList"]);
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

            ValgtGuiElemement = Request.Query["ValgtGuiElemement"];

            if (ValgtGuiElemement == "AktivitetList")
            {

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
