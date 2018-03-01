using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class Skills
    {
        public Skills()
        {
            ProfessionalSkills = new HashSet<ProfessionalSkills>();
        }

        public int SkillId { get; set; }
        public string Name { get; set; }

        public ICollection<ProfessionalSkills> ProfessionalSkills { get; set; }
    }
}
