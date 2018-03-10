using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels.Companies
{
    public class ProfessionalsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

        public ProfessionalsViewModel(ProfessionalPartnerships.Data.Models.Professionals p)
        {
            this.FirstName = p.FirstName;
            this.LastName = p.LastName;
            this.EmailAddress = p.EmailAddress;
            this.Phone = p.Phone;
            this.IsActive = p.IsActive;
        }

        public void ApplyTo(ProfessionalPartnerships.Data.Models.Professionals p)
        {
            p.FirstName = this.FirstName;
            p.LastName = this.LastName;
            p.EmailAddress = this.EmailAddress;
            p.Phone = this.Phone;
            p.IsActive = this.IsActive;
        }
    }
}
