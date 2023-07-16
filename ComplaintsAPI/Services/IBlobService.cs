using Microsoft.AspNetCore.Mvc;

namespace ComplaintsAPI.Services
{
    public interface IBlobService
    {
        public  Task<string> UploadPhoto(Stream photoStream, string fileName);
      

    }
}
