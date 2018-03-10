using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Constants;
using ProfessionalPartnerships.Web.Models;
using ProfessionalPartnerships.Web.Models.DashboardViewModels;
using ProfessionalPartnerships.Web.Services;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class HomeController : Controller
    {
        PartnershipsContext _db;
        private readonly IEmailSender _emailSender;

        [TempData]
        public string StatusMessage { get; set; }

        public HomeController(PartnershipsContext db, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Index", "AdministratorDashboard");
            }
            if (User.IsInRole("Student"))
            {
                return RedirectToAction("Index", "StudentDashboard");
            }
            if (User.IsInRole("Professional"))
            {
                return RedirectToAction("Index", "ProfessionalDashboard");
            }
            return View("Index");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(ContactUsViewModel model)
        {
            var localUrl = Url.Action(nameof(Index), controller: null, values: null, protocol: null, host: null, fragment: "contact");
            if (!ModelState.IsValid)
            {
                StatusMessage = "Error: " + String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)));
                return new LocalRedirectResult(localUrl);
            }

            var message = $@"
-Contact Info-
Name: {model.Name}
Email: {model.Email}
Phone Number: {model.PhoneNumber}

-Email-
Subject: {model.Subject}
Message: {model.Message}
";

            var emailAddressValue = _db.ConfigurationValues.FirstOrDefault(configValue => configValue.Key == ConfigurationValueKeys.SystemEmailAddress);

            if(emailAddressValue == null)
            {
                throw new ApplicationException($"No ConfigurationValue configured with Key '{ConfigurationValueKeys.SystemEmailAddress}'");
            }
            await _emailSender.SendEmailAsync(emailAddressValue.Value, "Contact Us Message", message);

            StatusMessage = "Email sent successfully!";
            return new LocalRedirectResult(localUrl);
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
