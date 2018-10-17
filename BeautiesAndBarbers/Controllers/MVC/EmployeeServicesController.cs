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
    public class EmployeeServicesController : Controller
    {
        private BnBContext db = new BnBContext();

        public ActionResult EmployeeBusinesses()
        {
            User user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            IQueryable<Employee> employeeBusinesses = db.Employees
                .Include(e => e.Business)
                .Include(e => e.User)
                .Include(e => e.EmployeeServices)
                .Where(e => e.UserId == user.UserId && e.IsDisabled == false);

            return View(employeeBusinesses);
        }
        // GET: EmployeeServices
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
            
            ViewBag.EmployeeId = id;
            IQueryable<EmployeeService> employeeServices = db.EmployeeServices.Where(es => es.EmployeeId == id);

            return View(employeeServices.ToList());
        }

        // GET: EmployeeServices/Details/5
        public ActionResult Details(int? eId, int? sId)
        {
            if (eId == null || sId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeService employeeService = db.EmployeeServices.Find(eId, sId);
            if (employeeService == null)
            {
                return HttpNotFound();
            }

            return View(employeeService);
        }

        // GET: EmployeeServices/Create
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

            ViewBag.ServiceId = new SelectList(ListsHelper.GetServices(), "ServiceId", "Description");
            EmployeeService employeeService = new EmployeeService
            {
                EmployeeId = (int)id
            };
            return View(employeeService);
        }

        // POST: EmployeeServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeId,ServiceId,IsDisabled")] EmployeeService employeeService)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeServices.Add(employeeService);
                db.SaveChanges();
                return RedirectToAction("Index", new { id=employeeService.EmployeeId });
            }
            ViewBag.ServiceId = new SelectList(ListsHelper.GetServices(), "ServiceId", "Description", employeeService.ServiceId);
            return View(employeeService);
        }

        // GET: EmployeeServices/Edit/5
        public ActionResult Edit(int? eId, int? sId)
        {
            if (eId == null || sId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeService employeeService = db.EmployeeServices.Find(eId, sId);
            if (employeeService == null)
            {
                return HttpNotFound();
            }

            ViewBag.ServiceId = new SelectList(ListsHelper.GetServices(), "ServiceId", "Description", employeeService.ServiceId);
            return View(employeeService);
        }

        // POST: EmployeeServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,ServiceId,IsDisabled")] EmployeeService employeeService)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeService).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { eId = employeeService.EmployeeId, sId = employeeService.ServiceId });
            }
            ViewBag.ServiceId = new SelectList(ListsHelper.GetServices(), "ServiceId", "Description", employeeService.ServiceId);
            return View(employeeService);
        }

        // GET: EmployeeServices/Delete/5
        public ActionResult Delete(int? eId, int? sId)
        {
            if (eId == null || sId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EmployeeService employeeService = db.EmployeeServices.Find(eId, sId);
            if (employeeService == null)
            {
                return HttpNotFound();
            }

            return View(employeeService);
        }

        // POST: EmployeeServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int eId, int sId)
        {
            EmployeeService employeeService = db.EmployeeServices.Find(eId, sId);
            db.EmployeeServices.Remove(employeeService);
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
