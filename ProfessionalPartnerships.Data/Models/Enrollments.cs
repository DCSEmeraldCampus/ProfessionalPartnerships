using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class Enrollments
    {
        public int EnrollmentId { get; set; }
        public int EnrollmentStatusId { get; set; }
        public int ProgramId { get; set; }
        public int StudentId { get; set; }
        public string Note { get; set; }

        public EnrollmentStatuses EnrollmentStatus { get; set; }
        public Programs Program { get; set; }
        public Students Student { get; set; }
    }
}
