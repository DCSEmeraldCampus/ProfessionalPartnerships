using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models;
using ProfessionalPartnerships.Web.Services.Interface;
using SendGrid.Helpers.Mail;

namespace ProfessionalPartnerships.Web.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly PartnershipsContext _db;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        public InvitationService(PartnershipsContext db, IEmailSender emailSender, UserManager<ApplicationUser> usermanager)
        {
            _db = db;
            _emailSender = emailSender;
            _userManager = usermanager;
        }
        public void CreateInvitation(string emailAddress, string role, int companyId, string websiteRootUrl)
        {
            // create invitation record
            var preExistingInvitation = _db.Invitations.FirstOrDefault(x =>
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
                    CompanyId = companyId == 0 ? (int?)null : (int?)companyId,
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

        public void ProcessInvitation(Guid id, string firstName, string lastName, string aspNetUserId, string emailAddress)
        {
            var invitation = _db.Invitations.FirstOrDefault(x => x.InvitationCode == id && x.IsActive);
            if (invitation == null)
            {
                throw new Exception(
                    "The invitation was not found.  Your invitation may have expired or has already been used.  Please contact the system administrator.");
            }
            invitation.IsActive = false;
            if (invitation.Role.Equals("Administrator"))
            {
                //Create an administrator record
            }
            else if (invitation.Role.Equals("Professional"))
            {
                var company = _db.Companies.FirstOrDefault(x => x.CompanyId == invitation.CompanyId);
                //Create a professional record
                _db.Professionals.Add(new Professionals
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EmailAddress = emailAddress,
                    IsActive = true,
                    Company = company,
                    AspNetUserId = aspNetUserId
                });
            }
            else if (invitation.Role.Equals("Student"))
            {
                //Create a student record
                _db.Students.Add(new Students
                {
                    FirstName = firstName,
                    LastName = lastName,
                    //IsActive = true,
                    AspNetUserId = aspNetUserId
                });
            }
            _db.SaveChanges();
        }

        public Invitations GetInvitation(Guid invitationCode)
        {
            var invitation = _db.Invitations.FirstOrDefault(x => x.InvitationCode == invitationCode && x.IsActive);
            if (invitation == null)
            {
                throw new Exception(
                    "The invitation was not found.  Your invitation may have expired or has already been used.  Please contact the system administrator.");
            }
            return invitation;
        }
    }
}