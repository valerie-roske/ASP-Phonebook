using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Phonebook.Entities;

namespace Phonebook.Models
{
    public class ContactWithCampaign
    {
        public Contact Contact { get; set; }
        public Campaign Campaign { get; set; }
    }
}