using DyTestor.Communication;
using DyTestor.Infrastructure;
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

        static void Main(string[] args)
        {
            int port = AppConfig.LISTEN_PORT;
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
            scaner.Connect(AppConfig.SCANER_IP, AppConfig.SCANER_PORT);            
            Console.WriteLine("Service has already started!");
            Console.ReadLine();            
        }

        private static void HttpCommunicator_Error(object sender, DyEventArgs e)
        {
            Console.WriteLine($"{e.Message}\n{Encoding.ASCII.GetString(e.Data)}");
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
            }
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
