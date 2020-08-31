using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using Helper;
using ViewModels;
using System.Globalization;
using System.IO;

namespace Rush.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ReportagesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        MenuHelper menuHelper = new MenuHelper();

    
        public ActionResult Index(Guid id)
        {

            return View(db.Reportages.Where(a => a.IsDeleted == false && a.ReportageGroupId == id).OrderBy(a => a.Priority).ToList());
        }

        
        public ActionResult Create(Guid id)
        {
            ViewBag.id = id;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reportage reportage, Guid id, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                string newFilenameUrl = string.Empty;
                if (fileUpload != null)
                {
                    string filename = Path.GetFileName(fileUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    newFilenameUrl = "/Uploads/reportage/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUpload.SaveAs(physicalFilename);

                    reportage.ImageUrl = newFilenameUrl;
                }
                #endregion

                Product product=new Product()
                {
                    Id=Guid.NewGuid(),
                    IsDeleted = false,
                    IsActive = reportage.IsActive,
                    CreationDate = DateTime.Now,
                    Title = reportage.FullName,
                    ProductTypeId = db.ProductTypes.FirstOrDefault(c=>c.Name== "reportage").Id
                };

                db.Products.Add(product);

                reportage.ProductId = product.Id;
                reportage.IsDeleted = false;
                reportage.CreationDate = DateTime.Now;
                reportage.Id = Guid.NewGuid();
                reportage.ReportageGroupId = id;
                db.Reportages.Add(reportage);

                db.SaveChanges();

                return RedirectToAction("Index", new { id = id });
            }
            ViewBag.id = id;
            return View(reportage);
        }


        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reportage reportage = db.Reportages.Find(id);
            if (reportage == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = reportage.ReportageGroupId;
            ViewBag.ReportageGroupId = new SelectList(db.ReportageGroups.Where(current => current.IsDeleted == false ).ToList(), "Id", "Title", reportage.ReportageGroupId);
            return View(reportage);
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Reportage reportage, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                string newFilenameUrl = string.Empty;
                if (fileUpload != null)
                {
                    string filename = Path.GetFileName(fileUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    newFilenameUrl = "/Uploads/reportage/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUpload.SaveAs(physicalFilename);

                    reportage.ImageUrl = newFilenameUrl;
                }
                #endregion

                Product product = db.Products.Find(reportage.ProductId);

                if (product != null)
                {
                    product.Title = reportage.FullName;
                    product.IsActive = reportage.IsActive;
                }

                reportage.IsDeleted = false;
                reportage.LastModifiedDate = DateTime.Now;
                db.Entry(reportage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = reportage.ReportageGroupId });
            }
            ViewBag.id = reportage.ReportageGroupId;
            ViewBag.ReportageGroupId = new SelectList(db.ReportageGroups.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "Title", reportage.ReportageGroupId);

            return View(reportage);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reportage reportage = db.Reportages.Find(id);
            if (reportage == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = reportage.ReportageGroupId;
            return View(reportage);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Reportage reportage = db.Reportages.Find(id);
            reportage.IsDeleted = true;
            reportage.DeletionDate = DateTime.Now;

            db.SaveChanges();

            return RedirectToAction("Index", new { id = reportage.ReportageGroupId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [Route("reportagelisttest")]
        [AllowAnonymous]
        public ActionResult List(string groupId, string amountRange, string daRange,string site)
        {

            if (amountRange == "0-0")
                amountRange = null;
            if (daRange == "0-0")
                daRange = null;
            if (groupId == "0")
                groupId = null;
            if (string.IsNullOrEmpty(site))
                site = null;


            Guid textId = new Guid("c1737127-eb53-4a4b-9138-42a1980b82fc");
            ReportageListViewModel reportageListView = new ReportageListViewModel();
            Text text = db.Texts.Find(textId);
            reportageListView.Menu = menuHelper.ReturnMenu();
            reportageListView.FooterLink = menuHelper.GetFooterLink();
            reportageListView.Text = text;
            reportageListView.BottomText = GetTextById("59e447d1-35a3-40c5-8ee6-833e7bc410ec");

            reportageListView.Rate = text.AverageRate.Value.ToString().Replace('/', '.');
            reportageListView.ReportageByGroups = ReturnReportage(groupId,amountRange,daRange,site);
            reportageListView.ReportageGroups = db.ReportageGroups.Where(current => current.IsDeleted == false && current.IsActive == true && current.IsPackage == false).OrderBy(current => current.Priority).ToList();
            reportageListView.Questions = db.AskedQuestions.Where(current => current.IsDeleted == false && current.Param == "reportage" && current.IsActive == true).OrderBy(current => current.Order).ToList();
            ViewBag.Title = text.PageTitle;
            ViewBag.Description = text.MetaDescription;
            ViewBag.Canonical = "https://www.rushweb.ir/reportage";
            ViewBag.param = textId;
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
            return View(reportageListView);
        }

       
        [Route("reportage")]
        [AllowAnonymous]
        public ActionResult List2(string groupId, string amountRange, string daRange,string site)
        {
            if (amountRange == "0-0")
                amountRange = null;
            if (daRange == "0-0")
                daRange = null;
            if (groupId == "0")
                groupId = null;
            if (string.IsNullOrEmpty(site))
                site = null;


            Guid textId = new Guid("c1737127-eb53-4a4b-9138-42a1980b82fc");
            ReportageListViewModel reportageListView = new ReportageListViewModel();
            Text text = db.Texts.Find(textId);

            reportageListView.Menu = menuHelper.ReturnMenu();
            reportageListView.FooterLink = menuHelper.GetFooterLink();
            reportageListView.FooterLink = menuHelper.GetFooterLink();
            reportageListView.BottomText = GetTextById("59e447d1-35a3-40c5-8ee6-833e7bc410ec");


            if (text != null)
            {
                reportageListView.Text = text;

                reportageListView.Rate = text.AverageRate.Value.ToString().Replace('/', '.');
                ViewBag.Title = text.PageTitle;
                ViewBag.Description = text.MetaDescription;

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
            }
     
            reportageListView.ReportageByGroups = ReturnReportage(groupId,amountRange,daRange,site);
            reportageListView.ReportageGroups = db.ReportageGroups.Where(current => current.IsDeleted == false && current.IsActive == true && current.IsPackage == false).OrderBy(current => current.Priority).ToList();
            reportageListView.Questions = db.AskedQuestions.Where(current => current.IsDeleted == false && current.Param == "reportage" && current.IsActive == true).OrderBy(current => current.Order).ToList();
         
            ViewBag.Canonical = "https://www.rushweb.ir/reportage";
            ViewBag.param = textId;
     
            return View(reportageListView);
        }

        public string GetTextById(string textId)
        {
            Guid Id = new Guid(textId);
            Text text = db.Texts.Find(Id);
            if (text != null)
                return text.Body;


            return string.Empty;
        }

        [Route("reportage/package")]
        [AllowAnonymous]
        public ActionResult Package()
        {
            Guid textId = new Guid("ce56ff8c-f061-471a-b9f1-88af9a8e57a1");
            ReportageListViewModel reportageListView = new ReportageListViewModel();
            reportageListView.ReportageByGroups = ReturnPackageReportage();

            Text text = db.Texts.Find(textId);
            reportageListView.Menu = menuHelper.ReturnMenu();
            reportageListView.Rate = text.AverageRate.Value.ToString().Replace('/', '.');
            reportageListView.FooterLink = menuHelper.GetFooterLink();
            reportageListView.FooterLink = menuHelper.GetFooterLink();
            reportageListView.BottomText = GetTextById("f83df812-966e-425f-b6aa-7340d5b00ea7");
            reportageListView.Text = text;
            reportageListView.Questions = db.AskedQuestions.Where(current => current.IsDeleted == false && current.Param == "reportagePackage" && current.IsActive == true).OrderBy(current => current.Order).ToList();
            reportageListView.ReportageGroups = db.ReportageGroups.Where(current => current.IsDeleted == false && current.IsActive == true && current.IsPackage == true).OrderBy(current => current.Priority).ToList();
            ViewBag.Title = text.PageTitle;
            ViewBag.Description = text.MetaDescription;
            ViewBag.Canonical = "https://www.rushweb.ir/reportage/package";
            ViewBag.param = textId;
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
            return View(reportageListView);
        }

        [AllowAnonymous]
        public List<ReportageByGroup> ReturnReportage(string groupId, string amountRange, string daRange,string site)
        {
            List<ReportageByGroup> reportageByGroup = new List<ReportageByGroup>();
            List<ReportageGroup> groupList = new List<ReportageGroup>();

            if (groupId == null)
                groupList = db.ReportageGroups
                    .Where(current =>
                        current.IsDeleted == false && current.IsActive == true && current.IsPackage == false)
                    .OrderBy(current => current.Priority).ToList();
            else
            {

                Guid id=new Guid(groupId);

                groupList = db.ReportageGroups
                    .Where(current => current.Id == id && current.IsDeleted == false && current.IsActive == true &&
                                      current.IsPackage == false).OrderBy(current => current.Priority).ToList();
            }


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



            List<Reportage> reportageList=new List<Reportage>();


            if (!string.IsNullOrEmpty(groupId) || !string.IsNullOrEmpty(amountRange) ||
                !string.IsNullOrEmpty(site) || !string.IsNullOrEmpty(daRange))
            {
                foreach (ReportageGroup group in groupList)
                {

                 
                        if (site == null)
                            reportageList = db.Reportages.Where(current => current.ReportageGroupId == group.Id&&
                                    current.DomainAuthority <= endDa && current.DomainAuthority >= startDa &&
                                    current.IsActive &&
                                    current.Price <= endAmount && current.Price >= startAmount &&
                                    current.IsDeleted == false )
                                .OrderBy(current => current.Priority).ToList();
                        else
                            reportageList = db.Reportages.Where(current => current.ReportageGroupId == group.Id &&
                                    current.DomainAuthority <= endDa && current.DomainAuthority >= startDa &&
                                    current.IsActive &&
                                    current.Price <= endAmount && current.Price >= startAmount &&
                                    current.IsDeleted == false &&
                                    (current.FullName.Contains(site) || current.Address.Contains(site)))
                                .OrderBy(current => current.Priority).ToList();
                   
                    reportageByGroup.Add(new ReportageByGroup
                    {
                        ReportageGroupId = group.Id,

                        ReportageGroupTitle = group.Title,

                        Reportages = reportageList,

                        Price = group.Price,

                        Value = group.Value
                    });
                }
            }
            else
            {
                foreach (ReportageGroup group in groupList)
                {

                   
                        reportageList = db.Reportages.Where(current =>
                            current.ReportageGroupId == group.Id&&
                                current.IsActive &&

                                current.IsDeleted == false )
                            .OrderBy(current => current.Priority).ToList();
                    
                    reportageByGroup.Add(new ReportageByGroup
                    {
                        ReportageGroupId = group.Id,

                        ReportageGroupTitle = group.Title,

                        Reportages = reportageList,

                        Price = group.Price,

                        Value = group.Value
                    });

                }
            }
            return reportageByGroup;

        }

        [AllowAnonymous]
        public List<ReportageByGroup> ReturnPackageReportage()
        {
            List<ReportageByGroup> reportageByGroup = new List<ReportageByGroup>();

            List<ReportageGroup> groupList = db.ReportageGroups.Where(current => current.IsPackage == true&& current.IsDeleted == false && current.IsActive == true).OrderBy(current => current.Priority).ToList();

            foreach (ReportageGroup group in groupList)
            {
                List<Reportage> reportages = db.Reportages
                    .Where(current => current.ReportageGroupId == group.Id &&
                        current.IsActive   && current.IsDeleted == false )
                    .OrderBy(current => current.Priority).ToList();


                reportageByGroup.Add(new ReportageByGroup
                {
                    ReportageGroupId = group.Id,
                    ReportageGroupTitle = group.Title,
                    Reportages = reportages,
                    Price = group.Price,
                    Value = group.Value
                });

            }
            return reportageByGroup;

        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SubmitStar(string rateStar, string param)
        {
            if (ModelState.IsValid)
            {
                Guid textId = new Guid(param);

                decimal backlinkRate;
                if (!rateStar.Any(char.IsDigit))
                    backlinkRate = ReturnRate(rateStar);
                else
                    backlinkRate = Convert.ToDecimal(rateStar.Replace('.', '/'));


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
        public void UpdateProduct()
        {
            //List<Reportage> reportages = db.Reportages.Where(c => c.IsDeleted == false).ToList();

            //foreach (Reportage reportage in reportages)
            //{
            //    Product product = new Product()
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = reportage.FullName,
            //        CreationDate = DateTime.Now,
            //        IsActive = reportage.IsActive,
            //        IsDeleted = false,
            //        ProductTypeId = db.ProductTypes.FirstOrDefault(c => c.Name == "reportage").Id,
            //    };

            //    reportage.ProductId = product.Id;
            //    db.Products.Add(product);
            //}
            //db.SaveChanges();

            List<ReportageGroup> reportageGroups = db.ReportageGroups.Where(c => c.IsDeleted == false).ToList();

            foreach (ReportageGroup reportageGroup in reportageGroups)
            {
                Product product = new Product()
                {
                    Id = Guid.NewGuid(),
                    Title = reportageGroup.Title,
                    CreationDate = DateTime.Now,
                    IsActive = reportageGroup.IsActive,
                    IsDeleted = false,
                    ProductTypeId = db.ProductTypes.FirstOrDefault(c => c.Name == "package").Id,
                };



                reportageGroup.ProductId = product.Id;
                db.Products.Add(product);
            }
            db.SaveChanges();
        }

        [AllowAnonymous]
        public void UpdateBuyAmount()
        {
            List<Reportage> reportages = db.Reportages.Where(c => c.IsDeleted == false).ToList();

            foreach (Reportage reportage in reportages)
            {
                reportage.BuyAmount = reportage.Price - 30000;
            }
            db.SaveChanges();
        }
    }
}
