using DyTestor.Configuration;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DyTestor.Communication
{
    public class ClientCommunitorTCP
    {
        public event ReceiveDelegate Received;
        public event ConnectedDelegate OnConnect;
        public event EventHandler<DyEventArgs> Error;
        private TcpClient tcpClient;
        public void Start()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        this.tcpClient.Client.Close();
                    }
                    catch { }
                    this.tcpClient = null;
                    this.tcpClient = new TcpClient();
                    this.connect();
                }
            });
            thread.IsBackground = true;
            thread.Start();

        }

        private void connect()
        {
            Ping ping = new Ping();
            string data = "ping test data";
            byte[] buf = Encoding.ASCII.GetBytes(data);
            while (ping.Send(AppConfig.SCANER_IP).Status != IPStatus.Success)
            {
                new Thread(() =>
                {
                    this.Error?.Invoke(this, new DyEventArgs() { Message = "The Scaner has offline!" });
                }).Start();
            }
            try
            {
                this.tcpClient.Connect(AppConfig.SCANER_IP, AppConfig.SCANER_PORT);
                new Thread(() => { this.OnConnect?.Invoke(); }).Start();
                int readbyte = 0;
                do
                {
                    byte[] temp = new byte[65535];
                    NetworkStream ns = this.tcpClient.GetStream();
                    readbyte = ns.Read(temp, 0, temp.Length);
                    if (readbyte == 0)
                        break;

                    byte[] result = new byte[readbyte];
                    Array.Copy(temp, 0, result, 0, readbyte);
                    new Thread(() =>
                    {
                        this.Received?.Invoke(result);
                    }).Start();
                    ns.Close();
                } while (readbyte > 0);
            }catch(Exception ex)
            {
                this.Error?.Invoke(this, new DyEventArgs() { Message = ex.Message });
            }
        }


        public void Send(byte[] data)
        {
                try
                {
                TcpState state = new TcpState(this.tcpClient);
                state.Stream.BeginWrite(data, 0, data.Length, new AsyncCallback(sendCallback), state);
                }
                catch (Exception ex)
                {
                    this.Error?.Invoke(this, new DyEventArgs() { Message = ex.Message + "send" });
                }
        }
        
        private void sendCallback(IAsyncResult ar)
        {
                TcpState state = (TcpState)ar.AsyncState;
                try
                {
                    state.Stream.EndWrite(ar);
                }
                catch (Exception ex)
                {
                    this.Error?.Invoke(this, new DyEventArgs() { Message = ex.Message + "   sendCallback" });
                }
        }
    }
}
