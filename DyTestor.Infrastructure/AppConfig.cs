using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DyTestor.Infrastructure
{
    public static class AppConfig
    {
        public static int LISTEN_PORT
        {
            get
            {
                return dyConfig.ListenPort;
            }
            set
            {
                dyConfig.ListenPort = value;
            }
        }
        private static DyConfig dyConfig;
        private static string filename = Environment.CurrentDirectory + "\\AppConfig.json";
        static AppConfig()
        {
            if (!File.Exists(filename))
            {
                dyConfig = new DyConfig();
                return;
            }

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte[] buf = new byte[fs.Length];
            fs.Read(buf, 0, buf.Length);
            fs.Close();
            string jsonString = Encoding.ASCII.GetString(buf);
            JObject json = JObject.Parse(jsonString);
            dyConfig = (DyConfig)json.ToObject(typeof(DyConfig));
        }

        public static void Save()
        {
            JObject json = JObject.FromObject(dyConfig);
            string jsonString = json.ToString();
            byte[] buf = Encoding.ASCII.GetBytes(jsonString);
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fs.Write(buf, 0, buf.Length);
            fs.Flush();
            fs.Close();
        }
    }
}
