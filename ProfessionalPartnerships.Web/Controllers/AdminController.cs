using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProfessionalPartnerships.Web.Models.AdminViewModels;
using ProfessionalPartnerships.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProfessionalPartnerships.Web.Controllers
{

    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        PartnershipsContext _dbContext;
        public AdminController(PartnershipsContext db)
        {
            _dbContext = db;
        }

        [HttpGet]
        public async Task<IActionResult> ManageUsers()
        {
            ViewData.Model = new ManageUsersViewModel
            {
                Companies = _dbContext.Companies.ToList()
            };
            return View();
        }

        [HttpGet]
        public ViewResult Programs()
        {
            var model = (from p in _dbContext.Programs
                         select new ProgramsViewModel()
                         {
                             ProgramId = p.ProgramId,
                             ProgramTypeName = _dbContext.ProgramTypes.Where(pt => pt.ProgramTypeId == p.ProgramTypeId).FirstOrDefault().Name,
                             SemesterName = _dbContext.Semesters.Where(s => s.SemesterId == p.SemesterId).FirstOrDefault().Name,
                             PointOfContactName = _dbContext.Professionals.Where(pr => pr.ProfessionalId == p.PointOfContactProfessionalId).FirstOrDefault().FirstName
                                            + " " + _dbContext.Professionals.Where(pr => pr.ProfessionalId == p.PointOfContactProfessionalId).FirstOrDefault().LastName,
                             AvailabilityDate = p.AvailabilityDate,
                             StartDate = p.StartDate,
                             EndDate = p.EndDate,
                             IsActive = p.IsActive,
                             MaximumStudentCount = p.MaximumStudentCount,
                             Description = p.Description,
                             IsApproved = p.IsApproved
                         });

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ManagePrograms()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateProgram()
        {
            var vm = new ProgramsViewModel();
            vm.SemesterOptions = _dbContext.Semesters.Select(x => new SelectListItem() { Value = x.SemesterId.ToString(), Text = x.Name }).ToList();
            vm.ProgramTypeOptions = _dbContext.ProgramTypes.Select(x => new SelectListItem() { Value = x.ProgramTypeId.ToString(), Text = x.Name }).ToList();
            vm.PointOfContactProfessionalOptions = _dbContext.Professionals.Select(x => new SelectListItem() { Value = x.ProfessionalId.ToString(), Text = x.FirstName + " " + x.LastName }).ToList();
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> EditProgram(string id)
        {
            var program = _dbContext.Programs.Find(int.Parse(id));
            var vm = new ProgramsViewModel();
            vm.SemesterOptions = _dbContext.Semesters.Select(x => new SelectListItem() { Value = x.SemesterId.ToString(), Text = x.Name }).ToList();
            vm.SelectedSemesterId = program.SemesterId.ToString();
            vm.ProgramTypeOptions = _dbContext.ProgramTypes.Select(x => new SelectListItem() { Value = x.ProgramTypeId.ToString(), Text = x.Name }).ToList();
            vm.SelectedProgramTypeId = program.ProgramTypeId.ToString();
            vm.PointOfContactProfessionalOptions = _dbContext.Professionals.Select(x => new SelectListItem() { Value = x.ProfessionalId.ToString(), Text = x.FirstName + " " + x.LastName }).ToList();
            vm.SelectedPointOfContactProfessionalId = program.PointOfContactProfessionalId.ToString();
            vm.IsActive = program.IsActive;
            vm.IsApproved = program.IsApproved;
            vm.MaximumStudentCount = program.MaximumStudentCount;
            vm.ProgramId = program.ProgramId;
            vm.StartDate = program.StartDate;
            vm.AvailabilityDate = program.AvailabilityDate;
            vm.EndDate = program.EndDate;
            vm.Description = program.Description;
            return View(vm);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagePrograms(ProgramsViewModel model)
        {
            if (ModelState.IsValid)
            {

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private int? ParseNullableInt(string val)
        {
            int? result = null;
            if (!string.IsNullOrEmpty(val))
            {
                result = int.Parse(val);
            }
            return result;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProgram(ProgramsViewModel model)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(new Programs
                {
                    SemesterId = int.Parse(model.SelectedSemesterId),
                    ProgramTypeId = int.Parse(model.SelectedProgramTypeId),
                    PointOfContactProfessionalId = ParseNullableInt(model.SelectedPointOfContactProfessionalId),
                    AvailabilityDate = model.AvailabilityDate,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IsActive = model.IsActive,
                    MaximumStudentCount = model.MaximumStudentCount,
                    Description = model.Description,
                    IsApproved = model.IsApproved,
                });
                _dbContext.SaveChanges();
                model.ActionWasSuccessful = true;
                return View(model);
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProgram(ProgramsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var program = _dbContext.Programs.Find(model.ProgramId);
                _dbContext.Update(program);

                program.SemesterId = int.Parse(model.SelectedSemesterId);
                program.ProgramTypeId = int.Parse(model.SelectedProgramTypeId);
                program.PointOfContactProfessionalId = ParseNullableInt(model.SelectedPointOfContactProfessionalId);
                program.AvailabilityDate = model.AvailabilityDate;
                program.StartDate = model.StartDate;
                program.EndDate = model.EndDate;
                program.IsActive = model.IsActive;
                program.MaximumStudentCount = model.MaximumStudentCount;
                program.Description = model.Description;
                program.IsApproved = model.IsApproved;

                _dbContext.SaveChanges();
                model.ActionWasSuccessful = true;
                return View(model);
            }
            return View(model);
        }
    }
}
