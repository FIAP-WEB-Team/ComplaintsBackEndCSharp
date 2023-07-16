using ComplaintsAPI.Model;
using ComplaintsAPI.Models;
using System.Net.Http;
using System.Text;

namespace ComplaintsAPI.Services
{
    public interface IPythonService
    {

        public  Task<ResponsePhotoModel> ResizePhoto(List<ByteArrayContent> photobyte);
        public Task<bool> SendEmail(ComplaintsModelToEmailDTO CMEDTO);
    
    }
}
