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
        public List<SelectListItem> Roles { get; set; }
        public List<UsersViewModel> Users { get; set; }
    }

    public class UsersViewModel
    {
        [Required]
        public int UserId { get; set; }
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "Role")]
        public string RoleName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> Roles { get; set; }
    }

    public class UpdateUserViewModel
    {
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string RoleName { get; set; }
        public string OriginalRoleName { get; set; }
    }
}