using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class EmployeeServiceViewModel
    {
        public int EmployeeId { get; set; }

        public int ServiceId { get; set; }

        public string Description { get; set; }

        public bool IsDisabled { get; set; }
    }
}