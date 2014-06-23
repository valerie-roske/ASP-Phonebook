using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Phonebook.Entities;
using Phonebook.Filters;
using Phonebook.Models;
using WebMatrix.WebData;

namespace Phonebook.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class ContactsController : Controller
    {
        private EfContext db = new EfContext();

        //
        // GET: /Contacts/

        public ActionResult Index(string search = null)

        {
            int currentUserID = WebSecurity.GetUserId(User.Identity.Name);
            IQueryable<Contact> allUsersContacts = db.Contacts.Where(contact => contact.OwnerID.Equals(currentUserID));

            if (null == search)
            {
                return View(SelectContactModels(allUsersContacts));
            }
            else
            {
                string query = HttpUtility.HtmlEncode(search);

                IEnumerable<Contact> filteredContacts = (SearchByNameOrPhoneNumber(allUsersContacts, query));

                return View(SelectContactModels(filteredContacts));
            }

        }

        private static IEnumerable<Contact> SearchByNameOrPhoneNumber(IQueryable<Contact> allUsersContacts, string query)
        {
            return allUsersContacts.Where(contact => contact.Name.Contains(query) || contact.PhoneNumber.Contains(query));
        }

        private static List<ContactModel> SelectContactModels(IEnumerable<Contact> filteredContacts)
        {
            return filteredContacts.Select(c => new ContactModel {ContactId = c.ContactId, Name = c.Name, PhoneNumber = c.PhoneNumber}).ToList();
        }
        
        private static ContactModel mapContactModel(Contact contact)
        {
            return new ContactModel {ContactId = contact.ContactId, Name = contact.Name, PhoneNumber = contact.PhoneNumber};
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
            return View(mapContactModel(contact));
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
        public ActionResult Create(ContactModel contactModel)
        {
            Contact contact = new Contact();
            
            contact.OwnerID = WebSecurity.GetUserId(User.Identity.Name);
            contact.Name = contactModel.Name;
            contact.PhoneNumber = contactModel.PhoneNumber;

            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contactModel);
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
            return View(mapContactModel(contact));
        }

        //
        // POST: /Contacts/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContactModel contactModel)
        {
            Contact contact = db.Contacts.Find(contactModel.ContactId);
            contact.Name = contactModel.Name;
            contact.PhoneNumber = contactModel.PhoneNumber;

            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mapContactModel(contact));
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
            return View(mapContactModel(contact));
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