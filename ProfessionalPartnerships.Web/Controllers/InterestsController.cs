using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class InterestsController : Controller
    {
        PartnershipsContext _db;

        public InterestsController(PartnershipsContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var interests = _db.Interests.ToList();
            return View(interests);
        }

        public IActionResult AddInterestView()
        {
            return View();
        }

        public IActionResult AddInterest(Interests interest)
        {
            _db.Interests.Add(interest);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult EditInterest(Interests interest)
        {
            Interests updateInterest = _db.Interests.SingleOrDefault(i => i.InterestId == interest.InterestId);
            if (updateInterest != null)
            {
                updateInterest.Name = interest.Name;
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult EditInterestView(int id)
        {
            Interests interest = _db.Interests.Find(id);
            return View(interest);
        }

        public IActionResult DeleteInterest(int id)
        {
            Interests interest = _db.Interests.Find(id);
            _db.Interests.Remove(interest);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}