using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Rush.Controllers
{
    public class OrderDetailInformationsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();


        public ActionResult Index(Guid id, Guid? orderDetailId)
        {
            List<OrderDetailInformation> orderDetailInformations = new List<OrderDetailInformation>();

            if (orderDetailId == null)
            {
                Order order = db.Orders.Find(id);

                if (order != null)
                    ViewBag.Title = "جزییات فاکتور شماره " + order.Code + " - نام کاربر: " + order.User.FullName;


                orderDetailInformations = db.OrderDetailInformations.Include(o => o.OrderDetail)
                    .Include(o => o.OrderDetailStatus).Include(o => o.Product)
                    .Where(o => o.OrderDetail.OrderId == id && o.IsDeleted == false)
                    .OrderByDescending(o => o.CreationDate)
                    .ToList();
            }

            else
            {
                OrderDetail orderDetail = db.OrderDetails.Find(orderDetailId);

                if (orderDetail != null)
                    ViewBag.Title = "جزییات پکیج " + orderDetail.Product.Title;


                orderDetailInformations = db.OrderDetailInformations.Include(o => o.OrderDetail)
                    .Include(o => o.OrderDetailStatus).Include(o => o.Product)
                    .Where(o => o.OrderDetailId == orderDetailId && o.IsDeleted == false)
                    .OrderByDescending(o => o.CreationDate)
                    .ToList();
            }
            return View(orderDetailInformations);
        }

        // GET: OrderDetailInformations/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetailInformation orderDetailInformation = db.OrderDetailInformations.Find(id);
            if (orderDetailInformation == null)
            {
                return HttpNotFound();
            }
            return View(orderDetailInformation);
        }

        // GET: OrderDetailInformations/Create
        public ActionResult Create()
        {
            ViewBag.OrderDetailId = new SelectList(db.OrderDetails, "Id", "FileUrl");
            ViewBag.OrderDetailStatusId = new SelectList(db.OrderDetailStatuses, "Id", "Title");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title");
            return View();
        }

        // POST: OrderDetailInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderDetailId,FileUrl,OrderDetailStatusId,ProductId,PublishLink,IsSendPublishSms,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] OrderDetailInformation orderDetailInformation)
        {
            if (ModelState.IsValid)
            {
                orderDetailInformation.IsDeleted = false;
                orderDetailInformation.CreationDate = DateTime.Now;
                orderDetailInformation.Id = Guid.NewGuid();
                db.OrderDetailInformations.Add(orderDetailInformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderDetailId = new SelectList(db.OrderDetails, "Id", "FileUrl", orderDetailInformation.OrderDetailId);
            ViewBag.OrderDetailStatusId = new SelectList(db.OrderDetailStatuses, "Id", "Title", orderDetailInformation.OrderDetailStatusId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", orderDetailInformation.ProductId);
            return View(orderDetailInformation);
        }

        // GET: OrderDetailInformations/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetailInformation orderDetailInformation = db.OrderDetailInformations.Find(id);
            if (orderDetailInformation == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderDetailId = orderDetailInformation.OrderDetailId;
            ViewBag.OrderId = orderDetailInformation.OrderDetail.OrderId;
            ViewBag.OrderType = orderDetailInformation.OrderDetail.Order.OrderType;

            ViewBag.OrderDetailStatusId = new SelectList(db.OrderDetailStatuses, "Id", "Title", orderDetailInformation.OrderDetailStatusId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", orderDetailInformation.ProductId);
            return View(orderDetailInformation);
        }

        // POST: OrderDetailInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderDetailInformation orderDetailInformation)
        {
            if (ModelState.IsValid)
            {
                orderDetailInformation.IsDeleted = false;
                db.Entry(orderDetailInformation).State = EntityState.Modified;
                db.SaveChanges();

                OrderDetail orderDetail = db.OrderDetails.Include(c => c.Order).FirstOrDefault(c => c.Id == orderDetailInformation.OrderDetailId);

                if (orderDetail.Order.OrderType == "reportage"|| orderDetail.Order.OrderType == "backlink")
                    return RedirectToAction("Index", new { id = orderDetailInformation.OrderDetail.OrderId });
                if (orderDetail.Order.OrderType == "package")
                    return RedirectToAction("Index", new { id = orderDetailInformation.OrderDetail.OrderId, orderDetailId = orderDetailInformation.OrderDetailId });
            }
            ViewBag.OrderDetailId = orderDetailInformation.OrderDetailId;
            ViewBag.OrderId = orderDetailInformation.OrderDetail.OrderId;
            ViewBag.OrderType = orderDetailInformation.OrderDetail.Order.OrderType;
            ViewBag.OrderDetailStatusId = new SelectList(db.OrderDetailStatuses, "Id", "Title", orderDetailInformation.OrderDetailStatusId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", orderDetailInformation.ProductId);
            return View(orderDetailInformation);
        }

        // GET: OrderDetailInformations/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetailInformation orderDetailInformation = db.OrderDetailInformations.Find(id);
            if (orderDetailInformation == null)
            {
                return HttpNotFound();
            }
            return View(orderDetailInformation);
        }

        // POST: OrderDetailInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            OrderDetailInformation orderDetailInformation = db.OrderDetailInformations.Find(id);
            orderDetailInformation.IsDeleted = true;
            orderDetailInformation.DeletionDate = DateTime.Now;

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


        public ActionResult Download(Guid id)
        {
            OrderDetailInformation orderDetailInformation = db.OrderDetailInformations.FirstOrDefault(current => current.Id == id);
            var fileVirtualPath = Server.MapPath("~" + orderDetailInformation.FileUrl);

            return File(fileVirtualPath, "application/force-download", Path.GetFileName(fileVirtualPath));
        }
    }
}
