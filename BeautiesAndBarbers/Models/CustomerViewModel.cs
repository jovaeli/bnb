using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    [NotMapped]
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string Picture { get; set; }
    }
}