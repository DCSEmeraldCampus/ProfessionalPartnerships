using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models.AdminViewModels.Enrollments;

namespace ProfessionalPartnerships.Web.Controllers
{
	[Route("[controller]/[action]")]
    public class EnrollmentsController : BaseController
    {
	    public EnrollmentsController(PartnershipsContext db) : base(db) { }

		[HttpGet]
	    public async Task<IActionResult> List()
	    {
		    var enrollments = await _db.Enrollments
			    .Include(e => e.Student)
				.Include(e => e.EnrollmentStatus)
				.Include(e => e.Program)
				.Include(e => e.Program.ProgramType)
				.Include(e => e.Program.Semester)
				.Include(p => p.Program.PointOfContactProfessional)
			    .Include(p => p.Program.PointOfContactProfessional.Company)
				.ToListAsync();

		    var viewModel = new EnrollmentsListViewModel(enrollments);

		    return View(viewModel);
	    }

	    [HttpGet("{enrollmentId?}")]
	    public async Task<IActionResult> Edit(int? enrollmentId)
	    {
		    var enrollment = enrollmentId.HasValue
			    ? await _db.Enrollments.FindAsync(enrollmentId)
			    : new Enrollments();
			
		    var viewModel = new EnrollmentsViewModel(enrollment);

			PopulateSelectLists(viewModel);
			return View(viewModel);
		}

	    [HttpPost]
	    [ValidateAntiForgeryToken]
	    public async Task<IActionResult> Save(EnrollmentsViewModel model)
	    {
		    if (!ModelState.IsValid)
		    {
			    PopulateSelectLists(model);
				return View("Edit", model);
		    }

		    if (model.EnrollmentId == default(int))
		    {
			    var enrollment = new Enrollments();
				model.ApplyTo(enrollment);
			    _db.Enrollments.Add(enrollment);
		    }
		    else
		    {
			    var enrollment = await _db.Enrollments.FindAsync(model.EnrollmentId);
			    model.ApplyTo(enrollment);
			}

		    await _db.SaveChangesAsync();

		    return RedirectToAction("List");
	    }

		[HttpGet("{enrollmentId}")]
	    public async Task<IActionResult> Delete(int enrollmentId)
	    {
		    var enrollment = await _db.Enrollments.FindAsync(enrollmentId);

		    if (enrollment != null)
		    {
			    _db.Enrollments.Remove(enrollment);
			    await _db.SaveChangesAsync();
		    }

			return RedirectToAction("List");
	    }

	    private void PopulateSelectLists(EnrollmentsViewModel model)
	    {
		    model.EnrollmentStatuses = _db.EnrollmentStatuses
			    .OrderBy(es => es.Name)
			    .Select(es => new SelectListItem
			    {
				    Text = es.Name,
				    Value = es.EnrollmentStatusId.ToString()
			    })
			    .ToList();

		    model.Students = _db.Students
				.OrderBy(s => s.LastName)
				.ThenBy(s => s.FirstName)
				.Select(s => new SelectListItem
				{
					Text = $"{s.LastName}, {s.FirstName}",
					Value = s.StudentId.ToString()
				})
				.ToList();

		    model.Programs = _db.Programs
			    .Include(e => e.ProgramType)
			    .Include(e => e.Semester)
			    .Include(p => p.PointOfContactProfessional)
			    .Include(p => p.PointOfContactProfessional.Company)
				.OrderBy(p => p.AvailabilityDate)
				.Select(p => new SelectListItem
				{
					Text = $"{p.PointOfContactProfessional.Company.Name} - {p.ProgramType.Name} ({p.StartDate} - {p.EndDate})",
					Value = p.ProgramId.ToString()
				})
				.ToList();
		}
	}
}