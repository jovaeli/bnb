using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class ClientListByCompanyViewModel 
    {
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }

        public int EmployeeId { get; set; }

        [Display(Name = "Active Clients")]
        public int ActiveClients { get; set; }

        [Display(Name = "Done Today")]
        public int DoneToday { get; set; }

        [Display(Name = "Done Total")]
        public int DoneTotal { get; set; }
    }
}