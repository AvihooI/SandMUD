using System;

namespace SandTcpServer
{
    //An enum for the three possible callback messages returned from the client handler
    internal enum CallbackMessageType
    {
        Connected,
        Disconnected,
        DataReceived,
    };

    //The CallBack message struct that contains the callback type, the client handler (typically the one who called the callback) and the associated data (as a byte array)
    internal struct CallbackMessage
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
}