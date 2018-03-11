using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels.Enrollments
{
    public class EnrollmentsListViewModel : List<EnrollmentsViewModel>
    {
	    public IEnumerable<EnrollmentsViewModel> Applied
		    => this.Where(x => x.EnrollmentStatusId == 1);

	    public IEnumerable<EnrollmentsViewModel> Approved
		    => this.Where(x => x.EnrollmentStatusId == 2);

	    public IEnumerable<EnrollmentsViewModel> Declined
		    => this.Where(x => x.EnrollmentStatusId == 3);

		public EnrollmentsListViewModel(IEnumerable<ProfessionalPartnerships.Data.Models.Enrollments> enrollments)
	    {
		    foreach (var enrollment in enrollments)
		    {
			    this.Add(new EnrollmentsViewModel(enrollment));
		    }
	    }
    }
}
