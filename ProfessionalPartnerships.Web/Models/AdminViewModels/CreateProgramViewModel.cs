using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProfessionalPartnerships.Data.Models;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels
{
    public class ProgramsViewModel
    {
        public int ProgramId { get; set; }

        public List<SelectListItem> SemesterOptions { get; set; }
        public string SemesterName { get; set; }

        [Required]
        public string SelectedSemesterId { get; set; }

        public List<SelectListItem> ProgramTypeOptions { get; set; }
        public string ProgramTypeName { get; set; }

        [Required]
        public string SelectedProgramTypeId { get; set; }

        [Required]
        public DateTime AvailabilityDate { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int MaximumStudentCount { get; set; }
        
        public List<SelectListItem> PointOfContactProfessionalOptions { get; set; }
        public string PointOfContactName { get; set; }

        public string SelectedPointOfContactProfessionalId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        public bool ActionWasSuccessful { get; set; }
    }
}
