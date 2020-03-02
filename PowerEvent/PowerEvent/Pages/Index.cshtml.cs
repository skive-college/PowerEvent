using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PowerEvent.Helpers;
using PowerEvent.Models;

namespace PowerEvent
{
    public class IndexModel : PageModel
    {
        public Login CurrentLogin { get; set; }

        public string ValgtGuiElemement { get; set; }

        public string TxtUsername { get; set; }

        public string TxtPassword { get; set; }

        public IActionResult OnGet()
        {
            loadTempDataLogin();
            checkScript();

            if (ValgtGuiElemement == "cmdLogin")
            {
                if (TxtUsername != "" && TxtPassword != "")
                {
                    CurrentLogin = DBAdapter.verifyLogin(TxtUsername, TxtPassword);
                    if (CurrentLogin != new Login())
                    {
                        saveTempDataLogin();
                    }
                }
            }
            return this.Page();
        }

        public void OnPost()
        {

        }


        private void checkScript()
        {
            try
            {
                TxtUsername = Request.Query["TxtUsername"];
            }
            catch
            {
            }
            try
            {
                TxtPassword = Request.Query["TxtPassword"];
            }
            catch
            {
            }
            ValgtGuiElemement = Request.Query["ValgtGuiElemement"];
        }


        private void saveTempDataLogin()
        {
            TempData.Set("CurrentLogin", CurrentLogin);
        }

        private void loadTempDataLogin()
        {
            Login tempLogin = TempData.Peek<Login>("CurrentLogin");
            if (tempLogin != null)
            {
                CurrentLogin = tempLogin;
            }
        }

    }
}