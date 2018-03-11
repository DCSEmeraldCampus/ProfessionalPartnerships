using System.Collections.Generic;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels.Enrollments
{
    public class EnrollmentsListViewModel : List<EnrollmentsViewModel>
    {
		public EnrollmentsListViewModel(IEnumerable<ProfessionalPartnerships.Data.Models.Enrollments> enrollments)
	    {
		    foreach (var enrollment in enrollments)
		    {
			    this.Add(new EnrollmentsViewModel(enrollment));
		    }
	    }
    }
}
