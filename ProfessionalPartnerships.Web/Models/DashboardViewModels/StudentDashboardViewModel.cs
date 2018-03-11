namespace ProfessionalPartnerships.Web
{
    public class StudentDashboardViewModel
    {
        public string description { get; set; }
        public int maximumStudentCount { get; set; }
        public string programTypeName { get; set; }
        public int programId { get; set; }
        public int enrolledCount { get; set;  }
        public string enrollmentStatus { get; set; }
        public string semesterName { get; set; }
    }
}