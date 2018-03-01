using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class Interests
    {
        public Interests()
        {
            StudentInterests = new HashSet<StudentInterests>();
        }

        public int InterestId { get; set; }
        public string Name { get; set; }

        public ICollection<StudentInterests> StudentInterests { get; set; }
    }
}
