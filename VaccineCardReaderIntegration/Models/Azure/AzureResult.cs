using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VaccineCardReaderIntegration.Models.Azure
{
    public class SelectionMark
    {
        public List<int> BoundingBox { get; set; }
        public double Confidence { get; set; }
        public string State { get; set; }
    }

    public class ReadResult
    {
        public int Page { get; set; }
        public double Angle { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Unit { get; set; }
        public List<SelectionMark> SelectionMarks { get; set; }
    }

    public class Cell
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public string Text { get; set; }
        public List<int> BoundingBox { get; set; }
        public bool IsHeader { get; set; }
        public int? ColumnSpan { get; set; }
        public int? RowSpan { get; set; }
    }

    public class Table
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public List<Cell> Cells { get; set; }
        public List<int> BoundingBox { get; set; }
    }

    public class PageResult
    {
        public int Page { get; set; }
        public List<Table> Tables { get; set; }
    }

    public class LastName
    {
        public string Type { get; set; }
        public string ValueString { get; set; }
        public string Text { get; set; }
        public int Page { get; set; }
        public List<double> BoundingBox { get; set; }
        public double Confidence { get; set; }
    }

    public class Dose2LotNumber
    {
        public string Type { get; set; }
        public double Confidence { get; set; }
    }

    public class Dose1LotNumber
    {
        public string Type { get; set; }
        public string ValueString { get; set; }
        public string Text { get; set; }
        public int Page { get; set; }
        public List<double> BoundingBox { get; set; }
        public double Confidence { get; set; }
    }

    public class Dose2Site
    {
        public string Type { get; set; }
        public double Confidence { get; set; }
    }

    public class FirstName
    {
        public string Type { get; set; }
        public string ValueString { get; set; }
        public string Text { get; set; }
        public int Page { get; set; }
        public List<double> BoundingBox { get; set; }
        public double Confidence { get; set; }
    }

    public class Dose1Site
    {
        public string Type { get; set; }
        public string ValueString { get; set; }
        public string Text { get; set; }
        public int Page { get; set; }
        public List<double> BoundingBox { get; set; }
        public double Confidence { get; set; }
    }

    public class Dose2Date
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public int Page { get; set; }
        public List<double> BoundingBox { get; set; }
        public double Confidence { get; set; }
    }

    public class Dose1Manufacturer
    {
        public string Type { get; set; }
        public string ValueString { get; set; }
        public string Text { get; set; }
        public int Page { get; set; }
        public List<double> BoundingBox { get; set; }
        public double Confidence { get; set; }
    }

    public class MiddleName
    {
        public string Type { get; set; }
        public string ValueString { get; set; }
        public string Text { get; set; }
        public int Page { get; set; }
        public List<double> BoundingBox { get; set; }
        public double Confidence { get; set; }
    }

    public class Dose1Date
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public int Page { get; set; }
        public List<double> BoundingBox { get; set; }
        public double Confidence { get; set; }
    }

    public class Dose2Manufacturer
    {
        public string Type { get; set; }
        public double Confidence { get; set; }
    }

    public class DOB
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public int Page { get; set; }
        public List<double> BoundingBox { get; set; }
        public double Confidence { get; set; }
    }

    public class Fields
    {
        public LastName LastName { get; set; }
        public Dose2LotNumber Dose2LotNumber { get; set; }
        public Dose1LotNumber Dose1LotNumber { get; set; }
        public Dose2Site Dose2Site { get; set; }
        public FirstName FirstName { get; set; }
        public Dose1Site Dose1Site { get; set; }
        public Dose2Date Dose2Date { get; set; }
        public Dose1Manufacturer Dose1Manufacturer { get; set; }
        public MiddleName MiddleName { get; set; }
        public Dose1Date Dose1Date { get; set; }
        public Dose2Manufacturer Dose2Manufacturer { get; set; }
        public DOB DOB { get; set; }
    }

    public class DocumentResult
    {
        public string DocType { get; set; }
        public string ModelId { get; set; }
        public List<int> PageRange { get; set; }
        public Fields Fields { get; set; }
        public double DocTypeConfidence { get; set; }
    }

    public class AnalyzeResult
    {
        public string Version { get; set; }
        public List<ReadResult> ReadResults { get; set; }
        public List<PageResult> PageResults { get; set; }
        public List<DocumentResult> DocumentResults { get; set; }
        public List<object> Errors { get; set; }
    }

    public class AzureResult
    {
        public string Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public AnalyzeResult AnalyzeResult { get; set; }
    }
}