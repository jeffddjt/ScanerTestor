using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.DataObject
{
    public class QRCodeDataObject:DataObjectBase
    {
        public string Content { get; set; }
        public string Line { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Sync { get; set; }
    }
}
