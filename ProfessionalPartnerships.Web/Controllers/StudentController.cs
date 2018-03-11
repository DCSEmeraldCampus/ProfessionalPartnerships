using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models;
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
            if (HttpContext.User.Identity != null && !string.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                var user = GetCurrentUser();
                if (user != null)
                    userId = user.Id;
            }

            //for testing
            //userId = "8a94b129-c319-4f8c-8cfe-9f07e8a44ae1";
            if (userId != null)
            {
                var myStudent = Database.Students.Where(x => x.AspNetUserId == userId).FirstOrDefault();
                if (myStudent != null)
                    return myStudent.StudentId;
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
            var studentId = GetCurrentStudentId();

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
                    .ToDictionary(w => w.ProgramId, w => (int) w.Stars);
            }

            var enrollmentTotals = Database.Enrollments.GroupBy(x => x.ProgramId).Select(g => new
            {
                g.First().ProgramId,
                EnrolledCount = g.Count(x => x.EnrollmentStatus.IsDisregardedInEnrollmentCount == false)
            });

            var rows = Database.Programs
                .Include(p => p.ProgramType)
                .Include(p => p.Semester)
                .Where(p => p.IsActive)
                .GroupJoin(enrollmentTotals,
                    p => p.ProgramId,
                    e => e.ProgramId, (p, e) => new {Program = p, Enrollments = e}).SelectMany(
                    e => e.Enrollments.Select(x => x.EnrolledCount).DefaultIfEmpty(),
                    (p, ct) => new ProgramViewModel
                    {
                        Description = p.Program.Description,
                        MaximumStudentCount = p.Program.MaximumStudentCount,
                        ProgramTypeName = p.Program.ProgramType.Name,
                        ProgramId = p.Program.ProgramId,
                        EnrolledCount = ct,
                        EnrollmentStatus = myEnrollments.ContainsKey(p.Program.ProgramId)
                            ? myEnrollments[p.Program.ProgramId]
                            : "",
                        SemesterName = p.Program.Semester.Name,
                        Stars = myReviews.ContainsKey(p.Program.ProgramId)
                            ? (int?) myReviews[p.Program.ProgramId]
                            : null,
                        ProgramAvailableToReview = p.Program.EndDate < DateTime.Now
                                                   && myEnrollments.ContainsKey(p.Program.ProgramId)
                                                   && myEnrollments[p.Program.ProgramId] == ApprovedStatus
                                                   && myReviews.ContainsKey(p.Program.ProgramId) == false
                    });

            return View(rows);
        }

        public IActionResult Review(int programId, string note, int stars)
        {
            var studentId = GetCurrentStudentId();
            if (studentId == 0) throw new ApplicationException("Note logged in");

            if (Database.StudentReviews.Any(x => x.StudentId == studentId && x.ProgramId == programId))
                throw new ApplicationException("Already reviewed program");

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
            var studentId = GetCurrentStudentId();
            if (studentId == 0) throw new ApplicationException("Not Logged In");
            ;

            if (Database.Enrollments.Any(x => x.StudentId == studentId && x.ProgramId == programId))
                throw new ApplicationException("Already Applied");

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

    }
}