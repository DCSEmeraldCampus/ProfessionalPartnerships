using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class ManageSystemController : Controller
    {
        PartnershipsContext _db;
        public ManageSystemController(PartnershipsContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Interests()
        {
            var interests = _db.Interests.ToList();
            return View(interests);
        }

        public IActionResult ProgramTypes()
        {
            var programTypes = _db.ProgramTypes.ToList();
            return View(programTypes);
        }

        public IActionResult Semesters()
        {
            var semesters = _db.Semesters.ToList();
            return View(semesters);
        }

        public IActionResult Skills()
        {
            var skills = _db.Skills.ToList();
            return View(skills);
        }

        public IActionResult ConfigurationValues()
        {
            var configValues = _db.ConfigurationValues.ToList();
            return View(configValues);
        }
    }
}