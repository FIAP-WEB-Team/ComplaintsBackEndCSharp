using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace ComplaintsAPI.Models
{
    public class AtualizationModel
    {
        public string TicketID { get; set; } = string.Empty;
        public string? ClerkDescription { get; set; } = string.Empty;
        [Range(1, 3, ErrorMessage = "This is not a valid level. Please choose between '1','2' and '3'.")]
        public int Level { get; set; }
        [RegularExpression("^(Pending|Forwarded|Completed)$", ErrorMessage = "This is not a valid type of channel comunication.")]
        public string Status { get; set; } = string.Empty;
    }
}
