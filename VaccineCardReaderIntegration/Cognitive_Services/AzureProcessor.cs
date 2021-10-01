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
using VaccineCardReaderIntegration.Helper;
using VaccineCardReaderIntegration.Models;
using VaccineCardReaderIntegration.Models.Azure;

namespace VaccineCardReaderIntegration.Cognitive_Services
{
    public class AzureProcessor
    {
        public static async Task<ScannedVaccineCardResult> ExtractText(IFormFile imageFile, string[] services)
        {
            ScannedVaccineCardResult scannedResult = new ScannedVaccineCardResult();
            try
            {
                if (Array.Exists(services, service => service.Equals(Constants.Services.Azure, StringComparison.OrdinalIgnoreCase)))
                {

                    string url = GetOperationLocation(imageFile).ToString();
                    byte[] buffer = Encoding.ASCII.GetBytes("");
                    HttpWebRequest request = HTTPHelper.GetHttpWebRequestObject(url, "GET", buffer.Length, "application/json", "Ocp-Apim-Subscription-Key", "9d80cbfdd0f74852941c809e0b7a79f9");
                    Stream stream = request.GetRequestStream();
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Close();

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader responseStreamReader = new StreamReader(responseStream);

                    // Get the result into an object

                    dynamic azureFormData = JObject.Parse(responseStreamReader.ReadToEnd());//parse token from result
                    AzureResult deserialisedAzureResult = JsonConvert.DeserializeObject<AzureResult>(azureFormData);
                    scannedResult.azureResult = ProcessResults(deserialisedAzureResult);
                    return await Task.FromResult(scannedResult);
                }
                return await Task.FromResult(scannedResult);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return scannedResult;
            }




        }
        private static VaccineCardDetails ProcessResults(AzureResult azureResult)
        {
            VaccineCardDetails cardDetails = new VaccineCardDetails();
            foreach (var value in azureResult.AnalyzeResult.DocumentResults)
            {
                cardDetails.FirstName = value.Fields.FirstName.ToString();
                cardDetails.LastName = value.Fields.LastName.ToString();
                cardDetails.MiddleName = value.Fields.MiddleName.ToString();
                cardDetails.DOB = value.Fields.DOB.ToString();
                cardDetails.Dose1Date = value.Fields.Dose1Date.ToString();
                cardDetails.Dose1LotNumber = value.Fields.Dose1LotNumber.ToString();
                cardDetails.Dose1Manufacturer = value.Fields.Dose1Manufacturer.ToString();
                cardDetails.Dose1Site = value.Fields.Dose2Site.ToString();
                cardDetails.Dose2Date = value.Fields.Dose2Date.ToString();
                cardDetails.Dose2LotNumber = value.Fields.Dose2LotNumber.ToString();
                cardDetails.Dose2Manufacturer = value.Fields.Dose2Manufacturer.ToString();
                cardDetails.Dose2Site = value.Fields.Dose2Site.ToString();
                cardDetails.FirstNameConfidence = value.Fields.FirstName.Confidence;
                cardDetails.LastNameConfidence = value.Fields.LastName.Confidence;
                cardDetails.MiddleNameConfidence = value.Fields.MiddleName.Confidence;
                cardDetails.DOBConfidence = value.Fields.DOB.Confidence;
                cardDetails.Dose1DateConfidence = value.Fields.Dose1Date.Confidence;
                cardDetails.Dose1LotNumberConfidence = value.Fields.Dose1LotNumber.Confidence;
                cardDetails.Dose1ManufacturerConfidence = value.Fields.Dose1Manufacturer.Confidence;
                cardDetails.Dose1SiteConfidence = value.Fields.Dose2Site.Confidence;
                cardDetails.Dose2DateConfidence = value.Fields.Dose2Date.Confidence;
                cardDetails.Dose2LotNumberConfidence = value.Fields.Dose2LotNumber.Confidence;
                cardDetails.Dose2ManufacturerConfidence = value.Fields.Dose2Manufacturer.Confidence;
                cardDetails.Dose2SiteConfidence = value.Fields.Dose2Site.Confidence;
            }
            return cardDetails;

        }


        private static Task<string> GetOperationLocation(IFormFile imageFile)
        {
            string resultID = "";
            string url = "https://eastasia.api.cognitive.microsoft.com/formrecognizer/v2.1/custom/models/d50524b3-4399-421d-bd4e-1bb2f1345a53/analyze";
            string fileURL = "gs://able-plating-320708.appspot.com/images/" + imageFile.FileName;
            byte[] buffer = Encoding.ASCII.GetBytes("client_id=176512682778-4kep66pdip7uv1ch23tia5mmrbcp28h6.apps.googleusercontent.com&client_secret=zSt87bDOErFyJz1TDylPHmdT&refresh_token=1//04bOedcPuvk9eCgYIARAAGAQSNwF-L9Ir_gtSQViCAXQn8Ptsb6AKxvaX_Av5sKujsWJ432zOKXF917bx7mIXUN-AbmGfwyRSTyQ&grant_type=refresh_token");
            HttpWebRequest request = HTTPHelper.GetHttpWebRequestObject(url, "POST", buffer.Length, "application/json", "Ocp-Apim-Subscription-Key", "9d80cbfdd0f74852941c809e0b7a79f9");
            Stream stream = request.GetRequestStream();
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader responseStreamReader = new StreamReader(responseStream);

                // Get the result into an object

                dynamic azureResponse = JObject.Parse(responseStreamReader.ReadToEnd());//parse token from result
                resultID = azureResponse["operation-location"];

                return Task.FromResult(resultID);
            }

            catch (Exception e)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);

            }

            return Task.FromResult(resultID);
        }
    }
}