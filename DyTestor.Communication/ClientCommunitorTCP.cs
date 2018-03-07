using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

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
            this.tcpClient.BeginConnect(serveriP, serverPort, new AsyncCallback(connectCallback), this.tcpClient);
        }

        private void connectCallback(IAsyncResult ar)
        {
            TcpClient client = (TcpClient)ar.AsyncState;
            try
            {
                client.EndConnect(ar);
                TcpState state = new TcpState(client);
                state.Stream.BeginRead(state.Buffer, 0, state.Buffer.Length, new AsyncCallback(receiveCallback), state);
                this.Connected = true;
                this.ConnectedNotify?.Invoke();
            }
            catch(Exception ex)
            {
                this.Connected = false;
                this.Error?.Invoke(this,new DyEventArgs() { Message = "Connect to server failed!",Data=Encoding.ASCII.GetBytes(ex.Message) });
            }
        }

        private void receiveCallback(IAsyncResult ar)
        {
            TcpState state = (TcpState)ar.AsyncState;
            int readbytes = 0;
            try
            {
                readbytes = state.Stream.EndRead(ar);
            }catch
            {
                readbytes = 0;
            }
            if (readbytes == 0)
            {
                this.Error?.Invoke(this,new DyEventArgs() { Message = "The Server is disconnect!" });
                this.Connected = false;
                return;
            }
            byte[] buf = new byte[readbytes];
            Array.Copy(state.Buffer, 0, buf, 0, readbytes);
            state.Stream.BeginRead(state.Buffer, 0, state.Buffer.Length, new AsyncCallback(receiveCallback), state);
            this.Received?.Invoke(buf);

        }

        public void Send(byte[] data)
        {
            TcpState state = new TcpState(this.tcpClient);
            state.Stream.BeginWrite(data, 0, data.Length, new AsyncCallback(sendCallback), state);

        }

        private void sendCallback(IAsyncResult ar)
        {
            TcpState state = (TcpState)ar.AsyncState;
            state.Stream.EndWrite(ar);
        }

        public void Disconnect()
        {
            this.tcpClient.Close();
            this.Connected = false;
        }
    }
}
