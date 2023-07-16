using ComplaintsAPI.Model;
using ComplaintsAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using System;
using Azure;
using System.Globalization;
using System.Net.Http.Json;
using System.Collections;
using System.Text;
using ComplaintsAPI.Models;

namespace ComplaintsAPI.Controllers
{
    [ApiController]
    [Route("Complaints")]
    public class ComplaintsController:ControllerBase
    {
        private readonly IComplaintsService _complaintsService;
        private readonly IBlobService _blobService;
        private readonly IPythonService _pythonService;
        ResponsePhotoModel ResponseModel;
        public ComplaintsController(IComplaintsService complaints, IBlobService blobService,  IPythonService python)
        {
            _complaintsService = complaints;
            _blobService = blobService;
            _pythonService = python;

        }


        //TODO a partir desse set gerar um request para o python para reduzir o tamanho da imagem e ai sim salvar no blob storage
        [HttpPost("set")]
        public async Task<IActionResult> Set([FromBody] ComplaintsModelDTO complaints) {
            List<string> urlPhotos= new List<string>();
            List<ByteArrayContent> images = new List<ByteArrayContent>();
            try
            {

                if (complaints.BytePhotos.Count > 0) 
                {
                    foreach (string fotos in complaints.BytePhotos)
                    {
                        string texto = @fotos;
                        byte[] imageBytes = Convert.FromBase64String(texto);
                        var byteContent = new ByteArrayContent(imageBytes);
                        byteContent.Headers.Add("Content-Disposition", $"form-data; name=\"images\"; filename=\"thefile\"");
                        images.Add(byteContent);
                    }
                  ResponseModel = await _pythonService.ResizePhoto(images);
                    int i = 0;
                    if (ResponseModel != null)
                    {
                        foreach (Stream stream in ResponseModel.CompressedImagesBytes)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await stream.CopyToAsync(memoryStream); 
                                memoryStream.Seek(0, SeekOrigin.Begin);

                               urlPhotos.Add(await _blobService.UploadPhoto(memoryStream, complaints.TicketUsername + i.ToString() + ".jpeg"));
                            }
                            i++;
                        }
                    }   
                
                }
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Status: {ex.Status}");
                Console.WriteLine($"ReasonPhrase: {ex.Message}");
                Console.WriteLine($"ErrorCode: {ex.ErrorCode}");              
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.ToString());
            }
            var complaint = new ComplaintsModel()
            {
                TicketUsername = complaints.TicketUsername,
                ClerkID = complaints.ClerkID,
                ClerkDescription = complaints.ClerkDescription,
                Channel = complaints.Channel,
                Type = complaints.Type,
                Description = complaints.Description,
                EmailClient = complaints.EmailClient,
                Level = complaints.Level,
                Status = complaints.Status,
                Photos = urlPhotos,
                Date = DateTime.Now.ToShortDateString(),
                LastUpdate=DateTime.Now.ToShortDateString(),
            };
            if (ResponseModel != null)
            {
                if (ResponseModel.CompressedImages.Count > 0)
                {
                    var complaintEmail = new ComplaintsModelToEmailDTO()
                    {
                        ticket_id = complaint.TicketID,
                        client_name = complaint.TicketUsername,
                        email_client = complaint.EmailClient,
                        channel = complaint.Channel,
                        type = complaint.Type,
                        level = complaint.Level,
                        status = complaint.Status,
                        description = complaint.Description,
                        images = ResponseModel.CompressedImages,
                        clerk_description = complaint.ClerkDescription,
                    };
                    await _pythonService.SendEmail(complaintEmail);
                }
                
            }
            else
            {
                var complaintEmail = new ComplaintsModelToEmailDTO()
                {
                    ticket_id = complaint.TicketID,
                    client_name = complaint.TicketUsername,
                    email_client = complaint.EmailClient,
                    channel = complaint.Channel,
                    type = complaint.Type,
                    level = complaint.Level,
                    status = complaint.Status,
                    description = complaint.Description,
                    clerk_description = complaint.ClerkDescription,
                };
                await _pythonService.SendEmail(complaintEmail);
            }
            return Ok(await _complaintsService.Set(complaint));
        }
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _complaintsService.Get());
        }
        [HttpGet("getwithid/{TicketID}")]
        public async Task<IActionResult> Getwithid( string TicketID)
        {
            return Ok(_complaintsService.GetSpecific(TicketID));
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] AtualizationModel att)
        {
            var SpecificModel = await _complaintsService.GetSpecific(att.TicketID);
       
            var complaintEmail = new ComplaintsModelToEmailDTO()
            {
                ticket_id = SpecificModel.TicketID,
                client_name = SpecificModel.TicketUsername,
                email_client = SpecificModel.EmailClient,
                channel = SpecificModel.Channel,
                type = SpecificModel.Type,
                level = SpecificModel.Level,
                status = SpecificModel.Status,
                description = SpecificModel.Description,
                clerk_description = SpecificModel.ClerkDescription,
            };
            await _pythonService.SendEmail(complaintEmail);
            
            return Ok(_complaintsService.Update(att));
        }
        [HttpDelete("delete/{TicketID}")]
        public async Task<IActionResult> Delete( string TicketID)
        {
            return Ok(await _complaintsService.Delete(TicketID));
        }
    

    }
}
