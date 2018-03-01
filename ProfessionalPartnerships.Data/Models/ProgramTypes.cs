using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class ProgramTypes
    {
        public ProgramTypes()
        {
            Programs = new HashSet<Programs>();
        }

        public int ProgramTypeId { get; set; }
        public string Name { get; set; }

        public ICollection<Programs> Programs { get; set; }
    }
}
