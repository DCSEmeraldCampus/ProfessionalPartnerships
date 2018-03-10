using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProfessionalPartnerships.Web.Models;
using ProfessionalPartnerships.Web.Services;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public async Task<IActionResult> SendEmail()
        //{
        //    EmailSender sender = new EmailSender(_configuration);
        //    await sender.SendEmailAsync("rick@rickdoes.net", "Dublin Emerald Campus Email Test",
        //        "<p>This is your test email</p>");
        //    return StatusCode(200);
        //}
    }
}
