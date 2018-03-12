using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DyTestor.Configuration
{
    public static class AppConfig
    {
        public static int LISTEN_PORT { get {return dyConfig.ListenPort;} }
        public static string SERVER_URL { get{  return dyConfig.ServerUrl;} }
        public static string SCANER_IP { get { return dyConfig.ScanerIP; } }
        public static int SCANER_PORT { get { return dyConfig.ScanerPort; }  }
        public static string DBSERVER_IP { get { return dyConfig.DBServerIP; } }
        public static string DBNAME { get { return dyConfig.DBName; } }
        public static string DBPASSWORD { get { return dyConfig.DBPassword; } }
        public static string DBCONNECTIONSTRING { get { return $"Server={DBSERVER_IP};Database={DBNAME};User ID=sa;Password={DBPASSWORD};"; } }
        public static string ASSEMBLY_LINE { get { return dyConfig.AssemblyLine; } }

        private static DyConfig dyConfig;
        private static string filename = Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath) + "\\AppConfig.json";
        static AppConfig()
        {
            GetConfig();
        }

        public static DyConfig GetConfig()
        {
            if (!File.Exists(filename))
            {
                dyConfig = new DyConfig();
                Save();
                return dyConfig;
            }

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte[] buf = new byte[fs.Length];
            fs.Read(buf, 0, buf.Length);
            fs.Close();
            string jsonString = Encoding.UTF8.GetString(buf).Replace("\r", "").Replace("\n", "");
            try
            {
                JObject json = JObject.Parse(jsonString);
                dyConfig = (DyConfig)json.ToObject(typeof(DyConfig));
                return dyConfig;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Save()
        {
            JObject json = JObject.FromObject(dyConfig);
            string jsonString = json.ToString();
            byte[] buf = Encoding.UTF8.GetBytes(jsonString);
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite);
            fs.Write(buf, 0, buf.Length);
            fs.Flush();
            fs.Close();
        }

        public static void Save(DyConfig config)
        {
            dyConfig = config;
            Save();
        }
    }
}
