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
        // public List<ApplicationUser> Applicationuser { get; set; }
      public List<UsersViewModel> AllUsers { get; set; }
        [Required]
        public string SelectRole { get; set; }
        public bool IsActive { get; set; }
        public List<SelectListItem> Roles { get; set; }

    }
    public class UsersViewModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string RoleName { get; set; }
       
    }
}