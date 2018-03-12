using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DyTestor.Configuration
{
    public static class AppConfig
    {
        public static int LISTEN_PORT { get {return dyConfig.ListenPort;} set { dyConfig.ListenPort = value;} }
        public static string SERVER_URL { get{  return dyConfig.ServerUrl;}set{ dyConfig.ServerUrl = value; } }
        public static string SCANER_IP { get { return dyConfig.ScanerIP; } set { dyConfig.ScanerIP = value; } }
        public static int SCANER_PORT { get { return dyConfig.ScanerPort; } set { dyConfig.ScanerPort = value; } }
        public static string DBSERVER_IP { get { return dyConfig.DBServerIP; } set { dyConfig.DBServerIP = value; } }
        public static string DBNAME { get { return dyConfig.DBName; } set { dyConfig.DBName = value; } }
        public static string DBPASSWORD { get { return dyConfig.DBPassword; } set { dyConfig.DBPassword = value; } }
        public static string DBCONNECTIONSTRING { get { return $"Server={DBSERVER_IP};Database={DBNAME};User ID=sa;Password={DBPASSWORD};"; } }

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
