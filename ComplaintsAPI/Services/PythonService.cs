using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ComplaintsAPI.Model;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using ComplaintsAPI.Models;

namespace ComplaintsAPI.Services
{
    public class PythonService:IPythonService
    {
        private readonly HttpClient _httpClient;
   

        public PythonService(HttpClient httpClient) { _httpClient = httpClient; }

        public async Task<ResponsePhotoModel> ResizePhoto(List<ByteArrayContent> photobyte)
        {           
            string url = "https://complaintsbackpython.azurewebsites.net/image_processing";
            List<Stream> compressedImagesBytes= new List<Stream>();
            List<String> compressedImages = new List<String>();
            using (MultipartFormDataContent formData = new MultipartFormDataContent())
            {
                foreach (var streamContent in photobyte) 
                {
                formData.Add(streamContent, "images");
                }
                var response = await _httpClient.PostAsync(url, formData);
                List<StreamContent> res_images = new List<StreamContent>();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadFromJsonAsync<Dictionary<String, List<String>>>();
                     compressedImages = responseBody!["compressed_images"];
                    foreach (var image in compressedImages)
                    {
                        byte[] byteArray =  Convert.FromBase64String(image) ;
                        Stream streamm = new MemoryStream(byteArray);
                        compressedImagesBytes.Add(streamm);
                    }                
                };
                ResponsePhotoModel resp = new ResponsePhotoModel(compressedImagesBytes, compressedImages);
                return resp;
            }
        }
        public async Task<bool> SendEmail(ComplaintsModelToEmailDTO CMEDTO)
        {
            string url = "https://complaintsbackpython.azurewebsites.net/email";
            var response = await _httpClient.PostAsJsonAsync(url, CMEDTO);
            bool sucess = response.IsSuccessStatusCode;
            return sucess;
        }
    }
}
