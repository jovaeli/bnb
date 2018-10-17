using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class UserEmployeeRating
    {
        [Key]
        public int UserEmployeeRatingId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Index("UserEmployeeRating_EmployeeId_UserId_Index", 1, IsUnique = true)]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Index("UserEmployeeRating_EmployeeId_UserId_Index", 2, IsUnique = true)]
        [Display(Name = "User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, 5, ErrorMessage = "{0} must have a number between {1} and {2}")]
        public int Rating { get; set; }

        [DataType(DataType.MultilineText)]
        public string Review { get; set; }

        [Display(Name = "Hidden")]
        public bool IsHidden { get; set; }

        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        [JsonIgnore] public virtual User User { get; set; }
        [JsonIgnore] public virtual Employee Employee { get; set; }
    }
}