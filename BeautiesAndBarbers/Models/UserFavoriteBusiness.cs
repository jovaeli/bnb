using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class UserFavoriteBusiness
    {
        [Key]
        public int UserFavoriteBusinessId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Index("UserFavoriteBusiness_UserId_BusinessId_Index", 1, IsUnique = true)]
        [Display(Name = "User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Index("UserFavoriteBusiness_UserId_BusinessId_Index", 2, IsUnique = true)]
        [Display(Name = "Business")]
        public int BusinessId { get; set; }

        [Display(Name = "Favorite")]
        public bool IsFavorite { get; set; }

        [JsonIgnore] public virtual Business Business { get; set; }
        [JsonIgnore] public virtual User User { get; set; }
    }
}