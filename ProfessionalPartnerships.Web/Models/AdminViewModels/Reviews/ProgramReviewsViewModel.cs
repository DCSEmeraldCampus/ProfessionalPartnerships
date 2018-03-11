using System.Collections.Generic;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels
{
    public class ProgramReviewsViewModel
    {
        public int ProgramId { get; set; }
        public decimal? AverageReview { get; set; }
        public string ProgramDescription { get; set; }
        public int NumberOfReviews { get; set; }
        public string ProgramType { get; set; }
        public string ProgramSemester { get; set; }
        public List<ProgramReview> ProgramReviews { get; set; }
    }
}