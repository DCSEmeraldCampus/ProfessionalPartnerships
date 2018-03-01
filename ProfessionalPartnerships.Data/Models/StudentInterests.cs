using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class StudentInterests
    {
        public int StudentId { get; set; }
        public int InterestId { get; set; }

        public Interests Interest { get; set; }
        public Students Student { get; set; }
    }
}
