using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using VaccineCardReaderIntegration.Cognitive_Services;
using VaccineCardReaderIntegration.Models;

namespace VaccineCardReaderIntegration.Controllers
{
    public class ProcessVaccineCardController : ApiController
    {
        public static ScannedVaccineCardResult scannedResult = new ScannedVaccineCardResult();
        public static long numberOfServicesCalled = 0;
        private static IEnumerable<Task<T>> Interleaved<T>(IEnumerable<Task<T>> tasks)
        {
            var inputTasks = tasks.ToList();
            var sources = (from _ in Enumerable.Range(0, inputTasks.Count)
                           select new TaskCompletionSource<T>()).ToList();
            int nextTaskIndex = -1;
            foreach (var inputTask in inputTasks)
            {
                inputTask.ContinueWith(completed =>
                {
                    var source = sources[Interlocked.Increment(ref nextTaskIndex)];
                    if (completed.IsFaulted)
                        source.TrySetException(completed.Exception.InnerExceptions);
                    else if (completed.IsCanceled)
                        source.TrySetCanceled();
                    else
                        source.TrySetResult(completed.Result);
                }, CancellationToken.None,
                   TaskContinuationOptions.ExecuteSynchronously,
                   TaskScheduler.Default);
            }
            return from source in sources
                   select source.Task;
        }

        [HttpPost]
        [Route("api/ProcessVaccineCard")]
        public HttpResponseMessage ProcessVaccineCard(string selectedServices, string imageURL)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                string[] services = selectedServices.Split(';');
                services = services.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                string imageBase64 = "";
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    byte[] image = new WebClient().DownloadData(imageURL);
                    string base64 = Convert.ToBase64String(image);
                    imageBase64 = base64;
                }

                string message = ($"Processing your vaccine card");
                response = new HttpResponseMessage(HttpStatusCode.Created);
                response.RequestMessage = new HttpRequestMessage(HttpMethod.Post, message);

                var tasks = new List<Task<ScannedVaccineCardResult>>();
                numberOfServicesCalled = services.Length;
                try
                {
                    tasks.Add(Task.Run(() => GoogleProcessor.ExtractText(imageURL, services)));
                    tasks.Add(Task.Run(() => GoogleProcessor.ExtractText(imageURL, services)));
                }
                catch (Exception ex)
                {
                    scannedResult = null;
                    Console.WriteLine(ex.Message);
                }

                foreach (var task in Interleaved(tasks))
                {

                    task.Wait();
                    scannedResult = task.Result;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
    }
}
