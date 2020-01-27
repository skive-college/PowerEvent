using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PowerEvent.Models;

namespace PowerEvent.Pages
{
    public class testModel : PageModel
    {
        [BindProperty]
        public string Aktivitet { get; set; }
        [BindProperty]
        public int[] SelectedTags { get; set; }
        public SelectList TagOptions { get; set; }
        [BindProperty]
        public List<SelectListItem> AktivitetList { get; set; }

        public List<SelectListItem> TempAktivitetList { get; set; }
        [BindProperty]
        public List<SelectListItem> PointTypeList { get; set; }

        public List<SelectListItem> TempPointTypeList { get; set; }
        public void OnGet()
        {
            AktivitetList = new List<SelectListItem>();
            TempAktivitetList = new List<SelectListItem>();
            TempPointTypeList = new List<SelectListItem>();
            loadTempAktivitet();
            loadTempTempPointTypeList();
            if (TempAktivitetList.Count == 0)
            {
                AktivitetList = new List<SelectListItem>()
                {
                new SelectListItem { Value = "1", Text = "Tyr MaxSec" },
                new SelectListItem { Value = "2", Text = "Bold MaxPoint" },
                new SelectListItem { Value = "3", Text = "Race MaxPoint" },
                new SelectListItem { Value = "4", Text = "Sumo MinSec" }
                };
                saveTempAktivitet();
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
                saveTempPointType();
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
            foreach (var item in TempPointTypeList)
            {
                if (item.Selected == true)
                {

                }
            }

        }

        private void saveTempAktivitet()
        {
            int i = 0;
            foreach (SelectListItem _sli in AktivitetList)
            {
                TempData.Set("Aktivitet" + i, _sli);
                i++;
            }
        }

        private void loadTempAktivitet()
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
                    i = -2;
                }
            }
        }
        private void saveTempPointType()
        {
            int i = 0;
            foreach (SelectListItem _sli in PointTypeList)
            {
                TempData.Set("PointType" + i, _sli);
                i++;
            }
        }
        private void loadTempTempPointTypeList()
        {
            for (int i = 0; i != -1; i++)
            {
                SelectListItem _sli = TempData.Get<SelectListItem>("PointType" + i);
                if (_sli != null)
                {
                    TempPointTypeList.Add(_sli);
                }
                else
                {
                    i = -2;
                }
            }
        }
    }
}
