using System.Collections;
using System.Collections.Generic;
using ProfessionalPartnerships.Data.Models;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels
{
    public class ManageProgramsViewModel {
        public List<ProgramTypes> ProgramTypes { get; set; }
        public List<ProgramsViewModel> Programs { get; set; }
    }
}