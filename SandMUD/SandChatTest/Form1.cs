using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SandDataGenerator;
using SandDataProcessor;
using SandTcpServer;


namespace SandChatTest
{
    public partial class Form1 : Form
    {
        Server server;
        HashSet<int> connections;
        public Form1()
        {
            connections = new HashSet<int>();

            InitializeComponent();

            server = new Server(23);

            server.ClientConnected += OnConnection;
            server.ClientDisconnected += OnDisconnection;
            server.DataReceived += OnDataReceived;

        }

        private void OnDataReceived(object sender, ServerEventArgs e)
        {
            DataGenerator dg = new DataGenerator();

            var data = DataProcessor.Process(e.Data, DataProcessor.DefaultPipeline);

            string str = e.ClientEndPoint.ToString() + ": " + System.Text.Encoding.Default.GetString(data);

            dg.AddText(str);

            foreach (var c in connections)
            {
                if (c != e.ClientHashCode) server.SendData(c, dg.GetData());
            }
        }



        private void OnDisconnection(object sender, ServerEventArgs e)
        {
            connections.Remove(e.ClientHashCode);
            listBox1.Items.Remove(e.ClientEndPoint.ToString());
        }

        private void OnConnection(object sender, ServerEventArgs e)
        {
            connections.Add(e.ClientHashCode);
            listBox1.Items.Add(e.ClientEndPoint.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            server.Activate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            server.Deactivate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (server.EventsPending)
            {
                server.Process();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            server.Deactivate();
        }
    }
}
