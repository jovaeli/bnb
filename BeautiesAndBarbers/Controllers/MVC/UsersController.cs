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
using System.IO;

namespace BeautiesAndBarbers.Controllers.MVC
{
    public class UsersController : Controller
    {
        private BnBContext db = new BnBContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details()
        {

            User user = db.Users.Where(u=>u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,UserName,Nickname,FirstName,LastName,Phone,Picture,PictureFile")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,Nickname,FirstName,LastName,Phone,Picture,PictureFile")] User user)
        {
            if (string.IsNullOrEmpty(user.FirstName) || user.FirstName.Trim() == string.Empty)
            {                
                ModelState.AddModelError("FirstName", "This field is required");
            }

            if (string.IsNullOrEmpty(user.LastName) || user.LastName.Trim() == string.Empty)
            {
                ModelState.AddModelError("LastName", "This field is required");
            }

            if (string.IsNullOrEmpty(user.Phone) || user.Phone.Trim() == string.Empty)
            {
                ModelState.AddModelError("Phone", "This field is required");
            }

            if (ModelState.IsValid)
            {
                BnBContext db2 = new BnBContext();
                User currentUser = db2.Users.Find(user.UserId);
                if (currentUser.UserName != user.UserName)
                {
                    UsersHelper.UpdateUserName(currentUser.UserName, user.UserName);
                }
                if ((currentUser.FirstName != user.FirstName) ||
                    (currentUser.LastName != user.LastName) ||
                    (currentUser.Nickname != user.Nickname) ||
                    (currentUser.UserName != user.UserName))
                {
                    Customer customer = db.Customers
                        .Where(c => c.UserId == user.UserId && c.IsAccountOwner == true)
                        .FirstOrDefault();
                    customer.CustomerName = user.Name;
                    db.Entry(customer).State = EntityState.Modified;
                }

                db2.Dispose();

                if (user.PictureFile != null)
                {
                    var folder = "~/Content/Pictures";
                    string pic = string.Format("{0}{1}", user.UserId, Path.GetExtension(user.PictureFile.FileName));
                    bool uploadResponse = FilesHelper.UploadImage(user.PictureFile, folder, pic);
                    if (uploadResponse)
                    {
                        user.Picture = string.Format("{0}/{1}", folder, pic);
                    }
                }

                db.Entry(user).State = EntityState.Modified;

                Response response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction("Details", new { id = user.UserId});
                }
                ModelState.AddModelError(string.Empty, response.Message);

            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
