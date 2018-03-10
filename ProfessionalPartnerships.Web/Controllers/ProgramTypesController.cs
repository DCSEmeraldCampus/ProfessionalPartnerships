using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class ProgramTypesController : Controller
    {
        PartnershipsContext _db;

        public ProgramTypesController(PartnershipsContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var programTypes = _db.ProgramTypes.ToList();
            return View(programTypes);
        }

        public IActionResult AddProgramTypeView()
        {
            return View();
        }

        public IActionResult AddProgramType(ProgramTypes programType)
        {
            _db.ProgramTypes.Add(programType);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult EditProgramType(ProgramTypes programType)
        {
            ProgramTypes updateProgramType = _db.ProgramTypes.SingleOrDefault(i => i.ProgramTypeId == programType.ProgramTypeId);
            if (updateProgramType != null)
            {
                updateProgramType.Name = programType.Name;
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult EditProgramTypeView(int id)
        {
            ProgramTypes programType = _db.ProgramTypes.Find(id);
            return View(programType);
        }

        public IActionResult DeleteProgramType(int id)
        {
            ProgramTypes programType = _db.ProgramTypes.Find(id);
            _db.ProgramTypes.Remove(programType);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}