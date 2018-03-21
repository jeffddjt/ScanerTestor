using DyTestor.Communication;
using DyTestor.Configuration;
using DyTestor.DataObject;
using DyTestor.Infrastructure;
using DyTestor.SericeContracts;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DyTestor.Service
{
    class Program
    {
        private static ServerCommunicatorTCP server;
        private static ClientCommunitorTCP scaner;
        private static HTTPCommunicator httpCommunicator;
        private static IQRCodeService qrCodeService;

        static void Main(string[] args)
        {
            int port = AppConfig.LISTEN_PORT;
            qrCodeService = ServiceLocator.GetService<IQRCodeService>();
            server = new ServerCommunicatorTCP(port);
            server.Error += Server_Error;
            server.Received += Server_Received;
            server.Notify += Server_Notify;
            server.Start();

            httpCommunicator = new HTTPCommunicator();
            httpCommunicator.Error += HttpCommunicator_Error; ;
            httpCommunicator.Received += HttpCommunicator_Received;


            scaner = new ClientCommunitorTCP();
            scaner.Received += new ReceiveDelegate(Scaner_Received);
            scaner.OnConnect += new ConnectedDelegate(Scaner_ConnectedNotify);
            scaner.Error += new EventHandler<DyEventArgs>(scanerError);
            connectScaner();
            Console.WriteLine("Service has already started!");


            Console.ReadLine();            
        }

        private static void scanerError(object sender, DyEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private static void connectScaner()
        {
            scaner.Start();
        }

        private static void HttpCommunicator_Error(object sender, DyEventArgs e)
        {
            string receive = Encoding.ASCII.GetString(e.Data);
            string content = receive.Split("&")[0];
            string line = receive.Split("&")[1];
            Console.WriteLine($"{e.Message}\n{content},{line}");
            QRCodeDataObject code = new QRCodeDataObject();
            code.Code = content;
            code.AssemblyLine = line;
            code.CreateTime = DateTime.Now;
            qrCodeService.Add(code);
        }

        private static void Scaner_ConnectedNotify()
        {
            Console.WriteLine("The scaner has already connected!");
            startScan();
        }

        private static void HttpCommunicator_Received(byte[] buf)
        {
            string data = Encoding.ASCII.GetString(buf);
            Console.WriteLine(data);
        }

        private static void Scaner_Received(byte[] buf)
        {
            string str = Encoding.ASCII.GetString(buf);
            Console.WriteLine(str);
            if (str.Contains("ER")||str.Contains("OK"))
            {
                Console.WriteLine(str);
                return;
            }
            QRCodeDataObject code = new QRCodeDataObject();
            code.Code = str;
            code.AssemblyLine = AppConfig.ASSEMBLY_LINE;
            code.CreateTime = DateTime.Now;
            Console.WriteLine(code.ToURLParameter());
            byte[] data = Encoding.UTF8.GetBytes(code.ToURLParameter());
            httpCommunicator.Send(data);

        }

        private static void Server_Notify(string msg)
        {
            Console.WriteLine("\n{0}", msg);
        }

        private static void Server_Received(byte[] buf)
        {
            CommunicationData msg = (CommunicationData)Serialization.Deserialize(buf);
            switch (msg.Command)
            {
                case "Start":
                    startScan();
                    break;
                case "Stop":
                    stopScan();
                    break;
                case "GetState":
                    getState();
                    break;
                case "GetConfig":
                    sendGetConfig(msg);
                    break;
                case "SaveConfig":
                    sendSaveConfig(msg);
                    break;
            }
        }

        private static void sendSaveConfig(CommunicationData msg)
        {
            DyConfig config = (DyConfig)msg.Data;
            AppConfig.Save(config);
        }

        private static void sendGetConfig(CommunicationData msg)
        {
            DyConfig config = AppConfig.GetConfig();
            msg.Data = config;
            byte[] buf = Serialization.Serialize(msg);
            server.Send(buf, msg.ClientIP, msg.ClientPort);
        }

        private static void getState()
        {

        }

        private static void startScan()
        {
            byte[] cmd = Encoding.ASCII.GetBytes("LON\r");
            Thread.Sleep(100);
            scaner.Send(cmd);
        }

        private static void stopScan()
        {
            
            byte[] cmd = Encoding.ASCII.GetBytes("LOFF\r");
            scaner.Send(cmd);
        }

        private static void Server_Error(string msg)
        {
            Console.WriteLine("\n{0}", msg);
        }
    }
}
