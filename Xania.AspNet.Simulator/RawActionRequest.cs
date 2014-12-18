using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Principal;
using System.Web.Mvc;

namespace Xania.AspNet.Simulator
{
    public class RawActionRequest: ActionRequest
    {
        public RawActionRequest()
        {
            
        }

        public RawActionRequest(string url, string method)
        {
            if (url.StartsWith("~"))
                url = url.Substring(1);

            UriPath = url;
            HttpMethod = method;
            HttpVersion = "HTTP/1.1";
        }

        public string UriPath { get; set; }
        
        public string HttpVersion { get; set; }
    }
}