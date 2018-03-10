using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProfessionalPartnerships.Data.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class BaseController : Controller
    {
        
        protected PartnershipsContext _db;

        public PartnershipsContext Database
        {
            get { return _db;  }
        }

        public BaseController(PartnershipsContext db)
        {
            _db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated == true)
            {
                ViewBag["UserName"] = User.Identity.Name;
            }
        }
    }
}