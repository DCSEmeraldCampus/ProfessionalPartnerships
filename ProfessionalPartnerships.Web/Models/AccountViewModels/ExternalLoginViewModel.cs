using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalPartnerships.Web.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public Guid InvitationCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
