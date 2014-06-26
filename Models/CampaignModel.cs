using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Phonebook.Models
{
    public class CampaignModel
    {
        public int CampaignId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public String Name { get; set; }
        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public List<ContactModel> ContactModels { get; set; }
    }
}