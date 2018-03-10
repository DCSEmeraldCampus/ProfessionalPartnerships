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
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> ManagePrograms()
        {

            return View();
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
        public async Task<IActionResult> ManageProfessionals()
        {
            ViewData.Model = new ManageProfessionalsCertificationsViewModel
            {
                Professionals = _dbContext.Professionals
                    .Include(i => i.Certifications)
                    //.Include(i => i.Certifications.Select(s => s.CertificationType))
                    .ToList(),
                Certifications = _dbContext.Certifications.ToList(),
                CertificationTypes = _dbContext.CertificationTypes.ToList()
            };

            return View();
        }
    }
}
