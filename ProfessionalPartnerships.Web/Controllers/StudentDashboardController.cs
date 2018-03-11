using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProfessionalPartnerships.Web.Models;
using ProfessionalPartnerships.Web.Models.DashboardViewModels;

namespace ProfessionalPartnerships.Web.Controllers
{

    public class StudentDashboardController : BaseController
    {

        public StudentDashboardController(PartnershipsContext db) : base(db)
        {
        }
        public ActionResult Index()
        {
            return View();
        }



        /*
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentDashboardController(
            PartnershipsContext db,
            UserManager<ApplicationUser> userManager) : base(db)
        {
            _userManager = userManager;
        }


        public ActionResult Index(string keyword)
        {
            return View();
        }

        private ApplicationUser GetCurrentUser() {
            var result = _userManager.GetUserAsync(HttpContext.User);
            result.Wait();
            return result.Result;
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

        private class EnrollmentInformation
        {
            public string EnrollmentStatus { get; set; }
            public string HasBeenReviewed { get; set; }
        }

        //probably should move to a service or repository
        private IEnumerable<StudentDashboardViewModel> AllPrograms()
        {
            int studentId = GetCurrentStudentId();

            var myEnrollments =  new Dictionary<int, string>();
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
                (p, ct) => new StudentDashboardViewModel
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

            
            return rows;
        }


        [HttpPost]
        public JsonResult GetPrograms()
        {
            return Json(AllPrograms());
        }


        */
    }
}
