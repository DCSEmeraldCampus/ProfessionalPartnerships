using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models.AdminViewModels.Enrollments;

namespace ProfessionalPartnerships.Web.Controllers
{
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
				.Include(p => p.Program.PointOfContactProfessional)
			    .Include(p => p.Program.PointOfContactProfessional.Company)
				.ToListAsync();

		    var viewModel = new EnrollmentsListViewModel(enrollments);

		    return View(viewModel);
	    }

	    [HttpPost]
	    public async Task<IActionResult> UpdateEnrollmentStatus(int enrollmentId, int newStatusId)
	    {
		    var enrollment = await _db.Enrollments.FindAsync(enrollmentId);

		    if (enrollment == null)
		    {
			    return StatusCode(404, $"Unable to locate Enrollment record #{enrollmentId}");
		    }

		    enrollment.EnrollmentStatusId = newStatusId;

		    await _db.SaveChangesAsync();

			return StatusCode(200);
	    }
	}
}