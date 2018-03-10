using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Web.Models.AdminViewModels;

namespace ProfessionalPartnerships.Web.Controllers
{
	[Route("admin/[controller]/[action]")]
	public class CompaniesController : Controller
	{
		[HttpGet]
		public async Task<IActionResult> List()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			if (ModelState.IsValid)
			{

			}
			// If we got this far, something failed, redisplay form
			return View();
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