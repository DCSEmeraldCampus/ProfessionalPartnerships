using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class Certifications
    {
        public int CertificationId { get; set; }
        public int CertificationTypeId { get; set; }
        public int ProfessionalId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Note { get; set; }

        public CertificationTypes CertificationType { get; set; }
        public Professionals Professional { get; set; }
    }
}
