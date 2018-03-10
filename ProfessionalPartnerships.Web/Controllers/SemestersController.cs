using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class SemestersController : Controller
    {
        PartnershipsContext _db;

        public SemestersController(PartnershipsContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var semesters = _db.Semesters.ToList();
            return View(semesters);
        }

        public IActionResult AddSemesterView()
        {
            return View();
        }

        public IActionResult AddSemester(Semesters semester)
        {
            _db.Semesters.Add(semester);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult EditSemester(Semesters semester)
        {
            Semesters updateSemester = _db.Semesters.SingleOrDefault(i => i.SemesterId == semester.SemesterId);
            if (updateSemester != null)
            {
                updateSemester.Name = semester.Name;
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult EditSemesterView(int id)
        {
            Semesters semester = _db.Semesters.Find(id);
            return View(semester);
        }

        public IActionResult DeleteSemester(int id)
        {
            Semesters semester = _db.Semesters.Find(id);
            _db.Semesters.Remove(semester);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}