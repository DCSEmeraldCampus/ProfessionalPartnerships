using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Constants;
using ProfessionalPartnerships.Web.Services.Interface;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace ProfessionalPartnerships.Web.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        PartnershipsContext _db;
        public EmailSender(IConfiguration configuration, PartnershipsContext db)
        {
            Configuration = configuration;
            _db = db;
        }

        public IConfiguration Configuration { get; }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailAddressValue = _db.ConfigurationValues.FirstOrDefault(configValue => configValue.Key == ConfigurationValueKeys.SystemEmailAddress);

            if (emailAddressValue == null)
            {
                throw new ApplicationException($"No ConfigurationValue configured with Key '{ConfigurationValueKeys.SystemEmailAddress}'");
            }

            var emailNameValue = _db.ConfigurationValues.FirstOrDefault(configValue => configValue.Key == ConfigurationValueKeys.SystemEmailName);

            var apiKey = Configuration["SENDGRID_APIKEY"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(emailAddressValue.Value, emailNameValue?.Value);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
