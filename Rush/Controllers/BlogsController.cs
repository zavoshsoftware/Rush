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
using System.Data.Entity.Validation;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Rush.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BlogsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        MenuHelper menuHelper = new MenuHelper();
        // GET: Blogs
        public ActionResult Index(Guid id)
        {
            var blogs = db.Blogs.Include(b => b.BlogGroup).Where(b => b.IsDeleted == false && b.BlogGroupId == id).OrderByDescending(b => b.CreationDate);
            ViewBag.id = id;
            return View(blogs.ToList());
        }

        // GET: Blogs/Details/5
        [AllowAnonymous]
        [Route("blog/{param}")]
        public ActionResult Details(string param)
        {

            //List<ServiceGroup> servicegroup = db.ServiceGroups.Where(current => current.AverageRate == null).ToList();
            //foreach(ServiceGroup service in servicegroup)
            //{
            //    service.AverageRate = 5;

            //}
            //List<Service> services = db.Services.Where(current => current.AverageRate == null).ToList();
            //foreach (Service ser in services)
            //{
            //    ser.AverageRate = 5;

            //}
            //db.SaveChanges();
            //List<Blog> blogs = db.Blogs.Where(current => current.IsDeleted == false).ToList();
            //foreach(var item in blogs)
            //{
            //    item.AverageRate = 5;
            //}
            //db.SaveChanges();
            if (param == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Where(current => current.UrlParam.ToLower() == param.ToLower() && current.IsDeleted == false).FirstOrDefault();
            BlogGroup blogGroup = db.BlogGroups.Where(current => current.Id == blog.BlogGroupId).FirstOrDefault();

            if (blog == null)
            {
                return HttpNotFound();
            }

            int visitNum = blog.Visit;
            blog.Visit = visitNum + 1;
            db.SaveChanges();
            User writter = db.Users.Find(blog.WritterId);

            BlogDetailViewModel blogDetail = new BlogDetailViewModel();
            blogDetail.Menu = menuHelper.ReturnMenu();
            blogDetail.Rate = blog.AverageRate.Value.ToString().Replace('/', '.');
            blogDetail.FooterLink = menuHelper.GetFooterLink();
            blogDetail.Username = menuHelper.ReturnUsername();
            blogDetail.Blog = blog;
            blogDetail.RecentBlogs = db.Blogs.Where(current => current.IsDeleted == false && current.IsActive == true).OrderByDescending(current => current.CreationDate).Take(5).ToList();
            blogDetail.BlogGroups = ReturnBlogGroupCount();
            blogDetail.Comments = ReturnComments(blog.Id);
            ViewBag.Title = blog.PageTitle;
            ViewBag.Description = blog.PageDescription;
            ViewBag.param = param;
            ViewBag.Canonical = "https://www.rushweb.ir/blog/" + blog.UrlParam;
            ViewBag.rate = blog.AverageRate.Value.ToString().Replace('/', '.');
            int rateCount = db.Rates.Where(current => current.EntityId == blog.Id).Count();
            if (rateCount > 0)
                ViewBag.RatingCount = rateCount;
            else
                ViewBag.RatingCount = 1;
            ViewBag.image = "https://www.rushweb.ir" + blog.ImageUrl;
            if (blogGroup.UrlParam.ToLower()== "reportage-agahi-news")
            {
                ViewBag.Canonical = "https://www.rushweb.ir/reportage";
               return RedirectPermanent("https://www.rushweb.ir/reportage");
            }
            else
                ViewBag.Canonical = "https://www.rushweb.ir/blog/" + param;

            ViewBag.creationDate = blog.CreationDate.ToString(CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(blog.LastModifiedDate.ToString()))
                ViewBag.ModifiedDate = blog.LastModifiedDate.Value.ToString(CultureInfo.InvariantCulture);
            else
                ViewBag.ModifiedDate = blog.CreationDate.ToString(CultureInfo.InvariantCulture);


            if (writter != null)
            {
                blogDetail.Writter = writter.FullName;
                blogDetail.WritterId = writter.Id.ToString();
                ViewBag.Auther = writter.FullName;
            }

            else
            {
                blogDetail.Writter = "rushweb";
                ViewBag.Auther = "rushweb";
                blogDetail.WritterId = null;
            }





            return View(blogDetail);
        }
        public List<CommentList> ReturnComments(Guid id)
        {
            List<CommentList> result = new List<CommentList>();
            List<BlogComment> comments = db.BlogComments.Include(b => b.Blog).Where(b => b.IsDeleted == false && b.IsActive == true && b.BlogId == id && b.ParentId == null).OrderByDescending(b => b.CreationDate).ToList();
            foreach (BlogComment comment in comments)
            {
                result.Add(new CommentList
                {
                    Comment = comment,
                    Responses = db.BlogComments.Where(current => current.IsDeleted == false && current.IsActive == true && current.ParentId == comment.Id).OrderByDescending(b => b.CreationDate).ToList()
                });


            }
            return result;
        }
        public List<BlogGroupCount> ReturnBlogGroupCount()
        {
            List<BlogGroupCount> blogGroupCount = new List<BlogGroupCount>();
            List<BlogGroup> blogGroups = db.BlogGroups.Where(current => current.IsDeleted == false && current.IsActive == true).ToList();
            foreach (BlogGroup blogGroup in blogGroups)
            {
                blogGroupCount.Add(new BlogGroupCount
                {
                    BlogGroup = blogGroup,
                    Count = db.Blogs.Where(current => current.IsDeleted == false && current.IsActive == true && current.BlogGroupId == blogGroup.Id).Count()
                });
            }
            return blogGroupCount;
        }
        // GET: Blogs/Create
        public ActionResult Create(Guid id)
        {
            ViewBag.id = id;
            ViewBag.WritterId = new SelectList(db.Users.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "FullName");
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog blog, HttpPostedFileBase fileUpload, Guid id, HttpPostedFileBase pdfUpload, HttpPostedFileBase fileUrlUpload)
        {
            if (ModelState.IsValid)
            {
                if (!CheckUrlParam(blog.UrlParam, null))
                {
                    #region Upload and resize image if needed
                    string newFilenameUrl = string.Empty;
                    if (fileUpload != null)
                    {
                        string filename = Path.GetFileName(fileUpload.FileName);
                        string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(filename);

                        newFilenameUrl = "/Uploads/blog/" + newFilename;
                        string physicalFilename = Server.MapPath(newFilenameUrl);

                        fileUpload.SaveAs(physicalFilename);

                        blog.ImageUrl = newFilenameUrl;
                    }
                    #endregion

                    #region Upload and resize Pdf if needed
                    string newPdfnameUrl = string.Empty;
                    if (pdfUpload != null)
                    {
                        string pdfname = Path.GetFileName(pdfUpload.FileName);
                        string newPdfname = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(pdfname);

                        newPdfnameUrl = "/Uploads/blog/" + newPdfname;
                        string physicalPdfname = Server.MapPath(newPdfnameUrl);

                        pdfUpload.SaveAs(physicalPdfname);

                        blog.PdfUrl = newPdfnameUrl;
                    }
                    #endregion

                    #region Upload and resize file if needed
                    string newFileUrlnameUrl = string.Empty;
                    if (fileUrlUpload != null)
                    {
                        string filename = Path.GetFileName(fileUrlUpload.FileName);
                        string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(filename);

                        newFileUrlnameUrl = "/Uploads/blog/" + newFilename;
                        string physicalFilename = Server.MapPath(newFileUrlnameUrl);

                        fileUrlUpload.SaveAs(physicalFilename);

                        blog.FileUrl = newFileUrlnameUrl;
                    }
                    #endregion
                    blog.IsDeleted = false;
                    blog.CreationDate = DateTime.Now;
                    blog.Id = Guid.NewGuid();
                    blog.BlogGroupId = id;
                    blog.AverageRate = 5;
                    db.Blogs.Add(blog);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = id });
                }
                else
                {
                    TempData["DupliacteParam"] = "پارامتر Url تکراری است";
                    ViewBag.id = id;
                    ViewBag.WritterId = new SelectList(db.Users.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "FullName");
                    return View(blog);
                }
            }

            ViewBag.id = id;
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = blog.BlogGroupId;
            ViewBag.WritterId = new SelectList(db.Users.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "FullName", blog.WritterId);
            ViewBag.BlogGroupId = new SelectList(db.BlogGroups.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "Title", blog.BlogGroupId);

            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Blog blog, HttpPostedFileBase fileUpload, HttpPostedFileBase pdfUpload, HttpPostedFileBase fileUrlUpload)
        {
            if (ModelState.IsValid)
            {
                if (!CheckUrlParam(blog.UrlParam, blog.Id))
                {
                    #region Upload and resize image if needed
                    string newFilenameUrl = blog.ImageUrl;
                    if (fileUpload != null)
                    {
                        string filename = Path.GetFileName(fileUpload.FileName);
                        string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(filename);

                        newFilenameUrl = "/Uploads/blog/" + newFilename;
                        string physicalFilename = Server.MapPath(newFilenameUrl);

                        fileUpload.SaveAs(physicalFilename);

                        blog.ImageUrl = newFilenameUrl;
                    }
                    #endregion
                    #region Upload and resize Pdf if needed
                    string newPdfnameUrl = blog.PdfUrl;
                    if (pdfUpload != null)
                    {
                        string pdfname = Path.GetFileName(pdfUpload.FileName);
                        string newPdfname = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(pdfname);

                        newPdfnameUrl = "/Uploads/blog/" + newPdfname;
                        string physicalPdfname = Server.MapPath(newPdfnameUrl);

                        pdfUpload.SaveAs(physicalPdfname);

                        blog.PdfUrl = newPdfnameUrl;
                    }
                    #endregion
                    #region Upload and resize file if needed
                    string newFileUrlnameUrl = blog.FileUrl;
                    if (fileUrlUpload != null)
                    {
                        string filename = Path.GetFileName(fileUrlUpload.FileName);
                        string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(filename);

                        newFileUrlnameUrl = "/Uploads/blog/" + newFilename;
                        string physicalFilename = Server.MapPath(newFileUrlnameUrl);

                        fileUrlUpload.SaveAs(physicalFilename);

                        blog.FileUrl = newFileUrlnameUrl;
                    }
                    #endregion
                    blog.IsDeleted = false;
                    blog.LastModifiedDate = DateTime.Now;
                    db.Entry(blog).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = blog.BlogGroupId });
                }
                else
                {
                    TempData["DupliacteParam"] = "پارامتر Url تکراری است";
                    ViewBag.id = blog.BlogGroupId;
                    return View(blog);
                }
            }
            ViewBag.id = blog.BlogGroupId;
            ViewBag.WritterId = new SelectList(db.Users.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "FullName", blog.WritterId);
            ViewBag.BlogGroupId = new SelectList(db.BlogGroups.Where(current => current.IsDeleted == false && current.IsActive == true).ToList(), "Id", "Title", blog.BlogGroupId);

            return View(blog);
        }

        // GET: Blogs/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = blog.BlogGroupId;
            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Blog blog = db.Blogs.Find(id);
            blog.IsDeleted = true;
            blog.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index", new { id = blog.BlogGroupId });
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
                result = db.Blogs.Where(current => current.IsDeleted == false && current.UrlParam.ToLower() == param.ToLower()).Any();
            else
                result = db.Blogs.Where(current => current.IsDeleted == false && current.UrlParam.ToLower() == param.ToLower() && current.Id != id).Any();

            return result;
        }
        [AllowAnonymous]
        public ActionResult SubmitComment(string id, string email, string name, string message)
        {
            try
            {

                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (!isEmail)
                    return Json("InvalidEmail", JsonRequestBehavior.AllowGet);
                else
                {
                    BlogComment comment = new BlogComment();
                    comment.Id = Guid.NewGuid();
                    comment.Email = email;
                    comment.Body = message;
                    comment.BlogId = new Guid(id);
                    comment.IsDeleted = false;
                    comment.IsActive = false;
                    comment.FullName = name;
                    comment.CreationDate = DateTime.Now;
                    db.BlogComments.Add(comment);
                    db.SaveChanges();
                    return Json("true", JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        [Route("blog/list/{param}")]
        public ActionResult List(string param)
        {
            BlogListViewModel blogList = new BlogListViewModel();
            blogList.BlogGroup = db.BlogGroups.Where(current => current.UrlParam.ToLower() == param.ToLower()).FirstOrDefault();
            blogList.Blogs = db.Blogs.Where(current => current.IsDeleted == false && current.IsActive == true && current.BlogGroup.UrlParam.ToLower() == param.ToLower()).OrderByDescending(current => current.CreationDate).ToList();
            blogList.Menu = menuHelper.ReturnMenu();
            blogList.FooterLink = menuHelper.GetFooterLink();
            blogList.Username = menuHelper.ReturnUsername();
            ViewBag.Title = blogList.BlogGroup.PageTitle;
            ViewBag.Description = blogList.BlogGroup.PageDescription;
            //if (param == "reportage-agahi-news")
            //    ViewBag.Canonical = "https://www.rushweb.ir/reportage";
            //else
            ViewBag.Canonical = "https://www.rushweb.ir/blog/list/" + param;
            return View(blogList);
        }

        [AllowAnonymous]
        [Route("Download/{id:Guid}")]
        public ActionResult DownloadFile(Guid id)
        {

            var blog = db.Blogs.Find(id);
            //Response.Close();
            Response.Clear();
            string contentType = "application/pdf";

            if (blog.PdfUrl != null)
            {
                return File(blog.PdfUrl, contentType, blog.Title + ".pdf");
            }

            else
                return HttpNotFound();



        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SubmitStar(string rateStar, string param)
        {
            if (ModelState.IsValid)
            {
                //Guid paramId = new Guid(id);

                decimal blogRate;
                if (!rateStar.Any(char.IsDigit))
                    blogRate = ReturnRate(rateStar);
                else
                    blogRate = Convert.ToDecimal(rateStar.Replace('.', '/'));

                Blog blog = db.Blogs.Where(current => current.UrlParam.ToLower() == param.ToLower() && current.IsDeleted == false).FirstOrDefault();
                blog.AverageRate = (blog.AverageRate + blogRate) / 2;

                Rate rate = new Rate();
                rate.Id = Guid.NewGuid();
                rate.EntityId = blog.Id;
                rate.CreationDate = DateTime.Now;
                rate.IsDeleted = false;
                rate.IP = Request.UserHostAddress;
                rate.StarRate = blogRate;
                rate.Type = 1;
                db.Rates.Add(rate);
                db.SaveChanges();

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
                return Json("false", JsonRequestBehavior.AllowGet);

        }
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
        [Route("search/{param}")]
        public ActionResult SearchResult(string param)
        {
            BlogListViewModel blogList = new BlogListViewModel();
            blogList.Blogs = db.Blogs.Where(current => current.IsDeleted == false && current.IsActive == true && current.Title.Contains(param.ToLower()) ).OrderByDescending(current => current.CreationDate).ToList();
            blogList.Menu = menuHelper.ReturnMenu();
            blogList.FooterLink = menuHelper.GetFooterLink();
            blogList.Username = menuHelper.ReturnUsername();
            ViewBag.Title = "نتایج جستجو عبارت : " + param ;
            ViewBag.Description = "نتایج جستجو عبارت: " + param ;
            return View(blogList);
        }
    }
}
