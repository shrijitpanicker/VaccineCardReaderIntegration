using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VaccineCardReaderIntegration.Models.Google
{
    public class TextSegment
    {
        public string StartOffset { get; set; }
        public string EndOffset { get; set; }
        public string Content { get; set; }
    }

    public class TextExtraction
    {
        public double Score { get; set; }
        public TextSegment TextSegment { get; set; }
    }

    public class Payload
    {
        public string AnnotationSpecId { get; set; }
        public string DisplayName { get; set; }
        public TextExtraction TextExtraction { get; set; }
    }

    public class DocumentText
    {
        public string Content { get; set; }
    }

    public class NormalizedVertice
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class BoundingPoly
    {
        public List<NormalizedVertice> NormalizedVertices { get; set; }
    }

    public class Layout
    {
        public TextSegment TextSegment { get; set; }
        public int PageNumber { get; set; }
        public BoundingPoly BoundingPoly { get; set; }
        public string TextSegmentType { get; set; }
    }

    public class DocumentDimensions
    {
        public string Unit { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Document
    {
        public DocumentText DocumentText { get; set; }
        public List<Layout> Layout { get; set; }
        public DocumentDimensions DocumentDimensions { get; set; }
        public int PageCount { get; set; }
    }

    public class PreprocessedInput
    {
        public Document Document { get; set; }
    }

    public class GoogleResult
    {
        public List<Payload> Payload { get; set; }
        public PreprocessedInput PreprocessedInput { get; set; }
    }
}