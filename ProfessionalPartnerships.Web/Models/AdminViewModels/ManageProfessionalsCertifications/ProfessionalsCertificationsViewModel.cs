using ProfessionalPartnerships.Data.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels.ManageProfessionalsCertifications
{
    public class ProfessionalsCertificationsViewModel
    {
        public int CertificationId { get; set; }
        public int CertificationTypeId { get; set; }
        public int ProfessionalId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Note { get; set; }

        public CertificationTypes CertificationType { get; set; }
        public Professionals Professional { get; set; }


        public ProfessionalsCertificationsViewModel()
        {
        }

        public Certifications CreateCertification()
        {
            var certification = new Certifications()
            {
                // TODO MDT 03-10-18, probably not needed (just being explicit)
                CertificationId = 0,

                CertificationTypeId = this.CertificationTypeId,
                ProfessionalId = this.ProfessionalId,
                EffectiveDate = this.EffectiveDate,
                ExpirationDate = this.ExpirationDate,
                Note = this.Note
            };

            return certification;
        }
    }
}
