using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PowerEvent
{
    public class ScoreBoardModel : PageModel
    {
        public string EventName { get; set; }
        public string TeamEt { get; set; }
        public string TeamTo { get; set; }
        public void OnGet()
        {
            EventName = "test";
            TeamEt = "teamet";
            TeamTo = "teamto";
        }
    }
}