using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.Infrastructure
{
    public class DyConfig
    {
        public int ListenPort { get; set; }

        public DyConfig()
        {
            this.ListenPort = 12001;
        }
    }
}
