using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class SkillsController : Controller
    {
        PartnershipsContext _db;

        public SkillsController(PartnershipsContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var skills = _db.Skills.ToList();
            return View(skills);
        }

        public IActionResult AddSkillView()
        {
            return View();
        }

        public IActionResult AddSkill(Skills skill)
        {
            _db.Skills.Add(skill);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult EditSkill(Skills skill)
        {
            Skills updateSkill = _db.Skills.SingleOrDefault(i => i.SkillId == skill.SkillId);
            if (updateSkill != null)
            {
                updateSkill.Name = skill.Name;
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult EditSkillView(int id)
        {
            Skills skill = _db.Skills.Find(id);
            return View(skill);
        }

        public IActionResult DeleteSkill(int id)
        {
            Skills skill = _db.Skills.Find(id);
            _db.Skills.Remove(skill);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}