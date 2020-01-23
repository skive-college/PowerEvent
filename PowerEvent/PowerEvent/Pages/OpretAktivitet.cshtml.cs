using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PowerEvent
{
    public class testModel : PageModel
    {
        [BindProperty]
        public int[] SelectedTags { get; set; }
        public SelectList TagOptions { get; set; }

        public List<SelectListItem> People { get; set; }
        public void OnGet()
        {
            People  = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "ivejrgiortgirtgTyr MaxSec" },
                new SelectListItem { Value = "2", Text = "Pete" },
                new SelectListItem { Value = "3", Text = "Katy" },
                new SelectListItem { Value = "4", Text = "Carl" }
            };
        }
        public void OnPost()
        {

        }
        public void OnPostcmdSubmit()
        {
            
        }
    }
}
