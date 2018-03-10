using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Services.Interface;
using SendGrid.Helpers.Mail;

namespace ProfessionalPartnerships.Web.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly PartnershipsContext _db;
        private readonly IEmailSender _emailSender;

        public InvitationService(PartnershipsContext db, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }
        public void CreateInvitation(string emailAddress, string role, int companyId, string websiteRootUrl)
        {
            // create invitation record
            var preExistingInvitation =  _db.Invitations.FirstOrDefault(x =>
                x.EmailAddress.Equals(emailAddress, StringComparison.CurrentCultureIgnoreCase));

            var invitationCode = Guid.NewGuid();

            if (preExistingInvitation != null && !preExistingInvitation.IsActive)
            {
                throw new Exception("This user has already been invited.  Manage them from the manage users screen.");
            }
            else if (preExistingInvitation == null)
            {
                _db.Invitations.Add(new Invitations
                {
                    EmailAddress = emailAddress,
                    InvitationCode = invitationCode,
                    Role = role,
                    CompanyId = companyId,
                    IsActive = true
                });
                _db.SaveChanges();
            }
            else
            {
                invitationCode = preExistingInvitation.InvitationCode;
            }
            //create email invitation
            var message =
                $"You have been invited to participate in the Dublin Emerald Campus Professional Partnership Program.  Please follow the this link to confirm your enrollment.<br/><br/>{websiteRootUrl}/Account/AcceptInvitation/{invitationCode}";
            //send email to invitee
            _emailSender.SendEmailAsync(emailAddress, "Dublin Emerald Campus Professional Partnership Invitation", message);
        }
    }
}