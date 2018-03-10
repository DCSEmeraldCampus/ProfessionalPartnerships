using System.ComponentModel.DataAnnotations;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels.Companies
{
    public class CompaniesViewModel
    {
		public int CompanyId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Address1 { get; set; }
        
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Zip { get; set; }
        
        public bool IsActive { get; set; }

		public int? PrimaryProfessionalId { get; set; }
    }
}
