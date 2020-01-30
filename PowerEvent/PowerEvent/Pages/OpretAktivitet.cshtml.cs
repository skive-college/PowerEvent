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
        public int SelectedAktivitetList { get; set; }

        public int TempSelectedInfoId { get; set; }

        [BindProperty]
        public string Aktivitet { get; set; }

        public string TempAktivitet { get; set; }

        
        public List<SelectListItem> AktivitetList { get; set; }

        public List<Aktivitet> TempAktivitetList { get; set; }

        public List<SelectListItem> PointTypeList { get; set; }

        public List<SelectListItem> TempPointTypeList { get; set; }

        public void OnGet()
        {
            loadTempDataTempPointTypeList();
            if (TempAktivitetList == null || TempAktivitetList.Count == 0)
            {
                loadTempAktivitetList();
                setAktivitetList();
            }
            else
            {
                setAktivitetList();
            }

            if (TempPointTypeList.Count == 0)
            {
                PointTypeList = new List<SelectListItem>()
                {
                new SelectListItem { Value = "4", Text = "MaxSec" },
                new SelectListItem { Value = "3", Text = "MinSec" },
                new SelectListItem { Value = "2", Text = "MaxPoint" },
                new SelectListItem { Value = "1", Text = "MinPoint" }
                };
                saveTempDataPointType();
            }
            else
            {
                PointTypeList = TempPointTypeList;
            }
        }
        
        public void OnPost()
        {

        }

        public void OnPostCmdDelete()
        {
            //delete metode
            loadTempAktivitetList();
            setAktivitetList();
            loadTempDataTempPointTypeList();
            PointTypeList = new List<SelectListItem>();
            PointTypeList = TempPointTypeList;
        }

        public void OnPostCmdSubmit()
        {

            //databaseSaveMetode(input)
            loadTempAktivitetList();
            setAktivitetList();
            loadTempDataTempPointTypeList();
            PointTypeList = new List<SelectListItem>();
            PointTypeList = TempPointTypeList;
        }

        private void setAktivitetList()
        {
            List<SelectListItem> temp = new List<SelectListItem>();
            int i = 0;
            foreach (var item in TempAktivitetList)
            {
                string pointTxt = "";
                if (item.PointType == 1)
                {
                    pointTxt = "MinPoint";
                }
                else if (item.PointType == 2)
                {
                    pointTxt = "MaxPoint";
                }
                else if (item.PointType == 3)
                {
                    pointTxt = "MinSec";
                }
                else if (item.PointType == 4)
                {
                    pointTxt = "MaxSec";
                }
                temp.Add(new SelectListItem { Value = i + "", Text = item.Navn + pointTxt });
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
    }
}
