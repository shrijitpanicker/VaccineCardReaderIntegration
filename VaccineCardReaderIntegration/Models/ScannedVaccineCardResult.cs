using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VaccineCardReaderIntegration.Models
{
    public class ScannedVaccineCardResult
    {
        public VaccineCardDetails awsResult;
        public VaccineCardDetails azureResult;
        public VaccineCardDetails googleResult;
    }
}