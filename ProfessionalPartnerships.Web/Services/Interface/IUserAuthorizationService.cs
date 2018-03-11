using System.Security.Claims;

namespace ProfessionalPartnerships.Web.Services.Interface
{
    public interface IUserAuthorizationService
    {
        bool IsUserAuthorizedToManageUsers(ClaimsPrincipal user);
    }
}