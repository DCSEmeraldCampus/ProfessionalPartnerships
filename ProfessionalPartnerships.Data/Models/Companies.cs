using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class Companies
    {
        public Companies()
        {
            Invitations = new HashSet<Invitations>();
            Professionals = new HashSet<Professionals>();
        }

        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int? PrimaryProfessionalId { get; set; }
        public bool IsActive { get; set; }

        public Professionals PrimaryProfessional { get; set; }
        public ICollection<Invitations> Invitations { get; set; }
        public ICollection<Professionals> Professionals { get; set; }       
    }
}
