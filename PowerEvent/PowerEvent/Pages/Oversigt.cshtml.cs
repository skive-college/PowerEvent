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
    public class Oversigt : PageModel
    {
        [BindProperty]
        public int SelectedEvent { get; set; }

        public List<Event> EventList { get; set; }

        public List<Hold> HoldListe { get; set; }

        public List<Deltager> DeltagerList { get; set; }


        public void OnGet()
        {
            EventList = new List<Event>();
            DeltagerList = new List<Deltager>();
            EventList = DBAdapter.getEvent();
            DeltagerList = DBAdapter.getDeltagere();
        }

        public void OnPost()
        {
            EventList = new List<Event>();
            DeltagerList = new List<Deltager>();
            EventList = DBAdapter.getEvent();
            DeltagerList = DBAdapter.getDeltagere();
        }

        public void OnPostCmdEvent()
        {

        }

    }
}
