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
using System.Globalization;

namespace Rush.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BackLinksController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        MenuHelper menu = new MenuHelper();
        public ActionResult Index()
        {
            return View(db.BackLinks.Where(a => a.IsDeleted == false).OrderByDescending(a => a.CreationDate).ToList());
        }


        public string GetTextById(string textId)
        {
            Guid Id = new Guid(textId);
            Text text = db.Texts.Find(Id);
            if (text != null)
                return text.Body;


            return string.Empty;
        }

        [Route("backlink")]
        [AllowAnonymous]
        public ActionResult Details(string amountRange, string daRange)
        {
            if (amountRange == "0-0")
                amountRange = null;


            decimal startAmount = 0;

            decimal endAmount = 100000000;

            if (amountRange != null)
            {
                string[] amountItems = amountRange.Split('-');

                startAmount = Convert.ToDecimal(amountItems[0]);

                endAmount = Convert.ToDecimal(amountItems[1]);
            }

            int startDa = 0;

            int endDa = 100;

            if (daRange != null)
            {
                string[] items = daRange.Split('-');

                startDa = Convert.ToInt32(items[0]);

                endDa = Convert.ToInt32(items[1]);
            }



            Guid textId = new Guid("4c7ef240-8d78-4bdc-b332-3303608c9c7a");
            BackLinkViewModel backLink = new BackLinkViewModel();
            Text text = db.Texts.Find(textId);
            //backLink.Text = db.Texts.Where(current => current.Id == new Guid("4088d9fd-3930-4daa-8d70-07dfd73c0491")).FirstOrDefault();
            backLink.Questions = db.AskedQuestions.Where(current => current.IsDeleted == false && current.Param == "backlink" && current.IsActive == true).OrderBy(current => current.Order).ToList();

            backLink.BackLinks = GetBackLinks(endDa, startDa, endAmount, startAmount);

            backLink.Menu = menu.ReturnMenu();
            backLink.FooterLink = menu.GetFooterLink();
            backLink.Username = menu.ReturnUsername();
            backLink.Body = db.Texts.Find(textId).Body;
            backLink.BottomText = GetTextById("d6169cea-9b7b-4ddf-a8c3-6757e7d8ece9");
            backLink.Rate = text.AverageRate.Value.ToString().Replace('/', '.');
            ViewBag.Title = text.PageTitle;
            ViewBag.Description = text.MetaDescription;
            ViewBag.Canonical = "https://www.rushweb.ir/backlink";
            ViewBag.param = null;
            ViewBag.rate = text.AverageRate.Value.ToString().Replace('/', '.');
            int rateCount = db.Rates.Where(current => current.EntityId == text.Id).Count();

            if (rateCount > 0)
                ViewBag.RatingCount = rateCount;
            else
                ViewBag.RatingCount = 1;
            ViewBag.image = "https://www.rushweb.ir" + text.ImageUrl;
            ViewBag.creationDate = text.CreationDate.ToString(CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(text.LastModifiedDate.ToString()))
                ViewBag.ModifiedDate = text.LastModifiedDate.Value.ToString(CultureInfo.InvariantCulture);
            else
                ViewBag.ModifiedDate = text.CreationDate.ToString(CultureInfo.InvariantCulture);
            return View(backLink);
        }

        public List<BackLinkItem> GetBackLinks(int endDa, int startDa, decimal endAmount, decimal startAmount)
        {
            List<BackLinkItem> backLinkItems = new List<BackLinkItem>();
            List<KeyValueViewModel> backLinkDetailItems = new List<KeyValueViewModel>();

            List<BackLink> backlinks = db.BackLinks.Where(current =>
                current.IsDeleted == false && current.IsActive && current.DomainAuthority <= endDa &&
                current.DomainAuthority >= startDa).OrderBy(current => current.Priority).ToList();

            foreach (BackLink backlink in backlinks)
            {
                List<BackLinkDetail> backLinkDetails = db.BackLinkDetails
                    .Where(c => c.BackLinkId == backlink.Id && c.IsDeleted == false && c.IsActive &&
                                c.Amount <= endAmount &&
                                c.Amount >= startAmount).OrderBy(c=>c.Duration).ToList();

                if (backLinkDetails.Count() == 0)
                    backlinks.Remove(backlink);

                backLinkDetailItems = new List<KeyValueViewModel>();
                foreach (BackLinkDetail backLinkDetail in backLinkDetails)
                {
                    

                    backLinkDetailItems.Add(new KeyValueViewModel()
                    {
                        Id = backLinkDetail.ProductId,
                        Title = backLinkDetail.Duration+" ماهه - "+backLinkDetail.Amount.ToString("N0")+" تومان"
                    });
                }

                backLinkItems.Add(new BackLinkItem()
                {
                    BackLink = backlink,
                    BackLinkDetails = backLinkDetailItems
                });
            }

            return backLinkItems;
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BackLink backLink)
        {
            if (ModelState.IsValid)
            {
                backLink.IsDeleted = false;
                backLink.CreationDate = DateTime.Now;
                backLink.Id = Guid.NewGuid();
                db.BackLinks.Add(backLink);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(backLink);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BackLink backLink = db.BackLinks.Find(id);
            if (backLink == null)
            {
                return HttpNotFound();
            }
            return View(backLink);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BackLink backLink)
        {
            if (ModelState.IsValid)
            {
                backLink.IsDeleted = false;
                backLink.LastModifiedDate = DateTime.Now;
                db.Entry(backLink).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(backLink);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BackLink backLink = db.BackLinks.Find(id);
            if (backLink == null)
            {
                return HttpNotFound();
            }
            return View(backLink);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            BackLink backLink = db.BackLinks.Find(id);
            backLink.IsDeleted = true;
            backLink.DeletionDate = DateTime.Now;

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
        [AllowAnonymous]
        [HttpPost]
        public ActionResult SubmitStar(string rateStar, string param)
        {
            if (ModelState.IsValid)
            {
                //Guid paramId = new Guid(id);

                decimal backlinkRate;
                if (!rateStar.Any(char.IsDigit))
                    backlinkRate = ReturnRate(rateStar);
                else
                    backlinkRate = Convert.ToDecimal(rateStar.Replace('.', '/'));

                Guid textId = new Guid("4c7ef240-8d78-4bdc-b332-3303608c9c7a");
                Text backLink = db.Texts.Find(textId);
                backLink.AverageRate = (backLink.AverageRate + backlinkRate) / 2;

                Rate rate = new Rate();
                rate.Id = Guid.NewGuid();
                rate.EntityId = textId;
                rate.CreationDate = DateTime.Now;
                rate.IsDeleted = false;
                rate.IP = Request.UserHostAddress;
                rate.StarRate = backlinkRate;
                rate.Type = 1;
                db.Rates.Add(rate);
                db.SaveChanges();

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
                return Json("false", JsonRequestBehavior.AllowGet);

        }
        [AllowAnonymous]
        public decimal ReturnRate(string rateStar)
        {
            decimal rate = 0;
            switch (rateStar.ToLower())
            {
                case "one stars":
                    {
                        rate = 1;
                        break;
                    }
                case "two stars":
                    {
                        rate = 2;
                        break;
                    }
                case "three stars":
                    {
                        rate = 3;
                        break;
                    }
                case "four stars":
                    {
                        rate = 4;
                        break;
                    }
                case "five stars":
                    {
                        rate = 5;
                        break;
                    }
                case "one & half star":
                    {
                        rate = Convert.ToDecimal(1.5);
                        break;
                    }
                case "two & half stars":
                    {
                        rate = Convert.ToDecimal(2.5);
                        break;
                    }
                case "three & half stars":
                    {
                        rate = Convert.ToDecimal(3.5);
                        break;
                    }
                case "four & half stars":
                    {
                        rate = Convert.ToDecimal(4.5);
                        break;
                    }
                case "half star":
                    {
                        rate = Convert.ToDecimal(0.5);
                        break;
                    }

            }
            return rate;
        }


        [AllowAnonymous]
        public string UpdateBackLinkDetailPrice()
        {
            List<BackLink> backlinks = db.BackLinks.Where(c => c.IsDeleted == false).ToList();

            foreach (BackLink backlink in backlinks)
            {
                if (backlink.OneMonthBackLink != null)
                {
                    Product product = new Product()
                    {
                        Id = Guid.NewGuid(),
                        Title = backlink.FullName + " 1 ماهه",
                        CreationDate = DateTime.Now,
                        IsActive = backlink.IsActive,
                        IsDeleted = false,
                        ProductTypeId = db.ProductTypes.FirstOrDefault(c => c.Name == "backlink").Id,
                    };

                    db.Products.Add(product);


                    BackLinkDetail backLinkDetail = new BackLinkDetail()
                    {
                        Id = Guid.NewGuid(),
                        Title = backlink.FullName + " 1 ماهه",
                        Duration = 1,
                        ProductId = product.Id,
                        BackLinkId = backlink.Id,
                        CreationDate = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        Amount = backlink.OneMonthBackLink.Value,

                    };

                    db.BackLinkDetails.Add(backLinkDetail);

                    db.SaveChanges();

                }
                if (backlink.ThreeMonthBackLink != null)
                {
                    Product product = new Product()
                    {
                        Id = Guid.NewGuid(),
                        Title = backlink.FullName + " 3 ماهه",
                        CreationDate = DateTime.Now,
                        IsActive = backlink.IsActive,
                        IsDeleted = false,
                        ProductTypeId = db.ProductTypes.FirstOrDefault(c => c.Name == "backlink").Id,
                    };

                    db.Products.Add(product);


                    BackLinkDetail backLinkDetail = new BackLinkDetail()
                    {
                        Id = Guid.NewGuid(),
                        Title = backlink.FullName + " 3 ماهه",
                        Duration = 3,
                        ProductId = product.Id,
                        BackLinkId = backlink.Id,
                        CreationDate = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        Amount = backlink.ThreeMonthBackLink.Value,

                    };

                    db.BackLinkDetails.Add(backLinkDetail);

                    db.SaveChanges();

                }
            }


            return string.Empty;
        }
    }
}
