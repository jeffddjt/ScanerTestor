using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.Infrastructure
{
    public class DyConfig
    {
        public int ListenPort { get; set; }
        public string ServerUrl { get; set; }
        public string ScanerIP { get; set; }
        public int ScanerPort { get; set; }

        public DyConfig()
        {
            this.ListenPort = 12001;
            this.ServerUrl = "http://127.0.0.1/api/QRCode/Add";
            this.ScanerIP = "192.168.40.251";
            this.ScanerPort = 9014;
        }
    }
}
