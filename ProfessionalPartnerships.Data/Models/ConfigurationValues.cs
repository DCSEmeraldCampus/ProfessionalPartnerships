﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class ConfigurationValues
    {
        public int ConfigurationValueId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
