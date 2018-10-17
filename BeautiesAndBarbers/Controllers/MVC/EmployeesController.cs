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
using System.Reflection;

namespace BeautiesAndBarbers.Controllers.MVC
{
    public class EmployeesController : Controller
    {
        private BnBContext db = new BnBContext();

        // GET: Employees
        public ActionResult Index()
        {
            User user = db.Users
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();

            IQueryable<int> userBusinesses = db.Businesses
                .Where(b => b.UserId == user.UserId)
                .Select(b => b.BusinessId);

            IQueryable<Employee> employees = db.Employees
                .Include(e => e.Business)
                .Include(e => e.User)
                .Where(e => userBusinesses.Contains(e.BusinessId));

            return View(employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
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
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Business business = db.Businesses.Find(id);
            if (business == null)
            {
                return HttpNotFound();
            }

            Employee employee = new Employee { BusinessId = (int)id};
            return View(employee);
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeId,BusinessId,Email,IsDisabled,EmployeePhoneHidden,RatingReviewDisabled")] Employee employee)
        {
            User currentUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            User employeeUser = db.Users.Where(eu => eu.UserName == employee.Email).FirstOrDefault();
            if (employeeUser != null)
            {
                employee.UserId = employeeUser.UserId;
                if (ModelState.IsValid)
                {
                    db.Employees.Add(employee);
                    //TODO: Send Email/Notification to New Employee for CONFIRMIMATION
                    db.SaveChanges();

                    UsersHelper.AddUserToRole(employeeUser.UserName, "Employee", currentUser.UserName);
                    return RedirectToAction("Details", "Businesses", new { id=employee.BusinessId});
                }                
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Employee's Email wasn't found");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
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
            employee.Email = db.Users.Find(employee.UserId).UserName;
            User user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,BusinessId,UserId,Email,IsDisabled,EmployeePhoneHidden,RatingReviewDisabled")] Employee employee)
        {
            ModelState.Remove("Email");
            employee.Email = db.Users.Find(employee.UserId).UserName;

            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Businesses", new { id = employee.BusinessId });
            }

            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
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
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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


//foreach (PropertyInfo prop in typeof(Employee).GetProperties())
//{
//    var b = ModelState.IsValidField(prop.Name);
//}
