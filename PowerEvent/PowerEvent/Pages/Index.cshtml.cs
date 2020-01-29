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
        [BindProperty]
        public int SelectedInfoId { get; set; }

        [BindProperty]
        public List<SelectListItem> InfoList { get; set; }

        public List<Event> EventInfoList { get; set; }


        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            InfoList = new List<SelectListItem>();
            EventInfoList = new List<Event>();
            EventInfoList = DBAdapter.getEvent();
            foreach (var item in EventInfoList)
            {
                InfoList.Add(new SelectListItem { Value = item.Id + "", Text = item.Navn });
            }

            //InfoList = new List<SelectListItem>()
            //{
            //    new SelectListItem { Value = "1", Text = "Test1" },
            //    new SelectListItem { Value = "2", Text = "Test2" },
            //    new SelectListItem { Value = "3", Text = "Test3" },
            //    new SelectListItem { Value = "4", Text = "Test4" },
            //};
        }


    }
}
