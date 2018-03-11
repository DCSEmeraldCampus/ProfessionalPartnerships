using System;
using ProfessionalPartnerships.Data.Models;

namespace ProfessionalPartnerships.Web.Services.Interface
{
    public interface IInvitationService
    {
        void CreateInvitation(string emailAddress, string role, int companyId, string websiteRootUrl);
        void ProcessInvitation(Guid id, string firstName, string lastName, string aspNetUserId, string emailAddress);
        Invitations GetInvitation(Guid invitationCode);
    }
}