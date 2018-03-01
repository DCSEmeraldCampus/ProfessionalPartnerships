using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class StudentReviews
    {
        public int StudentReviewId { get; set; }
        public int StudentId { get; set; }
        public int ProgramId { get; set; }
        public decimal Stars { get; set; }
        public string Note { get; set; }

        public Programs Program { get; set; }
        public Students Student { get; set; }
    }
}
