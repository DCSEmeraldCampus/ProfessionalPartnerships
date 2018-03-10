using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProfessionalPartnerships.Web.Models.AdminViewModels;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProfessionalPartnerships.Web.Controllers
{

    [Route("[controller]/[action]")]
    public class AdminController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;

       

        //public AdminController(PartnershipsContext db) : base(db) { }
        public AdminController(PartnershipsContext db,
               UserManager<ApplicationUser> userManager) : base(db)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ManagePrograms()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ManageUsers()
        {
            ViewData.Model = new ManageUsersViewModel
            {
                Companies = _db.Companies.ToList()
            };
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagePrograms(ProgramsViewModel model)
        {
            if (ModelState.IsValid)
            {

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SearchUsers()
        {
            SearchUserViewModel model = new SearchUserViewModel();
            model.Applicationuser = new List<ApplicationUser>();
            model.roles = new List<SelectListItem>();
            model.roles.Add (new SelectListItem { Text="Administrator",Value= "Administrator" });
            model.roles.Add(new SelectListItem { Text = "Professional", Value = "Professional" });
            model.roles.Add(new SelectListItem { Text = "Student", Value = "Student" });
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SearchUsers(SearchUserViewModel SelectedRole)
        {
            SearchUserViewModel model = new SearchUserViewModel();
            var UserList = _userManager.GetUsersInRoleAsync(SelectedRole.SelectRole);
            model.Applicationuser = new List<ApplicationUser>();
            if (UserList.Result != null && UserList.Result.Count > 0)
            {
                foreach (var users in UserList.Result)
                {
                    ApplicationUser newUser = new ApplicationUser();                  
                    newUser.UserName = users.UserName;
                    newUser.Id = users.Id;
                    newUser.Email = users.Email;
                    model.Applicationuser.Add(newUser);
                }
            }
            else
            {
                ViewData["Message"] = "No Users found for Selected Role";
            }
            model.roles = new List<SelectListItem>();
            model.roles.Add(new SelectListItem { Text = "Administrator", Value = "Administrator" });
            model.roles.Add(new SelectListItem { Text = "Professional", Value = "Professional" });
            model.roles.Add(new SelectListItem { Text = "Student", Value = "Student" });
            model.SelectRole = SelectedRole.SelectRole;
            return View(model);
        }
    }
}
