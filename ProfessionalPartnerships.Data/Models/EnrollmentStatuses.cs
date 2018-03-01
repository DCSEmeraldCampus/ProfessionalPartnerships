using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class EnrollmentStatuses
    {
        public EnrollmentStatuses()
        {
            Enrollments = new HashSet<Enrollments>();
        }

        public int EnrollmentStatusId { get; set; }
        public string Name { get; set; }
        public bool? IsDisregardedInEnrollmentCount { get; set; }

        public ICollection<Enrollments> Enrollments { get; set; }
    }
}
