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
        public async Task<IActionResult> ManageCompanies()
        {
           
            return View();

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageCompanies(CompaniesViewModel model)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(new Companies
                {
                    Name = model.Name,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    City = model.City,
                    State = model.State,
                    Zip = model.Zip,
                    IsActive = model.IsActive
                });
                _dbContext.SaveChanges();
                return View(model);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewCompanies()
        {
            List<CompaniesViewModel> result = new List<CompaniesViewModel>();
            var companies = _dbContext.Companies;
            result.Add(new CompaniesViewModel { Name = "Test", Address1 = "Test1", Address2 = "Test2", City = "dublin", State = "Ohio", Zip = "23123", IsActive = true });
            //foreach(var company in companies)
            //{
            //    result.Add(new CompaniesViewModel
            //    {
            //        Name = company.Name,
            //        Address1 = company.Address1,
            //        Address2 = company.Address2,
            //        City = company.City,
            //        State = company.State,
            //        Zip = company.Zip,
            //        IsActive = company.IsActive
            //    });
            //}
            
            return  View(result);
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
    }
}
