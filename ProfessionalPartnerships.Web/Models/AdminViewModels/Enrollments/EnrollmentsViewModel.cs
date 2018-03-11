using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels.Enrollments
{
    public class EnrollmentsViewModel
    {
	    public EnrollmentsViewModel(ProfessionalPartnerships.Data.Models.Enrollments enrollment)
	    {
		    this.EnrollmentId = enrollment.EnrollmentId;
		    this.EnrollmentStatusName = enrollment.EnrollmentStatus?.Name;
		    this.EnrollmentStatusId = enrollment.EnrollmentStatusId;
		    this.ProgramName = $"{enrollment.Program.PointOfContactProfessional.Company.Name} - {enrollment.Program.ProgramType.Name} ({enrollment.Program.StartDate} - {enrollment.Program.EndDate})";
		    this.StudentName = $"{enrollment.Student?.FirstName} {enrollment.Student?.LastName}";
	    }

	    public int EnrollmentId { get; }
		public int EnrollmentStatusId { get; }
	    public string EnrollmentStatusName { get; }
	    public string ProgramName { get; }
	    public string StudentName { get; }
    }
}
