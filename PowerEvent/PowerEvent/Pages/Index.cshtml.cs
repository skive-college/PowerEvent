﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PowerEvent.Models;

namespace PowerEvent.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public int SelectedInfoId { get; set; }

        public List<SelectListItem> InfoList { get; set; }

        public SelectList InfoSelectList { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            InfoList = new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "Test1" },
                new SelectListItem { Value = "2", Text = "Test2" },
                new SelectListItem { Value = "3", Text = "Test3" },
                new SelectListItem { Value = "4", Text = "Test4" },
            };
            SelectedInfoId = 0;
        }

    }
}
