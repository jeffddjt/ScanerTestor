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
                    Ping ping = new Ping();
                    string data = "ping test data";
                    byte[] buf = Encoding.ASCII.GetBytes(data);
                    PingReply reply = ping.Send(AppConfig.SCANER_IP);
                    if(reply.Status != IPStatus.Success)
                    {
                        this.Error?.Invoke(this, new DyEventArgs() { Message = "The Scaner has offline!" });
                        this.connected = false;
                        continue;
                        
                    }
                    if (connected)
                        continue;                    
                    try
                    {
                        NetworkStream ns = this.tcpClient.GetStream();
                        ns.Close();
                        this.tcpClient.Client.Close(0);
                        this.tcpClient.Close();
                    }
                    catch { }
                    this.tcpClient = null;
                    this.tcpClient = new TcpClient();
                    Thread.Sleep(3000);
                    this.connect();
                    
                }
            }).Start();

            //Thread detectThread = new Thread(detectScaner);
            //detectThread.IsBackground = true;
            //detectThread.Start();
        }

        public void Reconnect()
        {
            this.connected = false;
        }

        private void connect()
        {
            new Thread(()=> {
                try
                {
                    //this.tcpClient.BeginConnect(AppConfig.SCANER_IP, AppConfig.SCANER_PORT, new AsyncCallback(connectCallback), this.tcpClient);
                    //this.OnConnect?.Invoke();
                    this.tcpClient.Connect(AppConfig.SCANER_IP, AppConfig.SCANER_PORT);
                    this.Send(Encoding.ASCII.GetBytes("LON\r"));
                    this.startReceive();


                }
                catch (Exception ex)
                {
                    this.Error?.Invoke(this, new DyEventArgs() { Message = ex.Message });
                    this.connected = false;
                }

            }).Start();
        }
        private void startReceive()
        {
            new Thread(()=>
            {
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
                    }
                    catch
                    {
                        readbytes = 0;
                        return;
                    }
                } while (readbytes > 0);
                this.connected = false;
            }).Start();
        }
        private void connectCallback(IAsyncResult ar)
        {
            TcpClient client = (TcpClient)ar.AsyncState;
            client.EndConnect(ar);
            this.connected = true;

            this.Send(Encoding.ASCII.GetBytes("LON\r"));

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
                     NetworkStream ns = this.tcpClient.GetStream();
                     ns.Write(data, 0, data.Length);
                    //ns.BeginWrite(data, 0, data.Length, new AsyncCallback(sendCallback), this.tcpClient);
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
                TcpClient client = (TcpClient)ar.AsyncState;
                NetworkStream ns = client.GetStream();
                ns.EndWrite(ar);
            }catch(Exception ex)
            {
                this.Error?.Invoke(this,new DyEventArgs(){Message=ex.Message});
            }

        }
    }
}
