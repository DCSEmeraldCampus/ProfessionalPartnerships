using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ProfessionalPartnerships.Web.Services.Interface;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace ProfessionalPartnerships.Web.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private string _fromAddress;
        private string _fromName;
        public EmailSender(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            _fromAddress = "DublinEmeraldCampus@gmail.com";
            _fromName = "Dublin Emerald Campus";

            var apiKey = Configuration["SENDGRID_APIKEY"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_fromAddress, _fromName);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
