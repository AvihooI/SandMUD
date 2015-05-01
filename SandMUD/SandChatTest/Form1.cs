using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SandDataGenerator;
using SandTcpServer;

namespace SandChatTest
{
    public partial class Form1 : Form
    {
        private readonly HashSet<int> _connections;
        private readonly Server _server;

        public Form1()
        {
            _connections = new HashSet<int>();

            InitializeComponent();

            var sconfig = new ServerConfig(23, false, 1);

            _server = new Server(sconfig);

            _server.ClientConnected += OnConnection;
            _server.ClientDisconnected += OnDisconnection;
            _server.DataReceived += OnDataReceived;
        }

        private void OnDataReceived(object sender, ServerEventArgs e)
        {
            var dg = new AnsiGenerator();

            var data = e.Data.Where(b => (b == 13) || (b > 31 && b < 127)).ToArray();

            var str = e.ClientEndPoint + ": " + Encoding.Default.GetString(data);

            dg.AddText(str);

            foreach (var c in _connections.Where(c => c != e.ClientHashCode))
            {
                _server.SendData(c, dg.GetData());
            }
        }

        private void OnDisconnection(object sender, ServerEventArgs e)
        {
            _connections.Remove(e.ClientHashCode);
            listBox1.Items.Remove(e.ClientEndPoint.ToString());
        }

        private void OnConnection(object sender, ServerEventArgs e)
        {
            _connections.Add(e.ClientHashCode);
            listBox1.Items.Add(e.ClientEndPoint.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _server.Activate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _server.Deactivate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_server.EventsPending)
            {
                _server.Process();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _server.Deactivate();
        }
    }
}