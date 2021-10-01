using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using VaccineCardReaderIntegration.Models;

namespace VaccineCardReaderIntegration.Controllers
{
    public class ServerSentEventsController : ApiController
    {
        
        [HttpGet]
        [Route("api/VaccineCardResultSSE")]
        public HttpResponseMessage GetEvents(CancellationToken clientDisconnectToken)
        {
            HttpResponseMessage response = Request.CreateResponse();
            ProcessVaccineCardController processVaccineCard = new ProcessVaccineCardController();
            ScannedVaccineCardResult scannedVaccineCardResult = ProcessVaccineCardController.scannedResult;
            long numberOfServicesCalled = ProcessVaccineCardController.numberOfServicesCalled;
            string result = JsonConvert.SerializeObject(scannedVaccineCardResult);
            response.Content = new PushStreamContent(async (stream, httpContent, transportContext) =>
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    using (BlockingCollection<string> consumer = new BlockingCollection<string>())
                    {
                         
                        for (int i = 0; i < numberOfServicesCalled; i++)
                        {
                            await writer.WriteLineAsync("data: " + result);
                            await writer.WriteLineAsync();
                            await writer.FlushAsync();
                        }
                    }
                }
            }, "text/event-stream");
            return response;
        }
    }
}
