using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace zpgServer
{
    public static class WebWriter
    {
        public static void Reply(HttpListenerResponse response, byte[] buffer)
        {
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;

            try
            {
                lock (output)
                {
                    output.Write(buffer, 0, buffer.Length);
                }
                output.Close();
            }
            catch (Exception) { }
        }
        public static void Reply(HttpListenerResponse response, string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            Reply(response, buffer);
        }
        public static void SendFile(HttpListenerResponse response, string filename)
        {
            if (File.Exists(filename))
            {
                string ext = Path.GetExtension(filename);
                if (ext == ".html" || ext == ".js" || ext == ".css" || ext == ".txt")
                    Reply(response, File.ReadAllText(filename));
                else
                    Reply(response, File.ReadAllBytes(filename));
            }
        }
    }
}
