using ProfessionalPartnerships.Data.Models;
using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Web.Models.ProfessionalViewModels
{
    public class MyProgramsViewModel
    {
        public IList<ProgramSummaryViewModel> Programs { get; } = new List<ProgramSummaryViewModel>();
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public class ProgramSummaryViewModel
        {
            public string Company { get; set; }
            public string ProgramType { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string Semester { get; set; }
            public string AvailabilityDate { get; set; }
            public int CurrentStudents { get; set; }
            public int MaximumStudents { get; set; }
            public int CurrentApplications { get; set; }
            public bool IsApproved { get; set; }
            public bool IsActive { get; set; }
            public int ProgramId { get; set; }
        }
    }
}
