using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Helpers
{
    public class CustomerSearch
    {
        public int UserId { get; set; }

        public bool IsEmployee { get; set; }

        public string Term { get; set; }
    }
}