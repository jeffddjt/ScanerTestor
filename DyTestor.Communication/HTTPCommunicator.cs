using DyTestor.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DyTestor.Communication
{
    public class HTTPCommunicator
    {
        //public event ErrorDelegate Error;
        public event EventHandler<DyEventArgs> Error;
        public event ReceiveDelegate Received;

        private string url;

        public HTTPCommunicator()
        {
            this.url = AppConfig.SERVER_URL;
        }

        public void Send(byte[] data)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.ContentLength = data.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            HTTPState state = new HTTPState(request, data);
            state.Request.BeginGetResponse(new AsyncCallback(postCallback),state);//.GetResponse() as HttpWebResponse;

        }

        private void postCallback(IAsyncResult ar)
        {
            HTTPState state = (HTTPState)ar.AsyncState;
            try
            {
                HttpWebResponse response = state.Request.EndGetResponse(ar) as HttpWebResponse;
                Stream respStream = response.GetResponseStream();
                string result = string.Empty;
                using(StreamReader reader = new StreamReader(respStream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
                this.Received?.Invoke(Encoding.ASCII.GetBytes(result));

            }            
            catch(Exception ex)
            {
                string content = Encoding.UTF8.GetString(state.Data);
                content = content.Split('=')[1];
                this.Error?.Invoke(this, new DyEventArgs() { Message = ex.Message, Data = Encoding.ASCII.GetBytes(content) });
            }
        }
    }
}
