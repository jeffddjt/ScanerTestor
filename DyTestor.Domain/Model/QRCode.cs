using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.Domain.Model
{
    public class QRCode :AggregateRoot
    {
        public string Content { get; set; }
        public string Line { get; set; }
        public int Sort { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Sync { get; set; }
    }
}
