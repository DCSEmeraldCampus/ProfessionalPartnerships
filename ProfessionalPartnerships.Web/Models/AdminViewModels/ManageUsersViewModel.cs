using System.Collections;
using System.Collections.Generic;
using ProfessionalPartnerships.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels
{
    public class ManageUsersViewModel
    {
        public List<ProfessionalPartnerships.Data.Models.Companies> Companies { get; set; }
    }
    public class SearchUserViewModel
    {
        public List<ApplicationUser> Applicationuser { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public string SelectRole { get; set; }
        public Guid UserID { get; set; }
        public List<SelectListItem> roles { get; set; }
    }
}