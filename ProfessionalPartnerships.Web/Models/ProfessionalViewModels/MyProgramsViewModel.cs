using ProfessionalPartnerships.Data.Models;
using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Web.Models.ProfessionalViewModels
{
    public class MyProgramsViewModel
    {
        public IList<ProgramSummaryViewModel> Programs { get; } = new List<ProgramSummaryViewModel>();

        public class ProgramSummaryViewModel
        {
            public string Company { get; set; }
            public string ProgramType { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public int Id { get; set; }
        }
    }
}
