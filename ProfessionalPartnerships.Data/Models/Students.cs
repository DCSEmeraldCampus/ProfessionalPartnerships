using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class Students
    {
        public Students()
        {
            Enrollments = new HashSet<Enrollments>();
            StudentInterests = new HashSet<StudentInterests>();
            StudentReviews = new HashSet<StudentReviews>();
        }

        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
		public string AspNetUserId { get; set; }

        public ICollection<Enrollments> Enrollments { get; set; }
        public ICollection<StudentInterests> StudentInterests { get; set; }
        public ICollection<StudentReviews> StudentReviews { get; set; }
    }
}
