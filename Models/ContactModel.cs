using System.ComponentModel.DataAnnotations;

namespace Phonebook.Models
{
    public class ContactModel
    {
        public int ContactId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(30)]
        public string PhoneNumber { get; set; }
    }
}