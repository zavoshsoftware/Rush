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
    public class BackLinkDetailsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(Guid id)
        {
            var backLinkDetails = db.BackLinkDetails.Include(b => b.BackLink)
                .Where(b => b.BackLinkId == id && b.IsDeleted == false).OrderByDescending(b => b.CreationDate)
                .Include(b => b.Product);

            BackLink backlink = db.BackLinks.Find(id);

            if (backlink != null)
                ViewBag.Title = "فهرست ماه های " + backlink.FullName;

            return View(backLinkDetails.ToList());
        }

        public ActionResult Create(Guid id)
        {
            ViewBag.BackLinkId = id;
            return View();
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BackLinkDetail backLinkDetail, Guid id)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product()
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    IsActive = backLinkDetail.IsActive,
                    CreationDate = DateTime.Now,
                    Title = backLinkDetail.Title,
                    ProductTypeId = db.ProductTypes.FirstOrDefault(c => c.Name == "backlink").Id
                };

                db.Products.Add(product);

                backLinkDetail.ProductId = product.Id;


                backLinkDetail.IsDeleted=false;
				backLinkDetail.CreationDate= DateTime.Now; 
                backLinkDetail.Id = Guid.NewGuid();
                db.BackLinkDetails.Add(backLinkDetail);
                db.SaveChanges();
                return RedirectToAction("Index",new{id=id});
            }

            ViewBag.BackLinkId = id;
            return View(backLinkDetail);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BackLinkDetail backLinkDetail = db.BackLinkDetails.Find(id);
            if (backLinkDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.BackLinkId = backLinkDetail.BackLinkId;
            return View(backLinkDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BackLinkDetail backLinkDetail)
        {
            if (ModelState.IsValid)
            {

                Product product = db.Products.Find(backLinkDetail.ProductId);

                if (product != null)
                {
                    product.Title = backLinkDetail.Title;
                    product.IsActive = backLinkDetail.IsActive;
                }


                backLinkDetail.IsDeleted=false;
                db.Entry(backLinkDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = backLinkDetail.BackLinkId });
            }
            ViewBag.BackLinkId = backLinkDetail.BackLinkId;
            return View(backLinkDetail);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BackLinkDetail backLinkDetail = db.BackLinkDetails.Find(id);
            if (backLinkDetail == null)
            {
                return HttpNotFound();
            }
            return View(backLinkDetail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            BackLinkDetail backLinkDetail = db.BackLinkDetails.Find(id);
			backLinkDetail.IsDeleted=true;
			backLinkDetail.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index", new { id = backLinkDetail.BackLinkId });
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
