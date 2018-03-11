namespace ProfessionalPartnerships.Web.Models.StudentViewModels
{
    public class ProgramViewModel
    {
        public string Description { get; set; }
        public int MaximumStudentCount { get; set; }
        public string ProgramTypeName { get; set; }
        public int ProgramId { get; set; }
        public int EnrolledCount { get; set; }
        public string EnrollmentStatus { get; set; }
        public string SemesterName { get; set; }
        public bool ProgramAvailableToReview { get; set; }
        public int? Stars { get; set; }
    }
}