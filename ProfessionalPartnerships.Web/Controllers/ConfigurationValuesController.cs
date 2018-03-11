using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Models;

namespace ProfessionalPartnerships.Web.Controllers
{
    public class ConfigurationValuesController : Controller
    {
        PartnershipsContext _db;

        public ConfigurationValuesController(PartnershipsContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var ConfigurationValues = _db.ConfigurationValues.ToList();
            return View(ConfigurationValues);
        }
        
        public IActionResult EditConfigurationValue(ConfigurationValues configValue)
        {
            ConfigurationValues updateConfigValue = _db.ConfigurationValues.SingleOrDefault(s => s.ConfigurationValueId == configValue.ConfigurationValueId);
            if (updateConfigValue != null)
            {
                updateConfigValue.Value = configValue.Value;
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditConfigurationValueView(int id)
        {
            ConfigurationValues configValue = _db.ConfigurationValues.Find(id);
            return View(configValue);
        }
    }
}