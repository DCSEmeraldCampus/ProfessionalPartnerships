﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProfessionalPartnerships.Web.Models.AdminViewModels
{
    public class ProgramsViewModel
    {
        [Required]
        public int ProgramId { get; set; }
        [Required]
        public DateTime AvailabilityDate { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int MaximumStudentCount { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        public string ProgramTypeName { get; set; }
        public string SemesterName { get; set; }
        public string PointOfContactName { get; set; }

    }
}
