using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DyTestor.Communication
{
    public class Serialization
    {
        public static byte[] Serialize(object data)
        {
            MemoryStream ms = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, data);
            ms.Position = 0;
            byte[] buf = new byte[ms.Length];
            ms.Read(buf, 0, buf.Length);
            ms.Close();
            return buf;
        }
        public static object Deserialize(byte[] buf)
        {
            MemoryStream ms = new MemoryStream(buf);
            IFormatter formatter = new BinaryFormatter();
            object data = formatter.Deserialize(ms);
            ms.Close();
            return data;
        }
    }
}
