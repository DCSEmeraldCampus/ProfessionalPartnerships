using System;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class Invitations
    {
        public int InvitationId { get; set; }
        public string EmailAddress { get; set; }
        public Guid InvitationCode { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public int? CompanyId { get; set; }
        public Companies Company { get; set; }
    }
}