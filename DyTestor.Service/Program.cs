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
        private static bool connected = false;
        static void Main(string[] args)
        {
            int port = AppConfig.LISTEN_PORT;
            server = new ServerCommunicatorTCP(port);
            server.Error += Server_Error;
            server.Received += Server_Received;
            server.Notify += Server_Notify;
            server.Start();

            httpCommunicator = new HTTPCommunicator();

            scaner = new ClientCommunitorTCP();
            scaner.Error += Server_Error;
            scaner.Received += Scaner_Received;
            scaner.Connect(AppConfig.SCANER_IP, AppConfig.SCANER_PORT);            

            Console.WriteLine("Service has already started!");
            Console.ReadLine();            
        }

        private static void Scaner_Received(byte[] buf)
        {
            string str = Encoding.ASCII.GetString(buf);

            Console.WriteLine(str);
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
            throw new NotImplementedException();
        }

        private static void stopScan()
        {
            throw new NotImplementedException();
        }

        private static void Server_Error(string msg)
        {
            Console.WriteLine("\n{0}", msg);
        }
    }
}
