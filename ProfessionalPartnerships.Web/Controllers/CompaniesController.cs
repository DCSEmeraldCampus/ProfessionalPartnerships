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
			var companies = await _db.Companies.ToListAsync();
			return View(companies);
		}

		[HttpGet("{companyId?}")]
		public async Task<IActionResult> Edit(int companyId)
		{
			var company = await _db.Companies.FindAsync(companyId) ?? new Companies();
			return View(company);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Save([FromForm]Companies model)
		{
			if (!ModelState.IsValid)
			{
				return View("Edit", model);
			}

			//TODO: placeholder until switch to proper viewmodel
			if (model.CompanyId != default(int))
			{
				//pre-existing company, query and update
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
				await _db.SaveChangesAsync();
			}

			return RedirectToAction("List");
		}


	}
}