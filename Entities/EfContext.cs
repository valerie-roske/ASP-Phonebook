using System.Data.Entity;

namespace Phonebook.Entities
{
    public class EfContext : DbContext
    {
        public EfContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
    }
}