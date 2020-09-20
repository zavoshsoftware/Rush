using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Eshop.Helpers;
using Helper;
using Helpers;
using Microsoft.AspNet.Identity;
using Models;

using ViewModels;

namespace Rush.Controllers
{
    public class BasketController : Controller
    {
        private DatabaseContext db = new DatabaseContext();


        //[Route("cart")]
        //[HttpPost]
        //public ActionResult AddToCart(string code, string qty)
        //{
        //    Guid id = new Guid(code);

        //    SetCookie(id, qty);
        //    return Json("true", JsonRequestBehavior.AllowGet);
        //}

        public void SetCookie(string code, string quantity)
        {
            string cookievalue = null;

            if (Request.Cookies["basket"] != null)
            {
                bool changeCurrentItem = false;

                cookievalue = Request.Cookies["basket"].Value;

                string[] coockieItems = cookievalue.Split('/');

                for (int i = 0; i < coockieItems.Length - 1; i++)
                {
                    string[] coockieItem = coockieItems[i].Split('^');

                    if (coockieItem[0] == code)
                    {
                        coockieItem[1] = (Convert.ToInt32(coockieItem[1]) + 1).ToString();
                        changeCurrentItem = true;
                        coockieItems[i] = coockieItem[0] + "^" + coockieItem[1];
                        break;
                    }
                }

                if (changeCurrentItem)
                {
                    cookievalue = null;
                    for (int i = 0; i < coockieItems.Length - 1; i++)
                    {
                        cookievalue = cookievalue + coockieItems[i] + "/";
                    }

                }
                else
                    cookievalue = cookievalue + code + "^" + quantity + "/";

            }
            else
                cookievalue = code + "^" + quantity + "/";

            HttpContext.Response.Cookies.Set(new HttpCookie("basket")
            {
                Name = "basket",
                Value = cookievalue,
                Expires = DateTime.Now.AddDays(1)
            });
        }





        MenuHelper menuHelper = new MenuHelper();

        public decimal GetSidebarOrderIncome(List<ProductInSidebar> products)
        {
            decimal incomeTotal = 0;

            foreach (ProductInSidebar orderDetail in products)
            {
                Guid productId = new Guid(orderDetail.Id);

                Product product = db.Products.Find(productId);

                decimal amount = GetAmountByProduct(product);

                decimal buyAmount = GetBuyAmountByProduct(product);

                incomeTotal = incomeTotal + ((amount - buyAmount) * orderDetail.Quantity);
            }

            return incomeTotal;
        }

        public decimal GetPackagetotal(List<ProductInCart> products, string type)
        {
            decimal amount = 0;

            if (type == "total")
            {
                foreach (ProductInCart orderDetail in products)
                {
                    ReportageGroup reportageGroup =
                        db.ReportageGroups.FirstOrDefault(c => c.ProductId == orderDetail.Product.Id);

                    amount = amount + (reportageGroup.Price.Value * orderDetail.Quantity);

                }
            }

            if (type == "subtotal")
            {
                foreach (ProductInCart orderDetail in products)
                {
                    amount = amount + Convert.ToDecimal(orderDetail.RowAmount);
                }
            }

            return amount;
        }

        public decimal GetOrderIncome(List<ProductInCart> products)
        {
            decimal incomeTotal = 0;

            foreach (ProductInCart orderDetail in products)
            {
                decimal amount = GetAmountByProduct(orderDetail.Product);

                decimal buyAmount = GetBuyAmountByProduct(orderDetail.Product);


                incomeTotal = incomeTotal + ((amount - buyAmount) * orderDetail.Quantity);
            }

            return incomeTotal;
        }

