using ProfessionalPartnerships.Data.Models;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Web.Models.ProfessionalViewModels
{
    public class EditProgramViewModel
    {
        public bool ProgramFound { get; set; }
        public Programs Program { get; set; }
        public IEnumerable<Semesters> Semesters { get; set; }
        public IEnumerable<ProgramTypes> ProgramTypes { get; set; }
    }
}
