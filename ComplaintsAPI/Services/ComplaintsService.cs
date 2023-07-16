using ComplaintsAPI.Model;
using ComplaintsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
namespace ComplaintsAPI.Services
{
    public class ComplaintsService : IComplaintsService
    {
        private readonly IMongoCollection<ComplaintsModel> _complaints;
        public ComplaintsService(IComplaintsDataBaseSettings dataBaseSettings, IMongoClient mongoClient) 
        {
            var database =mongoClient.GetDatabase(dataBaseSettings.DatabaseName);
            _complaints= database.GetCollection<ComplaintsModel>(dataBaseSettings.CollectionName);
            
        }

        public async Task<List<ComplaintsModel>> Get()
        {
           var complaintsList=await _complaints.Find(complaint =>true).ToListAsync();
            return complaintsList;
        }
        public async Task<ComplaintsModel> GetSpecific(string ticketId)
        {
            var complaint =  _complaints.Find(complaint => complaint.TicketID== ticketId).FirstOrDefault();
            return complaint;
        }
        public async Task<string> Set(ComplaintsModel complaint)
        {
            try { 
                 await _complaints.InsertOneAsync(complaint);
                return "Succesful Insertion.";
            }
            catch {
                return "Something went wrong! The data is not valid";
            }
        }
        public async Task<string> Update(AtualizationModel att)
        {
            try
            {
                var updateDefinitionBuilder = Builders<ComplaintsModel>.Update;

                var updateDefinition = updateDefinitionBuilder
                    .Set(c => c.ClerkDescription, att.ClerkDescription)
                    .Set(c => c.Level, att.Level)
                    .Set(c => c.Status, att.Status)
                    .Set(c => c.LastUpdate, DateTime.Now.ToShortDateString());

                var filter = Builders<ComplaintsModel>.Filter.Eq(c => c.TicketID, att.TicketID);

                _complaints.UpdateOne(filter, updateDefinition);
                return "Succesful Update.";
            }
            catch
            {
                return "Something went wrong! The data is not valid";
            }
        }
        public async Task<string> Delete(string ticketId)
        {
            await _complaints.DeleteOneAsync(complaint => complaint.TicketID == ticketId);
            return "Succesful Deletion.";
        }
    }
}
