using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace BarcodeReader_ce_CF2
{
    public partial class setup : Form
    {
        public string ip = null;
        public int port = 0;
        fs FS = new fs();
        tcp tcpSend = new tcp();

        public setup()
        {
            InitializeComponent();

        }

        private void tbIgnore_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSendIgnore_Click(object sender, EventArgs e)
        {
            if (tbIgnore.Text.Length != 8)
            {
                if (MessageBox.Show("wrong format, are you sure?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
            }

            tcpSend.send("I" + tbIgnore.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (tbPassword.Text == tbPassword2.Text)
                lblPassError.Visible = false;
            else
                lblPassError.Visible = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (tbPassword.Text == tbPassword2.Text)
                lblPassError.Visible = false;
            else
                lblPassError.Visible = true;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FS.config[FS.serverIp] = tbIP.Text;
            FS.config[FS.serverPort] = tbPort.Text;
            FS.config[FS.localPort] = tbClientPort.Text;
            FS.config[FS.lightTime] = tbTime.Text;
            FS.config[FS.audioVolume] = nudVol.Value.ToString();
            FS.config[FS.allowManual] = cbManAcc.Checked.ToString();
            if (!lblPassError.Visible && tbPassword.Text != "")
                FS.config[FS.userPass] = tbPassword.Text;

            FS.writeConfig(FS.config);
        }

        private void setup_Load(object sender, EventArgs e)
        {
            FS.readConfig();

            ip = FS.config[FS.serverIp];
            port = Convert.ToInt32(FS.config[FS.serverPort]);
            tbIP.Text = ip;
            tbPort.Text = port.ToString();

            tbClientPort.Text = FS.config[FS.localPort];
            IPAddress[] addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            tbClientIP.Text = addresses[0].ToString();
            tbTime.Text = FS.config[FS.lightTime];
            nudVol.Value = Convert.ToInt32(FS.config[FS.audioVolume]);
            if (FS.config[FS.allowManual] != "")
                cbManAcc.Checked = bool.Parse(FS.config[FS.allowManual]);
        }

        private void btnDB_Click(object sender, EventArgs e)
        {
            using (db dbForm = new db())
            {
                dbForm.ShowDialog();
            }

        }
    }
}