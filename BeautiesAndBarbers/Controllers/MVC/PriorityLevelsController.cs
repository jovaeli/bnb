using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeautiesAndBarbers.Models;

namespace BeautiesAndBarbers.Controllers.MVC
{
    public class PriorityLevelsController : Controller
    {
        private BnBContext db = new BnBContext();

        // GET: PriorityLevels
        public ActionResult Index()
        {
            return View(db.PriorityLevels.OrderBy(pl => pl.Value).ToList());
        }

        // GET: PriorityLevels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriorityLevel priorityLevel = db.PriorityLevels.Find(id);
            if (priorityLevel == null)
            {
                return HttpNotFound();
            }
            return View(priorityLevel);
        }

        // GET: PriorityLevels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PriorityLevels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PriorityLevelId,Description,Value")] PriorityLevel priorityLevel)
        {
            if (ModelState.IsValid)
            {
                db.PriorityLevels.Add(priorityLevel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(priorityLevel);
        }

        // GET: PriorityLevels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriorityLevel priorityLevel = db.PriorityLevels.Find(id);
            if (priorityLevel == null)
            {
                return HttpNotFound();
            }
            return View(priorityLevel);
        }

        // POST: PriorityLevels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PriorityLevelId,Description,Value")] PriorityLevel priorityLevel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(priorityLevel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(priorityLevel);
        }

        // GET: PriorityLevels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriorityLevel priorityLevel = db.PriorityLevels.Find(id);
            if (priorityLevel == null)
            {
                return HttpNotFound();
            }
            return View(priorityLevel);
        }

        // POST: PriorityLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PriorityLevel priorityLevel = db.PriorityLevels.Find(id);
            db.PriorityLevels.Remove(priorityLevel);
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
