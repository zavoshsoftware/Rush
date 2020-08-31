using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Rush.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BlogCommentsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: BlogComments
        public ActionResult Index(Guid id)
        {
            List<BlogComment> blogComments = new List<BlogComment>();
            blogComments = db.BlogComments.Include(b => b.Blog).Where(b => b.IsDeleted == false && b.BlogId == id && b.ParentId == null).OrderByDescending(b => b.CreationDate).ToList();
            return View(blogComments);
        }


        public ActionResult Create(Guid id)
        {
            //ViewBag.BlogId = new SelectList(db.Blogs, "Id", "Title");
            ViewBag.id = id;
            return View();
        }

        // POST: BlogComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogComment blogComment, Guid id)
        {
            if (ModelState.IsValid)
            {
                blogComment.IsDeleted = false;
                blogComment.CreationDate = DateTime.Now;
                blogComment.Id = Guid.NewGuid();
                blogComment.BlogId = id;
                db.BlogComments.Add(blogComment);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }
            ViewBag.id = id;
            //ViewBag.BlogId = new SelectList(db.Blogs, "Id", "Title", blogComment.BlogId);
            return View(blogComment);
        }

        // GET: BlogComments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogComment blogComment = db.BlogComments.Find(id);
            if (blogComment == null)
            {
                return HttpNotFound();
            }
            //ViewBag.BlogId = new SelectList(db.Blogs, "Id", "Title", blogComment.BlogId);
            ViewBag.id = blogComment.Id;
            return View(blogComment);
        }

        // POST: BlogComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogComment blogComment)
        {
            if (ModelState.IsValid)
            {
                blogComment.IsDeleted = false;
                db.Entry(blogComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = blogComment.BlogId });
            }
            ViewBag.id = blogComment.BlogId;
            //ViewBag.BlogId = new SelectList(db.Blogs, "Id", "Title", blogComment.BlogId);
            return View(blogComment);
        }

        // GET: BlogComments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogComment blogComment = db.BlogComments.Find(id);
            if (blogComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = blogComment.BlogId;
            return View(blogComment);
        }

        // POST: BlogComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            BlogComment blogComment = db.BlogComments.Find(id);
            blogComment.IsDeleted = true;
            blogComment.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index", new { id = blogComment.BlogId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
