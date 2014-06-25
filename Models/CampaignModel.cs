using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Phonebook.Models
{
    public class CampaignModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public String Name { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        public List<ContactModel> ContactModels { get; set; }
    }
}