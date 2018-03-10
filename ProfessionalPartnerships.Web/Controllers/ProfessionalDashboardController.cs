using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class ProfessionalDashboardController : BaseController {
        public ProfessionalDashboardController(PartnershipsContext db) : base(db)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}