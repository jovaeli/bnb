using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    [NotMapped]
    public class BusinessHomeIndexViewModel
    {
        #region Business
        public int BusinessId { get; set; }

        [Display(Name = "Business")]
        public string Name { get; set; }

        public string Slogan { get; set; }

        [Display(Name = "Owner")]
        public int BusinessUserId { get; set; }

        [Display(Name = "Address")]
        public int AddressId { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Banner { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsDisabled { get; set; }

        [Display(Name = "Address Info Hidden")]
        public bool AddressInfoHidden { get; set; } 
        #endregion

        #region UserFavoriteBusiness
        public int UserFavoriteBusinessId { get; set; }

        [Display(Name = "Customer")]
        public int CustomerUserId { get; set; }

        public bool IsFavorite { get; set; }
        #endregion

        //public virtual User User { get; set; }
    }
}