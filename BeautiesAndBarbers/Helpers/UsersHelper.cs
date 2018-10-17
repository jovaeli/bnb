using BeautiesAndBarbers.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Security.Principal;

namespace BeautiesAndBarbers.Helpers
{
    public class UsersHelper : IDisposable
    {
        private static ApplicationDbContext userContext = new ApplicationDbContext();
        private static BnBContext db = new BnBContext();

        public static User User(string userName)
        {
            
            return db.Users.Where(u => u.UserName == userName).FirstOrDefault();
        }
        public static User User(int id)
        {
            return db.Users.Find(id);
        }
        
        public static User EmployeeUser(int employeeId)
        {
            return User(db.Employees.Find(employeeId).UserId);
        }
        public static bool DeleteUser(string userName, string roleName)
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            ApplicationUser userASP = userManager.FindByEmail(userName);

            if (userASP == null)
            {
                return false;
            }

            IdentityResult response = userManager.RemoveFromRoles(userASP.Id, roleName);
            return response.Succeeded;
        }

        public static bool UpdateUserName(string currentUserName, string newUserName)
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            ApplicationUser userASP = userManager.FindByEmail(currentUserName);

            if (userASP == null)
            {
                return false;
            }

            userASP.UserName = newUserName;
            userASP.Email = newUserName;
            IdentityResult response = userManager.Update(userASP);
            return response.Succeeded;
        }

        public static void CheckRole(string roleName)
        {
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

            // Check to see if Role Exists, if not crete it
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }
        }

        public static bool IsEmployeeOfCurrentBusiness(IPrincipal user, int employeeId)
        {
            return user.IsInRole("Employee") && User(user.Identity.Name).UserId == EmployeeUser(employeeId).UserId;
        }

        public static void CheckSuperUser()
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            string email = WebConfigurationManager.AppSettings["AdminUser"];
            string password = WebConfigurationManager.AppSettings["AdminPassword"];
            ApplicationUser userASP = userManager.FindByName(email);
            if (userASP == null)
            {
                CreateUserASP(email, "Admin", password);
                userManager.AddToRole(email, "Admin");
                return;
            }

        }

        public static User CreateUser(string email, string roleName)
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            ApplicationUser userASP = userManager.FindByEmail(email);
            User user = db.Users.Where(u => u.UserName == email).FirstOrDefault();

            if (user == null)
            {
                user = new User
                {
                    UserName = email,
                    Picture = "~/Content/Pictures/place-holder.png"
                };

                Customer customer = new Customer
                {
                    UserId = user.UserId,
                    CustomerName = user.UserName,
                    IsAccountOwner = true
                };

                db.Users.Add(user);
                db.Customers.Add(customer);
                db.SaveChanges();
                AddUserToRole(email, roleName);
                return user;
            }

            return null;
        }

        public static void CreateUserASP(string email, string roleName)
        {
            
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            ApplicationUser userASP = userManager.FindByEmail(email);
            if (userASP == null)
            {
                userASP = new ApplicationUser
                {
                    Email = email,
                    UserName = email
                };

                userManager.Create(userASP, email);

            }
            AddUserToRole(email, roleName);            
        }

        public static void CreateUserASP(string email, string roleName, string password)
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            ApplicationUser userASP = new ApplicationUser
            {
                Email = email,
                UserName = email
            };

            userManager.Create(userASP, password);
            AddUserToRole(email, roleName);

        }

        public static void AddUserToRole(string email, string roleName, string currentUserEmail = default(string))
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            ApplicationUser userASP = userManager.FindByEmail(email);

            
            if (userASP != null) 
            {
                if (userManager.IsInRole(userASP.Id, roleName) == false)
                {
                    userManager.AddToRole(userASP.Id, roleName);

                    ApplicationUser currentUser = currentUserEmail == null ? userASP : userManager.FindByEmail(currentUserEmail);

                    var x = HttpContext.Current.GetOwinContext();
                    if (x != null)
                    {
                        x.Get<ApplicationSignInManager>()
                        .SignIn(currentUser, false, false);
                    }                                           
                }
            }
        }

        public static async Task PasswordRecovery(string email)
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            ApplicationUser userASP = userManager.FindByEmail(email);
            if (userASP == null)
            {
                return;
            }

            User user = db.Users.Where(tp => tp.UserName == email).FirstOrDefault();
            if (user == null)
            {
                return;
            }

            Random random = new Random();
            string newPassword = string.Format("{0}{1}{2:04}*",
                user.FirstName.Trim().ToUpper().Substring(0, 1),
                user.LastName.Trim().ToLower(),
                random.Next(10000));

            userManager.RemovePassword(userASP.Id);
            userManager.AddPassword(userASP.Id, newPassword);

            string subject = "ECommerce Password Recovery";
            string body = string.Format(@"
                    <h1>ECommerce Password Recovery</h1>
                    <p>Your new password is: <strong>{0}</strong></p>
                    <p>Please change it for one that you remember easily</p>",
                    newPassword);
            await MailHelper.SendMail(email, subject, body);
        }

        public void Dispose()
        {
            userContext.Dispose();
            db.Dispose();
        }
    }
}