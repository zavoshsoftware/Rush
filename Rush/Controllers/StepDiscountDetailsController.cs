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
    [Authorize(Roles = "Administrator,SuperAdministrator")]
    public class StepDiscountDetailsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: StepDiscountDetails
        public ActionResult Index(Guid id)
        {
            List<StepDiscountDetail> stepDiscountDetails = db.StepDiscountDetails.Include(s => s.StepDiscount)
                .Where(s => s.IsDeleted == false && s.StepDiscountId == id).OrderByDescending(s => s.CreationDate)
                .ToList();

            ViewBag.title = db.StepDiscounts.Find(id).Title;
            

            return View(stepDiscountDetails);
        }

      
        // GET: StepDiscountDetails/Create
        public ActionResult Create(Guid id)
        {
            ViewBag.StepDiscountId = id;
            return View();
        }

        // POST: StepDiscountDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StepDiscountDetail stepDiscountDetail,Guid id)
        {
            if (ModelState.IsValid)
            {
                stepDiscountDetail.IsDeleted = false;
                stepDiscountDetail.CreationDate = DateTime.Now;
                stepDiscountDetail.StepDiscountId = id;
                stepDiscountDetail.Id = Guid.NewGuid();

                db.StepDiscountDetails.Add(stepDiscountDetail);
                db.SaveChanges();
                return RedirectToAction("Index", new {id = id});
            }

            ViewBag.StepDiscountId = id;
            return View(stepDiscountDetail);
        }

        // GET: StepDiscountDetails/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StepDiscountDetail stepDiscountDetail = db.StepDiscountDetails.Find(id);
            if (stepDiscountDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.StepDiscountId =   stepDiscountDetail.StepDiscountId;
            return View(stepDiscountDetail);
        }

        // POST: StepDiscountDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StepDiscountDetail stepDiscountDetail)
        {
            if (ModelState.IsValid)
            {
                stepDiscountDetail.IsDeleted = false;
                stepDiscountDetail.LastModifiedDate = DateTime.Now;

                db.Entry(stepDiscountDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = stepDiscountDetail.StepDiscountId });
            }
            ViewBag.StepDiscountId = stepDiscountDetail.StepDiscountId;
            return View(stepDiscountDetail);
        }

        // GET: StepDiscountDetails/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StepDiscountDetail stepDiscountDetail = db.StepDiscountDetails.Find(id);
            if (stepDiscountDetail == null)
            {
                return HttpNotFound();
            }
            return View(stepDiscountDetail);
        }

        // POST: StepDiscountDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            StepDiscountDetail stepDiscountDetail = db.StepDiscountDetails.Find(id);
            stepDiscountDetail.IsDeleted = true;
            stepDiscountDetail.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index", new { id = id });
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
