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

        //
        // GET: /Contacts/Details/5

        public ActionResult Details(int id = 0)
        {
            return PutContactModelInViewResult(id);
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
            return PutContactModelInViewResult(id);
        }

        //
        // POST: /Contacts/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContactModel contactModel)
        {
            Contact contact = db.Contacts.Find(contactModel.ContactId);
            if (UserIsAllowedToView(contact))
            {
                SetContactInfoFrom(contactModel, contact);
                return TryEdit(contact);
            }
            return HttpNotFound();
        }

        //
        // GET: /Contacts/Delete/5

        public ActionResult Delete(int id = 0)
        {
            return PutContactModelInViewResult(id);
        }

        //
        // POST: /Contacts/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            if (UserIsAllowedToView(contact))
            {   db.Contacts.Remove(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }


        public JsonResult SearchContacts(string searchString)
        {
            int currentUserId = WebSecurity.GetUserId(User.Identity.Name);
            var contacts = from contact in db.Contacts
                           where
                           (contact.Name.Contains(searchString) || contact.PhoneNumber.Contains(searchString))
                           select contact;

            List<ContactModel> contactViewModels = SelectContactModels(contacts);

            return Json(contactViewModels, JsonRequestBehavior.AllowGet);
        }

        private static List<ContactModel> SelectContactModels(IEnumerable<Contact> filteredContacts)
        {
            return filteredContacts.Select(c => new ContactModel {ContactId = c.ContactId, Name = c.Name, PhoneNumber = c.PhoneNumber}).ToList();
        }

        private static IEnumerable<Contact> SearchByNameOrPhoneNumber(IQueryable<Contact> allUsersContacts, string query)
        {
            return allUsersContacts.Where(contact => contact.Name.Contains(query) || contact.PhoneNumber.Contains(query));
        }

        private ActionResult PutContactModelInViewResult(int id)
        {
            Contact contact = db.Contacts.Find(id);
            if (UserIsAllowedToView(contact))
            {
                return View(GetContactModelFrom(contact));
            }

            return HttpNotFound();
        }

        private bool UserIsAllowedToView(Contact contact)
        {
            int currentUserId = WebSecurity.GetUserId(User.Identity.Name);
            return ((contact != null) && (contact.OwnerID == currentUserId));
        }

        private static void SetContactInfoFrom(ContactModel contactModel, Contact contact)
        {
            contact.Name = contactModel.Name;
            contact.PhoneNumber = contactModel.PhoneNumber;
        }

        private ActionResult TryEdit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(GetContactModelFrom(contact));
        }

        private static ContactModel GetContactModelFrom(Contact contact)
        {
            return new ContactModel {ContactId = contact.ContactId, Name = contact.Name, PhoneNumber = contact.PhoneNumber};
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}