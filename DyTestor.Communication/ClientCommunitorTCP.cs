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

        private bool connected = false;

        private TcpClient tcpClient;

        public void Start()
        {
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(1);
                    if (connected)
                        continue;                    
                    try
                    {
                        this.tcpClient.Client.Close();
                        Thread.Sleep(5000);
                    }
                    catch { }
                    Ping ping = new Ping();
                    string data = "ping test data";
                    byte[] buf = Encoding.ASCII.GetBytes(data);
                    PingReply replay = ping.Send(AppConfig.SCANER_IP);
                    while (replay.Status != IPStatus.Success)
                    {
                        this.Error?.Invoke(this, new DyEventArgs() { Message = "The Scaner has offline!" });
                        replay = ping.Send(AppConfig.SCANER_IP);
                    }
                    this.tcpClient = null;
                    this.tcpClient = new TcpClient()
                    {
                        ReceiveTimeout = 20000                        
                        
                    };
                    this.connect();
                }
            }).Start();
        }

        public void Reconnect()
        {
            this.connected = false;
        }

        private void connect()
        {
            try
            {
                AsyncCallback asyncCallback = new AsyncCallback(connectCallback);
                IAsyncResult result = this.tcpClient.BeginConnect(AppConfig.SCANER_IP, AppConfig.SCANER_PORT, new AsyncCallback(connectCallback), null);
                this.tcpClient.EndConnect(result);
                this.connected = true;
                this.OnConnect?.Invoke();


            }catch(Exception ex)
            {
                this.Error?.Invoke(this, new DyEventArgs() { Message = ex.Message });
                this.connected = false;
            }
        }

        private void connectCallback(IAsyncResult ar)
        {
            if (!this.connected)
                return;
            int readbytes = 0;
            byte[] buf = new byte[65536];
            do
            {
                try
                {
                    readbytes = this.tcpClient.Client.Receive(buf);
                    if (readbytes > 0)
                    {
                        byte[] data = new byte[readbytes];
                        Array.Copy(buf, 0, data, 0, readbytes);
                        this.Received?.Invoke(data);
                    }
                    else
                        break;
                }
                catch
                {
                    this.connected = false;
                        return;
                }
            } while (readbytes > 0);
            this.connected = false;
        }

        public void Send(byte[] data)
        {
                try
                {
                  this.tcpClient.Client.BeginSend(data,0,data.Length,SocketFlags.None,new AsyncCallback(sendCallback),null);                      
                }
                catch (Exception ex)
                {
                    this.connected = false;
                    this.Error?.Invoke(this, new DyEventArgs() { Message = ex.Message + "send" });
                }
        }
        
        private void sendCallback(IAsyncResult ar)
        {
            try
            {
                this.tcpClient.Client.EndSend(ar);
            }catch(Exception ex)
            {
                this.Error?.Invoke(this,new DyEventArgs(){Message=ex.Message});
            }

        }
    }
}
