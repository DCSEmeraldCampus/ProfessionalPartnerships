using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class Programs
    {
        public Programs()
        {
            Enrollments = new HashSet<Enrollments>();
            ProfessionalReviews = new HashSet<ProfessionalReviews>();
            StudentReviews = new HashSet<StudentReviews>();
        }

        public int ProgramId { get; set; }

        [Display(Name = "Semester")]
        public int SemesterId { get; set; }
        
        [Display(Name = "Program Type")]
        public int ProgramTypeId { get; set; }

        [Display(Name = "Availability Date")]
        [DataType(DataType.Date)]

        public DateTime AvailabilityDate { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

        [Display(Name = "Maximum Student Count")]
        public int MaximumStudentCount { get; set; }
        public int? PointOfContactProfessionalId { get; set; }
        public string Description { get; set; }
        public bool IsApproved { get; set; }

        public Professionals PointOfContactProfessional { get; set; }
        public ProgramTypes ProgramType { get; set; }
        public Semesters Semester { get; set; }
        public ICollection<Enrollments> Enrollments { get; set; }
        public ICollection<ProfessionalReviews> ProfessionalReviews { get; set; }
        public ICollection<StudentReviews> StudentReviews { get; set; }
    }
}
