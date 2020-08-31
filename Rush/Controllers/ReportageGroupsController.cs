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
    public class ReportageGroupsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ReportageGroups
        public ActionResult Index()
        {
            return View(db.ReportageGroups.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: ReportageGroups/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportageGroup reportageGroup = db.ReportageGroups.Find(id);
            if (reportageGroup == null)
            {
                return HttpNotFound();
            }
            return View(reportageGroup);
        }

        // GET: ReportageGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportageGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReportageGroup reportageGroup)
        {
            if (ModelState.IsValid)
            {

                Product product = new Product()
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    IsActive = reportageGroup.IsActive,
                    CreationDate = DateTime.Now,
                    Title = reportageGroup.Title,
                    ProductTypeId = db.ProductTypes.FirstOrDefault(c => c.Name == "package").Id
                };

                db.Products.Add(product);

                reportageGroup.ProductId = product.Id;

                reportageGroup.IsDeleted=false;
				reportageGroup.CreationDate= DateTime.Now; 
                reportageGroup.Id = Guid.NewGuid();
                db.ReportageGroups.Add(reportageGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reportageGroup);
        }

        // GET: ReportageGroups/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportageGroup reportageGroup = db.ReportageGroups.Find(id);
            if (reportageGroup == null)
            {
                return HttpNotFound();
            }
            return View(reportageGroup);
        }

        // POST: ReportageGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReportageGroup reportageGroup)
        {
            if (ModelState.IsValid)
            {

                
                Product product = db.Products.Find(reportageGroup.ProductId);

                if (product != null)
                {
                    product.Title = reportageGroup.Title;
                    product.IsActive = reportageGroup.IsActive;
                }

                    reportageGroup.IsDeleted=false;
                db.Entry(reportageGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reportageGroup);
        }

        // GET: ReportageGroups/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportageGroup reportageGroup = db.ReportageGroups.Find(id);
            if (reportageGroup == null)
            {
                return HttpNotFound();
            }
            return View(reportageGroup);
        }

        // POST: ReportageGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ReportageGroup reportageGroup = db.ReportageGroups.Find(id);
			reportageGroup.IsDeleted=true;
			reportageGroup.DeletionDate=DateTime.Now;
 
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
