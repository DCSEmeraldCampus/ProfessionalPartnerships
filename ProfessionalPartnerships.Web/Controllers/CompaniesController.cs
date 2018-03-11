using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models.AdminViewModels.Companies;

namespace ProfessionalPartnerships.Web.Controllers
{
    [Route("admin/[controller]/[action]")]
    public class CompaniesController : BaseController
    {
        public CompaniesController(PartnershipsContext db) : base(db) { }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var companies = await _db.Companies.Include(x => x.Professionals).ToListAsync();
            var viewModel = new CompaniesListViewModel(companies);
            return View(viewModel);
        }

        [HttpGet("{companyId?}")]
        public async Task<IActionResult> Edit(int? companyId)
        {
            var company = companyId.HasValue
                ? await _db.Companies.Include(x => x.Professionals).SingleOrDefaultAsync(x => x.CompanyId == companyId)
                : new Companies();

            var viewModel = new CompaniesViewModel(company);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(CompaniesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            if (model.CompanyId != default(int))
            {
                var company = await _db.Companies.FindAsync(model.CompanyId);

                model.ApplyTo(company);
            }
            else
            {
                _db.Add(new Companies
                {
                    Name = model.Name,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    City = model.City,
                    State = model.State,
                    Zip = model.Zip,
                    IsActive = model.IsActive
                });
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("List");
        }
    }
}