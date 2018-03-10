using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProfessionalPartnerships.Web.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class AdministratorDashboardController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministratorDashboardController(PartnershipsContext db,
            UserManager<ApplicationUser> userManager) : base(db)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }
    }
}