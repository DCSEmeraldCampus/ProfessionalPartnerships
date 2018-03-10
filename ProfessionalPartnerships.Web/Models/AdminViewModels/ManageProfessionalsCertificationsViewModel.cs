using System.Collections.Generic;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models.AdminViewModels.ManageProfessionalsCertifications;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels
{
    public class ManageProfessionalsCertificationsViewModel
    {
        public List<Professionals> Professionals { get; set; }
        public List<Certifications> Certifications { get; set; }
        public List<CertificationTypes> CertificationTypes { get; set; }

        public ProfessionalsCertificationsViewModel ProfessionalsCertificationsViewModel { get; set; }
    }
}