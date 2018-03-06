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
        private string url;

        public HTTPCommunicator()
        {
            this.url = AppConfig.SERVER_URL;
        }

        public string Send(byte[] data)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.ContentLength = data.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();
            string str = string.Empty;
            using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                str = reader.ReadToEnd();
            }
            return str;
        }
    }
}
