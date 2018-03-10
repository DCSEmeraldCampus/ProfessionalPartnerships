using System.Collections.Generic;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels.ManageProfessionalsCertifications
{
    public class ProfessionalsListViewModel : List<ProfessionalsViewModel>
    {
        public ProfessionalsListViewModel(IEnumerable<ProfessionalPartnerships.Data.Models.Professionals> professionals)
        {
            foreach (var professional in professionals)
            {
                this.Add(new ProfessionalsViewModel(professional));
            }

        }
    }
}
