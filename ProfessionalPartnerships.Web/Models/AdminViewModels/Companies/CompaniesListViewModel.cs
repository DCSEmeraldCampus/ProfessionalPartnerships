using System.Collections.Generic;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels.Companies
{
    public class CompaniesListViewModel : List<CompaniesViewModel>
    {
        public CompaniesListViewModel(IEnumerable<ProfessionalPartnerships.Data.Models.Companies> companies)
        {
            foreach (var company in companies)
            {
                this.Add(new CompaniesViewModel(company));
            }

        }
    }
}
