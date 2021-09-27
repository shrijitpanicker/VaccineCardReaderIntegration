using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VaccineCardReaderIntegration.Models
{
    public class Constants
    {
        public class Services
        {
            internal const string Google = "Google";
            internal const string Amazon = "Amazon";
            internal const string Azure = "Azure";
        }

        public class ScannedFields
        {
            internal const string FirstName = "FirstName";
            internal const string LastName = "LastName";
            internal const string MiddleName = "MiddleName";
            internal const string DOB = "DOB";
            internal const string Dose1Manufacturer = "Dose1Manufacturer";
            internal const string Dose1LotNumber = "Dose1LotNumber";
            internal const string Dose1Date = "Dose1Date";
            internal const string Dose1Site = "Dose1Site";
            internal const string Dose2Manufacturer = "Dose2Manufacturer";
            internal const string Dose2LotNumber = "Dose2LotNumber";
            internal const string Dose2Date = "Dose2Date";
            internal const string Dose2Site = "Dose2Site";
        }
    }
}