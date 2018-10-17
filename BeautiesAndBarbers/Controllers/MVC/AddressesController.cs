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
    public class AddressesController : Controller
    {
        private BnBContext db = new BnBContext();

        // GET: Addresses
        public ActionResult Index()
        {
            User user = db.Users
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();
            var addresses = db.Addresses
                .Include(a => a.Country)
                .Include(a => a.User)
                .Where(a => a.UserId == user.UserId);
            return View(addresses.ToList());
        }

        // GET: Addresses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // GET: Addresses/Create
        public ActionResult Create(string returnUrl)
        {
            if (!Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "~/Home/Index";
            }            

            User user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CountryId = new SelectList(ListsHelper.GetCountries(), "CountryId", "Name");
            
            Address address = new Address { UserId = user.UserId, ReturnUrl = returnUrl };
            return View(address);
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AddressId,CountryId,UserId,Description,FullName,Address1,Address2,City,State,ZIP,IsDefault,IsDisabled,ReturnUrl")] Address address)
        {
            if (!Url.IsLocalUrl(address.ReturnUrl))
            {
                address.ReturnUrl = "~/Home/Index";
            }

            if (ModelState.IsValid)
            {
                db.Addresses.Add(address);
                db.SaveChanges();
                return Redirect(address.ReturnUrl);
            }

            ViewBag.CountryId = new SelectList(ListsHelper.GetCountries(), "CountryId", "Name", address.CountryId);
            return View(address);
        }

        // GET: Addresses/Edit/5
        public ActionResult Edit(int? id, string returnUrl)
        {
            if (!Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "~/Home/Index";
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            address.ReturnUrl = returnUrl;
            ViewBag.CountryId = new SelectList(ListsHelper.GetCountries(), "CountryId", "Name", address.CountryId);
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AddressId,CountryId,UserId,Description,FullName,Address1,Address2,City,State,ZIP,IsDefault,IsDisabled,ReturnUrl")] Address address)
        {
            if (!Url.IsLocalUrl(address.ReturnUrl))
            {
                address.ReturnUrl = "~/Home/Index";
            }
            if (ModelState.IsValid)
            {
                db.Entry(address).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(address.ReturnUrl);
            }
            ViewBag.CountryId = new SelectList(ListsHelper.GetCountries(), "CountryId", "Name", address.CountryId);
            return View(address);
        }

        // GET: Addresses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Address address = db.Addresses.Find(id);
            db.Addresses.Remove(address);
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
