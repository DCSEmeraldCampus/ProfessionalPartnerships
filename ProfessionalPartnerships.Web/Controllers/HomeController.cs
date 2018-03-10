using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class HomeController : Controller
    {
        PartnershipsContext _db;

        public HomeController(PartnershipsContext db)
        {
            _db = db;
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
