using BeautiesAndBarbers.Helpers;
using BeautiesAndBarbers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautiesAndBarbers.Controllers.MVC
{
    public class HomeController : Controller
    {
        private BnBContext db = new BnBContext();

        public ActionResult Index()
        {
            string email = User.Identity.Name ?? string.Empty;
            
            return View(DBHelper.GetBusinessHomeIndex(db, email));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}