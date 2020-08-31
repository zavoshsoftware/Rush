using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using ViewModels;

namespace Rush.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ServiceFormsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ServiceForms
        public ActionResult Index()
        {
            var serviceForms = db.ServiceForms.Include(s => s.ServiceGroup).Where(s => s.IsDeleted == false).OrderByDescending(s => s.CreationDate).Include(s => s.ServiceType).Where(s => s.IsDeleted == false).OrderByDescending(s => s.CreationDate).Where(s => s.IsDeleted == false).OrderByDescending(s => s.CreationDate).Include(s => s.User).Where(s => s.IsDeleted == false).Include(b=>b.SiteType).OrderByDescending(s => s.CreationDate);
            return View(serviceForms.ToList());
        }

        // GET: ServiceForms/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceForm serviceForm = db.ServiceForms.Find(id);
            if (serviceForm == null)
            {
                return HttpNotFound();
            }
            return View(serviceForm);
        }

        // GET: ServiceForms/Create
        public ActionResult Create()
        {
            ViewBag.ServiceGroupId = new SelectList(db.ServiceGroups.Where(current=>current.IsDeleted==false && current.IsActive==true), "Id", "Title");
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "Id", "Title");
            ViewBag.SiteTypeId = new SelectList(db.SiteTypes, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "CellNum");
            return View();
        }

        // POST: ServiceForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceForm serviceForm)
        {
            if (ModelState.IsValid)
            {
                serviceForm.IsDeleted = false;
                serviceForm.CreationDate = DateTime.Now;
                serviceForm.Id = Guid.NewGuid();
                db.ServiceForms.Add(serviceForm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ServiceGroupId = new SelectList(db.ServiceGroups.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Title", serviceForm.ServiceGroupId);
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "Id", "Title", serviceForm.ServiceTypeId);
            ViewBag.SiteTypeId = new SelectList(db.SiteTypes, "Id", "Title", serviceForm.SiteTypeId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "CellNum", serviceForm.UserId);
            return View(serviceForm);
        }

        // GET: ServiceForms/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceForm serviceForm = db.ServiceForms.Find(id);
            if (serviceForm == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceGroupId = new SelectList(db.ServiceGroups, "Id", "Title", serviceForm.ServiceGroupId);
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "Id", "Title", serviceForm.ServiceTypeId);
            ViewBag.SiteTypeId = new SelectList(db.SiteTypes, "Id", "Title", serviceForm.SiteTypeId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", serviceForm.UserId);
            return View(serviceForm);
        }

        // POST: ServiceForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceForm serviceForm)
        {
            if (ModelState.IsValid)
            {
                serviceForm.IsDeleted = false;
                db.Entry(serviceForm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ServiceGroupId = new SelectList(db.ServiceGroups, "Id", "Title", serviceForm.ServiceGroupId);
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "Id", "Title", serviceForm.ServiceTypeId);
            ViewBag.SiteTypeId = new SelectList(db.SiteTypes, "Id", "Title", serviceForm.SiteTypeId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", serviceForm.UserId);
            return View(serviceForm);
        }

        // GET: ServiceForms/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceForm serviceForm = db.ServiceForms.Find(id);
            if (serviceForm == null)
            {
                return HttpNotFound();
            }
            return View(serviceForm);
        }

        // POST: ServiceForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ServiceForm serviceForm = db.ServiceForms.Find(id);
            serviceForm.IsDeleted = true;
            serviceForm.DeletionDate = DateTime.Now;

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
