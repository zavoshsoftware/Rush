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
    public class StepDiscountsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: StepDiscounts
        public ActionResult Index()
        {
            return View(db.StepDiscounts.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: StepDiscounts/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StepDiscount stepDiscount = db.StepDiscounts.Find(id);
            if (stepDiscount == null)
            {
                return HttpNotFound();
            }
            return View(stepDiscount);
        }

        // GET: StepDiscounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StepDiscounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] StepDiscount stepDiscount)
        {
            if (ModelState.IsValid)
            {
				stepDiscount.IsDeleted=false;
				stepDiscount.CreationDate= DateTime.Now; 
                stepDiscount.Id = Guid.NewGuid();
                db.StepDiscounts.Add(stepDiscount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(stepDiscount);
        }

        // GET: StepDiscounts/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StepDiscount stepDiscount = db.StepDiscounts.Find(id);
            if (stepDiscount == null)
            {
                return HttpNotFound();
            }
            return View(stepDiscount);
        }

        // POST: StepDiscounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] StepDiscount stepDiscount)
        {
            if (ModelState.IsValid)
            {
				stepDiscount.IsDeleted=false;
                db.Entry(stepDiscount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stepDiscount);
        }

        // GET: StepDiscounts/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StepDiscount stepDiscount = db.StepDiscounts.Find(id);
            if (stepDiscount == null)
            {
                return HttpNotFound();
            }
            return View(stepDiscount);
        }

        // POST: StepDiscounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            StepDiscount stepDiscount = db.StepDiscounts.Find(id);
			stepDiscount.IsDeleted=true;
			stepDiscount.DeletionDate=DateTime.Now;
 
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
