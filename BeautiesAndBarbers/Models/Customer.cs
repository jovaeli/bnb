using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Index("Customer_UserId_CustomerName_Index", 1, IsUnique = true)]
        [Display(Name = "User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Index("Customer_UserId_CustomerName_Index", 2, IsUnique = true)]
        [MaxLength(64, ErrorMessage = "The field {0} must be maximum {1} characters lenght")]
        [Display(Name = "Name")]
        public string CustomerName { get; set; }

        //[DataType(DataType.ImageUrl)]
        //public string Picture { get; set; }

        //[NotMapped]
        //[Display(Name = "Picture")]
        //public HttpPostedFileBase PictureFile { get; set; }

        public bool IsAccountOwner { get; set; }

        [NotMapped]
        [JsonIgnore] public string ReturnUrl { get; set; }
        [JsonIgnore] public virtual User User { get; set; }
    }
}