using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class ListStatus
    {
        [Key]
        public int ListStatusId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(64, ErrorMessage = "The field {0} must have a maximum of {1} characters")]
        [Index("ListStatus_Description_Index", IsUnique = true)]
        [Display(Name = "Status")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Priority Level")]
        public int PriorityLevelId { get; set; }

        public bool Active { get; set; }

        public bool Completed { get; set; }

        [Display(Name = "System Status")]
        public bool SystemStatus { get; set; }

        public bool Canceled { get; set; }

        public bool Unconfirmed { get; set; }

        public bool Confirmed { get; set; }

        public bool Current { get; set; }

        [JsonIgnore] public virtual PriorityLevel PriorityLevel { get; set; }
        [JsonIgnore] public virtual ICollection<ClientList> ClientLists { get; set; }
    }
}