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
using Microsoft.EntityFrameworkCore;
using ProfessionalPartnerships.Web.Models.AdminViewModels.Enrollments.Enums;

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
            var model = _db.Programs
                .Include(i => i.ProgramType)
                .Include(i => i.Semester)
                .Include(i => i.PointOfContactProfessional)
                .Include(i => i.Enrollments).ThenInclude(i => i.EnrollmentStatus)
                .Select(s => new ProgramsViewModel()
                {
                    ProgramId = s.ProgramId,

                    ProgramTypeName =
                        s.ProgramType != null
                        ? s.ProgramType.Name
                        : string.Empty,

                    SemesterName =
                        s.Semester != null
                        ? s.Semester.Name
                        : string.Empty,

                    PointOfContactName =
                        s.PointOfContactProfessional != null
                        ? $"{s.PointOfContactProfessional.FirstName} {s.PointOfContactProfessional.LastName}"
                        : string.Empty,

                    AvailabilityDate = s.AvailabilityDate,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    IsActive = s.IsActive,
                    EnrollmentCount =
                        s.Enrollments != null
                        ? s.Enrollments.Count(c => !c.EnrollmentStatus.IsDisregardedInEnrollmentCount ?? false)
                        : 0,
                    MaximumStudentCount = s.MaximumStudentCount,
                    Description = s.Description,
                    IsApproved = s.IsApproved
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
            var programId = int.Parse(id);
            var program = _db.Programs.Find(programId);

            var vm = new ProgramsViewModel();
            vm.SemesterOptions = _db.Semesters
                .Select(x => new SelectListItem() { Value = x.SemesterId.ToString(), Text = x.Name })
                .ToList();
            vm.SelectedSemesterId = program.SemesterId.ToString();
            vm.ProgramTypeOptions = _db.ProgramTypes
                .Select(x => new SelectListItem() { Value = x.ProgramTypeId.ToString(), Text = x.Name })
                .ToList();
            vm.SelectedProgramTypeId = program.ProgramTypeId.ToString();
            vm.PointOfContactProfessionalOptions = _db.Professionals
                .Select(x => new SelectListItem() { Value = x.ProfessionalId.ToString(), Text = x.FirstName + " " + x.LastName })
                .ToList();
            vm.SelectedPointOfContactProfessionalId = program.PointOfContactProfessionalId.ToString();
            vm.IsActive = program.IsActive;
            vm.IsApproved = program.IsApproved;
            vm.MaximumStudentCount = program.MaximumStudentCount;
            vm.ProgramId = program.ProgramId;
            vm.StartDate = program.StartDate;
            vm.AvailabilityDate = program.AvailabilityDate;
            vm.EndDate = program.EndDate;
            vm.Description = program.Description;

            // TODO MDT 03-11-18, add enrollments
            vm.EnrollmentsViewModels = _db.Enrollments
                .Include(i => i.EnrollmentStatus)
                .Include(i => i.Student)
                .Where(w => w.ProgramId == vm.ProgramId)
                .Select(s => new Models.AdminViewModels.Enrollments.EnrollmentsViewModel(s))
                .ToList();

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
            try
            {
                if (ModelState.IsValid)
                {
                    // MDT 03-11-18, check that approved is less than maxcount
                    var checkProgram = _db.Programs
                        .Include(i => i.Enrollments).ThenInclude(i => i.EnrollmentStatus)
                        .FirstOrDefault(fod => fod.ProgramId == model.ProgramId);

                    var approvedCount = checkProgram?.Enrollments.Count(c => !c.EnrollmentStatus.IsDisregardedInEnrollmentCount ?? false) ?? 0;
                    var maxCount = model.MaximumStudentCount;

                    var maxLimitReached = approvedCount > maxCount;

                    if (maxLimitReached)
                    {
                        // TODO MDT 03-11-18, this will prevent max limit from being exceeded (need to make it fail safer; ie: response message)
                        throw new NotImplementedException("MaxLimitReached");
                    }
                    else
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
                }

                // ModelState InValid
                return View(model);
            }
            catch(Exception ex)
            {
                // TODO MDT 03-11-18, need to return model to EditProgram screen; was getting error when 'return View(model)' within catch
                return RedirectToAction("EditProgram", new { id = model.ProgramId });
            }
        }

        [HttpPost]
        public bool ApproveEnrollment(string id)
        {
            var success = false;

            try
            {
                var enrollmentId = int.Parse(id);
                var enrollment = _db.Enrollments.FirstOrDefault(fod => fod.EnrollmentId == enrollmentId);

                // MDT 03-11-18, check that approved is less than maxcount
                var programId = enrollment?.ProgramId;
                var program = _db.Programs
                    .Include(i => i.Enrollments).ThenInclude(i => i.EnrollmentStatus)
                    .FirstOrDefault(fod=>fod.ProgramId == programId);

                var approvedCount = program?.Enrollments.Count(c => !c.EnrollmentStatus.IsDisregardedInEnrollmentCount ?? false) ?? 0;
                var maxCount = program?.MaximumStudentCount ?? 0;

                var maxLimitReached = approvedCount >= maxCount;

                if (maxLimitReached)
                {
                    // TODO MDT 03-11-18, this will prevent max limit from being exceeded (need to make it fail safer; ie: response message)
                    throw new NotImplementedException("MaxLimitReached");
                }

                enrollment.EnrollmentStatusId = (int)EnrollmentStatusEnum.Approved;

                _db.Update(enrollment);
                _db.SaveChanges();

                success = true;
            }
            catch (Exception ex)
            {
            }

            return success;
        }

        [HttpPost]
        public bool DeclineEnrollment(string id)
        {
            var success = false;

            try
            {
                var enrollmentId = int.Parse(id);
                var enrollment = _db.Enrollments.FirstOrDefault(fod => fod.EnrollmentId == enrollmentId);
                enrollment.EnrollmentStatusId = (int)EnrollmentStatusEnum.Declined;

                _db.Update(enrollment);
                _db.SaveChanges();

                success = true;
            }
            catch (Exception ex)
            {
            }

            return success;
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
                        if (AdminList != null && AdminList.Count > 0)
                        {
                            foreach (var admin in AdminList)
                            {
                                UsersViewModel newUser = new UsersViewModel();
                                newUser.UserName = (admin.FirstName + " " + admin.LastName).ToString();
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
                                newUser.UserName = (users.FirstName + " " + users.LastName).ToString();
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
                        if (StudentList != null && StudentList.Count > 0)
                        {
                            foreach (var student in StudentList)
                            {
                                UsersViewModel newUser = new UsersViewModel();
                                newUser.UserName = (student.FirstName + " " + student.LastName).ToString();
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
        public async Task<IActionResult> EditUser(int UserId, string RoleName)
        {
            UsersViewModel newUser = null;
            switch (RoleName)
            {
                case "Administrator":
                    var AdminData = _db.Administrators.Where(x => x.AdminId == UserId).FirstOrDefault();
                    if (AdminData != null)
                    {

                        newUser = new UsersViewModel();
                        newUser.FirstName = AdminData.FirstName;
                        newUser.LastName = AdminData.LastName;
                        newUser.Email = AdminData.EmailAddress;
                        newUser.IsActive = AdminData.IsActive;
                        newUser.UserID = UserId;
                        newUser.RoleName = RoleName;

                    }
                    else
                    {
                        ViewData["Message"] = "No Users found for Selected UserID";
                    }
                    break;
                case "Professional":
                    var UserData = _db.Professionals.Where(x => x.ProfessionalId == UserId).FirstOrDefault();
                    if (UserData != null)
                    {
                        newUser = new UsersViewModel();
                        newUser.FirstName = UserData.FirstName;
                        newUser.LastName = UserData.LastName;
                        newUser.Email = UserData.EmailAddress;
                        newUser.IsActive = UserData.IsActive;
                        newUser.UserID = UserId;
                        newUser.RoleName = RoleName;
                    }
                    else
                    {
                        ViewData["Message"] = "No Users found for Selected Role";
                    }
                    break;
                case "Student":
                    var StudentData = _db.Students.Where(x => x.StudentId == UserId).FirstOrDefault();
                    if (StudentData != null)
                    {
                        newUser = new UsersViewModel();
                        newUser.FirstName = StudentData.FirstName;
                        newUser.LastName = StudentData.LastName;
                        newUser.Email = StudentData.EmailAddress;
                        newUser.IsActive = StudentData.IsActive;
                        newUser.UserID = UserId;
                        newUser.RoleName = RoleName;
                    }
                    else
                    {
                        ViewData["Message"] = "No Users found for Selected Role";
                    }
                    break;

                default:
                    break;
            }
            return View(newUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UsersViewModel usersViewModel)
        {
            if (ModelState.IsValid)
            {

                switch (usersViewModel.RoleName)
                {
                    case "Administrator":
                        // var AdminData = _db.Administrators.Where(x => x.AdminId == UserId).FirstOrDefault();
                        if (usersViewModel != null)
                        {
                            var administrator = _db.Administrators.SingleOrDefault(x => x.AdminId == usersViewModel.UserID);
                            if (administrator != null)
                            {
                                administrator.FirstName = usersViewModel.FirstName;
                                administrator.LastName = usersViewModel.LastName;
                                administrator.EmailAddress = usersViewModel.Email;
                                administrator.IsActive = usersViewModel.IsActive;
                                _db.SaveChanges();
                                ViewData["Message"] = "Administrator Record Updated Successfully";
                            }

                        }
                        else
                        {
                            ViewData["Message"] = "Input is not valid";
                        }
                        break;
                    case "Professional":
                        if (usersViewModel != null)
                        {
                            var professional = _db.Professionals.SingleOrDefault(x => x.ProfessionalId == usersViewModel.UserID);
                            if (professional != null)
                            {
                                professional.FirstName = usersViewModel.FirstName;
                                professional.LastName = usersViewModel.LastName;
                                professional.EmailAddress = usersViewModel.Email;
                                professional.IsActive = usersViewModel.IsActive;
                                _db.SaveChanges();
                                ViewData["Message"] = "Professional Record Updated Successfully";
                            }

                        }
                        else
                        {
                            ViewData["Message"] = "Input is not valid";
                        }
                        break;
                    case "Student":
                        if (usersViewModel != null)
                        {
                            var student = _db.Students.SingleOrDefault(x => x.StudentId == usersViewModel.UserID);
                            if (student != null)
                            {
                                student.FirstName = usersViewModel.FirstName;
                                student.LastName = usersViewModel.LastName;
                                student.EmailAddress = usersViewModel.Email;
                                student.IsActive = usersViewModel.IsActive;
                                _db.SaveChanges();
                                ViewData["Message"] = "Student Record Updated Successfully";
                            }

                        }
                        else
                        {
                            ViewData["Message"] = "Input is not valid";
                        }
                        break;

                    default:
                        break;
                }

            }
            return View("EditUser", usersViewModel);
        }
    }
}