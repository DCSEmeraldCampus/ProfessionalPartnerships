using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels
{
    public class CompaniesViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address1 { get; set; }
        
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Zip { get; set; }
        
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
    }
}
