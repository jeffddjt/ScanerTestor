using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.DataObject
{
    public class QRCodeDataObject:DataObjectBase
    {
        public string Code { get; set; }
        public string AssemblyLine { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Sync { get; set; }
    }
}
