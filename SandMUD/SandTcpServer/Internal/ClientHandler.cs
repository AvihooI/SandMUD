using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SandTcpServer
{
    internal static class ConnectionIdentiferProducer
    {
        static int _nextIdentifier = 1;

        public static int NewIdentifier()
        {
            int result = _nextIdentifier;

            _nextIdentifier++;

            return result;
        }

    }

    internal sealed class ClientHandler
    {
        byte[] _buffer;

        private TcpClient _client;
        private Thread _handlerThread;
        private ConcurrentQueue<Byte[]> _dataToSendQueue;
        private DateTime _lastCheck;
        Action<CallbackMessage> _serverCallback;

        private int _id;

        //The client handler loop (to run in a separate thread), initiated upon the construction of the client handler object
        private void handlerLoop()
        {
            NetworkStream stream = null;
            Byte[] dataToSend = null;

            //The associated tcp client needs to be connected at this point... if it's not, this method will return (and the object would get garbage collected eventually since it's not contained anywhere)
            if (_client.Connected)
            {
                //We notify the server that we're connected via callback, and get the tcp client's stream for network I/O
                _serverCallback(new CallbackMessage(CallbackMessageType.Connected, this));
                stream = _client.GetStream();
            }
            else
                //Not connected, return
                return;

            //As long as we're connected, do network I/O operations
            while (_client.Connected)
            {
                //If we have data to receive...
                if (_client.Available > 0)
                {
                    //Get the data into our buffer
                    int bytesReceived = stream.Read(_buffer, 0, _buffer.Length);
                    //Create an array with the appropriate size
                    byte[] receivedData = new byte[bytesReceived];
                    //Copy from the buffer to that array
                    Array.Copy(_buffer, receivedData, bytesReceived);
                    //Use the callback to dispatch the data
                    _serverCallback(new CallbackMessage(CallbackMessageType.DataReceived, this, receivedData));
                }
                //Else if we have data to send, try to send it, if we catch an exception, we disconnect (because it means there's something wrong with the socket)
                //NOTE: this deals with a single member in the send queue, it doesn't try to send all the data in the queue (because sending is of lesser priority to receiving)
                else if (_dataToSendQueue.TryDequeue(out dataToSend))
                {
                    _lastCheck = DateTime.Now; //Just so that we won't unnecessarily ping later
                    try
                    {
                        stream.Write(dataToSend, 0, dataToSend.Length);
                    }
                    catch
                    {
                        Disconnect();
                    }
                }
                //We have no data to send or to receive, so we check the last time we pinged, if it's above 5 seconds (magic number atm, should change), we ping again
                else if ((DateTime.Now - _lastCheck).Seconds > 5)
                {
                    _lastCheck = DateTime.Now; //Set the last check to now
                    //Try to send an insignificant message (1 byte buffer with a null in it) - this doesn't display on typical MUD clients (should check if there's a better way)
                    try
                    {
                        dataToSend = new byte[1];
                        dataToSend[0] = 0;
                        stream.Write(dataToSend, 0, dataToSend.Length);
                    }
                    catch
                    {
                        Disconnect(); //We caught an exception - disconnect
                    }
                }
                //We have nothing to do for now (no data to send or receive, no pinging), so let this thread sleep for a little while (10 milliseconds at the moment, should change)
                else
                {
                    Thread.Sleep(10);
                }
            }

            _serverCallback(new CallbackMessage(CallbackMessageType.Disconnected, this)); //We're not connected anymore, send a disconnect message via callback
        }

        public ClientHandler(TcpClient client, Action<CallbackMessage> serverCallback)
        {
            _client = client;
            _serverCallback = serverCallback; //The server callback method - the only way the handler interfaces back with the server (this makes the handler independent from the server)
            _dataToSendQueue = new ConcurrentQueue<byte[]>(); //This queue handles incoming data to SEND - there's no immediate data sending in the handler

            _handlerThread = new Thread(new ThreadStart(handlerLoop));
            _handlerThread.Priority = ThreadPriority.Lowest; //This thread is very low priority because it only handles a single connection
            _handlerThread.Start();

            _lastCheck = DateTime.Now; //Set the lastCheck to now (this is responsible for pinging when there's no connection activity)

            _buffer = new byte[_client.ReceiveBufferSize]; //We use this buffer to receive data

            _id = ConnectionIdentiferProducer.NewIdentifier();
        }

        //This property gives the connection end point (IP + port usually)
        public EndPoint RemoteEndPoint
        {
            get
            {
                return _client.Client.RemoteEndPoint;
            }
        }

        //This method disconnects the client asynchronously (it wouldn't be thread safe to immediately close a connection)
        //TOOD: maybe find a better way to implement this using a flag (though this doesn't appear to cause to trouble atm)
        public void Disconnect()
        {
            _client.Client.DisconnectAsync(new SocketAsyncEventArgs());
        }

        //This queues up data to send (the queue is later consumed in the loop in the other thread)
        public void SendData(byte[] data)
        {
            _dataToSendQueue.Enqueue(data);
        }

        //Override of GetHashCode to guarrantee uniqueness
        public override int GetHashCode()
        {
            return _id;
        }
    }
}