using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using System.IO;
using Helpers;

namespace Rush.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class OrderDetailsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: OrderDetails
        public ActionResult Index(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.orderCode = order.Code;
            var orderDetails = db.OrderDetails.Include(o => o.Order).Where(o => o.IsDeleted == false && o.OrderId == order.Id).OrderByDescending(o => o.CreationDate).Include(o => o.OrderDetailStatus).Where(o => o.IsDeleted == false).OrderByDescending(o => o.CreationDate).Include(o => o.Product).Where(o => o.IsDeleted == false).OrderByDescending(o => o.CreationDate);
            return View(orderDetails.ToList());
        }

        public ActionResult Create(Guid id)
        {
            ViewBag.OrderId = id;
            ViewBag.OrderDetailStatusId = new SelectList(db.OrderDetailStatuses, "Id", "Title");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderDetail orderDetail, Guid id)
        {
            if (ModelState.IsValid)
            {
                orderDetail.IsDeleted = false;
                orderDetail.CreationDate = DateTime.Now;
                orderDetail.Id = Guid.NewGuid();
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.OrderId = id;
            ViewBag.OrderDetailStatusId = new SelectList(db.OrderDetailStatuses, "Id", "Title", orderDetail.OrderDetailStatusId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", orderDetail.ProductId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderId = orderDetail.OrderId;
            ViewBag.OrderDetailStatusId = new SelectList(db.OrderDetailStatuses, "Id", "Title", orderDetail.OrderDetailStatusId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", orderDetail.ProductId);
            return View(orderDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                orderDetail.IsDeleted = false;
                db.Entry(orderDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = orderDetail.OrderId });
            }
            ViewBag.OrderId = orderDetail.OrderId;
            ViewBag.OrderDetailStatusId = new SelectList(db.OrderDetailStatuses, "Id", "Title", orderDetail.OrderDetailStatusId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", orderDetail.ProductId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            orderDetail.IsDeleted = true;
            orderDetail.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index", new { id = orderDetail.OrderId });
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
            OrderDetail orderDetail = db.OrderDetails.Where(current => current.Id == id).FirstOrDefault();
            var FileVirtualPath = Server.MapPath("~" + orderDetail.FileUrl);

            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
        }

        public ActionResult SendPublishMessage(string orderDetailInfoId)
        {

            try
            {
                Guid id = new Guid(orderDetailInfoId);

                OrderDetailInformation orderDetailInformation = db.OrderDetailInformations.Find(id);

                if (orderDetailInformation == null)
                    return Json("false", JsonRequestBehavior.AllowGet);


                if (string.IsNullOrEmpty(orderDetailInformation.PublishLink))
                    return Json("notPublished", JsonRequestBehavior.AllowGet);


                string msg = "کاربر گرامی رپورتاژ شما در وب سایت " +
                             GetReportageNameByProductId(orderDetailInformation) + " منتشر شد. راش وب";


                SendSms.SendCommonSms(orderDetailInformation.OrderDetail.Order.User.CellNum, msg);

                orderDetailInformation.IsSendPublishSms = true;
                db.SaveChanges();

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public string GetReportageNameByProductId(OrderDetailInformation orderDetailInformation)
        {
            if (orderDetailInformation.OrderDetail.Order.OrderType == "reportage")
            {
                Reportage reportage = db.Reportages.FirstOrDefault(c => c.ProductId == orderDetailInformation.OrderDetail.ProductId);

                if (reportage == null)
                    return string.Empty;

                return reportage.FullName;
            }
            else if (orderDetailInformation.OrderDetail.Order.OrderType == "package")
            {
                Reportage reportage = db.Reportages.FirstOrDefault(c => c.ProductId == orderDetailInformation.ProductId);

                if (reportage == null)
                    return string.Empty;

                return reportage.FullName;
            }

            return string.Empty;
        }


    }
}
