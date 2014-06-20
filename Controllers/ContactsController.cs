using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Phonebook.Filters;
using Phonebook.Models.Contacts;
using Phonebook.Models;
using WebMatrix.WebData;

namespace Phonebook.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class ContactsController : Controller
    {
        private UsersContext db = new UsersContext();

        //
        // GET: /Contacts/

        public ActionResult Index(string search = null)

        {
            int currentUserID = WebSecurity.GetUserId(User.Identity.Name);
            IQueryable<Contact> userContacts = db.Contacts.Where(contact => contact.OwnerID.Equals(currentUserID));

            if (null == search)
            {
                return View(userContacts.ToList());
            }
            else
            {
                string query = HttpUtility.HtmlEncode(search);

                IEnumerable<Contact> enumerable = (userContacts.Where(contact => contact.Name.Contains(query)));

                return View(enumerable.ToList());
            }

        }

        //
        // GET: /Contacts/Details/5

        public ActionResult Details(int id = 0)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        //
        // GET: /Contacts/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Contacts/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contact contact)
        {

            contact.OwnerID = WebSecurity.GetUserId(User.Identity.Name);

            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        //
        // GET: /Contacts/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        //
        // POST: /Contacts/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        //
        // GET: /Contacts/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        //
        // POST: /Contacts/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
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