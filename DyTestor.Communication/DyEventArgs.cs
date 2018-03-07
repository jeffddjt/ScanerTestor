using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.Communication
{
    public class DyEventArgs:EventArgs
    {
        public string Message { get; set; }
        public byte[] Data { get; set; }
    }
}
