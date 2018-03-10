using System;
using System.Collections.Generic;

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
        public int SemesterId { get; set; }
        public int ProgramTypeId { get; set; }
        public DateTime AvailabilityDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
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
