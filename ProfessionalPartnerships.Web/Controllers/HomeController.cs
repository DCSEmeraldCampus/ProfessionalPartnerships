using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Web.Models;
    
using Microsoft.EntityFrameworkCore;
using ProfessionalPartnerships.Data.Models;

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
            /*
            _db.Students.Add(new Students
            {
                FirstName = "Joe",
                LastName = "Bogner"
            });

            var students = _db.Students.ToList();

            _db.SaveChanges();

            return View(students);
            */
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
    }
}
