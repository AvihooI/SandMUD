using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SandTcpServer;
using SandDataGenerator;

namespace SandTcpServerTest
{
    class Program
    {
        static Server server;

        private static void OnConnection(object sender, ServerEventArgs e)
        {
            Console.WriteLine("Connection from: {0}", e.ClientEndPoint.ToString());
        }

        static void OnDataReceived(object sender, ServerEventArgs e)
        {
            var str = System.Text.Encoding.Default.GetString(e.Data).Trim();

            Console.WriteLine("Data received from {0} : {1}", e.ClientEndPoint.ToString(), str);

            if (str.ToLower() == "quit")
                server.DisconnectClient(e.ClientHashCode);

            AnsiStyle style = new AnsiStyle();
            style.ForegroundColor = AnsiColor.Red;
            style.Bold = true;
            style.Italics = true;
            style.Underline = true;

            var dataGenerator = new DataGenerator();

            dataGenerator.SetStyle(style);
            dataGenerator.AddText(str);
            dataGenerator.AddText(" ");

            style.ForegroundColor = AnsiColor.Red;
            style.Bold = false;
            style.Italics = false;
            style.Underline = false;
            style.Blink = true;

            dataGenerator.SetStyle(style);
            dataGenerator.AddText(str);

            server.SendData(e.ClientHashCode, dataGenerator.GetData());
        }

        static void OnDisconnection(object sender, ServerEventArgs e)
        {
            Console.WriteLine("Disconnection from: {0}", e.ClientEndPoint.ToString());
        }

        static void Main(string[] args)
        {
            server = new Server(23);
            bool done = false;

            server.ClientConnected += OnConnection;
            server.ClientDisconnected += OnDisconnection;
            server.DataReceived += OnDataReceived;

            server.Activate();

            while(!done)
            {
                server.Process();
                Thread.Sleep(10);
            }

            server.Deactivate();
        }


    }
}
