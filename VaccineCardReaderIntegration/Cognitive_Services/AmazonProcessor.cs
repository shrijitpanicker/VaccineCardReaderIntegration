using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using VaccineCardReaderIntegration.Models;
using VaccineCardReaderIntegration.Models.Amazon;

namespace VaccineCardReaderIntegration.Cognitive_Services
{
    public class AmazonProcessor
    {
        public static async Task<ScannedVaccineCardResult> ExtractText(string base64, ILogger log, string[] services)
        {
            ScannedVaccineCardResult cardDetails = new ScannedVaccineCardResult();
            try
            {
                if (Array.Exists(services, service => service.Equals(Constants.Services.Amazon, StringComparison.OrdinalIgnoreCase)))
                {
                    ImageData imageData = new ImageData()
                    {
                        ImageBase64 = base64
                    };
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(imageData));

                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://pn8qmgvw85.execute-api.ap-south-1.amazonaws.com/default");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = await client.PostAsync("/textExtract", content);
                        string result = response.Content.ReadAsStringAsync().Result;
                        AmazonResult awsResult = JsonConvert.DeserializeObject<AmazonResult>(result);
                        cardDetails.awsResult = ProcessAwsResult(awsResult);
                        log.LogInformation(DateTime.Now.ToString());


                        return await Task.FromResult(cardDetails);
                    }
                }

                return await Task.FromResult(cardDetails);
            }
            catch (Exception)
            {
                return cardDetails;
            }

        }

        private static VaccineCardDetails ProcessAwsResult(AmazonResult result)
        {
            try
            {

                VaccineCardDetails details = new VaccineCardDetails();

                if (result.FirstName != null)
                {
                    details.FirstName = result.FirstName.Text;
                    details.FirstNameConfidence = result.FirstName.Confidence;
                }

                if (result.LastName != null)
                {
                    details.LastName = result.LastName.Text;
                    details.LastNameConfidence = result.LastName.Confidence;
                }

                if (result.MiddleName != null)
                {
                    details.MiddleName = result.MiddleName.Text;
                    details.MiddleNameConfidence = result.MiddleName.Confidence;
                }

                if (result.DOB != null)
                {
                    details.DOB = result.DOB.Text;
                    details.DOBConfidence = result.DOB.Confidence;
                }

                if (result.Dose1ManufacturerAndLotNumber != null)
                {
                    details.Dose1Manufacturer = Regex.Match(result.Dose1ManufacturerAndLotNumber.Text, @".+((?= [A-z].+ [0-9])|(?= [A-z].+[0-9]))").ToString();
                    details.Dose1ManufacturerConfidence = result.Dose1ManufacturerAndLotNumber.Confidence;

                    details.Dose1LotNumber = Regex.Replace(result.Dose1ManufacturerAndLotNumber.Text, details.Dose1Manufacturer, "");
                    details.Dose1LotNumberConfidence = result.Dose1ManufacturerAndLotNumber.Confidence;
                }

                if (result.Dose1Date != null)
                {
                    details.Dose1Date = result.Dose1Date.Text;
                    details.Dose1DateConfidence = result.Dose1Date.Confidence;
                }

                if (result.Dose1Site != null)
                {
                    details.Dose1Site = result.Dose1Site.Text;
                    details.Dose1SiteConfidence = result.Dose1Site.Confidence;
                }

                if (result.Dose2ManufacturerAndLotNumber != null)
                {
                    details.Dose2Manufacturer = Regex.Match(result.Dose2ManufacturerAndLotNumber.Text, @".+((?= [A-z].+ [0-9])|(?= [A-z].+[0-9]))").ToString();
                    details.Dose2LotNumber = Regex.Replace(result.Dose2ManufacturerAndLotNumber.Text, details.Dose2Manufacturer, "");
                    details.Dose2ManufacturerConfidence = result.Dose2ManufacturerAndLotNumber.Confidence;
                    details.Dose2LotNumberConfidence = result.Dose2ManufacturerAndLotNumber.Confidence;
                }

                if (result.Dose2Date != null)
                {
                    details.Dose2Date = result.Dose2Date.Text;
                    details.Dose2DateConfidence = result.Dose2Date.Confidence;
                }

                if (result.Dose2Site != null)
                {
                    details.Dose2Site = result.Dose2Site.Text;
                    details.Dose2SiteConfidence = result.Dose2Site.Confidence;
                }

                return details;
            }

            catch (NullReferenceException ex)
            {
                throw ex;
            }


        }



    }
}