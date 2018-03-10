using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using ProfessionalPartnerships.Web.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using ProfessionalPartnerships.Models.ProfessionalDashboard;
using Microsoft.EntityFrameworkCore;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class ProfessionalDashboardController : BaseController {

        private readonly UserManager<ApplicationUser> _userManager;

        public ProfessionalDashboardController(
            PartnershipsContext db,
            UserManager<ApplicationUser> userManager) : base(db)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = "Professional")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var professional = _db.Professionals
                                  .Include(p => p.Company)
                                  .Where(p => p.AspNetUserId == user.Id)
                                  .SingleOrDefault();

            var model = new ProfessionalDashboardViewModel();

            if (professional != null) 
            {
                model.Company = professional.Company;
            }

            return View(model);
        }
    }
}