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
    public class testModel : CustomPageBase
    {
        [TempData]
        public string Aktivitet { get; set; }
        [BindProperty]
        public int[] SelectedTags { get; set; }
        public SelectList TagOptions { get; set; }

        public List<SelectListItem> People { get; set; }
        public void OnGet()
        {
            if (People == null)
            {
                People = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Tyr MaxSec" },
                new SelectListItem { Value = "2", Text = "Bold MaxPoint" },
                new SelectListItem { Value = "3", Text = "Race MaxPoin" },
                new SelectListItem { Value = "4", Text = "Sumo MinSec" }
            };
            }
        }
        
        public void OnPost()
        {

        }
        public void OnPostCmdSubmit()
        {
            string test;
            test = Aktivitet;
        }

        private void saveTempUsers()
        {
            int i = 0;
            foreach (User user in Users)
            {
                TempData.Set("user" + i, user);
                i++;
            }
        }

        private void loadTempUsers()
        {
            for (int i = 0; i != -1; i++)
            {
                User u = TempData.Get<User>("user" + i);
                if (u != null)
                {
                    TempUsers.Add(u);
                }
                else
                {
                    i = -2;
                }
            }
        }

    }
}
