using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
 

namespace Phonebook.Models.Contacts
{
    [Bind(Exclude = "ContactId")]
    public class Contact
    {

        public int ContactId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

    }
}