using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class PriorityLevel
    {
        [Key]
        public int PriorityLevelId { get; set; }

        [Required(ErrorMessage ="The field {0} is required")]
        [MaxLength(64, ErrorMessage = "The field {0} must have a maximum of {1} characters")]
        [Index("PriorityLevel_Description_Index", IsUnique = true)]
        [Display(Name = "Priority Level")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, 128, ErrorMessage = "{0} must have a number between {1} and {2}")]
        public int Value { get; set; }

        [JsonIgnore] public virtual ICollection<ListStatus> ListStatus { get; set; }
    }
}