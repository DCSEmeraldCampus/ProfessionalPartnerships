using System.Collections.Generic;
using ProfessionalPartnerships.Data.Models;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels
{
    public class ManageProfessionalsCertificationsViewModel
    {
        public List<Professionals> Professionals { get; set; }
        public List<Certifications> Certifications { get; set; }
        public List<CertificationTypes> CertificationTypes { get; set; }
    }
}