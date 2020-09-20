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
using Helper;
using System.IO;
using System.Data.Entity.Core.Objects;
using Helpers;

namespace Rush.Controllers
{
    public class OrdersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        MenuHelper menuHelper = new MenuHelper();

        // GET: Orders
        public ActionResult Index(Guid? id)
        {
            List<Order> orders = new List<Order>();

            if (id == null)
                orders = db.Orders.Include(o => o.DiscountCode).Where(o => o.IsDeleted == false)
                    .Include(o => o.OrderStatus).Include(o => o.User)
                    .OrderByDescending(o => o.CreationDate).ToList();

            else
                orders = db.Orders.Include(o => o.DiscountCode).Where(o => o.UserId == id && o.IsDeleted == false)
                    .Include(o => o.OrderStatus).Include(o => o.User)
                    .OrderByDescending(o => o.CreationDate).ToList();

            return View(orders.ToList());
        }



        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.DiscountCodeId = new SelectList(db.DiscountCodes, "Id", "Code");
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.IsDeleted = false;
                order.CreationDate = DateTime.Now;
                order.Id = Guid.NewGuid();
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DiscountCodeId = new SelectList(db.DiscountCodes, "Id", "Code", order.DiscountCodeId);
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Title", order.OrderStatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(Guid? id)
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
            ViewBag.DiscountCodeId = new SelectList(db.DiscountCodes, "Id", "Code", order.DiscountCodeId);
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Title", order.OrderStatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                order.IsDeleted = false;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DiscountCodeId = new SelectList(db.DiscountCodes, "Id", "Code", order.DiscountCodeId);
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Title", order.OrderStatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(Guid? id)
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
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Order order = db.Orders.Find(id);
            order.IsDeleted = true;
            order.DeletionDate = DateTime.Now;

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



        [Authorize(Roles = "Customer")]

        public ActionResult List(string type)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            Guid id = new Guid(identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value);

