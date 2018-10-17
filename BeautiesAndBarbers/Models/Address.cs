using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(64, ErrorMessage = "The field {0} must have a maximum of {1} characters")]
        [Display(Name = "Address Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(64, ErrorMessage = "The field {0} must have a maximum of {1} characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(64, ErrorMessage = "The field {0} must have a maximum of {1} characters")]
        [Display(Name = "Street Address")]
        public string Address1 { get; set; }

        [MaxLength(64, ErrorMessage = "The field {0} must have a maximum of {1} characters")]
        [Display(Name = "Street Address")]
        public string Address2 { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(64, ErrorMessage = "The field {0} must have a maximum of {1} characters")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(64, ErrorMessage = "The field {0} must have a maximum of {1} characters")]
        [Display(Name = "State/ Province/ Region")]
        public string State { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(16, ErrorMessage = "The field {0} must have a maximum of {1} characters")]
        [Display(Name = "ZIP")]
        public string ZIP { get; set; }

        [Display(Name = "Default")]
        public bool IsDefault { get; set; }

        [Display(Name = "Disabled")]
        public bool IsDisabled { get; set; }

        [NotMapped] public string ReturnUrl { get; set; }

        [JsonIgnore] public virtual Country Country { get; set; }
        [JsonIgnore] public virtual User User { get; set; }
        [JsonIgnore] public virtual ICollection<Business> Businesses { get; set; }
    }
}