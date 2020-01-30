using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PowerEvent.Models;
using System.Collections.Generic;

namespace PowerEvent.Pages
{
    public class opretAktivit : PageModel
    {
        [BindProperty]
        public int SelectedInfoId { get; set; }

        [BindProperty]
        public string Aktivitet { get; set; }

        public SelectList TagOptions { get; set; }

        [BindProperty]
        public List<SelectListItem> AktivitetList { get; set; }

        public List<SelectListItem> TempAktivitetList { get; set; }

        public List<SelectListItem> PointTypeList { get; set; }

        public List<SelectListItem> TempPointTypeList { get; set; }

        public void OnGet()
        {
            AktivitetList = new List<SelectListItem>();
            TempAktivitetList = new List<SelectListItem>();
            TempPointTypeList = new List<SelectListItem>();
            loadTempDataAktivitet();
            loadTempDataTempPointTypeList();
            if (TempAktivitetList.Count == 0)
            {
                AktivitetList = new List<SelectListItem>()
                {
                new SelectListItem { Value = "1", Text = "Tyr MaxSec" },
                new SelectListItem { Value = "2", Text = "Bold MaxPoint" },
                new SelectListItem { Value = "3", Text = "Race MaxPoint" },
                new SelectListItem { Value = "4", Text = "Sumo MinSec" }
                };
                saveTempDataAktivitet();
            }
            else
            {
                AktivitetList = TempAktivitetList;
            }

            if (TempPointTypeList.Count == 0)
            {
                PointTypeList = new List<SelectListItem>()
                {
                new SelectListItem { Value = "1", Text = "MaxSec" },
                new SelectListItem { Value = "2", Text = "MinSec" },
                new SelectListItem { Value = "3", Text = "MaxPoint" },
                new SelectListItem { Value = "4", Text = "MinPoint" }
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

        }
        public void OnPostCmdSubmit()
        {
            int test;
            test = SelectedInfoId;
            //databaseSaveMetode(input)
            //AktivitetList = databaseLoadMetode()
            //saveTempDataAktivitet();
            loadTemp();
            AktivitetList = TempAktivitetList;
            PointTypeList = new List<SelectListItem>();
            PointTypeList = TempPointTypeList;
            Redirect("/OpretAktivitet");
        }

        private void saveTemp()
        {
            saveTempDataAktivitet();
            saveTempDataPointType();
        }
        private void loadTemp()
        {
            TempAktivitetList = new List<SelectListItem>();
            TempPointTypeList = new List<SelectListItem>();
            loadTempDataAktivitet();
            loadTempDataTempPointTypeList();
        }




        private void saveTempDataAktivitet()
        {
            int i = 0;
            foreach (SelectListItem _sli in AktivitetList)
            {
                TempData.Set("Aktivitet" + i, _sli);
                i++;
            }
        }

        private void loadTempDataAktivitet()
        {
            for (int i = 0; i != -1; i++)
            {
                SelectListItem _sli = TempData.Get<SelectListItem>("Aktivitet" + i);
                if (_sli != null)
                {
                    TempAktivitetList.Add(_sli);
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
