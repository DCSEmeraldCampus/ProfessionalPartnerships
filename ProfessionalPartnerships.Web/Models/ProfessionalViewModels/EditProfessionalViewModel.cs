namespace ProfessionalPartnerships.Web.Models.ProfessionalViewModels
{
    public class EditProfessionalViewModel {
        public string ErrorMessage { get; set; }
        public int ProfessionalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
    }
}