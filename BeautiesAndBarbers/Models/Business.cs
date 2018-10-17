using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class Business
    {
        [Key]
        public int BusinessId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(64, ErrorMessage = "The field {0} must be maximum {1} characters lenght")]
        [Display(Name = "Business")]
        public string Name { get; set; }

        [MaxLength(128, ErrorMessage = "The field {0} must be maximum {1} characters lenght")]
        public string Slogan { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Owner")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Address")]
        public int AddressId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(20, ErrorMessage = "The field {0} must be maximum {1} characters lenght")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Banner { get; set; }

        [NotMapped]
        [Display(Name="Banner Image")]
        public HttpPostedFileBase BannerFile { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Display(Name = "Barber Shop")]
        public bool IsBarberShop { get; set; }

        [Display(Name = "Hair Salon")]
        public bool IsHairSalon { get; set; }

        [Display(Name = "Nail Salon")]
        public bool IsNailSalon { get; set; }

        [Display(Name = "Make-Up")]
        public bool IsMakeUp { get; set; }

        [Display(Name = "Disabled")]
        public bool IsDisabled { get; set; }

        [Display(Name = "Address Info Hidden")]
        public bool AddressInfoHidden { get; set; }

        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        [NotMapped] public string ReturnUrl { get; set; }

        [JsonIgnore] public virtual User User { get; set; }
        [JsonIgnore] public virtual Address Address { get; set; }
        [JsonIgnore] public virtual ICollection<Employee> Employees { get; set; }
        [JsonIgnore] public virtual ICollection<UserFavoriteBusiness> UserFavoriteBusinesses { get; set; }


    }
}