            OrderListViewModel order = new OrderListViewModel();
            order.Menu = menuHelper.ReturnMenu();
            order.FooterLink = menuHelper.GetFooterLink();
            if(!string.IsNullOrEmpty(type))
            {
                order.Orders = db.Orders.Include(current => current.OrderStatus)
               .Where(x => x.IsDeleted == false && x.IsActive && x.UserId == id && x.OrderType == type)
               .OrderByDescending(c => c.CreationDate).ToList();
            }
            else
            {
                order.Orders = db.Orders.Include(current => current.OrderStatus)
               .Where(x => x.IsDeleted == false && x.IsActive && x.UserId == id)
               .OrderByDescending(c => c.CreationDate).ToList();

            }
            return View(order);
        }
        [Route("order/{code}")]

        [Authorize(Roles = "Customer")]
        public ActionResult Details(int code)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            Guid id = new Guid(identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value);

            Order order = db.Orders.FirstOrDefault(current =>
                current.Code == code && current.IsDeleted == false && current.IsActive && current.UserId == id);

            OrderDetailViewModel orderDetail = new OrderDetailViewModel();
            orderDetail.Menu = menuHelper.ReturnMenu();
            orderDetail.FooterLink = menuHelper.GetFooterLink();
            orderDetail.Order = order;
            orderDetail.OrderDetailInformations = db.OrderDetailInformations.Where(current => current.OrderDetail.OrderId == order.Id && current.IsDeleted == false && current.IsActive == true).ToList();
            orderDetail.OrderDetails = db.OrderDetails.Where(current => current.OrderId == order.Id && current.IsDeleted == false && current.IsActive == true).ToList();

            return View(orderDetail);
        }

        [Route("order/package/{orderDetailId}")]

        [Authorize(Roles = "Customer")]
        public ActionResult PackageDetails(Guid orderDetailId)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            Guid id = new Guid(identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value);



            OrderDetail orderDetail = db.OrderDetails.FirstOrDefault(current => current.Id == orderDetailId && current.IsDeleted == false && current.IsActive == true);

            OrderDetailInfoViewModel orderDetailInfo = new OrderDetailInfoViewModel();

            orderDetailInfo.Menu = menuHelper.ReturnMenu();
            orderDetailInfo.FooterLink = menuHelper.GetFooterLink();
            orderDetailInfo.OrderDetail = orderDetail;

            orderDetailInfo.orderDetailInformations = db.OrderDetailInformations.Where(current =>
                current.OrderDetailId == orderDetailId && current.IsDeleted == false && current.IsActive).ToList();



            return View(orderDetailInfo);
        }

        [Route("order/backlink/{orderDetailId}")]

        [Authorize(Roles = "Customer")]
        public ActionResult BacklinkDetails(Guid orderDetailId)
        { 

            OrderDetailInfoBacklinkViewModel orderDetailInfo = new OrderDetailInfoBacklinkViewModel();

            orderDetailInfo.Menu = menuHelper.ReturnMenu();
            orderDetailInfo.FooterLink = menuHelper.GetFooterLink();


            OrderDetailInformation orderDetailInformation = db.OrderDetailInformations.Where(current =>
                    current.OrderDetailId == orderDetailId && current.IsDeleted == false && current.IsActive)
                .Include(o => o.OrderDetailStatus).FirstOrDefault();

            if (orderDetailInformation != null)
            {
                orderDetailInfo.BacklinkKeyword = orderDetailInformation.BacklinkKeyword;
                orderDetailInfo.BacklinkUrl = orderDetailInformation.BacklinkUrl;
                orderDetailInfo.Id = orderDetailInformation.Id;
                orderDetailInfo.StatusCode = orderDetailInformation.OrderDetailStatus.Code;
            }


            OrderDetail orderDetail = db.OrderDetails.FirstOrDefault(current => current.Id == orderDetailId && current.IsDeleted == false && current.IsActive == true);

            if (orderDetail != null)
            {
                Order order = db.Orders.Find(orderDetail.OrderId);
                if (order != null)
                    ViewBag.orderId = order.Code;

                ViewBag.Title = "مشخصات بک لینک " + orderDetail.Product.Title;
            }

            return View(orderDetailInfo);
        }


        [Route("order/backlink/{orderDetailId}")]
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public ActionResult BacklinkDetails(OrderDetailInfoBacklinkViewModel orderDetailInfo)
        {
            OrderDetailInformation orderDetailInformation = db.OrderDetailInformations.Where(current =>
                    current.Id == orderDetailInfo.Id && current.IsDeleted == false && current.IsActive)
                .Include(c => c.OrderDetail).Include(c=>c.OrderDetailStatus).FirstOrDefault();


            if (ModelState.IsValid)
            {
             
                if (orderDetailInformation != null)
                {
                    orderDetailInformation.IsDeleted = false;
                    orderDetailInformation.BacklinkKeyword = orderDetailInfo.BacklinkKeyword;
                    orderDetailInformation.BacklinkUrl = orderDetailInfo.BacklinkUrl;
                    orderDetailInformation.OrderDetailStatusId =
                        db.OrderDetailStatuses.FirstOrDefault(c => c.Code == 2).Id;

                    orderDetailInformation.LastModifiedDate=DateTime.Now;

                    db.SaveChanges();

                    return Redirect("/order/" + orderDetailInformation.OrderDetail.Order.Code);

                }
            }

           
            orderDetailInfo.Menu = menuHelper.ReturnMenu();
            orderDetailInfo.FooterLink = menuHelper.GetFooterLink();


      

            if (orderDetailInfo != null)
            {
                orderDetailInfo.BacklinkKeyword = orderDetailInfo.BacklinkKeyword;
                orderDetailInfo.BacklinkUrl = orderDetailInfo.BacklinkUrl;
                orderDetailInfo.Id = orderDetailInformation.Id;
                orderDetailInfo.StatusCode = orderDetailInformation.OrderDetailStatus.Code;

            }



            OrderDetail orderDetail = db.OrderDetails.FirstOrDefault(current => current.Id == orderDetailInformation.OrderDetailId && current.IsDeleted == false && current.IsActive == true);

            if (orderDetail != null)
            {
                Order order = db.Orders.Find(orderDetail.OrderId);
                if (order != null)
                    ViewBag.orderId = order.Code;

                ViewBag.Title = "مشخصات بک لینک " + orderDetail.Product.Title;
            }

            return View(orderDetailInfo);
        }
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public ActionResult Upload()
        {
            if (Request.Files[0] != null)
            {
                var file = Request.Files[0];
                Guid orderDetailInfoId = new Guid(Path.GetFileNameWithoutExtension(file.FileName));
                OrderDetailInformation orderDetailInformation = db.OrderDetailInformations.Find(orderDetailInfoId);
                #region Upload and resize image if needed
                string newFilenameUrl = string.Empty;
                if (orderDetailInformation != null)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    newFilenameUrl = "/Uploads/reportage/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    file.SaveAs(physicalFilename);

                    orderDetailInformation.FileUrl = newFilenameUrl;
                    orderDetailInformation.OrderDetailStatusId = db.OrderDetailStatuses.FirstOrDefault(current => current.Code == 2).Id;
                    orderDetailInformation.LastModifiedDate = DateTime.Now;
                    db.Entry(orderDetailInformation).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                #endregion
                return Json(false, JsonRequestBehavior.AllowGet);

                //var fileName = Path.GetFileName(file.ContentType);

                //var path = Path.Combine(Server.MapPath("~/Junk/"), fileName);
                //file.SaveAs(path);
                //return RedirectToAction("Details");
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        public string UpdateDb()
        {
            List<Order> orders = db.Orders.ToList();

            foreach (Order order in orders)
            {
                order.OrderType = "reportage";

            }
            db.SaveChanges();

            List<OrderDetail> orderDetails = db.OrderDetails.ToList();

            foreach (OrderDetail orderDetail in orderDetails)
            {
                for (int i = 0; i < orderDetail.Quantity; i++)
                {

                    OrderDetailInformation odi = new OrderDetailInformation()
                    {
                        Id = Guid.NewGuid(),
                        OrderDetailId = orderDetail.Id,
                        PublishLink = orderDetail.PublishLink,
                        IsSendPublishSms = orderDetail.IsSendPublishSms,
                        CreationDate = DateTime.Now,
                        FileUrl = orderDetail.FileUrl,
                        IsActive = orderDetail.IsActive,
                        IsDeleted = orderDetail.IsDeleted,
                        LastModifiedDate = orderDetail.LastModifiedDate,
                        Description = orderDetail.Description,
                        OrderDetailStatusId = orderDetail.OrderDetailStatusId,
                        DeletionDate = orderDetail.DeletionDate,


                    };

                    db.OrderDetailInformations.Add(odi);

                }
            }
            db.SaveChanges();
            return String.Empty;
        }

        public ActionResult SendBackLinkSms()
        {
            try
            {
                string message = @"کاربر گرامی وب سایت راش وب، برای تمدید بک لینک به وب سایت راش وب مراجعه نمایید";
                DateTime threeDaysAgo = DateTime.Now.Date.AddDays(+3);
                Guid backLinkTypeId = new Guid("D2C2BF40-DAA8-41E5-A68C-859526DEC369");

                List<OrderDetailInformation> orderDetailInfo = db.OrderDetailInformations.Where(current => current.Product.ProductTypeId == backLinkTypeId
                 && current.IsActive && !current.IsDeleted
                 && DbFunctions.TruncateTime(current.FinishDate) == threeDaysAgo.Date
                 ).ToList();
                foreach (var item in orderDetailInfo)
                {
                    Order order = db.Orders.Where(current => current.IsActive && !current.IsDeleted
                    && current.Id == item.OrderDetail.OrderId
                    ).FirstOrDefault();

                    SendSms.SendCommonSms(order.User.CellNum, message);
                }

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
