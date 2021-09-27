using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VaccineCardReaderIntegration.Models.Amazon
{
        public class LastName
        {
            public string Text { get; set; }
            public double Confidence { get; set; }
        }

        public class FirstName
        {
            public string Text { get; set; }
            public double Confidence { get; set; }
        }

        public class DOB
        {
            public string Text { get; set; }
            public double Confidence { get; set; }
        }

        public class MiddleName
        {
            public string Text { get; set; }
            public double Confidence { get; set; }
        }

        public class Dose1ManufacturerAndLotNumber
        {
            public string Text { get; set; }
            public double Confidence { get; set; }
            public int RowIndex { get; set; }
            public int ColumnIndex { get; set; }
        }

        public class Dose1Date
        {
            public string Text { get; set; }
            public double Confidence { get; set; }
            public int RowIndex { get; set; }
            public int ColumnIndex { get; set; }
        }

        public class Dose1Site
        {
            public string Text { get; set; }
            public double Confidence { get; set; }
            public int RowIndex { get; set; }
            public int ColumnIndex { get; set; }
        }

        public class Dose2ManufacturerAndLotNumber
        {
            public string Text { get; set; }
            public double Confidence { get; set; }
            public int RowIndex { get; set; }
            public int ColumnIndex { get; set; }
        }

        public class Dose2Date
        {
            public string Text { get; set; }
            public double Confidence { get; set; }
            public int RowIndex { get; set; }
            public int ColumnIndex { get; set; }
        }
        public class Dose2Site
        {
            public string Text { get; set; }
            public double Confidence { get; set; }
            public int RowIndex { get; set; }
            public int ColumnIndex { get; set; }
        }

        public class AmazonResult
        {
            public LastName LastName { get; set; }
            public FirstName FirstName { get; set; }
            public DOB DOB { get; set; }
            public MiddleName MiddleName { get; set; }
            public Dose1ManufacturerAndLotNumber Dose1ManufacturerAndLotNumber { get; set; }
            public Dose1Date Dose1Date { get; set; }
            public Dose1Site Dose1Site { get; set; }
            public Dose2ManufacturerAndLotNumber Dose2ManufacturerAndLotNumber { get; set; }
            public Dose2Date Dose2Date { get; set; }
            public Dose2Site Dose2Site { get; set; }


        }
}