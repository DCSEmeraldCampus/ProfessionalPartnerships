using System.Collections;
using System.Collections.Generic;
using ProfessionalPartnerships.Data.Models;

namespace ProfessionalPartnerships.Web.Models.ProfessionalViewModels
{
    public class ManageCompanyUsersViewModel
    {
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public ICollection<Professionals> Professionals { get; set; }
    }
}