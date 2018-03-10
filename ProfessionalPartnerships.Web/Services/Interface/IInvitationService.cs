namespace ProfessionalPartnerships.Web.Services.Interface
{
    public interface IInvitationService
    {
        void CreateInvitation(string emailAddress, string role, int companyId, string websiteRootUrl);
    }
}