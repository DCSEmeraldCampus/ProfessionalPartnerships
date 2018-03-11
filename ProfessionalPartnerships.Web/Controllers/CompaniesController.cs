using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models.AdminViewModels.Companies;

namespace ProfessionalPartnerships.Web.Controllers
{
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

        [HttpGet()]
        public async Task<IActionResult> Edit(int? id)
        {
            var company = id.HasValue
                ? await _db.Companies.Include(x => x.Professionals).SingleOrDefaultAsync(x => x.CompanyId == id)
                : new Companies();

            var viewModel = new CompaniesViewModel(company);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(CompaniesViewModel model)
        {
            var isEdit = false;
            Companies c;
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            if (model.CompanyId != default(int))
            {
                isEdit = true;
                c = await _db.Companies.FindAsync(model.CompanyId);

                model.ApplyTo(c);
            }
            else
            {
                c = new Companies
                {
                    Name = model.Name,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    City = model.City,
                    State = model.State,
                    Zip = model.Zip,
                    IsActive = model.IsActive
                };
                _db.Add(c);
            }

            await _db.SaveChangesAsync();

            if (isEdit)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Edit", new {id = c.CompanyId});
            }
        }
    }
}