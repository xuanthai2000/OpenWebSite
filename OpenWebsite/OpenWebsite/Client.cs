using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics.Eventing.Reader;
using System.Security.Policy;

namespace OpenWebsite
{
    public partial class Client : Form
    {
        TcpClient TcpClient = new TcpClient();
        NetworkStream ServerStream = null;
        public Client()
        {
            InitializeComponent();
        }

        //Setup and start
        private void Setup()
        {
            try
            {
                CheckForIllegalCrossThreadCalls = false;
                if (TcpClient.Connected) Status.Text = "Connected.";
                else TcpClient.Connect(IP_textbox.Text, Int32.Parse(Port_textbox.Text));
                Thread ctThread = new Thread(ReceiveMessage);
                ctThread.Start();
                Url_textbox.Enabled = true;
                Open_textbox.Enabled = true;
            }
            catch
            {
                Status.Text = "Can't connect, please try again!";
            }
        }

        //Send message
        private void SendData(string message)
        {
            try
            {
                byte[] OutStream = Encoding.UTF8.GetBytes(message);
                ServerStream.Write(OutStream, 0, OutStream.Length);
                ServerStream.Flush();
            }
            catch
            {
                Status.Text = "Can't connect, please try again!";
            }
        }

        //Receive message from server
        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    ServerStream = TcpClient.GetStream();
                    var BufferSize = TcpClient.ReceiveBufferSize;
                    byte[] InStream = new byte[BufferSize];
                    ServerStream.Read(InStream, 0, BufferSize);
                    Status.Text = Encoding.UTF8.GetString(InStream);
                }
                catch
                {
                    Status.Text = "Closed.";
                    ServerStream.Close();
                    TcpClient.Close();
                    return;
                }
            }
        }
       
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //Connect
        private void Button1_Click(object sender, EventArgs e)
        {
            Thread stThread = new Thread(Setup);
            stThread.IsBackground = true;
            stThread.Start(); 
        }

        private void Client_Load(object sender, EventArgs e)
        {

        }
      
        private bool isUrl(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute);
        }
        //Click to send URL
        private void Open_textbox_Click(object sender, EventArgs e)
        {
            if (!isUrl(Url_textbox.Text.Trim()))
            {
                Status.Text = "Invalid URL.";
            }
            else SendData(Url_textbox.Text.Trim());
            SendData(textBox7.Text.Trim());
        }
    }
}
