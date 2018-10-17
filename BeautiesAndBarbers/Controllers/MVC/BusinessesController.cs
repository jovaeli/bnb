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
using Microsoft.AspNet.Identity.Owin;
using System.IO;

namespace BeautiesAndBarbers.Controllers.MVC
{
    public class BusinessesController : Controller
    {
        private BnBContext db = new BnBContext();

        // GET: Businesses
        public ActionResult Index()
        {
            User user = db.Users
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();

            var businesses = db.Businesses
                .Include(b => b.Address)
                .Include(b => b.User)
                .Where(b => b.UserId == user.UserId);
            return View(businesses.ToList());
        }

        // GET: Businesses/Details/5
        public ActionResult Details(int? id)
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

            return View(business);
        }

        // GET: Businesses/Page/5
        public ActionResult Page(int? id)
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
            
            //User user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();            
            //ViewBag.UserId = user == null ? 0 : user.UserId;
            //var x = business.Employees.SelectMany(e => e.EmployeeServices.GroupBy(es => es.Service.Description)).Select(g => g.First());
            //var x = business.Employees.SelectMany(e => e.EmployeeServices.Select(es => es.Service.Description)).Distinct();
            return View(business);
        }

        // GET: Businesses/Create
        public ActionResult Create()
        {
            User user = db.Users
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();
            if (user != null)
            {
                ViewBag.AddressId = new SelectList(ListsHelper.GetUserAddresses(user.UserId), "AddressId", "Description");

                Business business = new Business
                {
                    UserId = user.UserId,
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                return View(business);
            }
            ViewBag.AddressId = new SelectList(ListsHelper.GetUserAddresses(0), "AddressId", "Description");
            return View();            
        }

        // POST: Businesses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BusinessId,Name,Slogan,UserId,AddressId,Phone,Banner,BannerFile,Latitude,Longitude,IsBarberShop,IsHairSalon,IsNailSalon,IsMakeUp,IsDisabled,AddressInfoHidden,AddedDate,ModifiedDate")] Business business)
        {
            if (ModelState.IsValid)
            {
                business.AddedDate = DateTime.Now;
                business.ModifiedDate = DateTime.Now;

                db.Businesses.Add(business);

                Response response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    if (business.BannerFile != null)
                    {
                        string folder = "~/Content/Banners";
                        string pic = string.Format("{0}{1}", business.BusinessId, Path.GetExtension(business.BannerFile.FileName));

                        bool uploadResponse = FilesHelper.UploadImage(business.BannerFile, folder, pic);

                        if (uploadResponse)
                        {
                            business.Banner = string.Format("{0}/{1}", folder, pic);
                            db.Entry(business).State = EntityState.Modified;
                            db.SaveChanges();
                        } 
                    }
                    User user = db.Users.Find(business.UserId);
                    UsersHelper.AddUserToRole(user.UserName, "Owner");
                    return RedirectToAction("Create", "Employees", new { id=business.BusinessId});
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }

            ViewBag.AddressId = new SelectList(ListsHelper.GetUserAddresses(business.UserId), "AddressId", "Description", business.AddressId);
            return View(business);
        }

        // GET: Businesses/Edit/5
        public ActionResult Edit(int? id)
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

            ViewBag.AddressId = new SelectList(ListsHelper.GetUserAddresses(business.UserId), "AddressId", "Description", business.AddressId);
            return View(business);
        }

        // POST: Businesses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BusinessId,Name,Slogan,UserId,AddressId,Phone,Banner,BannerFile,Latitude,Longitude,IsBarberShop,IsHairSalon,IsNailSalon,IsMakeUp,IsDisabled,AddressInfoHidden,AddedDate,ModifiedDate")] Business business)
        {
            if (ModelState.IsValid)
            {
                business.ModifiedDate = DateTime.Now;

                if (business.BannerFile != null)
                {
                    var folder = "~/Content/Banners";
                    string pic = string.Format("{0}{1}", business.BusinessId, Path.GetExtension(business.BannerFile.FileName));
                    bool uploadResponse = FilesHelper.UploadImage(business.BannerFile, folder, pic);
                    if (uploadResponse)
                    {
                        business.Banner = string.Format("{0}/{1}", folder, pic);
                    }
                }

                db.Entry(business).State = EntityState.Modified;

                Response response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction("Index"); 
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }
            ViewBag.AddressId = new SelectList(ListsHelper.GetUserAddresses(business.UserId), "AddressId", "Description", business.AddressId);
            return View(business);
        }

        // GET: Businesses/Delete/5
        public ActionResult Delete(int? id)
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
            return View(business);
        }

        // POST: Businesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Business business = db.Businesses.Find(id);
            db.Businesses.Remove(business);
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
