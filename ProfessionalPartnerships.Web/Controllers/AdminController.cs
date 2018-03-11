﻿using System;
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

    [Route("[controller]/[action]")]
    public class AdminController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminController(PartnershipsContext db,
              UserManager<ApplicationUser> userManager) : base(db)
        {
            _userManager = userManager;
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
        public async Task<IActionResult> EditProgram(string id)
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
                return View(model);
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SearchUsers()
        {
            SearchUserViewModel model = new SearchUserViewModel();
            model.AllUsers = new List<UsersViewModel>();
            model.Roles = new List<SelectListItem>();
            model.Roles.Add(new SelectListItem { Text = "Administrator", Value = "Administrator" });
            model.Roles.Add(new SelectListItem { Text = "Professional", Value = "Professional" });
            model.Roles.Add(new SelectListItem { Text = "Student", Value = "Student" });
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SearchUsers(SearchUserViewModel SelectedRole)
        {
            SearchUserViewModel model = new SearchUserViewModel();
            model.AllUsers = new List<UsersViewModel>();
            if (ModelState.IsValid)
            {
                switch (SelectedRole.SelectRole)
                {
                    case "Administrator":
                        var AdminList = _db.Administrators.Where(x => x.IsActive == SelectedRole.IsActive).ToList();
                        if(AdminList!=null && AdminList.Count>0)
                        {
                            foreach(var admin in AdminList)
                            {
                                UsersViewModel newUser = new UsersViewModel();
                                newUser.UserName = (admin.FirstName +" "+ admin.LastName).ToString();
                                newUser.UserID = admin.AdminId;
                                newUser.Email = admin.EmailAddress;
                                newUser.RoleName = SelectedRole.SelectRole;
                                newUser.IsActive = admin.IsActive;
                                model.AllUsers.Add(newUser);
                            }
                           
                        }
                        else
                        {
                            ViewData["Message"] = "No Users found for Selected Role";
                        }
                        break;
                    case "Professional":
                        var UserList = _db.Professionals.Where(x => x.IsActive == SelectedRole.IsActive).ToList();
                        if (UserList != null && UserList.Count > 0)
                        {
                            foreach (var users in UserList)
                            {
                                UsersViewModel newUser = new UsersViewModel();
                                newUser.UserName = (users.FirstName + " " +users.LastName).ToString();
                                newUser.UserID = users.ProfessionalId;
                                newUser.Email = users.EmailAddress;
                                newUser.RoleName = SelectedRole.SelectRole;
                                newUser.IsActive = users.IsActive;
                                model.AllUsers.Add(newUser);
                            }
                        }
                        else
                        {
                            ViewData["Message"] = "No Users found for Selected Role";
                        }
                        break;
                    case "Student":
                        var StudentList = _db.Students.Where(x => x.IsActive == SelectedRole.IsActive).ToList();
                        if( StudentList!=null && StudentList.Count>0)
                        {
                            foreach(var student in StudentList)
                            {
                                UsersViewModel newUser = new UsersViewModel();
                                newUser.UserName = (student.FirstName +" " +student.LastName).ToString();
                                newUser.UserID = student.StudentId;
                                newUser.Email = student.EmailAddress;
                                newUser.RoleName = SelectedRole.SelectRole;
                               newUser.IsActive = student.IsActive;
                                model.AllUsers.Add(newUser);
                            }
                        }
                        else
                        {
                            ViewData["Message"] = "No Users found for Selected Role";
                        }
                        break;

                        default:
                        break;
                }              
            }
            model.Roles = new List<SelectListItem>();
            model.Roles.Add(new SelectListItem { Text = "Administrator", Value = "Administrator" });
            model.Roles.Add(new SelectListItem { Text = "Professional", Value = "Professional" });
            model.Roles.Add(new SelectListItem { Text = "Student", Value = "Student" });
            model.SelectRole = SelectedRole.SelectRole;
            return View(model);
        }

        [HttpGet("{UserId?}/{RoleName?}")]
        public async Task<IActionResult> EditUser(int UserId,string RoleName)
        {
           
            return View();
        }

    }
}