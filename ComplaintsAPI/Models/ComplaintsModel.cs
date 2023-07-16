using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ComplaintsAPI.Model
{
    public class ComplaintsModel
    {
        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TicketID { get; set; } = string.Empty;
        [BsonElement]
        public string ClerkID { get; set; } = string.Empty;
        [BsonElement]
        public string TicketUsername { get; set; } = string.Empty;
        [BsonElement]
        [EmailAddress(ErrorMessage = "This is not a valid email.")]
        public string EmailClient { get; set; } = string.Empty;
        [BsonElement]
        [RegularExpression("^(Email|Whatsapp|Site|Phone|Chatbox)$", ErrorMessage = "This is not a valid type of channel comunication.")]
        public string Channel { get; set; } = string.Empty;
        [BsonElement]
        [RegularExpression("^(DefectiveProduct|ProductNotDelivered|ReceivedWrongProduct)$", ErrorMessage = "This is not a valid type of channel comunication.")]
        public string Type { get; set; }=string.Empty;
        [BsonElement]
        public string? Description { get; set; }
        [BsonElement]
        public string? ClerkDescription { get; set; }
        [BsonElement]
        [Range(1, 3, ErrorMessage = "This is not a valid level. Please choose between '1','2' and '3'.")] 
        public int Level { get; set; }
        [BsonElement]
        [RegularExpression("^(Pending|Forwarded|Completed)$", ErrorMessage = "This is not a valid type of channel comunication.")]
        public string Status { get; set; } = string.Empty;
        [BsonElement]
        [RegularExpression(@"^\d{1,2}/\d{1,2}/\d{2}$", ErrorMessage = "This is not a valid short date.")]
        public string LastUpdate { get; set; } = string.Empty;
        [BsonElement]
        [RegularExpression(@"^\d{1,2}/\d{1,2}/\d{2}$", ErrorMessage = "This is not a valid short date.")]
        public string Date { get; set; } = string.Empty;
        [BsonElement]
       public List<string>? Photos { get; set; }= new List<string>();
       
    }
}
