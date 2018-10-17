using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeautiesAndBarbers.Models;
using BeautiesAndBarbers.Helpers;

namespace BeautiesAndBarbers.Controllers.MVC
{
    [Authorize]
    public class ClientListsController : Controller
    {
        private BnBContext db = new BnBContext();
        
        #region EmployeeFlow
        [Authorize(Roles = "Employee")]
        public ActionResult BusinessCustomers()
        {
            return View(ListsHelper.GetBusinessCustomers(User.Identity.Name));
        }

        // GET: ClientLists
        [Authorize(Roles = "Employee")]
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            
            if (!UsersHelper.IsEmployeeOfCurrentBusiness(User, (int)id))
            {
                return RedirectToAction("BusinessCustomers");
            }

            var clientLists = db.ClientLists
                .Where(c => c.EmployeeService.EmployeeId == id)
                .OrderBy(c => c.ListStatus.PriorityLevel.Value)
                .ThenBy(c => c.Appointment);

            ViewBag.Employee = employee;
            return View(clientLists.ToList());
        }

        // GET: ClientLists/Details/5
        [Authorize(Roles = "Employee")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientList clientList = db.ClientLists.Find(id);
            if (clientList == null)
            {
                return HttpNotFound();
            }
            return View(clientList);
        }

        // GET: ClientLists/Create
        [Authorize(Roles = "Employee")]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            if (!UsersHelper.IsEmployeeOfCurrentBusiness(User, (int)id))
            {
                return RedirectToAction("BusinessCustomers");
            }
            
            User user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            string actionName = ControllerContext.RouteData.Values["action"].ToString();

            ViewBag.ReturnUrl = string.Format("~/ClientLists/{0}/{1}", actionName, id);
            ViewBag.ServiceId = new SelectList(ListsHelper.GetEmployeeServices((int)id), "ServiceId", "Description");
            ViewBag.CustomerId = new SelectList(ListsHelper.GetCustomers(true, user.UserId), "CustomerId", "CustomerName");
            ViewBag.EmployeeName = employee.User.Name;
            ViewBag.EmployeeUserId = employee.User.UserId;

            ClientList clientList = new ClientList
            {
                EmployeeId = (int)id,
                Appointment = DateTime.Now
            };

