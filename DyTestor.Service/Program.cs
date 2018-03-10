using DyTestor.Communication;
using DyTestor.Configuration;
using DyTestor.DataObject;
using DyTestor.Infrastructure;
using DyTestor.SericeContracts;
using System;
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

            scaner.Received += Scaner_Received;
            scaner.ConnectedNotify += Scaner_ConnectedNotify;
            scaner.OnDisconnect += Scaner_OnDisconnect;
            connectScaner();           
            Console.WriteLine("Service has already started!");

            Thread detectThread = new Thread(new ThreadStart(detect));
            detectThread.IsBackground = true;
            detectThread.Start();

            Console.ReadLine();            
        }

        private static void detect()
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (!scaner.Connected)
                    connectScaner();
            }
        }

        private static void Scaner_OnDisconnect(object sender, DyEventArgs e)
        {
            Console.WriteLine(e.Message);
            connectScaner();
        }

        private static void connectScaner()
        {
            scaner.Connect(AppConfig.SCANER_IP, AppConfig.SCANER_PORT);
        }

        private static void HttpCommunicator_Error(object sender, DyEventArgs e)
        {
            string content = Encoding.ASCII.GetString(e.Data);
            Console.WriteLine($"{e.Message}\n{content}");
            QRCodeDataObject code = new QRCodeDataObject();
            code.Content = content;
            code.CreateTime = DateTime.Now;
            qrCodeService.Add(code);
        }

        private static void Scaner_ConnectedNotify()
        {
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
            if (str.Contains("[") || str.Contains("]"))
                return;

            str = $"content={str}";
            byte[] data = Encoding.UTF8.GetBytes(str);
            httpCommunicator.Send(data);

        }

        private static void Server_Notify(string msg)
        {
            Console.WriteLine("\n{0}", msg);
        }

        private static void Server_Received(byte[] buf)
        {
            string cmd = Encoding.ASCII.GetString(buf);
            switch (cmd)
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
            }
        }

        private static void getState()
        {

        }

        private static void startScan()
        {
            byte[] cmd = Encoding.ASCII.GetBytes("LON\r");
            scaner.Send(cmd);
        }

        private static void stopScan()
        {
            
            byte[] cmd = Encoding.ASCII.GetBytes("LOFF\r");
            scaner.Send(cmd);
            scaner.Disconnect();
        }

        private static void Server_Error(string msg)
        {
            Console.WriteLine("\n{0}", msg);
        }
    }
}
