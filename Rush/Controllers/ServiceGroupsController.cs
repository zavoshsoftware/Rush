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
using ViewModels;
using Helper;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Data.Entity.Validation;
using System.Globalization;

namespace Rush.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ServiceGroupsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        MenuHelper menuHelper = new MenuHelper();
        // GET: ServiceGroups
        public ActionResult Index()
        {
            return View(db.ServiceGroups.Where(a => a.IsDeleted == false).OrderByDescending(a => a.CreationDate).ToList());
        }

        // GET: ServiceGroups/Details/5
        //[AllowAnonymous]
        //[Route("serviceGroup/{param}")]
        //public ActionResult Details(string param)
        //{
        //    //List<ServiceGroup> list = db.ServiceGroups.Where(current => current.IsDeleted == false).ToList();
        //    //foreach(var item in list)
        //    //{
        //    //    item.AverageRate = 5;
        //    //}
        //    //db.SaveChanges();
        //    if (param == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    if (param == "سئو")
        //        return RedirectPermanent("https://www.rushweb.ir/serviceGroup/seo");
        //    ServiceGroup serviceGroup = db.ServiceGroups.Where(current => current.UrlParam == param).FirstOrDefault();
        //    if (serviceGroup == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    ServiceGroupDetialViewModel serviceGroupDetailViewModel = new ServiceGroupDetialViewModel();
        //    serviceGroupDetailViewModel.Menu = menuHelper.ReturnMenu();
        //    serviceGroupDetailViewModel.FooterLink = menuHelper.GetFooterLink();
        //    serviceGroupDetailViewModel.FooterLink = menuHelper.GetFooterLink();
        //    serviceGroupDetailViewModel.ServiceGroup = serviceGroup;
        //    serviceGroupDetailViewModel.Rate = serviceGroup.AverageRate.Value.ToString().Replace('/', '.');
        //    serviceGroupDetailViewModel.Questions = db.AskedQuestions.Where(current => current.IsDeleted == false && current.Param == serviceGroup.UrlParam && current.IsActive == true).OrderBy(current => current.Order).ToList();
        //    ViewBag.Title = serviceGroup.PageTitle;
        //    ViewBag.Description = serviceGroup.PageDescription;
        //    ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "Title");
        //    ViewBag.SiteTypeId = new SelectList(db.SiteTypes.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "Title");
        //    ViewBag.id = serviceGroup.Id;
        //    ViewBag.param = param;
        //    ViewBag.Canonical = "https://www.rushweb.ir/serviceGroup/" + param;
        //    ViewBag.rate = serviceGroup.AverageRate.Value.ToString().Replace('/', '.');
        //    int rateCount = db.Rates.Where(current => current.EntityId == serviceGroup.Id).Count();
        //    if (rateCount > 0)
        //        ViewBag.RatingCount = rateCount;
        //    else
        //        ViewBag.RatingCount = 1;
        //    ViewBag.image = "https://www.rushweb.ir" + serviceGroup.ImageUrl;
        //    ViewBag.creationDate = serviceGroup.CreationDate.ToString(CultureInfo.InvariantCulture);

        //    if (!string.IsNullOrEmpty(serviceGroup.LastModifiedDate.ToString()))
        //        ViewBag.ModifiedDate = serviceGroup.LastModifiedDate.Value.ToString(CultureInfo.InvariantCulture);
        //    else
        //        ViewBag.ModifiedDate = serviceGroup.CreationDate.ToString(CultureInfo.InvariantCulture);
           

        //    return View(serviceGroupDetailViewModel);
        //}

        // GET: ServiceGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceGroup serviceGroup, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                if (!CheckUrlParam(serviceGroup.UrlParam, null))
                {
                    #region Upload and resize image if needed
                    string newFilenameUrl = string.Empty;
                    if (fileUpload != null)
                    {
                        string filename = Path.GetFileName(fileUpload.FileName);
                        string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(filename);

                        newFilenameUrl = "/Uploads/service/" + newFilename;
                        string physicalFilename = Server.MapPath(newFilenameUrl);

                        fileUpload.SaveAs(physicalFilename);

                        serviceGroup.ImageUrl = newFilenameUrl;
                    }
                    #endregion
                    serviceGroup.IsDeleted = false;
                    serviceGroup.CreationDate = DateTime.Now;
                    serviceGroup.Id = Guid.NewGuid();
                    serviceGroup.AverageRate = 5;
                    db.ServiceGroups.Add(serviceGroup);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["DupliacteParam"] = "پارامتر Url تکراری است";
                    ViewBag.id = serviceGroup.Id;
                    return View(serviceGroup);
                }
            }

            return View(serviceGroup);
        }

        // GET: ServiceGroups/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceGroup serviceGroup = db.ServiceGroups.Find(id);
            if (serviceGroup == null)
            {
                return HttpNotFound();
            }
            return View(serviceGroup);
        }

        // POST: ServiceGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceGroup serviceGroup, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                if (!CheckUrlParam(serviceGroup.UrlParam, serviceGroup.Id))
                {
                    #region Upload and resize image if needed
                    string newFilenameUrl = serviceGroup.ImageUrl;
                    if (fileUpload != null)
                    {
                        string filename = Path.GetFileName(fileUpload.FileName);
                        string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(filename);

                        newFilenameUrl = "/Uploads/service/" + newFilename;
                        string physicalFilename = Server.MapPath(newFilenameUrl);

                        fileUpload.SaveAs(physicalFilename);

                        serviceGroup.ImageUrl = newFilenameUrl;
                    }
                    #endregion
                    serviceGroup.IsDeleted = false;
                    serviceGroup.LastModifiedDate = DateTime.Now;
                    db.Entry(serviceGroup).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["DupliacteParam"] = "پارامتر Url تکراری است";
                    ViewBag.id = serviceGroup.Id;
                    return View(serviceGroup);
                }
            }
            return View(serviceGroup);
        }

        // GET: ServiceGroups/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceGroup serviceGroup = db.ServiceGroups.Find(id);
            if (serviceGroup == null)
            {
                return HttpNotFound();
            }
            return View(serviceGroup);
        }

        // POST: ServiceGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ServiceGroup serviceGroup = db.ServiceGroups.Find(id);
            serviceGroup.IsDeleted = true;
            serviceGroup.DeletionDate = DateTime.Now;

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

        public bool CheckUrlParam(string param, Guid? id)
        {
            bool result;
            if (id == null)
                result = db.ServiceGroups.Where(current => current.IsDeleted == false && current.UrlParam.ToLower() == param.ToLower()).Any();
            else
                result = db.ServiceGroups.Where(current => current.IsDeleted == false && current.UrlParam.ToLower() == param.ToLower() && current.Id != id).Any();

            return result;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult InsertServiceForm(string site, string email, string mainWord, string desc, string siteType, string serviceType, string id, string phone)
        {
            try
            {
                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (!isEmail)
                    return Json("InvalidEmail", JsonRequestBehavior.AllowGet);
                else
                {
                    if (HttpContext.User.Identity.IsAuthenticated)
                    {
                        string cellnum = HttpContext.User.Identity.Name;

                        User user = db.Users.Where(current => current.CellNum == cellnum && current.IsDeleted == false && current.IsActive == true).FirstOrDefault();

                        ServiceForm serviceForm = new ServiceForm();
                        serviceForm.Id = Guid.NewGuid();
                        serviceForm.Email = email;
                        serviceForm.FormDescription = desc;
                        serviceForm.MainWords = mainWord;
                        serviceForm.SiteAddress = site;
                        serviceForm.ServiceTypeId = new Guid(serviceType);
                        serviceForm.SiteTypeId = new Guid(siteType);
                        serviceForm.IsActive = true;
                        serviceForm.IsDeleted = false;
                        serviceForm.CreationDate = DateTime.Now;
                        serviceForm.UserId = user.Id;
                        serviceForm.ServiceGroupId = new Guid(id);
                        serviceForm.Phone = phone;

                        db.ServiceForms.Add(serviceForm);
                        db.SaveChanges();
                        SendEmail(serviceForm);
                        return Json("true", JsonRequestBehavior.AllowGet);


                    }
                    else
                    {
                        return Json("login", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}
            catch
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public void SendEmail(ServiceForm service)
        {
            List<EmailAddress> emails = db.EmailAddress.Where(current => current.IsDeleted == false).ToList();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("site@rushweb.ir");
            foreach (var item in emails)
            {
                mail.To.Add(item.Email);
            }

            DateTime today = DateTime.Today;

            mail.Body = @" <div style='padding: 5px; direction: rtl; font-family:tahoma;'><h2>فرم درخواست زیر از سایت راش وب ارسال شده است</h2>
<table>
<tr>
<td>آدرس سایت
</td>
<td>" + service.SiteAddress + @"
</td>
</tr>

<tr>
<td>ایمیل
</td>
<td>" + service.Email + @"
</td>
</tr>

<tr>
<td>شماره تماس
</td>
<td>" + service.Phone + @"
</td>
</tr>

<tr>
<td>نوع سایت
</td>
<td>" + db.SiteTypes.Find(service.SiteTypeId).Title + @"
</td>
</tr>

<tr>
<td>نوع خدمت
</td>
<td>" + db.ServiceTypes.Find(service.ServiceTypeId).Title + @"
</td>
</tr>

<tr>
<td>کلمات کلیدی
</td>
<td>" + service.MainWords + @"
</td>
</tr>

<tr>
<td>توضیحات
</td>
<td>" + service.FormDescription + @"
</td>
</tr>
</table>
</div>
";
            mail.Subject = "فرم درخواست راش وب";

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("185.129.171.16");

            System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential("site@rushweb.ir", "aW0zo1$6");
            mail.Headers.Add("Message-Id",
                String.Concat("<", DateTime.Now.ToString("yyMMdd"), ".", DateTime.Now.ToString("HHmmss"),
                    "site@rushweb.ir"));

            smtp.UseDefaultCredentials = false;

            smtp.Credentials = basicAuthenticationInfo;

            mail.Priority = MailPriority.Normal;

            smtp.Send(mail);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SubmitStar(string rateStar, string param)
        {
            if (ModelState.IsValid)
            {
                //Guid paramId = new Guid(id);

                decimal starRate;
                if (!rateStar.Any(char.IsDigit))
                    starRate = ReturnRate(rateStar);
                else
                    starRate = Convert.ToDecimal(rateStar.Replace('.', '/'));

                ServiceGroup serviceGroup = db.ServiceGroups.Where(current => current.UrlParam.ToLower() == param.ToLower() && current.IsDeleted == false).FirstOrDefault();
                serviceGroup.AverageRate = (serviceGroup.AverageRate + starRate) / 2;

                Rate rate = new Rate();
                rate.Id = Guid.NewGuid();
                rate.EntityId = serviceGroup.Id;
                rate.CreationDate = DateTime.Now;
                rate.IsDeleted = false;
                rate.IP = Request.UserHostAddress;
                rate.StarRate = starRate;
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
        [Route("servicegroup/web-design")]
        public ActionResult DetailsForWebDesign(string param)
        {
            param = "web-design";
            ServiceGroup serviceGroup = db.ServiceGroups.FirstOrDefault(current => current.UrlParam == param);
            if (serviceGroup == null)
            {
                return HttpNotFound();
            }

            ServiceGroupDetialViewModel serviceGroupDetailViewModel = new ServiceGroupDetialViewModel();
            serviceGroupDetailViewModel.Menu = menuHelper.ReturnMenu();
            serviceGroupDetailViewModel.FooterLink = menuHelper.GetFooterLink();
            serviceGroupDetailViewModel.FooterLink = menuHelper.GetFooterLink();
            serviceGroupDetailViewModel.ServiceGroup = serviceGroup;
            serviceGroupDetailViewModel.Rate = serviceGroup.AverageRate.Value.ToString().Replace('/', '.');
            serviceGroupDetailViewModel.Questions = db.AskedQuestions.Where(current => current.IsDeleted == false && current.Param == serviceGroup.UrlParam && current.IsActive == true).OrderBy(current => current.Order).ToList();
            ViewBag.Title = serviceGroup.PageTitle;
            ViewBag.Description = serviceGroup.PageDescription;
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "Title");
            ViewBag.SiteTypeId = new SelectList(db.SiteTypes.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "Title");
            ViewBag.id = serviceGroup.Id;
            ViewBag.param = param;
            ViewBag.Canonical = "https://www.rushweb.ir/servicegroup/" + param;
            ViewBag.rate = serviceGroup.AverageRate.Value.ToString().Replace('/', '.');
            int rateCount = db.Rates.Where(current => current.EntityId == serviceGroup.Id).Count();
            if (rateCount > 0)
                ViewBag.RatingCount = rateCount;
            else
                ViewBag.RatingCount = 1;
            ViewBag.image = "https://www.rushweb.ir" + serviceGroup.ImageUrl;
            ViewBag.creationDate = serviceGroup.CreationDate.ToString(CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(serviceGroup.LastModifiedDate.ToString()))
                ViewBag.ModifiedDate = serviceGroup.LastModifiedDate.Value.ToString(CultureInfo.InvariantCulture);
            else
                ViewBag.ModifiedDate = serviceGroup.CreationDate.ToString(CultureInfo.InvariantCulture);


            return View(serviceGroupDetailViewModel);
        }

        [AllowAnonymous]
        [Route("servicegroup/seo")]
        public ActionResult DetailsForSeo(string param)
        {
            param = "seo";
            ServiceGroup serviceGroup = db.ServiceGroups.FirstOrDefault(current => current.UrlParam == param);
            if (serviceGroup == null)
            {
                return HttpNotFound();
            }

            ServiceGroupDetialViewModel serviceGroupDetailViewModel = new ServiceGroupDetialViewModel();
            serviceGroupDetailViewModel.Menu = menuHelper.ReturnMenu();
            serviceGroupDetailViewModel.FooterLink = menuHelper.GetFooterLink();
            serviceGroupDetailViewModel.FooterLink = menuHelper.GetFooterLink();
            serviceGroupDetailViewModel.ServiceGroup = serviceGroup;
            serviceGroupDetailViewModel.Rate = serviceGroup.AverageRate.Value.ToString().Replace('/', '.');
            serviceGroupDetailViewModel.Questions = db.AskedQuestions.Where(current => current.IsDeleted == false && current.Param == serviceGroup.UrlParam && current.IsActive == true).OrderBy(current => current.Order).ToList();
            ViewBag.Title = serviceGroup.PageTitle;
            ViewBag.Description = serviceGroup.PageDescription;
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "Title");
            ViewBag.SiteTypeId = new SelectList(db.SiteTypes.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "Title");
            ViewBag.id = serviceGroup.Id;
            ViewBag.param = param;
            ViewBag.Canonical = "https://www.rushweb.ir/servicegroup/" + param;
            ViewBag.rate = serviceGroup.AverageRate.Value.ToString().Replace('/', '.');
            int rateCount = db.Rates.Where(current => current.EntityId == serviceGroup.Id).Count();
            if (rateCount > 0)
                ViewBag.RatingCount = rateCount;
            else
                ViewBag.RatingCount = 1;
            ViewBag.image = "https://www.rushweb.ir" + serviceGroup.ImageUrl;
            ViewBag.creationDate = serviceGroup.CreationDate.ToString(CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(serviceGroup.LastModifiedDate.ToString()))
                ViewBag.ModifiedDate = serviceGroup.LastModifiedDate.Value.ToString(CultureInfo.InvariantCulture);
            else
                ViewBag.ModifiedDate = serviceGroup.CreationDate.ToString(CultureInfo.InvariantCulture);


            return View(serviceGroupDetailViewModel);
        }

        [AllowAnonymous]
        [Route("servicegroup/content-marketing")]
        public ActionResult DetailsForContent(string param)
        {
            param = "content-marketing";
            ServiceGroup serviceGroup = db.ServiceGroups.FirstOrDefault(current => current.UrlParam == param);
            if (serviceGroup == null)
            {
                return HttpNotFound();
            }

            ServiceGroupDetialViewModel serviceGroupDetailViewModel = new ServiceGroupDetialViewModel();
            serviceGroupDetailViewModel.Menu = menuHelper.ReturnMenu();
            serviceGroupDetailViewModel.FooterLink = menuHelper.GetFooterLink();
            serviceGroupDetailViewModel.FooterLink = menuHelper.GetFooterLink();
            serviceGroupDetailViewModel.ServiceGroup = serviceGroup;
            serviceGroupDetailViewModel.Rate = serviceGroup.AverageRate.Value.ToString().Replace('/', '.');
            serviceGroupDetailViewModel.Questions = db.AskedQuestions.Where(current => current.IsDeleted == false && current.Param == serviceGroup.UrlParam && current.IsActive == true).OrderBy(current => current.Order).ToList();
            ViewBag.Title = serviceGroup.PageTitle;
            ViewBag.Description = serviceGroup.PageDescription;
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "Title");
            ViewBag.SiteTypeId = new SelectList(db.SiteTypes.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "Title");
            ViewBag.id = serviceGroup.Id;
            ViewBag.param = param;
            ViewBag.Canonical = "https://www.rushweb.ir/servicegroup/" + param;
            ViewBag.rate = serviceGroup.AverageRate.Value.ToString().Replace('/', '.');
            int rateCount = db.Rates.Where(current => current.EntityId == serviceGroup.Id).Count();
            if (rateCount > 0)
                ViewBag.RatingCount = rateCount;
            else
                ViewBag.RatingCount = 1;
            ViewBag.image = "https://www.rushweb.ir" + serviceGroup.ImageUrl;
            ViewBag.creationDate = serviceGroup.CreationDate.ToString(CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(serviceGroup.LastModifiedDate.ToString()))
                ViewBag.ModifiedDate = serviceGroup.LastModifiedDate.Value.ToString(CultureInfo.InvariantCulture);
            else
                ViewBag.ModifiedDate = serviceGroup.CreationDate.ToString(CultureInfo.InvariantCulture);


            return View(serviceGroupDetailViewModel);
        }

        [AllowAnonymous]
        [Route("servicegroup/google-adwords")]
        public ActionResult DetailsForGoogleAds(string param)
        {
            param = "google-adwords";
            ServiceGroup serviceGroup = db.ServiceGroups.FirstOrDefault(current => current.UrlParam == param);
            if (serviceGroup == null)
            {
                return HttpNotFound();
            }

            ServiceGroupDetialViewModel serviceGroupDetailViewModel = new ServiceGroupDetialViewModel();
            serviceGroupDetailViewModel.Menu = menuHelper.ReturnMenu();
            serviceGroupDetailViewModel.FooterLink = menuHelper.GetFooterLink();
            serviceGroupDetailViewModel.FooterLink = menuHelper.GetFooterLink();
            serviceGroupDetailViewModel.ServiceGroup = serviceGroup;
            serviceGroupDetailViewModel.Rate = serviceGroup.AverageRate.Value.ToString().Replace('/', '.');
            serviceGroupDetailViewModel.Questions = db.AskedQuestions.Where(current => current.IsDeleted == false && current.Param == serviceGroup.UrlParam && current.IsActive == true).OrderBy(current => current.Order).ToList();
            ViewBag.Title = serviceGroup.PageTitle;
            ViewBag.Description = serviceGroup.PageDescription;
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "Title");
            ViewBag.SiteTypeId = new SelectList(db.SiteTypes.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "Title");
            ViewBag.id = serviceGroup.Id;
            ViewBag.param = param;
            ViewBag.Canonical = "https://www.rushweb.ir/servicegroup/" + param;
            ViewBag.rate = serviceGroup.AverageRate.Value.ToString().Replace('/', '.');
            int rateCount = db.Rates.Where(current => current.EntityId == serviceGroup.Id).Count();
            if (rateCount > 0)
                ViewBag.RatingCount = rateCount;
            else
                ViewBag.RatingCount = 1;
            ViewBag.image = "https://www.rushweb.ir" + serviceGroup.ImageUrl;
            ViewBag.creationDate = serviceGroup.CreationDate.ToString(CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(serviceGroup.LastModifiedDate.ToString()))
                ViewBag.ModifiedDate = serviceGroup.LastModifiedDate.Value.ToString(CultureInfo.InvariantCulture);
            else
                ViewBag.ModifiedDate = serviceGroup.CreationDate.ToString(CultureInfo.InvariantCulture);


            return View(serviceGroupDetailViewModel);
        }
    }
}
