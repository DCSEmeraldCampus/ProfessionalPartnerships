using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProfessionalPartnerships.Web.Controllers
{
    public class StudentDashboardController : BaseController
    {
        
        public StudentDashboardController(PartnershipsContext db) : base(db)
        {

        }

        public ActionResult Index()
        {          
            return View();
        }

        [HttpGet]
        public JsonResult GetPrograms()
        {
            return Json(Database.Programs.ToList());
        }
    }
}
