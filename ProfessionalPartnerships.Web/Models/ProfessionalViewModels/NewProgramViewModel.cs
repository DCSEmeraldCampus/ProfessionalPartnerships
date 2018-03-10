using ProfessionalPartnerships.Data.Models;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Web.Models.ProfessionalViewModels
{
    public class NewProgramViewModel
    {
        public Programs Program { get; set; }
        public IEnumerable<Semesters> Semesters { get; set; }
        public IEnumerable<ProgramTypes> ProgramTypes { get; set; }
    }
}
