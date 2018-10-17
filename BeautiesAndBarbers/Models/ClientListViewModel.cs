using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class ClientListViewModel 
    {
        public int ClientListId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Employee Service")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Customer")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "List Status")]
        public int ListStatusId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Appointment { get; set; }

        [NotMapped]
        public int EmployeeId { get; set; }

        public virtual ListStatus ListStatus { get; set; }
        public virtual User User { get; set; }
        public virtual EmployeeService EmployeeService { get; set; }

    }
}