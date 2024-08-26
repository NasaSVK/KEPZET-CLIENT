using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Intermec.DataCollection;
using Intermec.Communication.WLAN;
using System.Net;
using System.Net.Sockets;
using BarcodeReader_ce_CF2;
using System.Runtime.InteropServices;
using Microsoft.WindowsMobile.Status;


namespace BarcodeReader_ce
{

    public partial class Form1 : Form
    {
        
        
        //declare barcode reader object
        private Intermec.DataCollection.BarcodeReader bcr;

        tcp tcpRcv = new tcp();
        BackgroundWorker bw = new BackgroundWorker();
        int clientPort = 2004;
        string dataRcvd = null;
        Sound NokSound = new Sound(@"Program Files\barcodereader_ce_cf2\alarm.wav");
        Sound OKSound = new Sound(@"Program Files\barcodereader_ce_cf2\alarm2.wav");
        Timer lampTimer = new Timer();
        Timer ipTimer = new Timer();
        int ipTimerCounter = 0;// testTimerCounter = 0;
        bool stop = false;
        tcp tcpSend = new tcp();
        SystemState batterySystemState;
        IPAddress[] addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        List<multipackData> mpData = new List<multipackData>();

        fs FS = new fs();

        [DllImport("coredll.dll")]

        private static extern int AllKeys(bool bEnable);
        class multipackData
        {
            public multipackData(string n, System.Drawing.Color c)
            {
                this.nr = n;
                this.color = c;
            }
            public string nr { get; set; }
            public System.Drawing.Color color { get; set; }

        }

        public Form1()
        {
            InitializeComponent();
            //init controls
            lblBarcode.Text = "";
            lblInfo.Text = "";
            lblMP00.Text = "";
            lblMP01.Text = "";
            lblMP02.Text = "";
            lblMP03.Text = "";
            lblMP04.Text = "";
            lblMP05.Text = "";
            lblMP06.Text = "";
            lblMP07.Text = "";
            lblMP08.Text = "";
            lblMP09.Text = "";
            lblMP10.Text = "";
            lblMP11.Text = "";
            lblMultiPack.Text = "";
            lblPN.Text = "";
            lblPos.Text = "";
            lblPrevPallette.Text = "";
            button1.Visible = false;
            textBox1.Visible = false;
            lampText.Visible = false;
            btnAcc.Visible = false;


            lampTimer.Enabled = false;
            lampTimer.Interval = 15000;
            lampTimer.Tick += new System.EventHandler(lampTimerTick);
            AllKeys(true);
            NokSound.Volume = 3;

            batterySystemState = new SystemState(SystemProperty.PowerBatteryStrength);
            batterySystemState.Changed += new ChangeEventHandler(PowerBatteryStrength_Changed);

            PowerBatteryStrength_Changed(null, null);

            //setup bcr
            try
            {
                //create a instance of BarcodeReader class
                bcr = new BarcodeReader();
                //set BarcodeRead event
                bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeRead);
                bcr.symbology.Code39.Enable = true;
                bcr.symbology.Code128.Enable = true;
                bcr.symbology.Interleaved2Of5.Enable = true;
                bcr.symbology.Pdf417.Enable = true;
                bcr.symbologyOptions.Preamble = "";
                bcr.symbologyOptions.Postamble = "";

                lampText.Text = "";
                lampText.BackColor = SystemColors.Window;

                //sends the BarcodeRead event after each successful read
                bcr.ThreadedRead(true);
            }
            catch
            {

                //MessageBox.Show(exp.Message);
            }


            ipTimer.Enabled = true;
            ipTimer.Interval = 1000;
            ipTimer.Tick += new System.EventHandler(ipTimerTick);


            //testTimer.Enabled = true;
            //testTimer.Interval = 10000;
            //testTimer.Tick += new System.EventHandler(testTimerTick);


        }

