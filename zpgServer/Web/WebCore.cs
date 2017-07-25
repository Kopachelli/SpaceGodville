using System;
using System.Net;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Net.Security;

namespace zpgServer
{
    public static class WebCore
    {
        public static ThreadState threadState = ThreadState.NotStarted;

        public static void OnStart() { threadState = ThreadState.Running; }
        public static void OnStop() { threadState = ThreadState.Stopping; }

        public static void WebThread()
        {
            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            listener.Prefixes.Add("http://*:80/");
            listener.Prefixes.Add("https://*:443/");
            try
            {
                listener.Start();
                ConsoleEx.Log("Listening...");
            }
            catch (HttpListenerException ex)
            {
                if (ex.ErrorCode == 5)
                {
                    ConsoleEx.Log("Access denied on socket bind. Please launch the app with administrator "
                        + "priviledges to enable web functionality");
                    return;
                }
            }
            
            while (listener.IsListening && threadState != ThreadState.Stopping)
            {
                var callback = new AsyncCallback(ListenerCallback);
                IAsyncResult result = listener.BeginGetContext(callback, listener);
                result.AsyncWaitHandle.WaitOne();
                //System.Threading.Thread.Sleep(1);
            }
            if (listener.IsListening)
                listener.Stop();
        }

        public static void ListenerCallback(IAsyncResult result)
        {
            try
            {
                HttpListener listener = (HttpListener)result.AsyncState;
                HttpListenerContext context = listener.EndGetContext(result);
                HttpListenerRequest request = context.Request;
                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                // Construct a response.
                string requestedPage = "";
                foreach (string s in request.Url.Segments)
                {
                    requestedPage += s;
                }
                if (requestedPage == "/") { requestedPage = "/index.html"; }

                ConsoleEx.Log("(" + request.RemoteEndPoint.Address.ToString() + ") " + request.HttpMethod.ToString() + " " + requestedPage);

                if (!request.IsSecureConnection)
                {
                    WebWriter.SendFile(response, "www/forceSSL.html");
                }
                else if (File.Exists("www" + requestedPage))
                {
                    WebWriter.SendFile(response, "www" + requestedPage);
                }
                else
                {
                    if (requestedPage == "/hero") { WebRequest.OnHero(request, response); }
                    else if (requestedPage == "/pilotLog") { WebRequest.OnPilotLog(request, response); }
                    else if (requestedPage == "/register") { WebRequest.OnRegister(request, response); }
                    else if (requestedPage == "/login") { WebRequest.OnLogin(request, response); }
                    else if (requestedPage == "/update") { WebRequest.OnUpdate(request, response); }
                }
            }
            catch (Exception) { }
        }
    }
}