using System;
using System.Collections.Generic;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class Administrators
    {
        public int AdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string AspNetUserId { get; set; }        
    }
}
