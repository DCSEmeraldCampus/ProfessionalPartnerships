using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class Professionals
    {
        public Professionals()
        {
            Certifications = new HashSet<Certifications>();
            Companies = new HashSet<Companies>();
            ProfessionalReviews = new HashSet<ProfessionalReviews>();
            ProfessionalSkills = new HashSet<ProfessionalSkills>();
            Programs = new HashSet<Programs>();
        }

        public int ProfessionalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public int? CompanyId { get; set; }
        public bool IsActive { get; set; }
        public string AspNetUserId { get; set; }

        public Companies Company { get; set; }
        public ICollection<Certifications> Certifications { get; set; }
        public ICollection<Companies> Companies { get; set; }
        public ICollection<ProfessionalReviews> ProfessionalReviews { get; set; }
        public ICollection<ProfessionalSkills> ProfessionalSkills { get; set; }
        public ICollection<Programs> Programs { get; set; }
    }
}
