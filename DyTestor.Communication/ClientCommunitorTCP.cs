using DyTestor.Configuration;
using System;
using System.Collections.Generic;
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
                    }
                    catch { }
                    this.tcpClient = null;
                    this.tcpClient = new TcpClient();
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
        }

        public void Send(byte[] data)
        {
            new Thread(() =>
            {
                try
                {
                    IAsyncResult result = this.tcpClient.GetStream().BeginWrite(data, 0, data.Length, null, null);
                    this.tcpClient.GetStream().EndWrite(result);
                }
                catch (Exception ex)
                {
                    this.connected = false;
                    this.Error?.Invoke(this, new DyEventArgs() { Message = ex.Message + "sendCallback" });
                }
            }).Start();
        }
    }
}
