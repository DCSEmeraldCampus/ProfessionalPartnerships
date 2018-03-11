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
using Microsoft.AspNetCore.Mvc.Rendering;
using ProfessionalPartnerships.Web.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    
    public class AdminController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminController(PartnershipsContext db,
              UserManager<ApplicationUser> userManager) : base(db)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult ManageUsers()
        {
            var users = new List<UsersViewModel>();
            users.AddRange(_db.Administrators.Select(p => new UsersViewModel
            {
                Name = p.FirstName + " " + p.LastName,
                UserId = p.AdminId,
                Email = p.EmailAddress,
                RoleName = "Administrator",
                IsActive = p.IsActive
            }));
            users.AddRange(_db.Professionals.Select(p => new UsersViewModel
            {
                Name = p.FirstName + " " + p.LastName,
                UserId = p.ProfessionalId,
                Email = p.EmailAddress,
                RoleName = "Professional",
                IsActive = p.IsActive
            }));
            users.AddRange(_db.Students.Select(p => new UsersViewModel
            {
                Name = p.FirstName + " " + p.LastName,
                UserId = p.StudentId,
                Email = p.EmailAddress,
                RoleName = "Student",
                IsActive = p.IsActive
            }));

            ViewData.Model = new ManageUsersViewModel
            {
                Companies = _db.Companies.ToList(),
                Users = users
            };
            return View();
        }

        [HttpGet]
        public ViewResult Programs()
        {
            var model = (from p in _db.Programs
                         select new ProgramsViewModel()
                         {
                             ProgramId = p.ProgramId,
                             ProgramTypeName = _db.ProgramTypes.Where(pt => pt.ProgramTypeId == p.ProgramTypeId).FirstOrDefault().Name,
                             SemesterName = _db.Semesters.Where(s => s.SemesterId == p.SemesterId).FirstOrDefault().Name,
                             PointOfContactName = _db.Professionals.Where(pr => pr.ProfessionalId == p.PointOfContactProfessionalId).FirstOrDefault().FirstName
                                            + " " + _db.Professionals.Where(pr => pr.ProfessionalId == p.PointOfContactProfessionalId).FirstOrDefault().LastName,
                             AvailabilityDate = p.AvailabilityDate,
                             StartDate = p.StartDate,
                             EndDate = p.EndDate,
                             IsActive = p.IsActive,
                             MaximumStudentCount = p.MaximumStudentCount,
                             Description = p.Description,
                             IsApproved = p.IsApproved
                         });

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProgram()
        {
            var vm = new ProgramsViewModel();
            vm.SemesterOptions = _db.Semesters.Select(x => new SelectListItem() { Value = x.SemesterId.ToString(), Text = x.Name }).ToList();
            vm.ProgramTypeOptions = _db.ProgramTypes.Select(x => new SelectListItem() { Value = x.ProgramTypeId.ToString(), Text = x.Name }).ToList();
            vm.PointOfContactProfessionalOptions = _db.Professionals.Select(x => new SelectListItem() { Value = x.ProfessionalId.ToString(), Text = x.FirstName + " " + x.LastName }).ToList();
            return View(vm);
        }

        [HttpGet]
        public IActionResult EditProgram(string id)
        {
            var program = _db.Programs.Find(int.Parse(id));
            var vm = new ProgramsViewModel();
            vm.SemesterOptions = _db.Semesters.Select(x => new SelectListItem() { Value = x.SemesterId.ToString(), Text = x.Name }).ToList();
            vm.SelectedSemesterId = program.SemesterId.ToString();
            vm.ProgramTypeOptions = _db.ProgramTypes.Select(x => new SelectListItem() { Value = x.ProgramTypeId.ToString(), Text = x.Name }).ToList();
            vm.SelectedProgramTypeId = program.ProgramTypeId.ToString();
            vm.PointOfContactProfessionalOptions = _db.Professionals.Select(x => new SelectListItem() { Value = x.ProfessionalId.ToString(), Text = x.FirstName + " " + x.LastName }).ToList();
            vm.SelectedPointOfContactProfessionalId = program.PointOfContactProfessionalId.ToString();
            vm.IsActive = program.IsActive;
            vm.IsApproved = program.IsApproved;
            vm.MaximumStudentCount = program.MaximumStudentCount;
            vm.ProgramId = program.ProgramId;
            vm.StartDate = program.StartDate;
            vm.AvailabilityDate = program.AvailabilityDate;
            vm.EndDate = program.EndDate;
            vm.Description = program.Description;
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProgram(string id)
        {
            var program = _db.Programs.Find(int.Parse(id));
            _db.Remove(program);
            await _db.SaveChangesAsync();
            return View();
        }

        private int? ParseNullableInt(string val)
        {
            int? result = null;
            if (!string.IsNullOrEmpty(val))
            {
                result = int.Parse(val);
            }
            return result;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProgram(ProgramsViewModel model)
        {
            if (ModelState.IsValid)
            {
                _db.Add(new Programs
                {
                    SemesterId = int.Parse(model.SelectedSemesterId),
                    ProgramTypeId = int.Parse(model.SelectedProgramTypeId),
                    PointOfContactProfessionalId = ParseNullableInt(model.SelectedPointOfContactProfessionalId),
                    AvailabilityDate = model.AvailabilityDate,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IsActive = model.IsActive,
                    MaximumStudentCount = model.MaximumStudentCount,
                    Description = model.Description,
                    IsApproved = model.IsApproved,
                });
                await _db.SaveChangesAsync();
                model.ActionWasSuccessful = true;
                return View("Programs");
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProgram(ProgramsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var program = _db.Programs.Find(model.ProgramId);
                _db.Update(program);

                program.SemesterId = int.Parse(model.SelectedSemesterId);
                program.ProgramTypeId = int.Parse(model.SelectedProgramTypeId);
                program.PointOfContactProfessionalId = ParseNullableInt(model.SelectedPointOfContactProfessionalId);
                program.AvailabilityDate = model.AvailabilityDate;
                program.StartDate = model.StartDate;
                program.EndDate = model.EndDate;
                program.IsActive = model.IsActive;
                program.MaximumStudentCount = model.MaximumStudentCount;
                program.Description = model.Description;
                program.IsApproved = model.IsApproved;

                await _db.SaveChangesAsync();
                model.ActionWasSuccessful = true;
                return View(model);
            }
            return View(model);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> SearchUsers(SearchUserViewModel SelectedRole)
        //{
        //    SearchUserViewModel model = new SearchUserViewModel();
        //    model.AllUsers = new List<UsersViewModel>();
        //    if (ModelState.IsValid)
        //    {
        //        switch (SelectedRole.SelectRole)
        //        {
        //            case "Administrator":
        //                var AdminList = _db.Administrators.Where(x => x.IsActive == SelectedRole.IsActive).ToList();
        //                if(AdminList!=null && AdminList.Count>0)
        //                {
        //                    foreach(var admin in AdminList)
        //                    {
        //                        UsersViewModel newUser = new UsersViewModel();
        //                        newUser.UserName = (admin.FirstName +" "+ admin.LastName).ToString();
        //                        newUser.UserID = admin.AdminId;
        //                        newUser.Email = admin.EmailAddress;
        //                        newUser.RoleName = SelectedRole.SelectRole;
        //                        newUser.IsActive = admin.IsActive;
        //                        model.AllUsers.Add(newUser);
        //                    }

        //                }
        //                else
        //                {
        //                    ViewData["Message"] = "No Users found for Selected Role";
        //                }
        //                break;
        //            case "Professional":
        //                var UserList = _db.Professionals.Where(x => x.IsActive == SelectedRole.IsActive).ToList();
        //                if (UserList != null && UserList.Count > 0)
        //                {
        //                    foreach (var users in UserList)
        //                    {
        //                        UsersViewModel newUser = new UsersViewModel();
        //                        newUser.UserName = (users.FirstName + " " +users.LastName).ToString();
        //                        newUser.UserID = users.ProfessionalId;
        //                        newUser.Email = users.EmailAddress;
        //                        newUser.RoleName = SelectedRole.SelectRole;
        //                        newUser.IsActive = users.IsActive;
        //                        model.AllUsers.Add(newUser);
        //                    }
        //                }
        //                else
        //                {
        //                    ViewData["Message"] = "No Users found for Selected Role";
        //                }
        //                break;
        //            case "Student":
        //                var StudentList = _db.Students.Where(x => x.IsActive == SelectedRole.IsActive).ToList();
        //                if( StudentList!=null && StudentList.Count>0)
        //                {
        //                    foreach(var student in StudentList)
        //                    {
        //                        UsersViewModel newUser = new UsersViewModel();
        //                        newUser.UserName = (student.FirstName +" " +student.LastName).ToString();
        //                        newUser.UserID = student.StudentId;
        //                        newUser.Email = student.EmailAddress;
        //                        newUser.RoleName = SelectedRole.SelectRole;
        //                       newUser.IsActive = student.IsActive;
        //                        model.AllUsers.Add(newUser);
        //                    }
        //                }
        //                else
        //                {
        //                    ViewData["Message"] = "No Users found for Selected Role";
        //                }
        //                break;

        //                default:
        //                break;
        //        }              
        //    }
        //    model.Roles = new List<SelectListItem>();
        //    model.Roles.Add(new SelectListItem { Text = "Administrator", Value = "Administrator" });
        //    model.Roles.Add(new SelectListItem { Text = "Professional", Value = "Professional" });
        //    model.Roles.Add(new SelectListItem { Text = "Student", Value = "Student" });
        //    model.SelectRole = SelectedRole.SelectRole;
        //    return View(model);
        //}

        public IActionResult EditAdministrator(int id)
        {
            UsersViewModel user = null;
            var admin = _db.Administrators.FirstOrDefault(x => x.AdminId == id);
            var roles = new List<string> {"Administrator", "Professional", "Student"};
            if (admin != null)
            {
                user = new UsersViewModel
                {
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Email = admin.EmailAddress,
                    IsActive = admin.IsActive,
                    UserId = id,
                    RoleName = "Administrator",
                    Roles = roles
                };
            }
            else
            {
                ViewData["Message"] = "No Users found for Selected UserID";
            }
            return View("EditUser", user);
        }

        public IActionResult EditProfessional(int id)
        {
            UsersViewModel user = null;
            var admin = _db.Professionals.FirstOrDefault(x => x.ProfessionalId == id);
            var roles = new List<string> {"Administrator", "Professional", "Student"};
            if (admin != null)
            {
                user = new UsersViewModel
                {
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Email = admin.EmailAddress,
                    IsActive = admin.IsActive,
                    UserId = id,
                    RoleName = "Professional",
                    Roles = roles
                };
            }
            else
            {
                ViewData["Message"] = "No Users found for Selected UserID";
            }
            return View("EditUser", user);
        }
        public IActionResult EditStudent(int id)
        {
            UsersViewModel user = null;
            var roles = new List<string> {"Administrator", "Professional", "Student"};
            var admin = _db.Students.FirstOrDefault(x => x.StudentId == id);
            if (admin != null)
            {
                user = new UsersViewModel
                {
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Email = admin.EmailAddress,
                    IsActive = admin.IsActive,
                    UserId = id,
                    RoleName = "Student",
                    Roles = roles
                };
            }
            else
            {
                ViewData["Message"] = "No Users found for Selected UserID";
            }
            return View("EditUser", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateUser(UpdateUserViewModel user)
        {
            var success = false;
            if (user.OriginalRoleName != user.RoleName)
            {
                //Do work to remove record from old role and create in new role
                ViewData["Message"] = "Input is not valid";
            }
            else
            {
                //Save user properties
                switch (user.RoleName)
                {
                    case "Administrator":
                        var administrator = _db.Administrators.FirstOrDefault(x => x.AdminId == user.UserId);
                        if (administrator != null)
                        {
                            administrator.FirstName = user.FirstName;
                            administrator.LastName = user.LastName;
                            administrator.EmailAddress = user.Email;
                            administrator.IsActive = user.IsActive;
                            _db.SaveChanges();
                            ViewData["Message"] = "Administrator Record Updated Successfully";
                            success = true;
                        }
                        else
                        {
                            ViewData["Message"] = "Input is not valid";
                        }
                        break;
                    case "Professional":
                        var pro = _db.Professionals.FirstOrDefault(x => x.ProfessionalId == user.UserId);
                        if (pro != null)
                        {
                            var professional = _db.Professionals.SingleOrDefault(x => x.ProfessionalId == user.UserId);
                            if (professional != null)
                            {
                                professional.FirstName = user.FirstName;
                                professional.LastName = user.LastName;
                                professional.EmailAddress = user.Email;
                                professional.IsActive = user.IsActive;
                                _db.SaveChanges();
                                ViewData["Message"] = "Professional Record Updated Successfully";
                                success = true;
                            }
                        }
                        else
                        {
                            ViewData["Message"] = "Input is not valid";
                        }
                        break;
                    case "Student":
                        var student = _db.Students.FirstOrDefault(x => x.StudentId == user.UserId);
                        if (student != null)
                        {
                            student.FirstName = user.FirstName;
                            student.LastName = user.LastName;
                            student.EmailAddress = user.Email;
                            student.IsActive = user.IsActive;
                            _db.SaveChanges();
                            ViewData["Message"] = "Student Record Updated Successfully";
                            success = true;
                        }
                        else
                        {
                            ViewData["Message"] = "Input is not valid";
                        }
                        break;
                }
            }
            if (success)
            {
                return RedirectToAction("ManageUsers");
            }

            var roles = new List<string> { "Administrator", "Professional", "Student" };

            var editUser = new UsersViewModel
            {
                Roles = roles,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                UserId = user.UserId,
                RoleName = user.OriginalRoleName,
                ErrorMessage = ViewData["Message"].ToString()
            };
            return View("EditUser", editUser);
        }
    }
}