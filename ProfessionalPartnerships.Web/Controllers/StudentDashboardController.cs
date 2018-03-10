﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProfessionalPartnerships.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        public JsonResult GetPrograms(string keyword)
        {

            string userId = null;

            var myEnrollments = new Dictionary<int, string>();

            if (HttpContext.User.Identity != null && !String.IsNullOrEmpty(HttpContext.User.Identity.Name)) {
                var user = GetCurrentUser();
                if (user != null)
                {
                    userId = user.Id;
                }
            }

            //for testing
            userId = "8a94b129-c319-4f8c-8cfe-9f07e8a44ae1";
            if (userId != null) {
                var myStudent = Database.Students.Where(x => x.AspNetUserId == userId).FirstOrDefault();
                if (myStudent != null)
                {
                    foreach(var enrollment in Database.Enrollments.Include(e => e.EnrollmentStatus).Where(x => x.StudentId == myStudent.StudentId))
                    {
                        myEnrollments[enrollment.ProgramId] = enrollment.EnrollmentStatus.Name;
                    }
                }
            }
                       

            var enrollmentTotals = Database.Enrollments.GroupBy(x => x.ProgramId).Select(g => new
            {
                ProgramId = g.First().ProgramId,
                EnrolledCount = g.Count(x => x.EnrollmentStatus.IsDisregardedInEnrollmentCount == false)
            });


            var rows = Database.Programs.Include(p => p.ProgramType).GroupJoin(enrollmentTotals,
                    p => p.ProgramId,
                    e => e.ProgramId, (p, e) => new { Program = p, Enrollments = e }).SelectMany(
                    e => e.Enrollments.Select(x => x.EnrolledCount).DefaultIfEmpty(),
                (p, ct) => new {
                    description = p.Program.Description,
                    maximumStudentCount = p.Program.MaximumStudentCount,
                    programTypeName = p.Program.ProgramType.Name,
                    programId = p.Program.ProgramId,
                    enrolledCount = ct,
                    enrollmentStatus = myEnrollments.ContainsKey(p.Program.ProgramId) ? myEnrollments[p.Program.ProgramId] : ""
                });
            
            /*
            //seems to execute once per program
            var rows = Database.Programs.Include(p => p.ProgramType).Select(p=> new
                {
                    description = p.Description,
                    maximumStudentCount = p.MaximumStudentCount,
                    programTypeName = p.ProgramType.Name,
                    enrolledCount = enrollmentTotals.Where(x=>x.ProgramId == p.ProgramId).Select(x=>x.EnrolledCount).FirstOrDefault()
                });
            */

            if (!String.IsNullOrEmpty(keyword))
            {
                rows = rows.Where(x => x.description.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return Json(rows);
        }
    }
}
