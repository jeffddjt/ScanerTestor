using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DyTestor.Communication
{
    public class ClientCommunitorTCP
    {
        public  event ReceiveDelegate Received;
        //public  event ErrorDelegate Error;
        public event ConnectedDelegate ConnectedNotify;

        public event EventHandler<DyEventArgs> Error;

        public bool Connected = false;

        private TcpClient tcpClient;
        public ClientCommunitorTCP()
        {
            this.tcpClient = new TcpClient();
        }
        public void Connect(string serveriP,int serverPort)
        {
            try
            {
                this.tcpClient.Connect(serveriP, serverPort);
                this.Connected = true;
                this.ConnectedNotify?.Invoke();
            }
            catch(Exception ex)
            {
                this.Connected = false;
                this.Error?.Invoke(this, new DyEventArgs() { Message = $"Connect to {serveriP}:{serverPort} failed!", Data = Encoding.ASCII.GetBytes(ex.Message) });
            }
        }

        private void receiveThread(object obj)
        {
            while (true)
            {
                try
                {
                    NetworkStream ns = this.tcpClient.GetStream();
                    byte[] buf = new byte[this.tcpClient.ReceiveBufferSize];
                    int readBytes = ns.Read(buf, 0, buf.Length);
                    if (readBytes == 0)
                        break;

                    byte[] data = new byte[readBytes];
                    Array.Copy(buf, data, readBytes);
                    this.Received?.Invoke(data);

                }
                catch
                {
                    break;
                }
            }
            this.Error?.Invoke(this, new DyEventArgs() { Message = "The Scaner has already disconnected!" });
        }
        public void Send(byte[] buf)
        {
            NetworkStream ns = this.tcpClient.GetStream();
            ns.Write(buf, 0, buf.Length);
            Thread thread = new Thread(receiveThread);
            thread.IsBackground = true;
            thread.Start();
        }

        public void Init()
        {
            this.tcpClient.Close();            
        }
        //private void connectCallback(IAsyncResult ar)
        //{
        //    TcpClient client = (TcpClient)ar.AsyncState;
        //    try
        //    {
        //        client.EndConnect(ar);
        //        TcpState state = new TcpState(client);
        //        state.Stream.BeginRead(state.Buffer, 0, state.Buffer.Length, new AsyncCallback(receiveCallback), state);
        //        this.Connected = true;
        //        this.ConnectedNotify?.Invoke();
        //    }
        //    catch(Exception ex)
        //    {
        //        this.Connected = false;
        //        this.Error?.Invoke(this,new DyEventArgs() { Message = $"Connect to Scanner failed!", Data=Encoding.ASCII.GetBytes(ex.Message) });
        //    }
        //}

        //private void receiveCallback(IAsyncResult ar)
        //{
        //    TcpState state = (TcpState)ar.AsyncState;
        //    int readbytes = 0;
        //    try
        //    {
        //        readbytes = state.Stream.EndRead(ar);
        //    }catch
        //    {
        //        readbytes = 0;
        //    }
        //    if (readbytes == 0)
        //    {
        //        this.Connected = false;
        //        this.OnDisconnect?.Invoke(this, new DyEventArgs() { Message= "The Server is disconnect!"});
        //        return;
        //    }
        //    byte[] buf = new byte[readbytes];
        //    Array.Copy(state.Buffer, 0, buf, 0, readbytes);            
        //    this.Received?.Invoke(buf);
        //    state.Stream.BeginRead(state.Buffer, 0, state.Buffer.Length, new AsyncCallback(receiveCallback), state);

        //}

        //public void Send(byte[] data)
        //{
        //    TcpState state = new TcpState(this.tcpClient);
        //    state.Stream.BeginWrite(data, 0, data.Length, new AsyncCallback(sendCallback), state);

        //}

        //private void sendCallback(IAsyncResult ar)
        //{
        //    TcpState state = (TcpState)ar.AsyncState;
        //    state.Stream.EndWrite(ar);
        //}

        //public void Dispose()
        //{
        //    foreach(var k in this.Error.GetInvocationList())
        //    {
        //        this.Error -= new EventHandler<DyEventArgs>(k as EventHandler<DyEventArgs>);
        //    }
        //    foreach(var k in this.ConnectedNotify.GetInvocationList())
        //    {
        //        this.ConnectedNotify -= new ConnectedDelegate(k as ConnectedDelegate);
        //    }
        //    foreach(var k in this.ConnectError.GetInvocationList())
        //    {
        //        this.ConnectError -= new EventHandler<DyEventArgs>(k as EventHandler<DyEventArgs>);
        //    }
        //    foreach(var k in this.OnDisconnect.GetInvocationList())
        //    {
        //        this.OnDisconnect -= new EventHandler<DyEventArgs>(k as EventHandler<DyEventArgs>);
        //    }
        //    foreach(var k in this.Received.GetInvocationList())
        //    {
        //        this.Received -= new ReceiveDelegate(k as ReceiveDelegate);
        //    }
        //    this.tcpClient.Close();
        //}
    }
}
