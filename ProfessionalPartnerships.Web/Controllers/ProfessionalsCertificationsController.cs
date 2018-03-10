using ProfessionalPartnerships.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Web.Models.AdminViewModels.ManageProfessionalsCertifications;
using ProfessionalPartnerships.Web.Models.AdminViewModels;

namespace ProfessionalPartnerships.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class ProfessionalsCertificationsController : BaseController
    {
        public ProfessionalsCertificationsController(PartnershipsContext db) :base(db)
        {
        }

        public IActionResult CreateCertification(ManageProfessionalsCertificationsViewModel model)
        {
            var certificationViewModel = model.ProfessionalsCertificationsViewModel;
            var entity = certificationViewModel.CreateCertification();

            _db.Set<Certifications>().Add(entity);
            _db.SaveChanges();

            // TODO MDT 03-10-18, probably not what we need
            return RedirectToAction("ManageProfessionals", "Admin");
        }
    }
}
