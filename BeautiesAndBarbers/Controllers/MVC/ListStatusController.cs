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
    public class ListStatusController : Controller
    {
        private BnBContext db = new BnBContext();

        // GET: ListStatus
        public ActionResult Index()
        {
            var listStatus = db.ListStatus
                .Include(ls => ls.PriorityLevel)
                .OrderBy(ls => ls.PriorityLevel.Value)
                .ThenBy(ls => ls.Description);

            return View(listStatus.ToList());
        }

        // GET: ListStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListStatus listStatus = db.ListStatus.Find(id);
            if (listStatus == null)
            {
                return HttpNotFound();
            }
            return View(listStatus);
        }

        // GET: ListStatus/Create
        public ActionResult Create()
        {
            ViewBag.PriorityLevelId = new SelectList(ListsHelper.GetPriorityLevels(), "PriorityLevelId", "Description");
            return View();
        }

        // POST: ListStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ListStatusId,Description,PriorityLevelId,Active,Completed,SystemStatus,Canceled,Unconfirmed,Confirmed,Current")] ListStatus listStatus)
        {
            if (ModelState.IsValid)
            {
                db.ListStatus.Add(listStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PriorityLevelId = new SelectList(ListsHelper.GetPriorityLevels(), "PriorityLevelId", "Description", listStatus.PriorityLevelId);
            return View(listStatus);
        }

        // GET: ListStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListStatus listStatus = db.ListStatus.Find(id);
            if (listStatus == null)
            {
                return HttpNotFound();
            }
            ViewBag.PriorityLevelId = new SelectList(db.PriorityLevels, "PriorityLevelId", "Description", listStatus.PriorityLevelId);
            return View(listStatus);
        }

        // POST: ListStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ListStatusId,Description,PriorityLevelId,Active,Completed,SystemStatus,Canceled,Unconfirmed,Confirmed,Current")] ListStatus listStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PriorityLevelId = new SelectList(db.PriorityLevels, "PriorityLevelId", "Description", listStatus.PriorityLevelId);
            return View(listStatus);
        }

        // GET: ListStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListStatus listStatus = db.ListStatus.Find(id);
            if (listStatus == null)
            {
                return HttpNotFound();
            }
            return View(listStatus);
        }

        // POST: ListStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListStatus listStatus = db.ListStatus.Find(id);
            db.ListStatus.Remove(listStatus);
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


