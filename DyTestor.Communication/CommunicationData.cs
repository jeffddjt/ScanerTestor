using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.Communication
{
    [Serializable]
    public  class CommunicationData
    {
        public string Command { get; set; }
        public string ClientIP { get; set; }
        public int ClientPort { get; set; }
        public object Data { get; set; }
    }
}
