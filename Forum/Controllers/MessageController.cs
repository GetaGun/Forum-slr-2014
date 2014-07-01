using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forum.Models;
using WebMatrix.WebData;

namespace Forum.Controllers
{
    public class MessageController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult _Index()
        {
            var messages = db.Messages.Include(m => m.QuestionId);
            return View(messages.ToList());
        }

        public ActionResult Create(int id = 0)
        {
            ViewBag.UserId = WebSecurity.GetUserId(User.Identity.Name);
            ViewBag.QuestionId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Messages messages, int id = 0)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(messages);
                db.SaveChanges();
                return RedirectToAction("Details", "Home", new { id = id });
            }
            return View(messages);
        }

        public ActionResult Edit(int id = 0)
        {
            ViewBag.QuestionId = (from m in db.Messages where m.MessageId == id select m.QuestionId).First();
            Session["MessageId"] = id;

            Messages Message = db.Messages.Find(id);
            var UserId = WebSecurity.GetUserId(User.Identity.Name);

            if (Message == null || Message.UserId != UserId)
            {
                return HttpNotFound();
            }
            return View(Message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Messages Message, int QuestionId)
        {
            Message.MessageId = (int)Session["MessageId"];
            if (ModelState.IsValid)
            {
                db.Entry(Message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Home", new { id = QuestionId });
            }
            return View(Message);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var QuestionId = (from m in db.Messages where m.MessageId == id select m.QuestionId).First();

            Messages Message = db.Messages.Find(id);
            db.Messages.Remove(Message);
            db.SaveChanges();
            return RedirectToAction("Details", "Home", new { id = QuestionId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}