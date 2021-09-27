using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VaccineCardReaderIntegration.Models
{
    public class VaccineCardDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string DOB { get; set; }
        public string Dose1Manufacturer { get; set; }
        public string Dose1LotNumber { get; set; }
        public string Dose1Date { get; set; }
        public string Dose1Site { get; set; }
        public string Dose2Manufacturer { get; set; }
        public string Dose2LotNumber { get; set; }
        public string Dose2Date { get; set; }
        public string Dose2Site { get; set; }
        public double FirstNameConfidence { get; set; }
        public double LastNameConfidence { get; set; }
        public double MiddleNameConfidence { get; set; }
        public double DOBConfidence { get; set; }
        public double Dose1ManufacturerConfidence { get; set; }
        public double Dose1LotNumberConfidence { get; set; }
        public double Dose1DateConfidence { get; set; }
        public double Dose1SiteConfidence { get; set; }
        public double Dose2ManufacturerConfidence { get; set; }
        public double Dose2LotNumberConfidence { get; set; }
        public double Dose2DateConfidence { get; set; }
        public double Dose2SiteConfidence { get; set; }
    }
}