using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.Communication
{
    public delegate void ReceiveDelegate(byte[] buf);
    public delegate void ErrorDelegate(string msg);
}