        public decimal GetStepDiscountAmount(decimal income)
        {
            StepDiscount stepDiscount = db.StepDiscounts.FirstOrDefault(c => c.IsDeleted == false && c.IsActive);

            if (stepDiscount == null)
                return 0;

            List<StepDiscountDetail> stepDiscountDetails =
                db.StepDiscountDetails.Where(c => c.StepDiscountId == stepDiscount.Id).OrderBy(c => c.TargetValue).ToList();

            decimal discount = 0;
            bool isBiggetThanLastStep = true;

            foreach (StepDiscountDetail stepDiscountDetail in stepDiscountDetails)
            {
                if (income < stepDiscountDetail.TargetValue)
                {
                    discount = (income * stepDiscountDetail.DiscountPercent) / 100;
                    isBiggetThanLastStep = false;
                    break;
                }
            }

            if (isBiggetThanLastStep)
            {
                StepDiscountDetail lastStep = stepDiscountDetails.LastOrDefault();

                if (lastStep != null)
                    discount = (income * lastStep.DiscountPercent) / 100;
            }

            return discount;
        }

        [Route("Basket")]
        public ActionResult Basket(string basketType,int? orderCode)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                Guid userId = new Guid(identity.Name);
                User user = db.Users.FirstOrDefault(current => current.Id == userId);

                if (user != null)
                {
                    ViewBag.FullName = user.FullName;
                    ViewBag.CellNumber = user.CellNum;
                    ViewBag.isAuthorize = "true";
                }
                else
                    ViewBag.isAuthorize = "false";

            }
            else
                ViewBag.isAuthorize = "false";

            CartViewModel cart = new CartViewModel();

            cart.Menu = menuHelper.ReturnMenu();

            cart.FooterLink = menuHelper.GetFooterLink();
            List<ProductInCart> productInCarts = new List<ProductInCart>();
            if (string.IsNullOrEmpty(orderCode.ToString()))
            {
                productInCarts = GetProductInBasketByCoockie(basketType);
            }
            else
            {
                Order order = db.Orders.Where(x => x.Code == orderCode.Value).FirstOrDefault();
                basketType = order.OrderType;
                productInCarts = GetProductsInCartByOrder(orderCode.Value);
            }

            cart.Products = productInCarts;

