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
using Microsoft.EntityFrameworkCore;
using ProfessionalPartnerships.Web.Models;
using ProfessionalPartnerships.Web.Models.DashboardViewModels;
using ProfessionalPartnerships.Web.Models.StudentViewModels;

namespace ProfessionalPartnerships.Web.Controllers
{

    [Route("[controller]/[action]")]
    public class StudentController : BaseController
    {
        private const string ApprovedStatus = "Approved";

        private readonly UserManager<ApplicationUser> _userManager;
        public StudentController(PartnershipsContext db,
            UserManager<ApplicationUser> userManager) : base(db)
        {
            _userManager = userManager;
        }


        //can probably move to a helper or something
        private int GetCurrentStudentId()
        {
            string userId = null;
            if (HttpContext.User.Identity != null && !String.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                var user = GetCurrentUser();
                if (user != null)
                {
                    userId = user.Id;
                }
            }

            //for testing
            //userId = "8a94b129-c319-4f8c-8cfe-9f07e8a44ae1";
            if (userId != null)
            {
                var myStudent = Database.Students.Where(x => x.AspNetUserId == userId).FirstOrDefault();
                if (myStudent != null)
                {
                    return myStudent.StudentId;
                }
            }
            return 0;
        }

        private ApplicationUser GetCurrentUser()
        {
            var result = _userManager.GetUserAsync(HttpContext.User);
            result.Wait();
            return result.Result;
        }

        [HttpGet]
        public ViewResult Programs()
        {
            int studentId = GetCurrentStudentId();

            var myEnrollments = new Dictionary<int, string>();
            var myReviews = new Dictionary<int, int>();

            if (studentId != 0)
            {
                myEnrollments = Database.Enrollments
                   .Include(e => e.EnrollmentStatus)
                   .Where(x => x.StudentId == studentId)
                   .ToDictionary(w => w.ProgramId, w => w.EnrollmentStatus.Name);

                myReviews = Database.StudentReviews
                    .Where(w => w.StudentId == studentId)
                    .ToDictionary(w => w.ProgramId, w => (int)w.Stars);

            }

            var enrollmentTotals = Database.Enrollments.GroupBy(x => x.ProgramId).Select(g => new
            {
                ProgramId = g.First().ProgramId,
                EnrolledCount = g.Count(x => x.EnrollmentStatus.IsDisregardedInEnrollmentCount == false)
            });

            var rows = Database.Programs
                .Include(p => p.ProgramType)
                .Include(p => p.Semester)
                .Where(p => p.IsActive)
                .GroupJoin(enrollmentTotals,
                    p => p.ProgramId,
                    e => e.ProgramId, (p, e) => new { Program = p, Enrollments = e }).SelectMany(
                    e => e.Enrollments.Select(x => x.EnrolledCount).DefaultIfEmpty(),
                (p, ct) => new ProgramViewModel
                {
                    Description = p.Program.Description,
                    MaximumStudentCount = p.Program.MaximumStudentCount,
                    ProgramTypeName = p.Program.ProgramType.Name,
                    ProgramId = p.Program.ProgramId,
                    EnrolledCount = ct,
                    EnrollmentStatus = myEnrollments.ContainsKey(p.Program.ProgramId) ? myEnrollments[p.Program.ProgramId] : "",
                    SemesterName = p.Program.Semester.Name,
                    Stars = myReviews.ContainsKey(p.Program.ProgramId) ? (int?)myReviews[p.Program.ProgramId] : null,
                    ProgramAvailableToReview = p.Program.EndDate < DateTime.Now
                        && myEnrollments.ContainsKey(p.Program.ProgramId)
                        && myEnrollments[p.Program.ProgramId] == ApprovedStatus
                        && myReviews.ContainsKey(p.Program.ProgramId) == false
                });

            return View(rows);
        }

        public IActionResult Review(int programId, string note, int stars)
        {
            int studentId = GetCurrentStudentId();
            if (studentId == 0) throw new ApplicationException("Note logged in");

            var existing = Database.StudentReviews.FirstOrDefault(x => x.StudentId == studentId && x.ProgramId == programId);
            if (existing != null)
            {
                throw new ApplicationException("Already reviewed program");
            }

            Database.StudentReviews.Add(new StudentReviews
            {
                ProgramId = programId,
                StudentId = studentId,
                Note = note,
                Stars = stars
            });

            Database.SaveChanges();

            return RedirectToAction("Programs");
        }

        public IActionResult ApplyProgram(int programId, string note)
        {
            int studentId = GetCurrentStudentId();
            if (studentId == 0) throw new ApplicationException("Not Logged In"); ;

            var existing = Database.Enrollments.Where(x => x.StudentId == studentId && x.ProgramId == programId).FirstOrDefault();
            if (existing != null)
            {
                throw new ApplicationException("Already Applied");
            }

            Database.Enrollments.Add(new Enrollments
            {
                ProgramId = programId,
                StudentId = studentId,
                EnrollmentStatusId = 1,
                Note = note
            });

            Database.SaveChanges();

  
            return RedirectToAction("Programs");
        }


        /*
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
            if(ModelState.IsValid)
            {

                switch (usersViewModel.RoleName)
                {
                    case "Administrator":
                       // var AdminData = _db.Administrators.Where(x => x.AdminId == UserId).FirstOrDefault();
                        if (usersViewModel != null)
                        {
                            var administrator = _db.Administrators.SingleOrDefault(x => x.AdminId == usersViewModel.UserID);
                            if(administrator!=null)
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
        */
    }
}