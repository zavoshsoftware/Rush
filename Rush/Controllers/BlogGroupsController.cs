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

namespace Rush.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BlogGroupsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        MenuHelper menuHelper = new MenuHelper();
        // GET: BlogGroups
        public ActionResult Index()
        {
            return View(db.BlogGroups.Where(a => a.IsDeleted == false).OrderByDescending(a => a.CreationDate).ToList());
        }

        // GET: BlogGroups/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogGroup blogGroup = db.BlogGroups.Find(id);
            if (blogGroup == null)
            {
                return HttpNotFound();
            }
            return View(blogGroup);
        }

        // GET: BlogGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogGroup blogGroup, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
               
                if (!CheckUrlParam(blogGroup.UrlParam,null))
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

                        blogGroup.ImageUrl = newFilenameUrl;
                    }
                    #endregion
                    blogGroup.IsDeleted = false;
                    blogGroup.CreationDate = DateTime.Now;
                    blogGroup.Id = Guid.NewGuid();
                    db.BlogGroups.Add(blogGroup);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["DupliacteParam"] = "پارامتر Url تکراری است";
                    ViewBag.id = blogGroup.Id;
                    return View(blogGroup);
                }
            }

            return View(blogGroup);
        }

        // GET: BlogGroups/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogGroup blogGroup = db.BlogGroups.Find(id);
            if (blogGroup == null)
            {
                return HttpNotFound();
            }
            return View(blogGroup);
        }

        // POST: BlogGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogGroup blogGroup, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                if (!CheckUrlParam(blogGroup.UrlParam,blogGroup.Id))
                {
                    #region Upload and resize image if needed
                    string newFilenameUrl = blogGroup.ImageUrl;
                    if (fileUpload != null)
                    {
                        string filename = Path.GetFileName(fileUpload.FileName);
                        string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(filename);

                        newFilenameUrl = "/Uploads/blog/" + newFilename;
                        string physicalFilename = Server.MapPath(newFilenameUrl);

                        fileUpload.SaveAs(physicalFilename);

                        blogGroup.ImageUrl = newFilenameUrl;
                    }
                    #endregion
                    blogGroup.IsDeleted = false;
                    db.Entry(blogGroup).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["DupliacteParam"] = "پارامتر Url تکراری است";
                    ViewBag.id = blogGroup.Id;
                    return View(blogGroup);
                }
            }
            return View(blogGroup);
        }

        // GET: BlogGroups/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogGroup blogGroup = db.BlogGroups.Find(id);
            if (blogGroup == null)
            {
                return HttpNotFound();
            }
            return View(blogGroup);
        }

        // POST: BlogGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            BlogGroup blogGroup = db.BlogGroups.Find(id);
            blogGroup.IsDeleted = true;
            blogGroup.DeletionDate = DateTime.Now;

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
        [Route("blog")]
        public ActionResult List()
        {
            BlogGroupListViewModel blogGroupList = new BlogGroupListViewModel();
            //blogGroupList.BlogGroup = db.BlogGroups.Where(current => current.UrlParam.ToLower() == param.ToLower()).FirstOrDefault(); ;
            //blogGroupList.Blogs = db.Blogs.Where(current => current.IsDeleted == false && current.IsActive == true && current.BlogGroup.UrlParam.ToLower() == param.ToLower()).ToList();
            blogGroupList.BlogGroups = db.BlogGroups.Where(current => current.IsDeleted == false && current.IsActive == true).ToList();
            blogGroupList.Menu = menuHelper.ReturnMenu();
            blogGroupList.FooterLink = menuHelper.GetFooterLink();
            blogGroupList.Username = menuHelper.ReturnUsername();
            ViewBag.Title = "دسته بندی مطالب بلاگ";
            ViewBag.Description = "دسته بندی مطالب بلاگ";
            ViewBag.Canonical = "https://www.rushweb.ir/blog";
            return View(blogGroupList);
        }

        public bool CheckUrlParam(string param,Guid? id)
        {
            bool result;
            if (id==null)
             result = db.BlogGroups.Where(current => current.IsDeleted == false && current.UrlParam.ToLower() == param.ToLower()).Any();
            else
                result = db.BlogGroups.Where(current => current.IsDeleted == false && current.UrlParam.ToLower() == param.ToLower() && current.Id != id).Any();

            return result;
        }
    }
}
