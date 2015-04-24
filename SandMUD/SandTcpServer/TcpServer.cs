using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;

namespace SandTcpServer
{
    enum CallbackMessageType
    {
        Connected,
        Disconnected,
        DataReceived,
    };

    struct CallbackMessage
    {
        public CallbackMessageType CallbackMessageType;
        public ClientHandler ClientHandler;
        public Byte[] Data;

        public CallbackMessage(CallbackMessageType callbackMessageType, ClientHandler clientHandler, Byte[] data = null)
        {
            CallbackMessageType = callbackMessageType;
            ClientHandler = clientHandler;
            Data = data;
        }
    }

    sealed class ClientHandler
    {
        byte[] buffer;

        private TcpClient _client;
        private Thread _handlerThread;
        private ConcurrentQueue<Byte[]> _dataToSendQueue;
        private DateTime _lastCheck;
        Action<CallbackMessage> _serverCallback;
        
        private void handlerLoop()
        {
            NetworkStream stream = null;
            Byte[] dataToSend = null;

            if (_client.Connected)
            {
                _serverCallback(new CallbackMessage(CallbackMessageType.Connected, this));
                stream = _client.GetStream();
            }
            else
                return;

            while (_client.Connected)
            {
               
                if (_client.Available > 0)
                {
                    
                    int bytesReceived = stream.Read(buffer, 0, buffer.Length);

                    byte[] receivedData = new byte[bytesReceived];

                    Array.Copy(buffer, receivedData, bytesReceived);

                    _serverCallback(new CallbackMessage(CallbackMessageType.DataReceived, this, receivedData));
                }
                else if (_dataToSendQueue.TryDequeue(out dataToSend))
                {
                    try
                    {
                        stream.Write(dataToSend, 0, dataToSend.Length);
                    }
                    catch
                    {
                        Disconnect();
                    }
                }
                else if ((DateTime.Now - _lastCheck).Seconds > 5)
                {
                    _lastCheck = DateTime.Now;
                    try
                    {
                        dataToSend = new byte[1];
                        dataToSend[0] = 0;
                        stream.Write(dataToSend, 0, dataToSend.Length);
                    }
                    catch
                    {
                        Disconnect();
                    }
                }
                else
                {
                    Thread.Sleep(10);
                }
            }

            _serverCallback(new CallbackMessage(CallbackMessageType.Disconnected, this));
        }

        public ClientHandler(TcpClient client, Action<CallbackMessage> serverCallback)
        {
            _client = client;
            _serverCallback = serverCallback;
            _dataToSendQueue = new ConcurrentQueue<byte[]>();

            _handlerThread = new Thread(new ThreadStart(handlerLoop));
            _handlerThread.Priority = ThreadPriority.Lowest;
            _handlerThread.Start();

            _lastCheck = DateTime.Now;

            buffer = new byte[_client.ReceiveBufferSize];
        }

        public EndPoint RemoteEndPoint
        {
            get
            {
                return _client.Client.RemoteEndPoint;
            }
        }

        public void Disconnect()
        {
            _client.Client.DisconnectAsync(new SocketAsyncEventArgs());
        }

        public void SendData(byte[] data)
        {
            _dataToSendQueue.Enqueue(data);
        }
    }

    public class ServerEventArgs: EventArgs
    {
        public int ClientHashCode { get; set; }
        public EndPoint ClientEndPoint { get; set; }
        public byte[] Data { get; set; }
    }

    public sealed class Server
    {
        private TcpListener _server;
        private Thread _listenThread;
        private bool _activated;
        Dictionary<int, ClientHandler> _clientHandlers;
        ConcurrentQueue<CallbackMessage> _messages;

        //Event system
        public event EventHandler<ServerEventArgs> ClientConnected;
        public event EventHandler<ServerEventArgs> DataReceived;
        public event EventHandler<ServerEventArgs> ClientDisconnected;

        private void serverCallback(CallbackMessage msg)
        {
            _messages.Enqueue(msg);
        }

        private void listenLoop()
        {
            _server.Start();

            while (_activated)
            {
                var newClient = _server.AcceptTcpClient();

                var newHandler = new ClientHandler(newClient, serverCallback);
            }

            _server.Stop();

        }

        public Server(int port)
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Any, port);
            _clientHandlers = new Dictionary<int, ClientHandler>();
            _messages = new ConcurrentQueue<CallbackMessage>();

            _server = new TcpListener(ipEndPoint);

        }

        public void Activate()
        {
            if (_activated)
                return;

            _activated = true;
            _listenThread = new Thread(new ThreadStart(listenLoop));
            _listenThread.Priority = ThreadPriority.BelowNormal;
            _listenThread.Start();
        }

        public void Process()
        {

            while (!_messages.IsEmpty)
            {
                CallbackMessage msg;

                _messages.TryDequeue(out msg);
                var e = new ServerEventArgs();
                e.Data = msg.Data;
                e.ClientHashCode = msg.ClientHandler.GetHashCode();
                e.ClientEndPoint = msg.ClientHandler.RemoteEndPoint;

                switch(msg.CallbackMessageType)
                {
                    case CallbackMessageType.Connected:
                        _clientHandlers.Add(msg.ClientHandler.GetHashCode(), msg.ClientHandler);
                        if (ClientConnected != null) ClientConnected(this, e);
                        break;
                    case CallbackMessageType.Disconnected:
                        _clientHandlers.Remove(msg.ClientHandler.GetHashCode());
                        if (ClientDisconnected!= null) ClientDisconnected(this, e);
                        break;
                    case CallbackMessageType.DataReceived:
                        if(DataReceived != null) DataReceived(this, e);
                        break;
                }
            }
        }

        public void Deactivate()
        {
            if (!_activated)
                return;

            _activated = false;

            foreach (var item in _clientHandlers)
            {
                item.Value.Disconnect();
            }
        }

        public void DisconnectClient(int ConnectionHashCode)
        {
            ClientHandler handler;

            if (_clientHandlers.TryGetValue(ConnectionHashCode, out handler))
            {
                handler.Disconnect();
            }
        }

        public void SendData(int ConnectionHashCode, byte[] data)
        {
            ClientHandler handler;

            if(_clientHandlers.TryGetValue(ConnectionHashCode, out handler))
            {
                handler.SendData(data);
            }
        }
    }
}
