using System;

using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using OpenNETCF.Net.NetworkInformation;


namespace BarcodeReader_ce_CF2
{
    class tcp
    {
        public string serverIP = null;
        public int serverPort = 0;
        public List<string> config = new List<string>();

        public tcp()
        {

            using (StreamReader sr = new StreamReader(@"\Program Files\barcodereader_ce_cf2\config.txt"))
            {
                String line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    config.Add(line);
                }
                sr.Close();

            }

            serverIP = config[0];
            serverPort = Convert.ToInt32(config[1]);
            
        }

        public void send(string text)
        {
            {
                try
                {

                    IPAddress localSIP = IPAddress.Parse(serverIP);
                    
                    int localPort = serverPort;
                    
                    TcpClient tcpclnt = new TcpClient();
                    //Console.WriteLine("Connecting.....(" + localSIP.ToString() + ":" + localPort.ToString() + ")");
                    tcpclnt.SendBufferSize = 100;
                    tcpclnt.Connect(localSIP, localPort);
                    // use the ipaddress as in the server program

                    //Console.WriteLine("Connected");
                    //Console.Write("Enter the string to be transmitted : ");

                    String str = text;
                    Stream stm = tcpclnt.GetStream();

                    ASCIIEncoding asen = new ASCIIEncoding();
                    byte[] ba = asen.GetBytes(str);
                    Console.WriteLine("Transmitting...");

                    stm.Write(ba, 0, ba.Length);
                    stm.Close();
                    byte[] bb = new byte[100];
                    //int k = stm.Read(bb, 0, 100);

                    //for (int i = 0; i < k; i++)
                    //    Console.Write(Convert.ToChar(bb[i]));

                    tcpclnt.Close();
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Error..... " + ex.StackTrace);
                }
            }
        }

        public static bool Ping(string ipaddress)
        {
            //IPHostEntry hostEnt = Dns.GetHostEntry("www.opennetcf.com");
            IPAddress ip = IPAddress.Parse(ipaddress); //hostEnt.AddressList[0];

            Ping ping = new Ping();

            byte[] sendBuffer = new byte[]
            {
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10,
                0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18,
                0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20,
            };


            try
            {
                PingReply reply = ping.Send(ip, sendBuffer, 500, null);
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }


        }





    }
}
