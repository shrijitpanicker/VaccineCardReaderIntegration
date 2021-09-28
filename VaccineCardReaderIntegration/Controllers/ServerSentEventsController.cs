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

namespace VaccineCardReaderIntegration.Controllers
{
    public class ServerSentEventsController : ApiController
    {
        [HttpGet]
        [Route("/api/VaccineCardResultSSE")]
        public HttpResponseMessage GetEvents(CancellationToken clientDisconnectToken)
        {
            var response = Request.CreateResponse();
            response.Content = new PushStreamContent(async (stream, httpContent, transportContext) =>
            {
                using (var writer = new StreamWriter(stream))
                {
                    using (var consumer = new BlockingCollection<string>())
                    {
                        var eventGeneratorTask = EventGeneratorAsync(consumer, clientDisconnectToken);
                        foreach (var @event in consumer.GetConsumingEnumerable(clientDisconnectToken))
                        {
                            await writer.WriteLineAsync("data: " + @event);
                            await writer.WriteLineAsync();
                            await writer.FlushAsync();
                        }
                        await eventGeneratorTask;
                    }
                }
            }, "text/event-stream");
            return response;
        }

        private async Task EventGeneratorAsync(BlockingCollection<string> producer, CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    producer.Add(DateTime.Now.ToString(), cancellationToken);
                    await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
                }
            }
            finally
            {
                producer.CompleteAdding();
            }
        }
    }
}
