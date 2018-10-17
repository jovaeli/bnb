using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(3, ErrorMessage = "The field {0} must have a maximum of {1} characters")]
        [Index("Country_Code_Index", IsUnique = true)]
        public string Code { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(64, ErrorMessage = "The field {0} must have a maximum of {1} characters")]
        [Display(Name ="Country")]
        public string Name { get; set; }

        [JsonIgnore] public virtual ICollection<Address> Addresses { get; set; }
    }
}