using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class CertificationTypes
    {
        public CertificationTypes()
        {
            Certifications = new HashSet<Certifications>();
        }

        public int CertificationTypeId { get; set; }
        public string Name { get; set; }

        public ICollection<Certifications> Certifications { get; set; }
    }
}
