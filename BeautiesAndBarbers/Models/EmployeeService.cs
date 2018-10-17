using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class EmployeeService
    {
        [Key, Column(Order = 0)]
        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Key, Column(Order = 1)]
        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Service")]
        public int ServiceId { get; set; }

        [Display(Name = "Disabled")]
        public bool IsDisabled { get; set; }

        [JsonIgnore] public virtual Service Service { get; set; }
        [JsonIgnore] public virtual Employee Employee { get; set; }
        [JsonIgnore] public virtual ICollection<ClientList> ClientLists { get; set; }
    }
}