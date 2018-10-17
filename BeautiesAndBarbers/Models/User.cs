using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(256, ErrorMessage = "The field {0} must be maximum {1} characters lenght")]
        [Display(Name = "E-Mail")]
        [Index("User_UserName_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [MaxLength(32, ErrorMessage = "The field {0} must be maximum {1} characters lenght")]
        public string Nickname { get; set; }

        //[Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(64, ErrorMessage = "The field {0} must be maximum {1} characters lenght")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(64, ErrorMessage = "The field {0} must be maximum {1} characters lenght")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Name")]
        public string Name { get { return Nickname != null ? Nickname : FullName.Trim() != string.Empty ? FullName : UserName; } }

        [MaxLength(20, ErrorMessage = "The field {0} must be maximum {1} characters lenght")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Picture { get; set; }

        [NotMapped]
        [Display(Name = "Picture")]
        public HttpPostedFileBase PictureFile { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        //[JsonIgnore] public virtual ICollection<ClientList> ClientLists { get; set; }
        [JsonIgnore] public virtual ICollection<Employee> Employees { get; set; }
        [JsonIgnore] public virtual ICollection<Address> Addresses { get; set; }
        [JsonIgnore] public virtual ICollection<Business> Businesses { get; set; }
        [JsonIgnore] public virtual ICollection<UserEmployeeRating> UserEmployeeRatings { get; set; }
        [JsonIgnore] public virtual ICollection<UserFavoriteBusiness> UserFavoriteBusinesses { get; set; }
        [JsonIgnore] public virtual ICollection<Customer> Customers { get; set; }
    }
}