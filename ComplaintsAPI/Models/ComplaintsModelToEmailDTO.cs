using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ComplaintsAPI.Model
{
    public class ComplaintsModelToEmailDTO
    {
        public string ticket_id { get; set; }=string.Empty;
        public string client_name { get; set; } = string.Empty;
        public string email_client { get; set; } = string.Empty;
        public string channel { get; set; } = string.Empty;  
        public string type { get; set; }=string.Empty;
        public int level;
        public string status { get; set; } = string.Empty;
        public string? description { get; set; }
        public List<string>? images { get; set; }= new List<string>();
        public string clerk_description { get; set;} = string.Empty;
    }
}
