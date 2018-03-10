using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfessionalPartnerships.Data.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class EnrollmentsController : BaseController
    {
	    public EnrollmentsController(PartnershipsContext db) : base(db) { }

	    public async Task<IActionResult> List()
	    {
		    var enrollments = await _db.Enrollments
			    .Include(e => e.Student)
				.Include(e => e.EnrollmentStatus)
				.Include(e => e.Program)
				.Include(p => p.Program.PointOfContactProfessional)
			    .Include(p => p.Program.PointOfContactProfessional.Company)
				.ToListAsync();

		    return View(enrollments);
	    }
    }
}