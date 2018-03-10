using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class AdministratorDashboardController : BaseController
    {
        public AdministratorDashboardController(PartnershipsContext db) : base(db)
        {
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}