            return View(clientList);
        }

        // POST: ClientLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientListId,CustomerId,EmployeeId,ServiceId,Appointment,CustomerName")] ClientList clientList)
        {
            User user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            bool isEmployee = UsersHelper.IsEmployeeOfCurrentBusiness(User, clientList.EmployeeId);
            if (!isEmployee)
            {
                return RedirectToAction("BusinessCustomers");
            }

            Customer customer = db.Customers.Find(clientList.CustomerId);

            if (string.IsNullOrEmpty(clientList.CustomerName))
            {
                ModelState.AddModelError("CustomerName", "The field Customer is required");
            }
            else if (customer != null)
            {
                if (isEmployee && customer.UserId == user.UserId && customer.IsAccountOwner)
                {
                    ModelState.AddModelError(string.Empty, "Same employee customer not allowed");
                } 
            }
            else
            {
                ModelState.AddModelError("CustomerName", "Customer not found");
            }

            if (ModelState.IsValid)
            {
                clientList.ListStatusId = db.ListStatus.Where(ls => ls.Confirmed == true).FirstOrDefault().ListStatusId;
                clientList.AddedByUserId = user.UserId;
                clientList.AddedDate = DateTime.Now;
                clientList.ModifiedByUserId = user.UserId;
                clientList.ModifiedDate = clientList.AddedDate;

                db.ClientLists.Add(clientList);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = clientList.EmployeeId });
            }

            ViewBag.ServiceId = new SelectList(ListsHelper.GetEmployeeServices(clientList.EmployeeId), "ServiceId", "Description", clientList.EmployeeId);
            ViewBag.CustomerId = new SelectList(ListsHelper.GetCustomers(isEmployee, user.UserId), "CustomerId", "CustomerName", clientList.CustomerId);
            ViewBag.ReturnUrl = string.Format("~/ClientLists/Create/{0}", clientList.EmployeeId);
            ViewBag.EmployeeName = user.Name;
            ViewBag.EmployeeUserId = user.UserId;
            return View(clientList);
        }

        // GET: ClientLists/Edit/5
        [Authorize(Roles = "Employee")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientList clientList = db.ClientLists.Find(id);
            if (clientList == null)
            {
                return HttpNotFound();
            }

            User user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            bool isEmployee = UsersHelper.IsEmployeeOfCurrentBusiness(User, clientList.EmployeeId);

            ViewBag.ServiceId = new SelectList(ListsHelper.GetEmployeeServices(clientList.EmployeeId), "ServiceId", "Description", clientList.ServiceId);
            
            return View(clientList);
        }

        // POST: ClientLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClientListId,ListStatusId,CustomerId,EmployeeId,ServiceId,Appointment,ServiceStarted,ServiceEnded,AddedByUserId,AddedDate")] ClientList clientList)
        {
            User user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                clientList.ModifiedByUserId = user.UserId;
                clientList.ModifiedDate = DateTime.Now;
                db.Entry(clientList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = clientList.EmployeeId });
            }

            ////ModelState Validation
            //var s = string.Empty;
            //var n = 0;
            //foreach (var modelState in ViewData.ModelState)
            //{
            //    foreach (ModelError error in modelState.Value.Errors)
            //    {
            //        s = string.Format("{0} {1}", modelState.Key, error.ErrorMessage);
            //        n = 0;
            //    }
            //}

            bool isEmployee = UsersHelper.IsEmployeeOfCurrentBusiness(User, clientList.EmployeeId);
            clientList.Customer = db.Customers.Find(clientList.CustomerId);
            ViewBag.ServiceId = new SelectList(ListsHelper.GetEmployeeServices(clientList.EmployeeId), "ServiceId", "Description", clientList.ServiceId);
            
            return View(clientList);
        }

        // GET: ClientLists/Delete/5
        [Authorize(Roles = "Employee")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientList clientList = db.ClientLists.Find(id);
            if (clientList == null)
            {
                return HttpNotFound();
            }
            return View(clientList);
        }

        // POST: ClientLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientList clientList = db.ClientLists.Find(id);
            db.ClientLists.Remove(clientList);
            db.SaveChanges();
            return RedirectToAction("Index");
        } 
        #endregion

        #region CustomerFlow
        [AllowAnonymous]
        public ActionResult EmployeeCustomers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            bool isEmployee = UsersHelper.IsEmployeeOfCurrentBusiness(User, (int)id);
            if (isEmployee)
            {
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.Employee = employee;

            var clientLists = db.ClientLists
                .Where(c => c.EmployeeService.EmployeeId == id && c.ListStatus.Active == true)
                .OrderBy(c => c.ListStatus.PriorityLevel.Value)
                .ThenBy(c => c.Appointment);

            User user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.UserId = user == null ? 0 : user.UserId;
            return View(clientLists.ToList());
        }

        // GET: ClientLists/Create
        [Authorize(Roles = "User")]
        public ActionResult CreateMe(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            if (UsersHelper.IsEmployeeOfCurrentBusiness(User, (int)id))
            {
                return RedirectToAction("Create", new { id = id });
            }

            User user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            string actionName = ControllerContext.RouteData.Values["action"].ToString();

            ViewBag.ReturnUrl = string.Format("~/ClientLists/{0}/{1}", actionName, id);
            ViewBag.ServiceId = new SelectList(ListsHelper.GetEmployeeServices((int)id), "ServiceId", "Description");
            ViewBag.CustomerId = new SelectList(ListsHelper.GetCustomers(false, user.UserId), "CustomerId", "CustomerName");
            ViewBag.EmployeeName = employee.User.Name;

            ClientList clientList = new ClientList
            {
                EmployeeId = (int)id,
                Appointment = DateTime.Now,
                ListStatusId = db.ListStatus.Where(ls => ls.Unconfirmed == true).FirstOrDefault().ListStatusId                
            };

            return View(clientList);
        }

        // POST: ClientLists/CreateMe
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMe([Bind(Include = "ClientListId,ListStatusId,CustomerId,EmployeeId,ServiceId,Appointment")] ClientList clientList)
        {
            DateTime now = DateTime.Now;
            User user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            
            if (UsersHelper.IsEmployeeOfCurrentBusiness(User, clientList.EmployeeId))
            {
                return RedirectToAction("Create", new { clientList = clientList });
            }
            
            if (clientList.Appointment < now.Subtract(new TimeSpan(0, 2, 0)))
            {
                ModelState.AddModelError("Appointment", "Value must be greater than or equal to {now}");
            }

            if (ModelState.IsValid)
            {
                clientList.AddedByUserId = user.UserId;
                clientList.AddedDate = now;
                clientList.ModifiedByUserId = user.UserId;
                clientList.ModifiedDate = now;

                db.ClientLists.Add(clientList);
                db.SaveChanges();
                return RedirectToAction("EmployeeCustomers", new { id = clientList.EmployeeId });
            }

            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            ViewBag.ReturnUrl = string.Format("~/ClientLists/{0}/{1}", actionName, clientList.EmployeeId);
            ViewBag.ServiceId = new SelectList(ListsHelper.GetEmployeeServices(clientList.EmployeeId), "ServiceId", "Description", clientList.EmployeeId);
            ViewBag.CustomerId = new SelectList(ListsHelper.GetCustomers(false, user.UserId), "CustomerId", "CustomerName", clientList.CustomerId);
            ViewBag.EmployeeName = UsersHelper.EmployeeUser(clientList.EmployeeId).Name;

            return View(clientList);
        }

        // GET: ClientLists/EditMe/5
        [Authorize(Roles = "User")]
        public ActionResult EditMe(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientList clientList = db.ClientLists.Find(id);
            if (clientList == null)
            {
                return HttpNotFound();
            }

            User user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            
            bool isEmployee = UsersHelper.IsEmployeeOfCurrentBusiness(User, clientList.EmployeeId);
            if (user.UserId != clientList.Customer.UserId)
            {
                if (isEmployee)
                {
                    return RedirectToAction("Edit");
                }
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ServiceId = new SelectList(ListsHelper.GetEmployeeServices(clientList.EmployeeId), "ServiceId", "Description", clientList.ServiceId);
            
            return View(clientList);
        }

        // POST: ClientLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMe([Bind(Include = "ClientListId,ListStatusId,CustomerId,EmployeeId,ServiceId,Appointment,AddedByUserId,AddedDate")] ClientList clientList)
        {
            User user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            Customer customer = db.Customers.Find(clientList.CustomerId);
            if (user.UserId != customer.UserId)
            {
                ModelState.AddModelError(string.Empty, "Autorization Denied");
            }

            if (ModelState.IsValid)
            {
                clientList.ModifiedByUserId = user.UserId;
                clientList.ModifiedDate = DateTime.Now;
                db.Entry(clientList).State = EntityState.Modified;
                db.SaveChanges();

                if (UsersHelper.IsEmployeeOfCurrentBusiness(User, clientList.EmployeeId))
                {
                    return RedirectToAction("Index", new { id = clientList.EmployeeId });
                }
                return RedirectToAction("EmployeeCustomers", new { id = clientList.EmployeeId });
            }

            ViewBag.ServiceId = new SelectList(ListsHelper.GetEmployeeServices(clientList.EmployeeId), "ServiceId", "Description", clientList.ServiceId);
            clientList.Customer = customer;

            return View(clientList);
        }

        #endregion
        
        #region EditShortCuts

        // GET: ClientLists/Confirm/5
        [Authorize(Roles = "Employee")]
        public ActionResult Confirm(int? id)
        {
            return Shortcut(id, "Confirm");
        }

        // GET: ClientLists/Completed/5
        [Authorize(Roles = "Employee")]
        public ActionResult Completed(int? id)
        {
            return Shortcut(id, "Completed");
        }

        // GET: ClientLists/Current/5
        [Authorize(Roles = "Employee")]
        public ActionResult Current(int? id)
        {
            return Shortcut(id, "Current");
        }

        // GET: ClientLists/Cancel/5
        [Authorize(Roles = "User, Employee")]
        public ActionResult Cancel(int? id)
        {
            return Shortcut(id, "Cancel");
        }

        private ActionResult Shortcut(int? id, string action)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientList clientList = db.ClientLists.Find(id);
            if (clientList == null)
            {
                return HttpNotFound();
            }
            User user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            bool isUser = user.UserId == clientList.Customer.UserId;
            bool isEmployee = UsersHelper.IsEmployeeOfCurrentBusiness(User, clientList.EmployeeId);


            if (isUser || isEmployee)
            {
                DateTime now = DateTime.Now;
                clientList.ModifiedByUserId = user.UserId;
                clientList.ModifiedDate = now;
                switch (action)
                {
                    case "Confirm":
                        clientList.ListStatusId = db.ListStatus.Where(ls => ls.Confirmed == true).FirstOrDefault().ListStatusId;
                        break;

                    case "Current":
                        clientList.ListStatusId = db.ListStatus.Where(ls => ls.Current == true).FirstOrDefault().ListStatusId;
                        clientList.ServiceStarted = now;
                        break;

                    case "Completed":
                        clientList.ListStatusId = db.ListStatus.Where(ls => ls.Completed == true).FirstOrDefault().ListStatusId;
                        clientList.ServiceEnded = now;
                        break;

                    case "Cancel":
                        clientList.ListStatusId = db.ListStatus.Where(ls => ls.Canceled == true).FirstOrDefault().ListStatusId;
                        break;
                    default:
                        return RedirectToAction("Index", new { id = clientList.EmployeeId });
                }

                db.Entry(clientList).State = EntityState.Modified;
                db.SaveChanges();

                if (isEmployee)
                {
                    return RedirectToAction("Index", new { id = clientList.EmployeeId });
                }
                return RedirectToAction("EmployeeCustomers", new { id = clientList.EmployeeId });
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
