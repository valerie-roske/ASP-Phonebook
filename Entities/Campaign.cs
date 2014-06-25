using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Phonebook.Entities
{
    public class Campaign
    {
        public int CampaignId { get; set; }
        [StringLength(100)]
        public String Name { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; } 
    }
}