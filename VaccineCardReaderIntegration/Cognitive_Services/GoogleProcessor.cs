using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VaccineCardReaderIntegration.Models;
using VaccineCardReaderIntegration.Models.Google;

namespace VaccineCardReaderIntegration.Cognitive_Services
{
    public class GoogleProcessor
    {

        private VaccineCardDetails googleScannedDetails = new VaccineCardDetails();
        private const string url = "https://automl.googleapis.com/v1/projects/176512682778/locations/us-central1/models/TEN8125010498241429504:predict";
        public static async Task<ScannedVaccineCardResult> ExtractText(IFormFile imageFile, string[] services)
        {
            ScannedVaccineCardResult scannedResult = new ScannedVaccineCardResult();
            try
            {
                VaccineCardDetails cardDetails = new VaccineCardDetails();
                if (Array.Exists(services, service => service.Equals(Constants.Services.Google, StringComparison.OrdinalIgnoreCase)))
                {
                    string firebaseStorageName = System.Environment.GetEnvironmentVariable("FirebaseStorageName", EnvironmentVariableTarget.Process);
                    string fileURL = "gs://" + firebaseStorageName + "/images/" + imageFile.FileName + ".pdf";
                    string accessToken = await GetAccessToken();
                    string formattedAccessToken = string.Format("Bearer {0}", accessToken);
                    string data = @"{""payload"": {""document"": {""input_config"": {""gcs_source"": {""input_uris"": """ + fileURL + @"""}}}}}";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "POST";
                    request.Headers.Add("Authorization", formattedAccessToken);
                    request.ContentType = "application/json";
                    request.ContentLength = data.Length;
                    using (Stream webStream = request.GetRequestStream())
                    using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                    {
                        requestWriter.Write(data);
                    }
                    GoogleResult deserialisedGoogleResponse = new GoogleResult();

                    WebResponse webResponse = request.GetResponse();
                    using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();
                        deserialisedGoogleResponse = JsonConvert.DeserializeObject<GoogleResult>(response);
                        scannedResult.googleResult = ProcessResults(deserialisedGoogleResponse);
                        return await Task.FromResult(scannedResult);

                    }

                }
                return await Task.FromResult(scannedResult);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return scannedResult;
            }
        }
        private static VaccineCardDetails ProcessResults(GoogleResult result)
        {
            VaccineCardDetails scannedDetails = new VaccineCardDetails();
            foreach (Payload value in result.Payload)
            {
                if (value.DisplayName.Equals(Constants.ScannedFields.FirstName, StringComparison.OrdinalIgnoreCase))
                {
                    scannedDetails.FirstName = scannedDetails.FirstName + value.TextExtraction.TextSegment.Content;
                    scannedDetails.FirstNameConfidence = scannedDetails.FirstNameConfidence + value.TextExtraction.Score;
                }
                else if (value.DisplayName.Equals(Constants.ScannedFields.LastName, StringComparison.OrdinalIgnoreCase))
                {
                    scannedDetails.LastName = scannedDetails.LastName + value.TextExtraction.TextSegment.Content;
                    scannedDetails.LastNameConfidence = scannedDetails.LastNameConfidence + value.TextExtraction.Score;
                }
                else if (value.DisplayName.Equals(Constants.ScannedFields.MiddleName, StringComparison.OrdinalIgnoreCase))
                {
                    scannedDetails.MiddleName = scannedDetails.MiddleName + value.TextExtraction.TextSegment.Content;
                    scannedDetails.MiddleNameConfidence = scannedDetails.MiddleNameConfidence + value.TextExtraction.Score;
                }
                else if (value.DisplayName.Equals(Constants.ScannedFields.DOB, StringComparison.OrdinalIgnoreCase))
                {
                    scannedDetails.DOB = scannedDetails.DOB + value.TextExtraction.TextSegment.Content;
                    scannedDetails.DOBConfidence = scannedDetails.DOBConfidence + value.TextExtraction.Score;
                }
                else if (value.DisplayName.Equals(Constants.ScannedFields.Dose1Manufacturer, StringComparison.OrdinalIgnoreCase))
                {
                    scannedDetails.Dose1Manufacturer = scannedDetails.Dose1Manufacturer + value.TextExtraction.TextSegment.Content;
                    scannedDetails.Dose1ManufacturerConfidence = scannedDetails.Dose1ManufacturerConfidence + value.TextExtraction.Score;
                }
                else if (value.DisplayName.Equals(Constants.ScannedFields.Dose1LotNumber, StringComparison.OrdinalIgnoreCase))
                {
                    scannedDetails.Dose1LotNumber = scannedDetails.Dose1LotNumber + value.TextExtraction.TextSegment.Content;
                    scannedDetails.Dose1LotNumberConfidence = scannedDetails.Dose1LotNumberConfidence + value.TextExtraction.Score;
                }
                else if (value.DisplayName.Equals(Constants.ScannedFields.Dose1Date, StringComparison.OrdinalIgnoreCase))
                {
                    scannedDetails.Dose1Date = scannedDetails.Dose1Date + value.TextExtraction.TextSegment.Content;
                    scannedDetails.Dose1DateConfidence = scannedDetails.Dose1DateConfidence + value.TextExtraction.Score;
                }
                else if (value.DisplayName.Equals(Constants.ScannedFields.Dose1Site, StringComparison.OrdinalIgnoreCase))
                {
                    scannedDetails.Dose1Site = scannedDetails.Dose1Site + value.TextExtraction.TextSegment.Content;
                    scannedDetails.Dose1SiteConfidence = scannedDetails.Dose1SiteConfidence + value.TextExtraction.Score;
                }
                else if (value.DisplayName.Equals(Constants.ScannedFields.Dose2Manufacturer, StringComparison.OrdinalIgnoreCase))
                {
                    scannedDetails.Dose2Manufacturer = scannedDetails.Dose2Manufacturer + value.TextExtraction.TextSegment.Content;
                    scannedDetails.Dose2ManufacturerConfidence = scannedDetails.Dose2ManufacturerConfidence + value.TextExtraction.Score;
                }
                else if (value.DisplayName.Equals(Constants.ScannedFields.Dose2LotNumber, StringComparison.OrdinalIgnoreCase))
                {
                    scannedDetails.Dose2LotNumber = scannedDetails.Dose2LotNumber + value.TextExtraction.TextSegment.Content;
                    scannedDetails.Dose2LotNumberConfidence = scannedDetails.Dose2LotNumberConfidence + value.TextExtraction.Score;
                }
                else if (value.DisplayName.Equals(Constants.ScannedFields.Dose2Date, StringComparison.OrdinalIgnoreCase))
                {
                    scannedDetails.Dose2Date = scannedDetails.Dose2Date + value.TextExtraction.TextSegment.Content;
                    scannedDetails.Dose2DateConfidence = scannedDetails.Dose2DateConfidence + value.TextExtraction.Score;
                }
                else if (value.DisplayName.Equals(Constants.ScannedFields.Dose2Site, StringComparison.OrdinalIgnoreCase))
                {
                    scannedDetails.Dose2Site = scannedDetails.Dose2Site + value.TextExtraction.TextSegment.Content;
                    scannedDetails.Dose2SiteConfidence = scannedDetails.Dose2SiteConfidence + value.TextExtraction.Score;
                }
            }
            return scannedDetails;
        }

        private static Task<string> GetAccessToken()
        {
            string clientID = System.Environment.GetEnvironmentVariable("ClientID", EnvironmentVariableTarget.Process);
            string clientSecret = System.Environment.GetEnvironmentVariable("ClientSecret", EnvironmentVariableTarget.Process);
            string refreshToekn = System.Environment.GetEnvironmentVariable("RefreshToekn", EnvironmentVariableTarget.Process);
            string parameter = String.Format("client_id={0}&client_secret={1}&refresh_token={2}&grant_type=refresh_token", clientID, clientSecret, refreshToekn);
            byte[] buffer = Encoding.ASCII.GetBytes(parameter);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://oauth2.googleapis.com/token");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = buffer.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader responseStreamReader = new StreamReader(responseStream);

            // Get the result into an object
            dynamic googleAuthResponse = JObject.Parse(responseStreamReader.ReadToEnd());//parse token from result
            string accessToken = googleAuthResponse.access_token;

            return Task.FromResult(accessToken);
        }

        public static async Task DeleteUploadedFiles(IFormFile imageFile)
        {
            // Authentication
            string firebaseAPI = System.Environment.GetEnvironmentVariable("FirebaseAPI", EnvironmentVariableTarget.Process);
            FirebaseAuthProvider auth = new FirebaseAuthProvider(new FirebaseConfig(firebaseAPI));
            string userEmail = System.Environment.GetEnvironmentVariable("UserEmail", EnvironmentVariableTarget.Process);
            string userPassword = System.Environment.GetEnvironmentVariable("UserPassword", EnvironmentVariableTarget.Process);
            string firebaseStorageName = System.Environment.GetEnvironmentVariable("FirebaseStorageName", EnvironmentVariableTarget.Process);
            FirebaseAuthLink a = await auth.SignInWithEmailAndPasswordAsync(userEmail, userPassword);

            // Construct FirebaseStorage with path to where you want to upload the file and put it there
            Task PDFDelete = new FirebaseStorage(
            firebaseStorageName,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true,
                })
            .Child("images")
            .Child(imageFile.FileName + ".pdf")
            .DeleteAsync();
            await PDFDelete;
        }
    }
}