using System;
using System.Net;

namespace SandTcpServer
{
    //The event args class for the Server's event system
    public sealed class ServerEventArgs : EventArgs
    {
        private int _clientHashCode;
        private EndPoint _clientEndPoint;
        private byte[] _data;

        public int ClientHashCode //Client hash code - allows to interface with a specific connection
        {
            get { return _clientHashCode; }
        }

        public EndPoint ClientEndPoint //The endpoint of the client for the event (usually IP + port)
        {
            get { return _clientEndPoint; }
        }

        public byte[] Data //The data as an array of bytes (only received data event has that member, otherwise it's null)
        {
            get { return _data; }
        }

        public ServerEventArgs(int clientHashCode, EndPoint clientEndPoint, byte[] data)
        {
            _clientHashCode = clientHashCode;
            _clientEndPoint = clientEndPoint;
            _data = data;
        }
    }
}