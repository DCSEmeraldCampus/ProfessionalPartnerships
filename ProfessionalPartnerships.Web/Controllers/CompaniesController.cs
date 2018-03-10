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

		[HttpGet("{companyId}")]
		public async Task<IActionResult> Edit(int companyId)
		{
			var company = await _db.Companies.FindAsync(companyId);
			return View(company);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(CompaniesViewModel model)
		{
			if (ModelState.IsValid)
			{

			}
			// If we got this far, something failed, redisplay form
			return View(model);
		}


	}
}