using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DatabaseClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PowerEvent.Models;
using PowerEvent.Helpers;

namespace PowerEvent.Pages
{
    public class ForsideModel : PageModel
    {
        public List<Event> EventInfoList { get; set; }

        public void OnGet()
        {
            EventInfoList = new List<Event>();
            EventInfoList = DBAdapter.getEvent();
        }

        public void OnPost()
        {
            EventInfoList = new List<Event>();
            EventInfoList = DBAdapter.getEvent();
        }

    }
}
