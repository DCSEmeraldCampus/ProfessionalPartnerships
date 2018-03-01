using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class ProfessionalReviews
    {
        public int ProfessionalReviewId { get; set; }
        public int ProfessionalId { get; set; }
        public int ProgramId { get; set; }
        public int Stars { get; set; }
        public string Note { get; set; }

        public Professionals Professional { get; set; }
        public Programs Program { get; set; }
    }
}
