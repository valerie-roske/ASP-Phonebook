using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Phonebook.Models.Contacts;

namespace Phonebook.Models
{
    public class PhonebookEntities : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
    }
}