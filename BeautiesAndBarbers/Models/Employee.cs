using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Index("Employee_BusinessId_UserId_Index", 1, IsUnique = true)]
        [Display(Name = "Business")]
        public int BusinessId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Index("Employee_BusinessId_UserId_Index", 2, IsUnique = true)]
        [Display(Name = "Employee")]
        public int UserId { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(256, ErrorMessage = "The field {0} must be maximum {1} characters lenght")]
        [Display(Name = "E-Mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Disabled")]
        public bool IsDisabled { get; set; }

        [Display(Name = "Employee's Phone Hidden")]
        public bool EmployeePhoneHidden { get; set; }

        [Display(Name = "Rating Review Disabled")]
        public bool RatingReviewDisabled { get; set; }

        [JsonIgnore] public virtual User User { get; set; }
        [JsonIgnore] public virtual Business Business { get; set; }
        [JsonIgnore] public virtual ICollection<EmployeeService> EmployeeServices { get; set; }
        [JsonIgnore] public virtual ICollection<UserEmployeeRating> UserEmployeeRatings { get; set; }
    }
}