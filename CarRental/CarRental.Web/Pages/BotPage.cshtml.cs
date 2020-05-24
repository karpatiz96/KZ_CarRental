using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CarRental.Web.Pages
{
    public class BotPageModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public BotPageModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Secret { get; set; }

        public void OnGet()
        {
            Secret = _configuration.GetValue<String>("BotSercret");
        }
    }
}