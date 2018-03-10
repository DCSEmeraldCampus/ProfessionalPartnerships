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

        [Authorize(Roles = "Professional")]
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
                    Id = program.ProgramId
                });
            }

            return View(model);
        }

        [Authorize(Roles = "Professional")]
        public async Task<IActionResult> Program(int id)
        {
            var professional = await GetCurrentProfessinal();

            var program = _db.Programs
                    .Where(p => p.PointOfContactProfessionalId == professional.ProfessionalId)
                    .Where(p => p.ProgramId == id)
                    .Include(p => p.ProgramType)
                    .SingleOrDefault();

            var model = new EditProgramViewModel();

            if (program != null)
            {
                model.ProgramFound = true;

                model.Program = program;
                model.Semesters = _db.Semesters.ToList();
                model.ProgramTypes = _db.ProgramTypes.ToList();
            }
            else
            {
                model.ProgramFound = false;
                model.Program = null;
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Professional")]
        public async Task<IActionResult> UpdateProgram(int id, EditProgramViewModel model)
        {
            if (ModelState.IsValid)
            {
                var professional = await GetCurrentProfessinal();

                var program = _db.Programs
                        .Where(p => p.PointOfContactProfessionalId == professional.ProfessionalId)
                        .Where(p => p.ProgramId == id)
                        .Include(p => p.ProgramType)
                        .SingleOrDefault();

                if (program != null)
                {
                    program.Description = model.Program.Description;
                    program.SemesterId = model.Program.SemesterId;
                    program.ProgramTypeId = model.Program.ProgramTypeId;
                    program.AvailabilityDate = model.Program.AvailabilityDate;
                    program.StartDate = model.Program.StartDate;
                    program.EndDate = model.Program.EndDate;
                    program.IsActive = model.Program.IsActive;
                    program.MaximumStudentCount = model.Program.MaximumStudentCount;

                    _db.SaveChanges();
                    return Redirect("Programs");
                }
                else
                {
                    model.Semesters = _db.Semesters.ToList();
                    model.ProgramTypes = _db.ProgramTypes.ToList();
                    return View("Program", model);
                }
            }
            else
            {
                model.Semesters = _db.Semesters.ToList();
                model.ProgramTypes = _db.ProgramTypes.ToList();
                return View("Program", model);
            }
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