            if (basketType == "reportage")
            {
                decimal subTotal = GetSubtotal(productInCarts);


                cart.SubTotal = subTotal.ToString("n0") + " تومان";
                decimal income = GetOrderIncome(productInCarts);


                decimal discountAmount = GetStepDiscountAmount(income);

                cart.DiscountAmount = discountAmount.ToString("n0") + " تومان";

                cart.Total = (subTotal - discountAmount).ToString("n0");
            }
            else if (basketType == "backlink")
            {
                decimal subTotal = GetSubtotal(productInCarts);


                cart.SubTotal = subTotal.ToString("n0") + " تومان";
              

             
                cart.DiscountAmount = "0";

                cart.Total = (subTotal ).ToString("n0");
            }
            else if (basketType == "package")
            {
                decimal discount = 0;
                decimal total = 0;
                decimal subTotal = 0;

                foreach (ProductInCart productInCart in productInCarts)
                {
                    Guid productId = new Guid(productInCart.Id);

                    ReportageGroup reportageGroup =
                        db.ReportageGroups.FirstOrDefault(c => c.ProductId == productId);

                    if (reportageGroup != null)
                    {
                        if (reportageGroup.Value != null && reportageGroup.Price != null)
                        {
                            subTotal = subTotal + reportageGroup.Value.Value;
                            total = total + reportageGroup.Price.Value;
                        }
                    }
                }

                discount = subTotal - total;

                cart.SubTotal = subTotal.ToString("n0") + " تومان";

                cart.DiscountAmount = discount.ToString("n0") + " تومان";

                cart.Total = total.ToString("n0");
            }
            return View(cart);
        }


        [HttpPost]
        public ActionResult GetSideBarBasket(string productType)
        {
            try
            {
                SidebarBasketViewModel cart = new SidebarBasketViewModel();

                List<ProductInSidebar> productInCarts = GetSidebarProductInBasketByCoockie(productType);

                cart.Products = productInCarts;


                if (productType == "reportage")
                {
                    decimal subTotal = GetSidebarSubtotal(productInCarts);

                    cart.SubTotal = subTotal.ToString("n0") + " تومان";

                    decimal income = GetSidebarOrderIncome(productInCarts);

                    decimal discountAmount = GetStepDiscountAmount(income);

                    cart.DiscountAmount = discountAmount.ToString("n0") + " تومان";

                    cart.Total = (subTotal - discountAmount).ToString("n0");
                }

                else if (productType == "backlink")
                {
                    decimal subTotal = GetSidebarSubtotal(productInCarts);

                    cart.SubTotal = subTotal.ToString("n0") + " تومان";



                    cart.DiscountAmount = "0";

                    cart.Total = (subTotal).ToString("n0");
                }

                else if (productType == "package")
                {
                    decimal discount = 0;
                    decimal total = 0;
                    decimal subTotal = 0;

                    foreach (ProductInSidebar productInCart in productInCarts)
                    {
                        Guid productId = new Guid(productInCart.Id);

                        ReportageGroup reportageGroup =
                            db.ReportageGroups.FirstOrDefault(c => c.ProductId == productId);

                        if (reportageGroup != null)
                        {
                            if (reportageGroup.Value != null && reportageGroup.Price != null)
                            {
                                subTotal = subTotal + reportageGroup.Value.Value;
                                total = total + reportageGroup.Price.Value;
                            }
                        }
                    }

                    discount = subTotal - total;

                    cart.SubTotal = subTotal.ToString("n0") + " تومان";

                    cart.DiscountAmount = discount.ToString("n0") + " تومان";

                    cart.Total = total.ToString("n0");
                }


                return Json(cart, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);

            }
        }

        public List<ProductInSidebar> GetSidebarProductInBasketByCoockie(string cookieName)
        {
            List<ProductInSidebar> productInCarts = new List<ProductInSidebar>();

            string[] basketItems = GetCookie(cookieName);

            if (basketItems != null)
            {
                for (int i = 0; i < basketItems.Length - 1; i++)
                {
                    string[] productItem = basketItems[i].Split('^');

                    Guid productId = new Guid(productItem[0]);

                    Product product =
                        db.Products.FirstOrDefault(current =>
                            current.IsDeleted == false && current.Id == productId);

                    productInCarts.Add(new ProductInSidebar()
                    {
                        Quantity = Convert.ToInt32(productItem[1]),
                        Id = product.Id.ToString(),
                        ProductTitle = product.Title,
                    });
                }
            }

            return productInCarts;
        }
        public List<ProductInCart> GetProductInBasketByCoockie(string cookieName)
        {
            List<ProductInCart> productInCarts = new List<ProductInCart>();

            string[] basketItems = GetCookie(cookieName);

            if (basketItems != null)
            {
                for (int i = 0; i < basketItems.Length - 1; i++)
                {
                    string[] productItem = basketItems[i].Split('^');

                    Guid productId = new Guid(productItem[0]);

                    Product product =
                        db.Products.FirstOrDefault(current =>
                            current.IsDeleted == false && current.Id == productId);

                    productInCarts.Add(new ProductInCart()
                    {
                        Product = product,
                        Quantity = Convert.ToInt32(productItem[1]),
                        Id = productItem[0]
                    });
                }
            }

            return productInCarts;
        }

        public List<ProductInCart> GetProductsInCartByOrder(int code)
        {
            List<ProductInCart> products = new List<ProductInCart>();
            Order order = db.Orders.Where(x => x.Code == code).FirstOrDefault();
            List<OrderDetail> details = db.OrderDetails.Where(current => current.OrderId == order.Id).ToList();
            foreach (OrderDetail orderDetail in details)
            {
                products.Add(new ProductInCart()
                {
                    Id = orderDetail.ProductId.ToString(),
                    Quantity = orderDetail.Quantity,
                    Product = orderDetail.Product
                });
            }
            return products;
        }

        public string[] GetCookie(string cookieName)
        {
            if (Request.Cookies[cookieName] != null)
            {
                string cookievalue = Request.Cookies[cookieName].Value;

                string[] basketItems = cookievalue.Split('/');

                return basketItems;
            }

            return null;
        }

        public decimal GetSubtotal(List<ProductInCart> orderDetails)
        {
            decimal subTotal = 0;

            foreach (ProductInCart orderDetail in orderDetails)
            {
                decimal amount = GetAmountByProduct(orderDetail.Product);

                subTotal = subTotal + (amount * orderDetail.Quantity);
            }

            return subTotal;
        }

        public decimal GetSidebarSubtotal(List<ProductInSidebar> orderDetails)
        {
            decimal subTotal = 0;

            foreach (ProductInSidebar orderDetail in orderDetails)
            {
                Guid productId = new Guid(orderDetail.Id);

                Product product = db.Products.Find(productId);

                decimal amount = GetAmountByProduct(product);

                subTotal = subTotal + (amount * orderDetail.Quantity);
            }

            return subTotal;
        }

        public decimal GetAmountByProduct(Product product)
        {
            decimal amount = 0;

            if (product.ProductType.Name == "reportage")
            {
                Reportage reportage = db.Reportages.FirstOrDefault(c => c.ProductId == product.Id);

                amount = reportage.Price;

                if (reportage.IsInPromotion)
                {
                    if (reportage.DiscountAmount != null)
                        amount = reportage.DiscountAmount.Value;
                }

                return amount;
            }
            else if (product.ProductType.Name == "package")
            {
                ReportageGroup reportageGroup = db.ReportageGroups.FirstOrDefault(c => c.ProductId == product.Id);

                if (reportageGroup != null)
                {
                    if (reportageGroup.Value != null)
                        amount = reportageGroup.Value.Value;

                    else
                        amount = 0;
                }
                else
                    amount = 0;


                return amount;
            }
            else if (product.ProductType.Name == "backlink")
            {
                BackLinkDetail backLinkDetail = db.BackLinkDetails.FirstOrDefault(c => c.ProductId == product.Id);

                if (backLinkDetail != null)
                {
                    amount = backLinkDetail.Amount;
                }
                else
                    amount = 0;


                return amount;
            }
            return amount;
        }


        public decimal GetAmountByProductForInsertOrderDetail(Product product)
        {
            decimal amount = 0;

            if (product.ProductType.Name == "reportage")
            {
                Reportage reportage = db.Reportages.FirstOrDefault(c => c.ProductId == product.Id);

                amount = reportage.Price;

                if (reportage.IsInPromotion)
                {
                    if (reportage.DiscountAmount != null)
                        amount = reportage.DiscountAmount.Value;
                }

                return amount;
            }
            else if (product.ProductType.Name == "backlink")
            {
                BackLinkDetail backLinkDetail = db.BackLinkDetails.FirstOrDefault(c => c.ProductId == product.Id);

                if (backLinkDetail != null)
                    amount = backLinkDetail.Amount;
                else
                    amount = 0;


                return amount;
            }
            else if (product.ProductType.Name == "package")
            {
                ReportageGroup reportageGroup = db.ReportageGroups.FirstOrDefault(c => c.ProductId == product.Id);

                if (reportageGroup != null)
                {
                    if (reportageGroup.Price != null)
                        amount = reportageGroup.Price.Value;

                    else
                        amount = 0;
                }
                else
                    amount = 0;


                return amount;
            }
            return amount;
        }

        public decimal GetBuyAmountByProduct(Product product)
        {
            decimal amount = 0;

            if (product.ProductType.Name == "reportage")
            {
                Reportage reportage = db.Reportages.FirstOrDefault(c => c.ProductId == product.Id);

                if (reportage.BuyAmount != null)
                    amount = reportage.BuyAmount.Value;

                return amount;
            }

            return amount;
        }

        public decimal GetDiscount()
        {
            if (Request.Cookies["discount"] != null)
            {
                try
                {
                    string cookievalue = Request.Cookies["discount"].Value;

                    string[] basketItems = cookievalue.Split('/');
                    return Convert.ToDecimal(basketItems[0]);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return 0;
        }


        public ActionResult BankRedirect()
        {
            return View();
        }

        public ActionResult Finalize(string basketType, int? orderCode)
        {
            try
            {
                List<ProductInCart> productInCarts = new List<ProductInCart>();
                Order order = new Order();
                if (string.IsNullOrEmpty(orderCode.ToString()))
                {
                     productInCarts = GetProductInBasketByCoockie(basketType);
                    order = ConvertCoockieToOrder(productInCarts);
                }
                else
                {
                    order = db.Orders.Where(x => x.Code == orderCode.Value).FirstOrDefault();
                    basketType = order.OrderType;
                    productInCarts = GetProductsInCartByOrder(orderCode.Value);
                }

                

                if (order != null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                        Guid userId = new Guid(identity.Name);
                        User user = db.Users.FirstOrDefault(current => current.Id == userId);

                        if (user != null)
                        {
                            order.UserId = user.Id;
                            order.DeliverFullName = user.FullName;
                            order.DeliverCellNumber = user.CellNum;
                            order.OrderType = basketType;

                            if (basketType == "reportage")
                            {
                                decimal income = GetOrderIncome(productInCarts);

                                decimal discountAmount = GetStepDiscountAmount(income);

                                order.DiscountAmount = discountAmount;

                                order.TotalAmount = GetTotalAmount(order.SubTotal, order.DiscountAmount);
                            }
                            else if (basketType == "backlink")
                            {
                                order.DiscountAmount = 0;

                                order.TotalAmount = order.SubTotal.Value;
                            }
                            else if (basketType == "package")
                            {
                                decimal subtotal = GetPackagetotal(productInCarts, "subtotal");

                                order.SubTotal = subtotal;

                                decimal total = GetPackagetotal(productInCarts, "total");

                                order.DiscountAmount = subtotal - total;

                                order.TotalAmount = total;
                            }

                            OrderStatus orderStatus = db.OrderStatuses.FirstOrDefault(current => current.Code == 2);
                            if (orderStatus != null)
                                order.OrderStatusId = orderStatus.Id;


                            db.SaveChanges();

                            InsertOrderDetailInfo(order);

                            db.SaveChanges();

                            RemoveCookie(basketType);

                            string res = zp.ZarinPalRedirect(order, order.TotalAmount);



                            return Json(res, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json("false", JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);

            }
        }
        ZarinPalHelper zp = new ZarinPalHelper();

        public void InsertOrderDetailInfo(Order order)
        {
            List<OrderDetail> orderDetails = db.OrderDetails.Where(c => c.OrderId == order.Id).ToList();

            foreach (OrderDetail orderDetail in orderDetails)
            {
                if (order.OrderType == "package")
                {
                    ReportageGroup reportageGroup = db.ReportageGroups
                        .FirstOrDefault(c => c.ProductId == orderDetail.ProductId);

                    List<Reportage> reportages = db.Reportages.Where(c =>
                        c.ReportageGroupId == reportageGroup.Id && c.IsActive && c.IsDeleted == false).ToList();

                    for (int i = 0; i < orderDetail.Quantity; i++)
                    {
                        foreach (Reportage reportage in reportages)
                        {
                            OrderDetailInformation orderDetailInformation = new OrderDetailInformation()
                            {
                                Id = Guid.NewGuid(),
                                OrderDetailId = orderDetail.Id,
                                OrderDetailStatusId = db.OrderDetailStatuses.FirstOrDefault(current => current.Code == 1).Id,
                                CreationDate = DateTime.Now,
                                IsDeleted = false,
                                IsActive = true,
                                ProductId = reportage.ProductId
                            };

                            db.OrderDetailInformations.Add(orderDetailInformation);
                        }
                    }
                }

                else if (order.OrderType == "reportage")
                {
                    for (int i = 0; i < orderDetail.Quantity; i++)
                    {
                        OrderDetailInformation orderDetailInformation = new OrderDetailInformation()
                        {
                            Id = Guid.NewGuid(),
                            OrderDetailId = orderDetail.Id,
                            OrderDetailStatusId = db.OrderDetailStatuses.FirstOrDefault(current => current.Code == 1).Id,
                            CreationDate = DateTime.Now,
                            IsDeleted = false,
                            IsActive = true,
                        };

                        db.OrderDetailInformations.Add(orderDetailInformation);
                    }
                }

                else if (order.OrderType == "backlink")
                {
                    for (int i = 0; i < orderDetail.Quantity; i++)
                    {
                        OrderDetailInformation orderDetailInformation = new OrderDetailInformation()
                        {
                            Id = Guid.NewGuid(),
                            OrderDetailId = orderDetail.Id,
                            OrderDetailStatusId = db.OrderDetailStatuses.FirstOrDefault(current => current.Code == 1).Id,
                            CreationDate = DateTime.Now,
                            IsDeleted = false,
                            IsActive = true,
                        };

                        db.OrderDetailInformations.Add(orderDetailInformation);
                    }
                }

            }
        }

        #region Finalize

        public Guid GetUser(string cellnumber, string fullname, string email)
        {
            User user = db.Users.FirstOrDefault(c => c.CellNum == cellnumber);
            if (user != null)
                return user.Id;
            else
            {
                User oUser = new User()
                {
                    Id = Guid.NewGuid(),
                    FullName = fullname,
                    CellNum = cellnumber,
                    Code = 1,
                    RoleId = db.Roles.FirstOrDefault(c => c.Name == "productcustomer").Id,
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    IsDeleted = false
                };

                db.Users.Add(oUser);

                return oUser.Id;
            }
        }

        //[Route("Basket/remove/{code}")]
        public ActionResult RemoveFromBasket(string code, string productType)
        {
            try
            {

                string[] coockieItems = GetCookie(productType);


                for (int i = 0; i < coockieItems.Length - 1; i++)
                {
                    string[] coockieItem = coockieItems[i].Split('^');

                    if (coockieItem[0] == code)
                    {
                        string removeArray = coockieItem[0] + "^" + coockieItem[1];
                        coockieItems = coockieItems.Where(current => current != removeArray).ToArray();
                        break;
                    }
                }

                string cookievalue = null;
                for (int i = 0; i < coockieItems.Length - 1; i++)
                {
                    cookievalue = cookievalue + coockieItems[i] + "/";
                }

                HttpContext.Response.Cookies.Set(new HttpCookie(productType)
                {
                    Name = productType,
                    Value = cookievalue,
                    Expires = DateTime.Now.AddDays(1)
                });

                return Json("true", JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);

            }
        }


        public decimal GetTotalAmount(decimal? subtotal, decimal? discount)
        {
            decimal discountAmount = 0;
            if (discount != null)
                discountAmount = (decimal)discount;



            if (subtotal == null)
                subtotal = 0;

            return (decimal)subtotal - discountAmount;
        }
        CodeCreator codeCreator = new CodeCreator();

        public Order ConvertCoockieToOrder(List<ProductInCart> products)
        {
            try
            {

                Order order = new Order();

                order.Id = Guid.NewGuid();
                order.IsActive = true;
                order.IsDeleted = false;
                order.IsPaid = false;
                order.CreationDate = DateTime.Now;
                order.LastModifiedDate = DateTime.Now;
                order.Code = codeCreator.ReturnOrderCode();
                order.OrderStatusId = db.OrderStatuses.FirstOrDefault(current => current.Code == 1).Id;
                order.SubTotal = GetSubtotal(products);




                //order.DiscountAmount = GetDiscount();
                //order.DiscountCodeId = GetDiscountId();

                //order.TotalAmount = Convert.ToDecimal(order.SubTotal - order.DiscountAmount);


                db.Orders.Add(order);

                foreach (ProductInCart product in products)
                {
                    decimal amount = GetAmountByProductForInsertOrderDetail(product.Product);
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        ProductId = product.Product.Id,
                        Quantity = product.Quantity,
                        Amount = amount * product.Quantity,
                        IsDeleted = false,
                        IsActive = true,
                        CreationDate = DateTime.Now,
                        OrderId = order.Id,
                        Price = amount,
                        OrderDetailStatusId = db.OrderDetailStatuses.FirstOrDefault(current => current.Code == 1).Id
                    };


                    db.OrderDetails.Add(orderDetail);
                }

                return order;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public Guid? GetDiscountId()
        {
            if (Request.Cookies["discount"] != null)
            {
                try
                {
                    string cookievalue = Request.Cookies["discount"].Value;

                    string[] basketItems = cookievalue.Split('/');

                    DiscountCode discountCode =
                        db.DiscountCodes.FirstOrDefault(current => current.Code == basketItems[1]);

                    return discountCode?.Id;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }


        public void RemoveCookie(string cookieType)
        {
            if (Request.Cookies[cookieType] != null)
            {
                Response.Cookies[cookieType].Expires = DateTime.Now.AddDays(-1);
            }
        }
        #endregion


        public long GetAmountByAuthority(string authority)
        {
            ZarinpallAuthority zarinpallAuthority =
               db.ZarinpallAuthorities.FirstOrDefault(current => current.Authority == authority);

            if (zarinpallAuthority != null)
                return Convert.ToInt64(zarinpallAuthority.Amount);

            return 0;
        }

        private String MerchantId = "28370b76-bf5f-11ea-8f7a-000c295eb8fc";

        public Order GetOrderByAuthority(string authority)
        {
            ZarinpallAuthority zarinpallAuthority =
                db.ZarinpallAuthorities.FirstOrDefault(current => current.Authority == authority);

            if (zarinpallAuthority != null)
                return zarinpallAuthority.Order;

            else
                return null;
        }

        [Route("callback")]
        public ActionResult CallBack(string authority, string status)
        {
            String Status = status;
            CallBackViewModel callBack = new CallBackViewModel()
            {
                Menu = menuHelper.ReturnMenu(),
                FooterLink = menuHelper.GetFooterLink()
            };

            if (Status != "OK")
            {
                callBack.IsSuccess = false;
            }

            else
            {
                try
                {
                    var zarinpal = ZarinPal.ZarinPal.Get();
                    zarinpal.DisableSandboxMode();
                    String Authority = authority;
                    long Amount = GetAmountByAuthority(Authority);

                    var verificationRequest = new ZarinPal.PaymentVerification(MerchantId, Amount, Authority);
                    var verificationResponse = zarinpal.InvokePaymentVerification(verificationRequest);
                    if (verificationResponse.Status == 100 || verificationResponse.Status == 101)
                    {
                        Order order = GetOrderByAuthority(authority);
                        if (order != null)
                        {
                            order.IsPaid = true;
                            order.PaymentDate = DateTime.Now;
                            order.RefId = verificationResponse.RefID;

                            db.SaveChanges();

                            callBack.IsSuccess = true;
                            callBack.OrderCode = order.Code.ToString();
                            callBack.RefrenceId = verificationResponse.RefID;

                            // UpdateUserPoint(order);

                            OrderDetail orderDetail = db.OrderDetails.FirstOrDefault(current => current.OrderId == order.Id);

                            if (orderDetail != null)
                            {
                                Product product = db.Products.Find(orderDetail.ProductId);

                                if (product != null)
                                {
                                    User user = db.Users.Find(order.UserId);
                                    string message = "سفارش شما با شماره پیگیری " + callBack.RefrenceId + " در وب سایت راش وب ثبت گردید ";
                                    SendSms.SendCommonSms(user.CellNum, message);
                                    List<User> admins = db.Users.Where(current => current.IsActive && !current.IsDeleted && current.Role.Name == "Administrator").ToList();
                                    foreach (User admin in admins)
                                    {
                                        message = "یک سفارش با کد " + order.Code + " در وب سایت راش وب ثبت شده است.";
                                        SendSms.SendCommonSms(admin.CellNum, message);
                                    }
                                    //ViewBag.Email = order.DeliverEmail;

                                    //CreateEmail(order.Email, fileLink, orderType);

                                }
                            }

                        }
                        else
                        {
                            callBack.IsSuccess = false;
                            callBack.RefrenceId = "سفارش پیدا نشد";
                        }
                    }
                    else
                    {
                        callBack.IsSuccess = false;
                        callBack.RefrenceId = verificationResponse.Status.ToString();
                    }
                }
                catch (Exception e)
                {
                    callBack.IsSuccess = false;
                    callBack.RefrenceId = "خطا سیستمی. لطفا با پشتیبانی سایت تماس بگیرید";
                }
            }
            return View(callBack);

        }

    }
}