using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using Rush.ViewModels;

namespace Rush.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AskedQuestionsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: AskedQuestions
        public ActionResult Index()
        {
            var askedQuestions = db.AskedQuestions.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate);
            return View(askedQuestions.ToList());
        }

        // GET: AskedQuestions/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AskedQuestion askedQuestion = db.AskedQuestions.Find(id);
            if (askedQuestion == null)
            {
                return HttpNotFound();
            }
            return View(askedQuestion);
        }

        // GET: AskedQuestions/Create
        public ActionResult Create()
        {
            ViewBag.Param = RetrunQuestionList(null);
            return View();
        }

        // POST: AskedQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AskedQuestion askedQuestion)
        {
            if (ModelState.IsValid)
            {
				askedQuestion.IsDeleted=false;
				askedQuestion.CreationDate= DateTime.Now; 
                askedQuestion.Id = Guid.NewGuid();
                db.AskedQuestions.Add(askedQuestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Param = RetrunQuestionList(askedQuestion.Param);
            return View(askedQuestion);
        }

        // GET: AskedQuestions/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AskedQuestion askedQuestion = db.AskedQuestions.Find(id);
            if (askedQuestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.Param = RetrunQuestionList(askedQuestion.Param);
            return View(askedQuestion);
        }

        // POST: AskedQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AskedQuestion askedQuestion)
        {
            if (ModelState.IsValid)
            {
				askedQuestion.IsDeleted=false;
                db.Entry(askedQuestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Param = RetrunQuestionList(askedQuestion.Param);
            return View(askedQuestion);
        }

        // GET: AskedQuestions/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AskedQuestion askedQuestion = db.AskedQuestions.Find(id);
            if (askedQuestion == null)
            {
                return HttpNotFound();
            }
            return View(askedQuestion);
        }

        // POST: AskedQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AskedQuestion askedQuestion = db.AskedQuestions.Find(id);
			askedQuestion.IsDeleted=true;
			askedQuestion.DeletionDate=DateTime.Now;
 
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
        public SelectList RetrunQuestionList(string param)
        {
            List<QuestionParamListViewModel> questionList = new List<QuestionParamListViewModel>();
            List<ServiceGroup> serviceGroups = db.ServiceGroups.Where(current => current.IsDeleted == false && current.IsActive == true).ToList();
            foreach (ServiceGroup serviceGroup in serviceGroups)
            {
                questionList.Add(new QuestionParamListViewModel { Title = serviceGroup.Title, Id = serviceGroup.UrlParam });
            }

            List<Service> services = db.Services.Where(current => current.IsDeleted == false && current.IsActive == true).ToList();
            foreach (Service service in services)
            {
                questionList.Add(new QuestionParamListViewModel { Title = service.Title, Id = service.UrlParam });
            }

            questionList.Add(new QuestionParamListViewModel { Title = "بک لینک", Id = "backlink" });
            questionList.Add(new QuestionParamListViewModel { Title = "خرید رپورتاژ", Id = "reportage" });
            questionList.Add(new QuestionParamListViewModel { Title = "خرید پکیج رپورتاژ", Id = "reportagePackage" });

            if (string.IsNullOrEmpty(param))
               return new SelectList(questionList, "Id", "Title");
            else
                return new SelectList(questionList, "Id", "Title",param);
        }
    }
}
