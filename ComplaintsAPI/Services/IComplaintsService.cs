using ComplaintsAPI.Model;
using ComplaintsAPI.Models;
using Microsoft.AspNetCore.Mvc;
namespace ComplaintsAPI.Services
{
    public interface IComplaintsService
    {
        Task<List<ComplaintsModel>> Get();
        Task<ComplaintsModel> GetSpecific(string TicketId);
        Task<string> Set(ComplaintsModel complaint);
        Task<string> Update(AtualizationModel complaint);
        Task<string> Delete(string idTicketId);
    }
}
