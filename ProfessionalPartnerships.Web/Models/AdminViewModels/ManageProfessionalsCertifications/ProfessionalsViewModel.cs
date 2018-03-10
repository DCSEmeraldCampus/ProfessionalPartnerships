using ProfessionalPartnerships.Data.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels.ManageProfessionalsCertifications
{
    public class ProfessionalsViewModel
    {
        public int ProfessionalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public int? CompanyId { get; set; }
        public bool IsActive { get; set; }

        // TODO MDT, get background check certification
        //public Certifications BackgroundCheckCertification { get; set; }
        public int CertificationsCount { get; set; }

        public ProfessionalsViewModel()
        {

        }

        public ProfessionalsViewModel(ProfessionalPartnerships.Data.Models.Professionals professional)
        {
            this.ProfessionalId = professional.ProfessionalId;
            this.FirstName = professional.FirstName;
            this.LastName = professional.LastName;
            this.EmailAddress = professional.EmailAddress;
            this.Phone = professional.Phone;
            this.CompanyId = professional.CompanyId;
            this.IsActive = professional.IsActive;

            // TODO MDT, not checking if cert is background check type
            this.CertificationsCount = professional.Certifications.Count();
        }

        public void ApplyTo(ProfessionalPartnerships.Data.Models.Professionals professional)
        {

            professional.ProfessionalId = this.ProfessionalId;
            professional.FirstName = this.FirstName;
            professional.LastName = this.LastName;
            professional.EmailAddress = this.EmailAddress;
            professional.Phone = this.Phone;
            professional.CompanyId = this.CompanyId;
            professional.IsActive = this.IsActive;
        }

        public Certifications CreateBackgroundCheckCertification()
        {
            // TODO MDT, get id for background check cert
            var backgroundCheckCertificationType = "Background Check";
            var backgroundCheckCertification = new Certifications()
            {
                CertificationId = 0,

                //public int CertificationId { get; set; }
                //public int CertificationTypeId { get; set; }
                //public int ProfessionalId { get; set; }
                //public DateTime EffectiveDate { get; set; }
                //public DateTime? ExpirationDate { get; set; }
                //public string Note { get; set; }

                //public CertificationTypes CertificationType { get; set; }
                //public Professionals Professional { get; set; }

            };

            return backgroundCheckCertification;
        }
    }
}
