using System.Threading.Tasks;

namespace ProfessionalPartnerships.Web.Services.Interface
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