        void bcr_BarcodeRead(object sender, BarcodeReadEventArgs bre)
        {
            if (lblPos.Text != "")
            {
                if (MessageBox.Show("are you sure?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
            }
            lblInfo.Text = "";
            lblPrevPallette.Text = "";
            lblPN.Text = "";
            lblPos.Text = "";
            //lblMultiPackText.Text = "";
            foreach (Control c in this.Controls)
            {
                if (c is Label && c.Name.StartsWith("lblMP"))//pozicia z design-u x2
                {
                    c.Text = "";
                }
            }

            if (stop)
            {
                lampText.Text = "STOP";
                lampText.BackColor = System.Drawing.Color.OrangeRed;
                lampTimer.Enabled = true;
                NokSound.Volume = Convert.ToInt32(FS.config[FS.audioVolume]);
                NokSound.Play();
                NokSound.Volume = 3;
                return;

            }


            int loopCount = 0;
            while (!tcp.Ping(FS.config[FS.serverIp]))
            {

                loopCount++;
                if (loopCount > 10)
                {
                    MessageBox.Show("network error!! check network");
                    break;
                }


            }

            string displayBc = (bre.strDataBuffer[0] == 'S')? bre.strDataBuffer.Remove(0, 1):bre.strDataBuffer;
            
            //##################################################
            lblBarcode.Text = displayBc;//bre.strDataBuffer;
            //##################################################
            SetText("B:" + bre.strDataBuffer);
            try
            {

                tcpSend.send(bre.strDataBuffer);
                SetText("S@" + DateTime.Now.ToShortTimeString() + ":" + bre.strDataBuffer);

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Console.WriteLine("Error..... " + ex.StackTrace);
            }
        }

        //private void btnExit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (bcr != null)
        //        {
        //            bcr.Dispose();
        //            bcr = null;
        //        }

        //    }
        //    catch (Exception exp)
        //    {
        //        MessageBox.Show(exp.Message);
        //    }

        //    Application.Exit();
        //}

        private void lampTimerTick(object sender, EventArgs e)
        {
            lampTimer.Enabled = false;
            lampText.Text = "";
            lampText.BackColor = SystemColors.Window;

        }
        private void ipTimerTick(object sender, EventArgs e)
        {
            addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            ipTimerCounter++;
            if (addresses[0].ToString() == "127.0.0.1" && ipTimerCounter < 10)
                return;
            ipTimer.Enabled = false;
            ipTimer.Dispose();
            startListener();


        }


        #region tcpListener

        public void startListener()
        {

            FS.readConfig();            

            clientPort = Convert.ToInt32(FS.config[FS.localPort]);
            //btnAcc.Visible = bool.Parse(config[7]);
            int loopCount = 0;
            while (!tcp.Ping(FS.config[FS.serverIp]))
            {

                loopCount++;
                if (loopCount > 10)
                {
                    MessageBox.Show("network error!! restart app");
                    break;
                }


            }

            try
            {

                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
                bw.DoWork += new DoWorkEventHandler(bwDoWork);
                bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            bw.RunWorkerAsync();

        }

        private IPAddress getIp()
        {
            IPAddress[] addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            return addresses[0];
        }

        public delegate void mydelegate();


        private void parseData()
        {
            string pos = null;
            string pn = null;
            string prevPal = null;
            string result = null;
            string multipack = null;
            string info = null;

            //MessageBox.Show("rcvd: " + dataRcvd);

            if (this.InvokeRequired)
            {
                this.Invoke(new mydelegate(parseData));
                return;
            }
            try
            {
                List<string> lines = new List<string>();
                string[] tempLines = dataRcvd.Split('\n');
                //ToList
                foreach (string s in tempLines)
                    lines.Add(s);

                pn = lines[0].Trim();// 1013.0000.000.00 
                pos = lines[1].Trim();// x5 Y41 S2
                prevPal = lines[2].Trim();// 1234567
                result = lines[3].Trim();// "OK"/"NOK"/""
                multipack = lines[4].Trim();//G1234|B2345|
                info = lines[5].Trim();
                if (pos != "")
                    btnAcc.Visible = bool.Parse(FS.config[FS.allowManual]);
                if (multipack.Length != 0)
                {
                    string[] temp = multipack.Split('|');

                    foreach (string s in temp)
                    {
                        try
                        {
                            mpData.Add(new multipackData(s.Substring(1, 4), s.Substring(0, 1) == "G" ? System.Drawing.Color.Lime : System.Drawing.Color.Black));
                        }
                        catch
                        {
                            SetText("multipack data error:" + dataRcvd);

                        }
                    }
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("received data(" + dataRcvd + ") parse error:" + ex.ToString());
            }
            if (result == "OK")
            {
                lampText.Text = "OK";
                lampText.BackColor = System.Drawing.Color.Lime;
                lampTimer.Enabled = true;
                btnAcc.Visible = false;
                lblBarcode.Text = "";
                lblPN.Text = "";
                lblPos.Text = "";
                lblPrevPallette.Text = "";
                lblInfo.Text = "";
                OKSound.Volume = Convert.ToInt32(FS.config[FS.audioVolume]);
                OKSound.Play();
                OKSound.Volume = 3;
            }
            else if (result == "NOK")
            {
                lampText.Text = "NOK";
                lampText.BackColor = System.Drawing.Color.OrangeRed;
                lampTimer.Enabled = true;
                NokSound.Volume = Convert.ToInt32(FS.config[FS.audioVolume]);
                NokSound.Play();
                NokSound.Volume = 3;

            }
            else
            {
                lampText.Text = "";
                lampText.BackColor = SystemColors.Window;
            }

            if (result == "STOP")
            {
                stop = true;
            }
            else
            {
                stop = false;
            }

            if (info == "teamLeader logged in")
                cbLeader.Checked = true;
            else if (info == "clear mode set")
                cbClear.Checked = true;
            else if (info == "clear mode reset")
                cbClear.Checked = false;
            else if (info.EndsWith("clear mode reset, teamLeader logged out") || info == "teamLeader logged out")
            {
                cbClear.Checked = false;
                cbLeader.Checked = false;
            }





            lblPN.Text = pn;
            lblPos.Text = pos;
            lblPrevPallette.Text = prevPal;
            //lblMultiPackText.Text = multipack;
            lblInfo.Text = info;

            foreach (multipackData m in mpData)
            {

            }
            int i = 1;
            foreach (Control c in this.Controls)
            {
                if (c is Label && c.Name.StartsWith("lblMP"))
                {
                    string s = c.Name.Substring(5, 2);
                    i = Convert.ToInt32(s);
                    if (i < mpData.Count)
                    {
                        c.Text = mpData[i].nr;
                        c.ForeColor = mpData[i].color;
                    }
                    else
                    {
                        c.Text = "";
                        c.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
            mpData.Clear();
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                //this.listBox1.Items.Add(text);
                //listBox1.SelectedIndex = listBox1.Items.Count - 1;
                FS.logData(text);

            }
        }

        #region backgroundWorker

        private void bwDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            IPAddress localAdd = getIp();//IPAddress.Parse("192.168.0.150");//SERVER_IP);

            TcpListener listener = new TcpListener(localAdd, clientPort);//IPAddress.Any, PORT_NO);

            //MessageBox.Show("listener start at:" + localAdd + ":" + clientPort.ToString() + "...");
            //Console.WriteLine("listener start at:" + localAdd + ":" + port.ToString() + "...");
            listener.Start();
            SetText("listener started@" + localAdd.ToString() + ":" + clientPort.ToString());

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                //Console.WriteLine("received : " + dataReceived);
                //worker.ReportProgress(dataReceived);
                dataRcvd = dataReceived;
                SetText("R@" + DateTime.Now.ToShortTimeString() + ":" + dataReceived);
                nwStream.Write(buffer, 0, bytesRead);

                nwStream.Close();// otestovať ked bude stale padať
                client.Close();


                parseData();
            }
            //listener.Stop();//unreachable
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                Console.WriteLine("Canceled!");
            }

            else if (!(e.Error == null))
            {
                Console.WriteLine("Error: " + e.Error.Message);
            }

            else
            {
                Console.WriteLine("Done!");
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine(e.ProgressPercentage.ToString() + "%");
        }
        #endregion

        #endregion

        void PowerBatteryStrength_Changed(object sender, ChangeEventArgs args)
        {
            if (SystemState.PowerBatteryStrength.ToString() == "VeryLow")
                lblBat.Text = "<20%";
            else if (SystemState.PowerBatteryStrength.ToString() == "Low")
                lblBat.Text = "40%";
            else if (SystemState.PowerBatteryStrength.ToString() == "Medium")
                lblBat.Text = "60%";
            else if (SystemState.PowerBatteryStrength.ToString() == "High")
                lblBat.Text = "80%";
            else if (SystemState.PowerBatteryStrength.ToString() == "VeryHigh")
                lblBat.Text = "100%";


            //MesssageBox.Show("Current battery strength is " + );
        }

        private void btnAcc_Click(object sender, EventArgs e)
        {
            tcpSend.send("TAccept");
            SetText("S@" + DateTime.Now.ToShortTimeString() + ": accept");

        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            pass pd = new pass();
            setup cd = new setup();
            DialogResult dr = pd.ShowDialog();
            DialogResult res = DialogResult.None;

            if (dr == DialogResult.OK)
            {
                res = cd.ShowDialog();
            }
            if (res == DialogResult.Abort)
            {
                if (bcr != null)
                    bcr.Dispose();
                Application.Exit();

            }


            cd.Dispose();
            pd.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tcpSend.send(textBox1.Text);
            //using (db dbForm = new db())
            //{
            //    dbForm.ShowDialog();
            //}
        }
    }
}