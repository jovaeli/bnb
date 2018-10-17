using BeautiesAndBarbers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Helpers
{
    public class DBHelper
    {
        public static Response SaveChanges(BnBContext db)
        {
            try
            {
                db.SaveChanges();
                return new Response { Succeeded = true };
            }
            catch (Exception ex)
            {
                var response = new Response { Succeeded = false };
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("_Index"))
                {
                    response.Message = "There is a record with the same value";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    response.Message = "There record can't be deleted because it has related records";
                }
                else
                {
                    response.Message = ex.Message;
                }
                return response;
            }
        }

        public static List<BusinessHomeIndexViewModel> GetBusinessHomeIndex(BnBContext db, string userEmail)
        {
            User user = db.Users.Where(u => u.UserName == userEmail).FirstOrDefault();

            int userId = user == null ? 0 : user.UserId;

            var allBusinesses = db.Businesses.Where(b => b.IsDisabled == false).ToList();
            var userFavoriteBusinesses = db.UserFavoriteBusinesses.Where(ufb => ufb.UserId == userId).ToList();

            var businessHomeIndex = (from bus in allBusinesses
                                     join ufb in userFavoriteBusinesses on bus.BusinessId equals ufb.BusinessId into groupjoin
                                     from joined in groupjoin.DefaultIfEmpty()
                                     select new BusinessHomeIndexViewModel
                                     {
                                         BusinessId = bus.BusinessId,
                                         Name = bus.Name,
                                         Slogan = bus.Slogan,
                                         BusinessUserId = bus.UserId,
                                         AddressId = bus.AddressId,
                                         Phone = bus.Phone,
                                         Banner = bus.Banner,
                                         Latitude = bus.Latitude,
                                         Longitude = bus.Longitude,
                                         IsDisabled = bus.IsDisabled,
                                         AddressInfoHidden = bus.AddressInfoHidden,
                                         UserFavoriteBusinessId = joined == null ? 0 : joined.UserFavoriteBusinessId,
                                         CustomerUserId = userId,
                                         IsFavorite = joined == null ? false : joined.IsFavorite
                                     }).ToList();
            return businessHomeIndex;
        }



    }
}