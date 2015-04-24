using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace NetworkProgrammingLearn
{

    class Program
    {
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        static void Main(string[] args)
        {
            var endPoint = new IPEndPoint(IPAddress.Loopback, 23);
            var server = new TcpListener(endPoint);

            var buffer = new Byte[1024];
            String data = null;

            server.Start();

            while(true)
            {
                var client = server.AcceptTcpClient();
                Console.WriteLine("Connected");
                client.Client.Send(GetBytes("Test test"));

                client.Close();
            }
            
            server.Stop();

        }
    }
}
