using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forum.Models;

namespace Forum.Controllers
{
    public class ForumGroupController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            return View(db.QuestionGroups.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionGroups Group)
        {
            if (ModelState.IsValid)
            {
                db.QuestionGroups.Add(Group);
                db.SaveChanges();
                return RedirectToAction("Create", "Home");
            }
            return View(Group);
        }

        public ActionResult Delete(int id = 0)
        {
            QuestionGroups Group = db.QuestionGroups.Find(id);
            if (Group == null)
            {
                return HttpNotFound();
            }
            return View(Group);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            QuestionGroups Group = db.QuestionGroups.Find(id);
            db.QuestionGroups.Remove(Group);
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