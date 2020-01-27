using System;
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
    public class IndexModel : CustomPageBase
    {
        [BindProperty]
        public List<SelectListItem> Placeholder { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Placeholder = new List<SelectListItem>() 
            {
                new SelectListItem { Value = "1", Text = "Test" },

            };
        }

    }
}
