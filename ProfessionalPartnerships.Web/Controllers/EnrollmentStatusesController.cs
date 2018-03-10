using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class EnrollmentStatusesController : Controller
    {
        PartnershipsContext _db;

        public EnrollmentStatusesController(PartnershipsContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var enrollmentStatuses = _db.EnrollmentStatuses.ToList();
            return View(enrollmentStatuses);
        }

        public IActionResult AddEnrollmentStatusView()
        {
            return View();
        }

        public IActionResult AddEnrollmentStatus(EnrollmentStatuses status)
        {
            _db.EnrollmentStatuses.Add(status);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult EditEnrollmentStatus(EnrollmentStatuses status)
        {
            EnrollmentStatuses updateStatus = _db.EnrollmentStatuses.SingleOrDefault(s => s.EnrollmentStatusId == status.EnrollmentStatusId);
            if (updateStatus != null)
            {
                updateStatus.Name = status.Name;
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult EditEnrollmentStatusView(int id)
        {
            EnrollmentStatuses status = _db.EnrollmentStatuses.Find(id);
            return View(status);
        }

        public IActionResult DeleteEnrollmentStatus(int id)
        {
            EnrollmentStatuses status = _db.EnrollmentStatuses.Find(id);
            _db.EnrollmentStatuses.Remove(status);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}