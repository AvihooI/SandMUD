using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SandTcpServer
{
    public struct ServerConfig
    {
        public bool Local;
        public uint MaximalConnections;
        public int Port;

        public ServerConfig(int port = 23, bool local = false, uint maximalConnections = 0)
        {
            Port = port;
            Local = local;
            MaximalConnections = maximalConnections;
        }
    }

    public sealed class Server
    {
        private readonly Dictionary<int, ClientHandler> _clientHandlers;
        private readonly uint _maximalConnections;
        private readonly ConcurrentQueue<CallbackMessage> _messages;
        private readonly TcpListener _server;
        private Thread _listenThread;

        public Server(ServerConfig serverConfig)
        {
            var ipEndPoint = new IPEndPoint(serverConfig.Local ? IPAddress.Loopback : IPAddress.Any, serverConfig.Port);
            _clientHandlers = new Dictionary<int, ClientHandler>();
            _messages = new ConcurrentQueue<CallbackMessage>();
            _maximalConnections = serverConfig.MaximalConnections;
            _server = new TcpListener(ipEndPoint);
        }

        public Server(int port) : this(new ServerConfig(port))
        {
        }

        public Server() : this(new ServerConfig())
        {
        }

        //Added property: activated - tells if the server is activated or not

        public bool Activated { get; private set; }
        //Added property: events pending - tells if the server has event raising processes waiting

        public bool EventsPending
        {
            get { return !_messages.IsEmpty; }
        }

        public event EventHandler<ServerEventArgs> ClientConnected;
        public event EventHandler<ServerEventArgs> DataReceived;
        public event EventHandler<ServerEventArgs> ClientDisconnected;
        //The server callback method is to be called from the client handlers... the messages aren't dealt with immediately but are pushed to a queue
        //The reason for that is that the client handler threads shouldn't deal with the processing of messages (and the associated event driven system)
        private void ServerCallback(CallbackMessage msg)
        {
            _messages.Enqueue(msg);
        }

        //This listen loop runs on the listener thread - the thread itself has a blocking call so the loop isn't a problem
        private void ListenLoop()
        {
            _server.Start(); //Start the server

            //Loop as long as the server is activated - receive new connections and assign them to new client handlers
            //From then on, the client handler does everything 
            while (Activated)
            {
                if (_server.Pending())
                {
                    //Added: maximal connections based on server configuration
                    var tcpClient = _server.AcceptTcpClient();

                    if ((_maximalConnections == 0) || (_clientHandlers.Count < _maximalConnections))
                    {
                        // ReSharper disable once ObjectCreationAsStatement - we will get a callback that'll put the object back in a container
                        new ClientHandler(tcpClient, ServerCallback);
                    }
                    else
                    {
                        tcpClient.Close();
                    }
                }
                else
                {
                    Thread.Sleep(10);
                }
            }

            _server.Stop(); //Stop the server - since we're no longer activated
            _listenThread.Abort();
        }

        //The activate method actually activates the listening thread and accepts new connections
        public void Activate()
        {
            if (Activated) //Do nothing if we're already activated
                return;

            Activated = true;
            _listenThread = new Thread(ListenLoop);
            _listenThread.Start();
        }

        //The process method is called to process all the callback messages and generate the appropriate events, it should be called routinely to get new events
        public void Process()
        {
            while (!_messages.IsEmpty)
            {
                CallbackMessage msg;

                _messages.TryDequeue(out msg); //We know this would work since messages isn't empty
                var e = new ServerEventArgs(msg.ClientHandler.GetHashCode(), msg.ClientHandler.RemoteEndPoint, msg.Data);
                //We create a server args with the right values

                //Then we call the appropriate event handler based on the callback message
                switch (msg.CallbackMessageType)
                {
                    case CallbackMessageType.Connected:
                        _clientHandlers.Add(msg.ClientHandler.GetHashCode(), msg.ClientHandler);
                        if (ClientConnected != null) ClientConnected(this, e);
                        break;
                    case CallbackMessageType.Disconnected:
                        _clientHandlers.Remove(msg.ClientHandler.GetHashCode());
                        if (ClientDisconnected != null) ClientDisconnected(this, e);
                        break;
                    case CallbackMessageType.DataReceived:
                        if (DataReceived != null) DataReceived(this, e);
                        break;
                }
            }
        }

        //Deactivating does two things: it changes the activated flag to false so that the listening loop will stop
        //It also disconnects every client handler
        public void Deactivate()
        {
            if (!Activated)
                return;

            Activated = false;

            foreach (var item in _clientHandlers)
            {
                item.Value.Disconnect();
            }
        }

        //Disconnect a specific client
        public void DisconnectClient(int clientHashCode)
        {
            ClientHandler handler;

            if (_clientHandlers.TryGetValue(clientHashCode, out handler))
            {
                handler.Disconnect();
            }
        }

        //Send data to client
        public void SendData(int clientHashCode, byte[] data)
        {
            ClientHandler handler;

            if (_clientHandlers.TryGetValue(clientHashCode, out handler))
            {
                handler.SendData(data);
            }
        }
    }
}