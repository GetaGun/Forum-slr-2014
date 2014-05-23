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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}