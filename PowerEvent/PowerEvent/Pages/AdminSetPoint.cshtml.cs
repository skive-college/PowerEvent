using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PowerEvent
{
    public class AdminSetPointModel : PageModel
    {
        [BindProperty]
        public bool IsChecked { get; set; }

        [BindProperty]
        public int skift { get; set; }
        public int TempSkift { get; set; }
        public void OnGet()
        {
        }
        public void OnPost()
        {

        }
        public void OnPostCmdSkift()
        {
            _ = IsChecked == true;
        }
    }
}