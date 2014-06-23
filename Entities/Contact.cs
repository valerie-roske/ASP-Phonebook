using System.ComponentModel.DataAnnotations;

namespace Phonebook.Entities
{
    public class Contact
    {
        public int ContactId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(30)]
        public string PhoneNumber { get; set; }
        public int OwnerID { get; set; }
    }
}