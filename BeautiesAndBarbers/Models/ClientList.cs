using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class ClientList
    {
        public ClientList()
        {
            DateTime defaultDateTime = new DateTime(1900, 1, 1);
            ServiceStarted = defaultDateTime;
            ServiceEnded = defaultDateTime;
            AddedDate = defaultDateTime;
            ModifiedDate = defaultDateTime;
        }

        [Key]
        public int ClientListId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Index("ClientList_EmployeeId_ServiceId_CustomerId_ListStatusId_Appointment_Index", 1, IsUnique = true)]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Index("ClientList_EmployeeId_ServiceId_CustomerId_ListStatusId_Appointment_Index", 2, IsUnique = true)]
        [Display(Name = "Service")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} not found")]
        [Index("ClientList_EmployeeId_ServiceId_CustomerId_ListStatusId_Appointment_Index", 3, IsUnique = true)]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [NotMapped]
        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Index("ClientList_EmployeeId_ServiceId_CustomerId_ListStatusId_Appointment_Index", 4, IsUnique = true)]
        [Display(Name = "List Status")]
        public int ListStatusId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Index("ClientList_EmployeeId_ServiceId_CustomerId_ListStatusId_Appointment_Index", 5, IsUnique = true)]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Appointment { get; set; }

        public DateTime ServiceStarted { get; set; }
        public DateTime ServiceEnded { get; set; }

        public int AddedByUserId { get; set; }
        public DateTime AddedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime ModifiedDate { get; set; }

        [JsonIgnore] public virtual ListStatus ListStatus { get; set; }
        [JsonIgnore] public virtual Customer Customer { get; set; }
        [JsonIgnore] public virtual EmployeeService EmployeeService { get; set; }
    }
}