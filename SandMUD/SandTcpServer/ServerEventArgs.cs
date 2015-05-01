using System;
using System.Net;

namespace SandTcpServer
{
    //The event args class for the Server's event system
    public sealed class ServerEventArgs : EventArgs
    {
        public ServerEventArgs(int clientHashCode, EndPoint clientEndPoint, byte[] data)
        {
            ClientHashCode = clientHashCode;
            ClientEndPoint = clientEndPoint;
            Data = data;
        }

        public int ClientHashCode //Client hash code - allows to interface with a specific connection
        { get; private set; }

        public EndPoint ClientEndPoint //The endpoint of the client for the event (usually IP + port)
        { get; private set; }

        public byte[] Data
            //The data as an array of bytes (only received data event has that member, otherwise it's null)
        { get; private set; }
    }
}