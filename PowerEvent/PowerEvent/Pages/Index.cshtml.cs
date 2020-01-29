using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DatabaseClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PowerEvent.Models;
using PowerEvent.Helpers;

namespace PowerEvent.Pages
{
    public class IndexModel : PageModel
    {
        public List<Event> EventInfoList { get; set; }


        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

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
