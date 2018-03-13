using DyTestor.Communication;
using DyTestor.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DyTestor.Web
{
    public class Communicator
    {
        private TcpClient client;
        public Communicator()
        {
            this.client = new TcpClient();
        }
        public DyConfig GetConfig()
        {
            this.client.Connect("device", 12001);
            CommunicationData msg = new CommunicationData();
            msg.ClientIP = this.client.Client.LocalEndPoint.ToString().Split(":")[0];
            msg.ClientPort = int.Parse(this.client.Client.LocalEndPoint.ToString().Split(":")[1]);
            msg.Command = "GetConfig";
            NetworkStream ns = this.client.GetStream();
            byte[] buf = Serialization.Serialize(msg);
            ns.Write(buf, 0, buf.Length);
            byte[] readData = new byte[client.ReceiveBufferSize];
            int readByte = ns.Read(readData, 0, readData.Length);
            if (readByte == 0)
                return default(DyConfig);
            this.client.Close();
            byte[] data = new byte[readByte];
            Array.Copy(readData, data, readByte);
            CommunicationData receive = (CommunicationData)Serialization.Deserialize(data);
            return (DyConfig)receive.Data;
        }
        public void SaveConfig(DyConfig config)
        {
            this.client.Connect("device", 12001);
            CommunicationData msg = new CommunicationData();
            msg.Command = "SaveConfig";
            msg.Data = config;
            byte[] buf = Serialization.Serialize(msg);
            NetworkStream ns = this.client.GetStream();
            ns.Write(buf, 0, buf.Length);
            this.client.Close();
        }
    }
}
