using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forum.Models;
using PagedList;
using WebMatrix.WebData;

namespace Forum.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ViewResult Index(string sortQuestion, string currentFilter, string questionGroup, string searchString, int? page)
        {   
            ViewBag.UserId = WebSecurity.GetUserId(User.Identity.Name);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var questions = from q in db.Questions select q;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                questions = questions.Where(q => q.QuestionName.ToUpper().Contains(searchString.ToUpper())
                                       || q.QuestionKeyword.ToUpper().Contains(searchString.ToUpper())
                                       || q.QuestionDescription.ToUpper().Contains(searchString.ToUpper()));
            }

            var questionGroupLst = new List<string>();
            var questionGroupsQry = from q in db.Questions select q.QuestionGroups.QuestionGroupName;
            questionGroupLst.AddRange(questionGroupsQry.Distinct());
            ViewBag.questionGroup = new SelectList(questionGroupLst);

            if (!String.IsNullOrEmpty(questionGroup))
            {
                questions = questions.Where(q => q.QuestionGroups.QuestionGroupName == questionGroup);
            }

            ViewBag.CurrentSort = sortQuestion;
            ViewBag.MainParm = String.IsNullOrEmpty(sortQuestion) ? "Name" : "";
            ViewBag.SubmainParm = sortQuestion == "vote desc" ? "vote" : "vote desc";

            switch (sortQuestion)
            {
                case "Name":
                    questions = questions.OrderBy(q => q.Votes.Count);
                    break;
                case "vote desc":
                    questions = questions.OrderByDescending(q => q.Votes.Count);
                    break;
                case "vote":
                    questions = questions.OrderBy(q => q.Votes.Count);
                    break;
                default:
                    questions = questions.OrderByDescending(q => q.Votes.Count);
                    break;
            }
            
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            return View(questions.ToPagedList(PageNumber, PageSize));
        }
        
        public ActionResult _Read(int id = 0)
        {
            Questions Question = db.Questions.Find(id);

            if (Question == null)
            {
                return HttpNotFound();
            }

            return View(Question);
        }
        
        public ActionResult Details(int id = 0)
        {            
            ViewBag.VoteCount = db.Votes.Where(v => v.QuestionId == id).Count();
            Questions Question = db.Questions.Find(id);                      

            if (Question == null)
            {
                return HttpNotFound();
            }

            return View(Question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details_Create_Vote(Votes Vote, Questions Question)
        {
            db.Votes.Add(Vote);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            ViewBag.QuestionGroupId = new SelectList(db.QuestionGroups, "QuestionGroupId", "QuestionGroupName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Questions Question)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(Question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }           
            ViewBag.QuestionGroupId= new SelectList(db.QuestionGroups, "QuestionGroupId", "QuestionGroupName", Question.QuestionGroupId);
            return View(Question);
        }

        public ActionResult Edit(int id = 0)
        {
            Session["QuestionId"] = id;
            Questions Question = db.Questions.Find(id);
            if (Question == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuestionGroupSelected = new SelectList(db.QuestionGroups, "QuestionGroupId", "QuestionGroupName");
            return View(Question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Questions Question)
        {
            Question.QuestionId = (int)Session["QuestionId"];
            if (ModelState.IsValid)
            {
                db.Entry(Question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuestionGroupSelected = new SelectList(db.QuestionGroups, "QuestionGroupId", "QuestionGroupName", Question.QuestionGroupId);
            return View(Question);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Questions Question = db.Questions.Find(id);
            db.Questions.Remove(Question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}