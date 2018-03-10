using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class CertificationTypesController : Controller
    {
        PartnershipsContext _db;

        public CertificationTypesController(PartnershipsContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var certTypes = _db.CertificationTypes.ToList();
            return View(certTypes);
        }

        public IActionResult AddCertificationTypeView()
        {
            return View();
        }

        public IActionResult AddCertificationType(CertificationTypes type)
        {
            _db.CertificationTypes.Add(type);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult EditCertificationType(CertificationTypes type)
        {
            CertificationTypes updateType = _db.CertificationTypes.SingleOrDefault(t => t.CertificationTypeId == type.CertificationTypeId);
            if (updateType != null)
            {
                updateType.Name = type.Name;
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult EditCertificationTypeView(int id)
        {
            CertificationTypes type = _db.CertificationTypes.Find(id);
            return View(type);
        }

        public IActionResult DeleteCertificationType(int id)
        {
            CertificationTypes type = _db.CertificationTypes.Find(id);
            _db.CertificationTypes.Remove(type);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}