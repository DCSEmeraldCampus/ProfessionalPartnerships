using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class Semesters
    {
        public Semesters()
        {
            Programs = new HashSet<Programs>();
        }

        public int SemesterId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<Programs> Programs { get; set; }
    }
}
