using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ComplaintsAPI.Model
{
    public class ComplaintsModelDTO
    {
        
        public string ClerkID { get; set; } = string.Empty;
        public string TicketUsername { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "This is not a valid email.")]
        public string EmailClient { get; set; } = string.Empty;
        [RegularExpression("^(Email|Whatsapp|Site|Phone|Chatbox)$", ErrorMessage = "This is not a valid type of channel comunication.")]
        public string Channel { get; set; } = string.Empty;
        [RegularExpression("^(DefectiveProduct|ProductNotDelivered|ReceivedWrongProduct)$", ErrorMessage = "This is not a valid type of channel comunication.")]
        public string Type { get; set; }=string.Empty;
        public string? Description { get; set; }
        public string? ClerkDescription { get; set; } = string.Empty;
        [Range(1, 3, ErrorMessage = "This is not a valid level. Please choose between '1','2' and '3'.")] 
        public int Level { get; set; }
        [RegularExpression("^(Pending|Forwarded|Completed)$", ErrorMessage = "This is not a valid type of channel comunication.")]
        public string Status { get; set; } = string.Empty;
       public List<string>? BytePhotos { get; set; }= new List<string>(); // i change this because we need to get the bytes pass to the python and last this is insert in the mongodb
    }
}
