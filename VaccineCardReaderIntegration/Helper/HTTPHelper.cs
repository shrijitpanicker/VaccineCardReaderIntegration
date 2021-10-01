using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace VaccineCardReaderIntegration.Helper
{
    public class HTTPHelper
    {
        public static HttpWebRequest GetHttpWebRequestObject(string url, string requestType, long dataLength, string contentType, string header = null, string token = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = requestType;
            if(token != null)
            {
                request.Headers.Add(header, token);
            }
            request.ContentType = contentType;
            request.ContentLength = dataLength;

            return request;
        }
    }
}