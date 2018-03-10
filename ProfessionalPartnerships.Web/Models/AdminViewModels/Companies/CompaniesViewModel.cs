using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels.Companies
{
    public class CompaniesViewModel
    {
        public int CompanyId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Address 1")]
        public string Address1 { get; set; }

        [DisplayName("Address 2")]
        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zip { get; set; }

        [DisplayName("Active?")]
        public bool IsActive { get; set; }

        [DisplayName("Primary Professional")]
        public int? PrimaryProfessionalId { get; set; }

        public List<ProfessionalsViewModel> AssociatedProfessionals { get; set; }

        public CompaniesViewModel()
        {

        }

        public CompaniesViewModel(ProfessionalPartnerships.Data.Models.Companies company)
        {
            this.CompanyId = company.CompanyId;
            this.Name = company.Name;
            this.Address1 = company.Address1;
            this.Address2 = company.Address2;
            this.City = company.City;
            this.State = company.State;
            this.Zip = company.Zip;
            this.IsActive = company.IsActive;
            this.PrimaryProfessionalId = company.PrimaryProfessionalId;
            this.AssociatedProfessionals = company.Professionals.Select(x => new ProfessionalsViewModel(x)).ToList();
        }

        public void ApplyTo(ProfessionalPartnerships.Data.Models.Companies company)
        {
            company.Name = this.Name;
            company.Address1 = this.Address1;
            company.Address2 = this.Address2;
            company.City = this.City;
            company.State = this.State;
            company.Zip = this.Zip;
            company.IsActive = this.IsActive;
            company.PrimaryProfessionalId = this.PrimaryProfessionalId;
        }
    }
}
