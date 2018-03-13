﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace DyTestor.Communication
{
    public class ClientCommunitorTCP : IDisposable
    {
        public  event ReceiveDelegate Received;
        //public  event ErrorDelegate Error;
        public event ConnectedDelegate ConnectedNotify;

        public event EventHandler<DyEventArgs> Error;
        public event EventHandler<DyEventArgs> OnDisconnect;
        public event EventHandler<DyEventArgs> ConnectError;

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
                this.tcpClient.BeginConnect(serveriP, serverPort, new AsyncCallback(connectCallback), this.tcpClient);
            }
            catch(Exception ex)
            {
                this.Connected = false;
                this.ConnectError?.Invoke(this, new DyEventArgs() { Message = $"Connect to {serveriP}:{serverPort} failed!", Data = Encoding.ASCII.GetBytes(ex.Message) });
            }
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
                this.Error?.Invoke(this,new DyEventArgs() { Message = $"Connect to Scanner failed!", Data=Encoding.ASCII.GetBytes(ex.Message) });
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
                this.Connected = false;
                this.OnDisconnect?.Invoke(this, new DyEventArgs() { Message= "The Server is disconnect!"});
                return;
            }
            byte[] buf = new byte[readbytes];
            Array.Copy(state.Buffer, 0, buf, 0, readbytes);            
            this.Received?.Invoke(buf);
            state.Stream.BeginRead(state.Buffer, 0, state.Buffer.Length, new AsyncCallback(receiveCallback), state);

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

        public void Dispose()
        {
            foreach(var k in this.Error.GetInvocationList())
            {
                this.Error -= new EventHandler<DyEventArgs>(k as EventHandler<DyEventArgs>);
            }
            foreach(var k in this.ConnectedNotify.GetInvocationList())
            {
                this.ConnectedNotify -= new ConnectedDelegate(k as ConnectedDelegate);
            }
            foreach(var k in this.ConnectError.GetInvocationList())
            {
                this.ConnectError -= new EventHandler<DyEventArgs>(k as EventHandler<DyEventArgs>);
            }
            foreach(var k in this.OnDisconnect.GetInvocationList())
            {
                this.OnDisconnect -= new EventHandler<DyEventArgs>(k as EventHandler<DyEventArgs>);
            }
            foreach(var k in this.Received.GetInvocationList())
            {
                this.Received -= new ReceiveDelegate(k as ReceiveDelegate);
            }
            this.tcpClient.Close();
        }
    }
}