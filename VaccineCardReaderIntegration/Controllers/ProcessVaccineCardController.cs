using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using VaccineCardReaderIntegration.Cognitive_Services;
using VaccineCardReaderIntegration.Models;

namespace VaccineCardReaderIntegration.Controllers
{
    public class ProcessVaccineCardController : ApiController
    {
        private readonly ILogger _log;
        [HttpPost]
        [Route("/api/ProcessVaccineCard")]

        public HttpResponseMessage ProcessVaccineCard(string selectedServices, IFormFile imageFile)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                _log.LogInformation("C# HTTP trigger function processed a request.");
                string[] services = selectedServices.Split(';');
                services = services.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                string imageBase64 = "";
                if (imageFile.Length > 0)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        imageFile.CopyTo(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();
                        string base64 = Convert.ToBase64String(fileBytes);
                        imageBase64 = base64;
                    }
                }

                ScannedVaccineCardResult scannedResult = new ScannedVaccineCardResult();

                var tasks = new List<Task<ScannedVaccineCardResult>>();
                try
                {
                    tasks.Add(Task.Run(() => AmazonProcessor.ExtractText(imageBase64, _log, services)));
                    tasks.Add(Task.Run(() => GoogleProcessor.ExtractText(imageFile, services)));
                }
                catch (Exception ex)
                {
                    _log.LogError("Error occured", ex.Message);
                    scannedResult = null;
                }

                string message = ($"Processing your vaccine card");
                response = new HttpResponseMessage(HttpStatusCode.Created);
                response.RequestMessage = new HttpRequestMessage(HttpMethod.Post, message);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }

    }
}
