using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Phonebook.Entities;
using Phonebook.Models;
using WebMatrix.WebData;

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

        //
        // GET: /Campaigns/Export
        public ActionResult Export()
        {
            return View();
        }


        //
        // POST: /Campaigns/Export
        [HttpPost, ActionName("Export")]
        public ActionResult Export(CampaignExportModel campaignExportModel)
        {
            DateTime startDateTime = campaignExportModel.StartDateTime;
            DateTime endDateTime = campaignExportModel.EndDateTime;

            IQueryable contactWithCampaigns =
                from contacts in db.Contacts
                from campaign in contacts.Campaigns
                where campaign.Date >= startDateTime && campaign.Date <= endDateTime
                select new ContactWithCampaign()
                {
                    Contact = contacts,
                    Campaign = campaign
                };

            string path = @"c:\Mallerie\JustPrinting.txt";

            // This text is always added, making the file longer over time 
            // if it is not deleted. 
            string headers = "Campaign Name, Campaign Date, Contact Name, Contact Phone Number" + Environment.NewLine;
            System.IO.File.WriteAllText(path, headers);
            foreach (ContactWithCampaign contactWithCampaign in contactWithCampaigns)
            {
                System.IO.File.AppendAllText(path,
                    contactWithCampaign.Campaign.Name + "," + contactWithCampaign.Campaign.Date + "," + contactWithCampaign.Contact.Name + "," + contactWithCampaign.Contact.PhoneNumber + Environment.NewLine);
            }

            return View("Index", db.Campaigns.ToList());
        }

        [HttpPost]
        public JsonResult AddContact(int contactID, int campaignID)
        {
            var contact = db.Contacts.Find(contactID);
            var campaign = db.Campaigns.Find(campaignID);

            campaign.Contacts.Add(contact);
            db.SaveChanges();

            return Json("Success");
        }


         //Post: /Campaigns/DeleteContact
         [HttpPost]
         public JsonResult DeleteContact(int contactID, int campaignID)
         {
             var campaign = db.Campaigns.Find(campaignID);
             var contact = db.Contacts.Find(contactID);
             campaign.Contacts.Remove(contact);
 
             db.SaveChanges();
             return Json("Success");
         }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}