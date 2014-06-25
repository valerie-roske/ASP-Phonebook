using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Phonebook.Entities;

namespace Phonebook.Controllers
{
    public class CampaignsController : Controller
    {
        private EfContext db = new EfContext();

        //
        // GET: /Campaigns/

        public ActionResult Index()
        {
            return View(db.Campaigns.ToList());
        }

        //
        // GET: /Campaigns/Details/5

        public ActionResult Details(int id = 0)
        {
            Campaign campaign = db.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            return View(campaign);
        }

        //
        // GET: /Campaigns/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Campaigns/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                db.Campaigns.Add(campaign);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(campaign);
        }

        //
        // GET: /Campaigns/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Campaign campaign = db.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            return View(campaign);
        }

        //
        // POST: /Campaigns/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campaign).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(campaign);
        }

        //
        // GET: /Campaigns/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Campaign campaign = db.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            return View(campaign);
        }

        //
        // POST: /Campaigns/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Campaign campaign = db.Campaigns.Find(id);
            db.Campaigns.Remove(campaign);
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