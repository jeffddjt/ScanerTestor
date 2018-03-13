using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.Configuration
{
    [Serializable]
    public class DyConfig
    {
        public int ListenPort { get; set; }
        public string ServerUrl { get; set; }
        public string ScanerIP { get; set; }
        public int ScanerPort { get; set; }
        public string AssemblyLine { get; set; }
        public string DBServerIP { get; set; }
        public string DBName { get; set; }
        public string DBPassword { get; set; }


        public DyConfig()
        {
            this.ListenPort = 12001;
            this.ServerUrl = "http://testor.bclzdd.com/api/QRCode/Add";
            this.ScanerIP = "192.168.100.100";
            this.ScanerPort = 9004;
            this.AssemblyLine = "1";
            this.DBServerIP = "db";
            this.DBName = "ScanerTestor";
            this.DBPassword = "aaaa1111!";
        }
    }
}
