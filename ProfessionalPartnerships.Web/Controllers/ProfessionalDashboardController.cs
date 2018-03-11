using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using ProfessionalPartnerships.Web.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using ProfessionalPartnerships.Models.ProfessionalDashboard;
using Microsoft.EntityFrameworkCore;
using ProfessionalPartnerships.Web.Models.ProfessionalViewModels;

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
            var professional = await GetCurrentProfessinal();

            var model = new ProfessionalDashboardViewModel();

            if (professional != null) 
            {
                model.Company = professional.Company;
            }

            return View(model);
        }

        [Authorize]
        public IActionResult ManageUsers()
        {
            var u = _userManager.FindByNameAsync(User.Identity.Name);
            var pro = _db.Professionals.Include(x => x.Company).Include(x=>x.Company.Professionals).FirstOrDefault(x => x.AspNetUserId == u.Result.Id);
            var model = new ManageCompanyUsersViewModel
            {
                CompanyName = pro.Company.Name,
                CompanyId = pro.CompanyId.Value,
                Professionals = pro.Company.Professionals
            };
            return View(model);
        }

        public async Task<IActionResult> Programs()
        {
            var professional = await GetCurrentProfessinal();
            var compnay = professional.Company;

            var programs = _db.Programs.Where(p => p.PointOfContactProfessionalId == professional.ProfessionalId).Include(p => p.ProgramType);

            var model = new MyProgramsViewModel();

            foreach(var program in programs)
            {
                model.Programs.Add(new MyProgramsViewModel.ProgramSummaryViewModel
                {
                    Company = compnay.Name,
                    ProgramType = program.ProgramType.Name,
                    StartDate = program.ProgramType.ShowTime 
                        ? $"{program.StartDate.ToShortDateString()} {program.StartDate.ToShortTimeString()}"
                        : program.StartDate.ToShortDateString(),
                    EndDate = program.ProgramType.ShowTime
                         ? $"{program.EndDate.ToShortDateString()} {program.EndDate.ToShortTimeString()}"
                         : program.EndDate.ToShortDateString(),
                });
            }

            return View(model);
        }

        public IActionResult EditProfessional(int id)
        {
            var pro = _db.Professionals.FirstOrDefault(p => p.ProfessionalId == id);
            if (pro == null)
            {
                ViewData.Model = new EditProfessionalViewModel
                {
                    ErrorMessage = "The professional could not be found.  Please try again"
                };
            }
            else
            {
                ViewData.Model = new EditProfessionalViewModel
                {
                    ProfessionalId = pro.ProfessionalId,
                    FirstName = pro.FirstName,
                    LastName = pro.LastName,
                    Email = pro.EmailAddress,
                    Phone = pro.Phone,
                    IsActive = pro.IsActive,
                };
            }
            return View();
        }

        public IActionResult UpdateProfessional(EditProfessionalViewModel professional)
        {
            var pro = _db.Professionals.FirstOrDefault(p => p.ProfessionalId == professional.ProfessionalId);
            pro.FirstName = professional.FirstName;
            pro.LastName = professional.LastName;
            pro.EmailAddress = professional.Email;
            pro.Phone = professional.Phone;
            pro.IsActive = professional.IsActive;
            _db.SaveChanges();
            return RedirectToAction("ManageUsers");
        }

        private async Task<Professionals> GetCurrentProfessinal()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var professional = _db.Professionals
                                  .Include(p => p.Company)
                                  .Where(p => p.AspNetUserId == user.Id)
                                  .SingleOrDefault();

            return professional;
        }


    }
}