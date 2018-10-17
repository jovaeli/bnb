using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class UserAddressViewModel
    {
        #region Address

        public int AddressId { get; set; }

        public int UserId { get; set; }

        public string Description { get; set; }

        public string FullName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZIP { get; set; }

        public bool IsDefault { get; set; }

        public bool IsDisabled { get; set; }

        #endregion

        #region Country

        public int CountryId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        #endregion
    }
}