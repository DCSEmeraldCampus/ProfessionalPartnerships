using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProfessionalPartnerships.Data.Models;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels.Enrollments
{
	public class EnrollmentsViewModel
    {
	    public int EnrollmentId { get; set; }

	    [Required]
	    [DisplayName("Enrollment Status")]
		public int EnrollmentStatusId { get; set; }

	    [Required]
	    [DisplayName("Program")]
		public int ProgramId { get; set; }

	    [Required]
	    [DisplayName("Student")]
		public int StudentId { get; set; }
	    public string Notes { get; set; }

		public string EnrollmentStatusName { get; }
	    public string ProgramName { get; }
	    public string StudentName { get; }
	    public string PointOfContactName { get; }
	    public string Semester { get; }

		public List<SelectListItem> EnrollmentStatuses { get; set; }
		public List<SelectListItem> Students { get; set; }
		public List<SelectListItem> Programs { get; set; }

	    public EnrollmentsViewModel()
	    {
		    
	    }

		public EnrollmentsViewModel(ProfessionalPartnerships.Data.Models.Enrollments enrollment)
	    {
		    this.EnrollmentId = enrollment.EnrollmentId;
		    this.EnrollmentStatusId = enrollment.EnrollmentStatusId;
		    this.StudentId = enrollment.StudentId;
		    this.ProgramId = enrollment.ProgramId;
		    this.Notes = enrollment.Note;

			this.EnrollmentStatusName = enrollment.EnrollmentStatus?.Name;
		    this.ProgramName = $"{enrollment.Program?.PointOfContactProfessional?.Company?.Name} - {enrollment.Program?.ProgramType?.Name} ({enrollment.Program?.StartDate} - {enrollment.Program?.EndDate})";
		    this.StudentName = $"{enrollment.Student?.FirstName} {enrollment.Student?.LastName}";
		    this.PointOfContactName = $"{enrollment.Program?.PointOfContactProfessional?.FirstName} {enrollment.Program?.PointOfContactProfessional?.LastName}";
		    this.Semester = enrollment.Program?.Semester?.Name;   
	    }
		
		public void ApplyTo(ProfessionalPartnerships.Data.Models.Enrollments enrollment)
		{
			enrollment.ProgramId = this.ProgramId;
			enrollment.StudentId = this.StudentId;
			enrollment.EnrollmentStatusId = this.EnrollmentStatusId;
			enrollment.Note = this.Notes;
	    }
	}
}
