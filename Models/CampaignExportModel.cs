using System;
using System.ComponentModel.DataAnnotations;

namespace Phonebook.Models
{
    public class CampaignExportModel
    {
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDateTime { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDateTime { get; set; }
    }
}