using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(64, ErrorMessage = "The field {0} must have a maximum of {1} characters")]
        [Index("Service_Description_Index", IsUnique = true)]
        [Display(Name = "Service")]
        public string Description { get; set; }

        [JsonIgnore] public virtual ICollection<EmployeeService> EmployeeServices { get; set; }
    }
}