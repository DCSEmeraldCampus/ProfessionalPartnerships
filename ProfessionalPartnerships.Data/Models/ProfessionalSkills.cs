using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class ProfessionalSkills
    {
        public int ProfessionalId { get; set; }
        public int SkillId { get; set; }

        public Professionals Professional { get; set; }
        public Skills Skill { get; set; }
    }
}
