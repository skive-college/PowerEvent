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
            DeltagerList = new List<Deltager>();
            EventList = DBAdapter.getEvent();
            checkEventList();

        }

        public void OnPost()
        {
            EventList = DBAdapter.getEvent();
            checkEventList();
        }



        private void checkEventList()
        {
            //on click for dropdown event liste script
            int i = -1;
            try
            {
                i = int.Parse(Request.Query["id"]);
            }
            catch
            {

            }
            if (i != -1)
            {
                DeltagerList = DBAdapter.getDeltagere(i);
            }
            else
            {
                EventList = DBAdapter.getEvent();
            }
        }


    }
}
