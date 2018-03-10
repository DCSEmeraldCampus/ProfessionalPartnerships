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
        public async Task<IActionResult> ManagePrograms()
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

        [HttpGet]
        public ViewResult Programs()
        {
            var model = (from p in _dbContext.Programs
                         select new ProgramsViewModel()
                         {
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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProgram(ProgramsViewModel model)
        {
            if(ModelState.IsValid)
            {
                _dbContext.Add(new Programs
                {

                });
                _dbContext.SaveChanges();
                return View(model);
            }
            return View(model);
        }
    }
}
