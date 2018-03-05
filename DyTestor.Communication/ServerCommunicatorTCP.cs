using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DyTestor.Communication
{
    public class ServerCommunicatorTCP
    {
        public event ReceiveDelegate Received;
        public event ErrorDelegate Error;

        private TcpListener listener;
        private Dictionary<string, TcpState> clientDict = new Dictionary<string, TcpState>();
        public ServerCommunicatorTCP(int listenPort)
        {
            this.listener = new TcpListener(IPAddress.Any, listenPort);
        }
        public void Start()
        {
            this.listener.Start(30);
            this.listener.BeginAcceptTcpClient(new AsyncCallback(connectCallback), this.listener);
        }
        public void Stop()
        {
            this.listener.Stop();
        }
        public IList<string> GetClientList()
        {
            return this.clientDict.Keys.ToList();
        }
        private void connectCallback(IAsyncResult ar)
        {
            TcpListener server = (TcpListener)ar.AsyncState;
            TcpClient client = server.EndAcceptTcpClient(ar);
            TcpState state = new TcpState(client);
            this.clientDict.Add(state.ID, state);
            state.Stream.BeginRead(state.Buffer, 0, state.Buffer.Length, new AsyncCallback(receiveCallback), state);
            server.BeginAcceptTcpClient(new AsyncCallback(connectCallback), server);
        }

        private void receiveCallback(IAsyncResult ar)
        {
            TcpState state = (TcpState)ar.AsyncState;
            int readbytes = 0;
            try
            {
                readbytes = state.Stream.EndRead(ar);
            }
            catch
            {
                readbytes = 0;
            }
            if(readbytes==0)
            {
                this.Error("Client is already disconnect!");
                this.clientDict.Remove(state.ID);
                return;
            }
            byte[] buf = new byte[readbytes];
            Array.Copy(state.Buffer, 0, buf, 0, readbytes);
            state.Stream.BeginRead(state.Buffer, 0, state.Buffer.Length, new AsyncCallback(receiveCallback), state);
            this.Received(buf);
        }

        public void Send(byte[] data,string remoteIP,int remotePort)
        {
            string id = string.Format("{0}{1}", remoteIP, remotePort);
            TcpState state = this.clientDict[id];
            state.Stream.BeginWrite(data, 0, data.Length, new AsyncCallback(sendCallback), state);
        }

        public void Send(byte[] data, string remote)
        {
            TcpState state = this.clientDict[remote];
            state.Stream.BeginWrite(data, 0, data.Length, new AsyncCallback(sendCallback), state);
        }
        public void Broadcast(byte[] data)
        {
            foreach(string remote in this.clientDict.Keys)
            {
                TcpState state = this.clientDict[remote];
                state.Stream.BeginWrite(data, 0, data.Length, new AsyncCallback(sendCallback), state);
            }
        }
        private void sendCallback(IAsyncResult ar)
        {
            TcpState state = (TcpState)ar.AsyncState;
            state.Stream.EndWrite(ar);
        }
    }
}
