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
using System.Globalization;

namespace Rush.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ServicesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        MenuHelper menuHelper = new MenuHelper();

        // GET: Services
        public ActionResult Index(Guid id)
        {
            var services = db.Services.Include(s => s.ServiceGroup).Where(s => s.IsDeleted == false && s.ServiceGroupId == id).OrderByDescending(s => s.CreationDate);
            ViewBag.id = id;
            return View(services.ToList());
        }

        // GET: Services/Details/5
        [AllowAnonymous]
        [Route("service/{param}")]
        public ActionResult Details(string param)
        {
            //List<Service> list = db.Services.Where(current => current.IsDeleted == false).ToList();
            //foreach (var item in list)
            //{
            //    item.AverageRate = 5;
            //}
            //db.SaveChanges();
            if (param == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //if (param == "shopping-web-design")
            //    return RedirectPermanent("https://www.rushweb.ir");

            Service service = db.Services.Where(current => current.UrlParam == param).FirstOrDefault();
            if (service == null)
            {
                return HttpNotFound();
            }

            ServiceDetailViewModel serviceDetailViewModel = new ServiceDetailViewModel();
            serviceDetailViewModel.Menu = menuHelper.ReturnMenu();
            serviceDetailViewModel.FooterLink = menuHelper.GetFooterLink();
            serviceDetailViewModel.FooterLink = menuHelper.GetFooterLink();
            serviceDetailViewModel.Service = service;
            serviceDetailViewModel.Rate = service.AverageRate.Value.ToString().Replace('/', '.');
            serviceDetailViewModel.Questions = db.AskedQuestions.Where(current => current.Param == param && current.IsDeleted == false && current.IsActive == true).ToList();
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "Title");
            ViewBag.SiteTypeId = new SelectList(db.SiteTypes.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "Title");
            ViewBag.Title = service.PageTitle;
            ViewBag.Description = service.PageDescription;
            ViewBag.id = service.Id;
            ViewBag.param = param;
            ViewBag.Canonical = "https://www.rushweb.ir/service/" + param;
            ViewBag.rate = service.AverageRate.Value.ToString().Replace('/', '.');
            int rateCount = db.Rates.Where(current => current.EntityId == service.Id).Count();

            if (rateCount > 0)
                ViewBag.RatingCount = rateCount;
            else
                ViewBag.RatingCount = 1;
            ViewBag.image = "https://www.rushweb.ir" + service.ImageUrl;
            ViewBag.creationDate = service.CreationDate.ToString(CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(service.LastModifiedDate.ToString()))
                ViewBag.ModifiedDate = service.LastModifiedDate.Value.ToString(CultureInfo.InvariantCulture);
            else
                ViewBag.ModifiedDate = service.CreationDate.ToString(CultureInfo.InvariantCulture);

            return View(serviceDetailViewModel);
        }

        // GET: Services/Create
        public ActionResult Create(Guid id)
        {
            ViewBag.id = id;
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Service service, Guid id, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                if (!CheckUrlParam(service.UrlParam, null))
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

                        service.ImageUrl = newFilenameUrl;
                    }
                    #endregion
                    service.IsDeleted = false;
                    service.CreationDate = DateTime.Now;
                    service.Id = Guid.NewGuid();
                    service.ServiceGroupId = id;
                    service.AverageRate = 5;
                    db.Services.Add(service);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = id });
                }
                else
                {
                    TempData["DupliacteParam"] = "پارامتر Url تکراری است";
                    ViewBag.id = id;
                    return View(service);
                }
            }


            ViewBag.id = id;
            return View(service);
        }

        // GET: Services/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = service.ServiceGroupId;
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Service service, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                if (!CheckUrlParam(service.UrlParam, service.Id))
                {
                    #region Upload and resize image if needed
                    string newFilenameUrl = service.ImageUrl;
                    if (fileUpload != null)
                    {
                        string filename = Path.GetFileName(fileUpload.FileName);
                        string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(filename);

                        newFilenameUrl = "/Uploads/service/" + newFilename;
                        string physicalFilename = Server.MapPath(newFilenameUrl);

                        fileUpload.SaveAs(physicalFilename);

                        service.ImageUrl = newFilenameUrl;
                    }
                    #endregion
                    service.IsDeleted = false;
                    service.LastModifiedDate = DateTime.Now;
                    db.Entry(service).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = service.ServiceGroupId });
                }
                else
                {
                    TempData["DupliacteParam"] = "پارامتر Url تکراری است";
                    ViewBag.id = service.ServiceGroupId;
                    return View(service);
                }
            }
            ViewBag.id = service.ServiceGroupId;
            return View(service);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = service.ServiceGroupId;
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Service service = db.Services.Find(id);
            service.IsDeleted = true;
            service.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index", new { id = service.ServiceGroupId });
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
                result = db.Services.Where(current => current.IsDeleted == false && current.UrlParam.ToLower() == param.ToLower()).Any();
            else
                result = db.Services.Where(current => current.IsDeleted == false && current.UrlParam.ToLower() == param.ToLower() && current.Id != id).Any();

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
                        serviceForm.ServiceId = new Guid(id);
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

                Service service = db.Services.Where(current => current.UrlParam.ToLower() == param.ToLower() && current.IsDeleted == false).FirstOrDefault();
                service.AverageRate = (service.AverageRate + starRate) / 2;

                Rate rate = new Rate();
                rate.Id = Guid.NewGuid();
                rate.EntityId = service.Id;
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
    }
}
