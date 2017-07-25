using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace zpgServer
{
    public static class WebReader
    {
        public static Dictionary<string, string> GetPostArgs(HttpListenerRequest request)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding);
            string[] args = HttpUtility.UrlDecode(reader.ReadToEnd()).Split('&');
            foreach (string arg in args)
            {
                output.Add(arg.Substring(0, arg.IndexOf('=')), arg.Substring(arg.IndexOf('=') + 1));
            }
            return output;
        }
    }
}
