using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.Domain.Model
{
    public class QRCode :AggregateRoot
    {
        public string Code { get; set; }
        public string AssemblyLine { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Sync { get; set; }
    }
}
