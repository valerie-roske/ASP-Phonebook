using System;
using System.ComponentModel.DataAnnotations;

namespace Phonebook.Models
{
    public class CampaignExportModel
    {
        [DataType(DataType.Date)]
        public DateTime StartDateTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDateTime { get; set; }
    }
}