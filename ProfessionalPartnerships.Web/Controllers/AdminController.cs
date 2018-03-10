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

namespace ProfessionalPartnerships.Web.Controllers
{

    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        PartnershipsContext _dbContext;
        public  AdminController(PartnershipsContext db) 
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
            return View();
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



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProgram(ProgramsViewModel model)
        {
            if(ModelState.IsValid)
            {
                _dbContext.Add(new Programs
                {
                    // temporarily hardcoding relationship id values for testing
                    SemesterId = 1,
                    ProgramTypeId = 2,
                    PointOfContactProfessionalId = 1,
                    AvailabilityDate = model.AvailabilityDate,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IsActive = model.IsActive,
                    MaximumStudentCount = model.MaximumStudentCount,
                    Description = model.Description,
                    IsApproved = model.IsApproved,
                });
                _dbContext.SaveChanges();
                return View(model);
            }
            return View(model);
        }
    }
}
