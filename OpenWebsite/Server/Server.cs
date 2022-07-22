using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Server : Form
    {
        IPAddress IP;
        Int32 Port;
        TcpListener TcpServer;
        Socket Client = null;
        NetworkStream ClientStream;
        public Server()
        {
            InitializeComponent();
        }


        //Setup and start server
        private void Setup()
        {
            CheckForIllegalCrossThreadCalls = false;
            IP = IPAddress.Parse("127.0.0.1");
            Port = Int32.Parse(Port_textbox.Text);
            TcpServer = new TcpListener(IPAddress.Any, Port);
            TcpServer.Start();
            Thread ServerThread = new Thread(Connect);
            ServerThread.IsBackground = true;
            ServerThread.Start();
        }

        //Waiting for client and accept connection
        private void Connect()
        {
            while (true)
            {
                Client = TcpServer.AcceptSocket();
                var t = new Thread(new ParameterizedThreadStart(Listen));
                t.IsBackground = true;
                t.Start(Client);
            }
        }

        //Send a message
        private void SendData(string message, object ClientObj)
        {
            Client = ClientObj as Socket;
            ClientStream = new NetworkStream(Client);
            byte[] note = Encoding.UTF8.GetBytes(message);
            ClientStream.Write(note, 0, note.Length);
        }

        //Give message from client
        private string ReceiveData()
        {
            byte[] BufferSize = new byte[Client.ReceiveBufferSize];
            ClientStream.Read(BufferSize, 0, BufferSize.Length);
            string message = Encoding.UTF8.GetString(BufferSize);
            return message;
        }
        private bool IsValid(string url)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => true;
                //Creating the HttpWebRequest 
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too. 
                request.Method = "HEAD";
                //Getting the Web Response. 
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200 
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false. 
                return false;
            }
        }

        //Add http, https
        private string ValidateUrl(string url)
        {
            string head1 = "http://";
            string head2 = "https://";
            if (url.Contains(head1) || url.Contains(head2))
                return url;
            else
                return head1 + url;
        }

        //Listen to client to send and receive 
        private void Listen(object ClientObj)
        {
            Log("Connect ", "Connected", Client.RemoteEndPoint.ToString());
            SendData("Connected to server!", ClientObj);
            while (true)
            {
                string stt = "";
                string stt1 = "";
                string check = "";
                try
                {
                    UrlProcessing_label.Text = ReceiveData();
                    string dt = ReceiveData();
                    Int32 choose = Int32.Parse(dt);
                    SendData("Loading...", ClientObj);

                    string url = ValidateUrl(UrlProcessing_label.Text);

                    if (IsValid(url))
                    {
                        string browserName = "";
                        RegistryKey browserKey1;
                        browserKey1 = Registry.CurrentUser.OpenSubKey(@"Software\Clients\StartMenuInternet");

                        string[] browserName1 = browserKey1.GetSubKeyNames();

                        foreach (string t in browserName1)
                        {
                            using (RegistryKey tempKey = browserKey1.OpenSubKey(t))
                            {
                                foreach (string keyName in tempKey.GetValueNames())
                                {
                                    switch (choose)
                                    {
                                        case 1:
                                            if (tempKey.GetValue(keyName).ToString() == "Google Chrome")
                                            {
                                                browserName = "chrome.exe";
                                                Process.Start(new ProcessStartInfo(browserName, url));
                                                SendData("Successful", ClientObj);
                                                stt = "Successful";
                                                stt1 = "Successful";
                                                check = "true";
                                            }
                                            break;
                                        case 2:
                                            if (tempKey.GetValue(keyName).ToString() == "Cốc Cốc")
                                            {
                                                browserName = "browser.exe";
                                                Process.Start(new ProcessStartInfo(browserName, url));
                                                SendData("Successful", ClientObj);
                                                stt = "Successful";
                                                stt1 = "Successful";
                                                check = "true";
                                            }
                                            break;
                                        case 3:
                                            if (tempKey.GetValue(keyName).ToString() == "Opera Stable")
                                            {
                                                browserName = "opera.exe";
                                                Process.Start(new ProcessStartInfo(browserName, url));
                                                SendData("Successful", ClientObj);
                                                stt = "Successful";
                                                stt1 = "Successful";
                                                check = "true";
                                            }
                                            break;
                                        case 4:
                                            if (tempKey.GetValue(keyName).ToString() == "Safari")
                                            {
                                                browserName = "safari.exe";
                                                Process.Start(new ProcessStartInfo(browserName, url));
                                                SendData("Successful", ClientObj);
                                                stt = "Successful";
                                                stt1 = "Successful";
                                                check = "true";
                                            }
                                            break;
                                        case 5:
                                            if (tempKey.GetValue(keyName).ToString() == "Mozilla Firefox")
                                            {
                                                browserName = "firefox.exe";
                                                Process.Start(new ProcessStartInfo(browserName, url));
                                                SendData("Successful", ClientObj);
                                                stt = "Successful";
                                                stt1 = "Successful";
                                                check = "true";
                                            }
                                            break;
                                        case 6:
                                            if (tempKey.GetValue(keyName).ToString() == "Internet Explorer")
                                            {
                                                browserName = "iexplore.exe";
                                                Process.Start(new ProcessStartInfo(browserName, url));
                                                SendData("Successful", ClientObj);
                                                stt = "Successful";
                                                stt1 = "Successful";
                                                check = "true";
                                            }                                          
                                            break;
                                        default:
                                            MessageBox.Show("Please enter again choose browser");
                                            stt = "Unsuccessful";
                                            stt1 = "Error";
                                            check = "true";
                                            break;
                                    }
                                    if (stt1 == "Successful" || stt1 == "Error")
                                        break;
                                }
                            }
                            if (stt1 == "Successful" || stt1 == "Error")
                            {
                                stt1 = "";
                                break;
                            }
                        }
                        if (check != "true")
                        {
                            RegistryKey browserKeys;
                            browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet");

                            if (browserKeys == null)
                            {
                                browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
                            }

                            string[] browserNames = browserKeys.GetSubKeyNames();

                            foreach (string browser in browserNames)
                            {
                                using (RegistryKey tempKey = browserKeys.OpenSubKey(browser))
                                {
                                    foreach (string keyName in tempKey.GetValueNames())
                                    {
                                        switch (choose)
                                        {
                                            case 1:
                                                if (tempKey.GetValue(keyName).ToString() == "Google Chrome")
                                                {
                                                    browserName = "chrome.exe";
                                                    Process.Start(new ProcessStartInfo(browserName, url));
                                                    SendData("Successful", ClientObj);
                                                    stt = "Successful";
                                                    stt1 = "Successful";
                                                    check = "true";
                                                }
                                                break;
                                            case 2:
                                                if (tempKey.GetValue(keyName).ToString() == "Cốc Cốc")
                                                {
                                                    browserName = "browser.exe";
                                                    Process.Start(new ProcessStartInfo(browserName, url));
                                                    SendData("Successful", ClientObj);
                                                    stt = "Successful";
                                                    stt1 = "Successful";
                                                    check = "true";
                                                }
                                                break;
                                            case 3:
                                                if (tempKey.GetValue(keyName).ToString() == "Opera Stable")
                                                {
                                                    browserName = "opera.exe";
                                                    Process.Start(new ProcessStartInfo(browserName, url));
                                                    SendData("Successful", ClientObj);
                                                    stt = "Successful";
                                                    stt1 = "Successful";
                                                    check = "true";
                                                }
                                                break;
                                            case 4:
                                                if (tempKey.GetValue(keyName).ToString() == "Safari")
                                                {
                                                    browserName = "safari.exe";
                                                    Process.Start(new ProcessStartInfo(browserName, url));
                                                    SendData("Successful", ClientObj);
                                                    stt = "Successful";
                                                    stt1 = "Successful";
                                                    check = "true";
                                                }
                                                break;
                                            case 5:
                                                if (tempKey.GetValue(keyName).ToString() == "Mozilla Firefox")
                                                {
                                                    browserName = "firefox.exe";
                                                    Process.Start(new ProcessStartInfo(browserName, url));
                                                    SendData("Successful", ClientObj);
                                                    stt = "Successful";
                                                    stt1 = "Successful";
                                                    check = "true";
                                                }
                                                break;
                                            case 6:
                                                if (tempKey.GetValue(keyName).ToString() == "Internet Explorer")
                                                {
                                                    browserName = "iexplore.exe";
                                                    Process.Start(new ProcessStartInfo(browserName, url));
                                                    SendData("Successful", ClientObj);
                                                    stt = "Successful";
                                                    stt1 = "Successful";
                                                    check = "true";
                                                }
                                                break; 
                                        }
                                        if (stt1 == "Successful") 
                                            break;
                                    }
                                }
                                if (stt1 == "Successful")
                                {
                                    stt1 = "";
                                    break;
                                }
                            }
                        }
                        if (check != "true")
                        {
                            MessageBox.Show("Browser is uninstalled. \nOpen default browser");
                            System.Diagnostics.Process.Start(url);
                            SendData("Successful", ClientObj);
                            stt = "Successful";
                        }
                    }
                    else
                    {
                        SendData("Unsuccessful", ClientObj);
                        stt = "Unsuccessful";
                        MessageBox.Show("URL is error.\n Please enter the URL again.");
                    }

                    Log(UrlProcessing_label.Text, stt, Client.RemoteEndPoint.ToString());
                }
                catch
                {
                    ClientStream.Flush();
                    ClientStream.Close();
                    return;
                }
            }
        }

        //Save log
        private void expLog(ListViewItem a)
        {
            StreamWriter writer = File.AppendText("log.txt");
            writer.WriteLine(a.Text + " " + a.SubItems[1].Text + " " + a.SubItems[2].Text + " " + a.SubItems[3].Text);
            writer.WriteLine(a.Text + "+" + a.SubItems[1].Text + "+" + a.SubItems[2].Text + "+" + a.SubItems[3].Text);
            writer.Flush();
            writer.Close();
            writer.Dispose();
        }


        //Write to log textbox
        private void Log(string data, string status, string client)
        {
            ListViewItem ldata = new ListViewItem(data);
            ListViewItem.ListViewSubItem  lstatus = new ListViewItem.ListViewSubItem(ldata, status);
            ListViewItem.ListViewSubItem lclient = new ListViewItem.ListViewSubItem(ldata, client);
            ListViewItem.ListViewSubItem ltime = new ListViewItem.ListViewSubItem(ldata, DateTime.Now.ToString());

            ldata.SubItems.Add(lstatus);
            ldata.SubItems.Add(lclient);
            ldata.SubItems.Add(ltime);
            listView1.Items.Insert(0, ldata);
            expLog(ldata);
        }

       

        private void Button2_Click(object sender, EventArgs e)
        {
            Start_btn.Enabled = false;
            Setup();
            label2.Text = "ON";
        }
        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Client != null)
            {
                ClientStream.Flush();
                ClientStream.Close();
                Client.Close();
            }
        }
    }
}
