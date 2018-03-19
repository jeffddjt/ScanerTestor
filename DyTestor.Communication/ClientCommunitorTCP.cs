﻿using DyTestor.Configuration;
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
                    Thread.Sleep(3000);
                    if (connected)
                        continue;
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
            this.tcpClient.BeginConnect(AppConfig.SCANER_IP, AppConfig.SCANER_PORT, new AsyncCallback(connectCallback), this.tcpClient);
        }

        private void connectCallback(IAsyncResult ar)
        {
            TcpClient client = (TcpClient)ar.AsyncState;
            try
            {
                client.EndConnect(ar);
                this.connected = true;
                this.OnConnect?.Invoke();
                TcpState state = new TcpState(client);
                state.Stream.BeginRead(state.Buffer, 0, state.Buffer.Length, new AsyncCallback(receiveCallback), state);
            }catch
            {
                this.connected = false;
            }
        }

        private void receiveCallback(IAsyncResult ar)
        {
            TcpState state = (TcpState)ar.AsyncState;
            int readbytes = 0;
            try
            {
                readbytes = state.Stream.EndRead(ar);
                if(readbytes==0)
                {
                    this.connected = false;
                    return;
                }
                byte[] data = new byte[readbytes];
                Array.Copy(state.Buffer, 0, data, 0, readbytes);
                this.Received?.Invoke(data);
                state.Stream.BeginRead(state.Buffer, 0, state.Buffer.Length, new AsyncCallback(receiveCallback), state);
            }
            catch
            {
                this.connected = false;
            }
        }

        public void Send(byte[] data)
        {
            TcpState state = new TcpState(this.tcpClient);
            state.Stream.BeginWrite(data, 0, data.Length, new AsyncCallback(sendCallback), state);
        }

        private void sendCallback(IAsyncResult ar)
        {
            TcpState state = (TcpState)ar.AsyncState;
            try
            {
                state.Stream.EndWrite(ar);
            }
            catch
            {
                this.connected = false;
            }
        }
    }
}
