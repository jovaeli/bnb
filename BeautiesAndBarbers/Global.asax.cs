using BeautiesAndBarbers.Helpers;
using BeautiesAndBarbers.Migrations;
using BeautiesAndBarbers.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BeautiesAndBarbers
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static BnBContext db = new BnBContext();
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BnBContext, Configuration>());
            CheckRolesAndSuperUser();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(Object sender, EventArgs e)
        {            
            User user = db.Users.Where(u=>u.UserName == User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                HttpContext.Current.Session.Add("CurrentUserId", user.UserId);
            }            
        }

        private void CheckRolesAndSuperUser()
        {
            UsersHelper.CheckRole("Admin");
            UsersHelper.CheckRole("User");
            UsersHelper.CheckRole("Owner");
            UsersHelper.CheckRole("Employee");
            UsersHelper.CheckSuperUser();
        }
    }
}
