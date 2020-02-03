using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PowerEvent.Helpers;
using PowerEvent.Models;

namespace PowerEvent
{
    public class HoldOpsaetningModel : PageModel
    {

        public List<Deltager> DeltagerList { get; set; }


        public void OnGet()
        {
            DeltagerList = DBAdapter.getDeltagere();
        }

        public void OnPost()
        {

        }
    }
}