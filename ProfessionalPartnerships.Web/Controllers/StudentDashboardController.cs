﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProfessionalPartnerships.Web.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class StudentDashboardController : BaseController
    {

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

        //probably should move to a service or repository
        private IEnumerable<StudentDashboardViewModel> AllPrograms()
        {
            int studentId = GetCurrentStudentId();

            var myEnrollments = new Dictionary<int, string>();

            if (studentId != 0)
            {
                foreach (var enrollment in Database.Enrollments.Include(e => e.EnrollmentStatus).Where(x => x.StudentId == studentId))
                {
                    myEnrollments[enrollment.ProgramId] = enrollment.EnrollmentStatus.Name;
                }
            }

            var enrollmentTotals = Database.Enrollments.GroupBy(x => x.ProgramId).Select(g => new
            {
                ProgramId = g.First().ProgramId,
                EnrolledCount = g.Count(x => x.EnrollmentStatus.IsDisregardedInEnrollmentCount == false)
            });

            var rows = Database.Programs
                .Include(p => p.ProgramType)
                .Include(p=> p.Semester)
                .Where(p=>p.IsActive)
                .GroupJoin(enrollmentTotals,
                    p => p.ProgramId,
                    e => e.ProgramId, (p, e) => new { Program = p, Enrollments = e }).SelectMany(
                    e => e.Enrollments.Select(x => x.EnrolledCount).DefaultIfEmpty(),
                (p, ct) => new StudentDashboardViewModel
                {
                    description = p.Program.Description,
                    maximumStudentCount = p.Program.MaximumStudentCount,
                    programTypeName = p.Program.ProgramType.Name,
                    programId = p.Program.ProgramId,
                    enrolledCount = ct,
                    enrollmentStatus = myEnrollments.ContainsKey(p.Program.ProgramId) ? myEnrollments[p.Program.ProgramId] : "",
                    semesterName = p.Program.Semester.Name
                });

            
            return rows;
        }

        [HttpPost]
        public JsonResult GetPrograms()
        {
            return Json(AllPrograms());
        }

        [HttpPost]
        public JsonResult ApplyProgram(int programId, string note)
        {
            int studentId = GetCurrentStudentId();
            if (studentId == 0) return Json(new { error = "Not Logged In" });

            var existing = Database.Enrollments.Where(x => x.StudentId == studentId && x.ProgramId == programId).FirstOrDefault();
            if (existing != null) {
                return Json(new { error = "Already applied" });
            }

            Database.Enrollments.Add(new Enrollments
            {
                ProgramId = programId,
                StudentId = studentId,
                EnrollmentStatusId = 1,
                Note = note
            });

            Database.SaveChanges();

            var result = AllPrograms().Where(x => x.programId == programId).FirstOrDefault();

            return Json(result);
        }
    }
}
