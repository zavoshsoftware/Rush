using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Rush.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SiteTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: SiteTypes
        public ActionResult Index()
        {
            return View(db.SiteTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: SiteTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteType siteType = db.SiteTypes.Find(id);
            if (siteType == null)
            {
                return HttpNotFound();
            }
            return View(siteType);
        }

        // GET: SiteTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SiteTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] SiteType siteType)
        {
            if (ModelState.IsValid)
            {
				siteType.IsDeleted=false;
				siteType.CreationDate= DateTime.Now; 
                siteType.Id = Guid.NewGuid();
                db.SiteTypes.Add(siteType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(siteType);
        }

        // GET: SiteTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteType siteType = db.SiteTypes.Find(id);
            if (siteType == null)
            {
                return HttpNotFound();
            }
            return View(siteType);
        }

        // POST: SiteTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] SiteType siteType)
        {
            if (ModelState.IsValid)
            {
				siteType.IsDeleted=false;
                db.Entry(siteType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(siteType);
        }

        // GET: SiteTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteType siteType = db.SiteTypes.Find(id);
            if (siteType == null)
            {
                return HttpNotFound();
            }
            return View(siteType);
        }

        // POST: SiteTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SiteType siteType = db.SiteTypes.Find(id);
			siteType.IsDeleted=true;
			siteType.DeletionDate=DateTime.Now;
 
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
