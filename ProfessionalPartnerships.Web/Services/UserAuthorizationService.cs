using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models;
using ProfessionalPartnerships.Web.Services.Interface;

namespace ProfessionalPartnerships.Web.Services
{
    public class UserAuthorizationService: IUserAuthorizationService
    {
        private PartnershipsContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserAuthorizationService(PartnershipsContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public bool IsUserAuthorizedToManageUsers(ClaimsPrincipal user)
        {
            var u = _userManager.FindByNameAsync(user.Identity.Name);
            if (u == null)
                return false;
            var pro = _db.Professionals.Include(x=>x.Company).FirstOrDefault(x => x.AspNetUserId == u.Result.Id);
            return pro?.Company.PrimaryProfessionalId != null && pro.Company.PrimaryProfessionalId.Value == pro.ProfessionalId;
        }
    }
}