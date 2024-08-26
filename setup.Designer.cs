namespace BarcodeReader_ce_CF2
{
    partial class setup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        //private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnDB = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tbIgnore = new System.Windows.Forms.TextBox();
            this.btnSendIgnore = new System.Windows.Forms.Button();
            this.cbManAcc = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.nudVol = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbTime = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.lblPassError = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbPassword2 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbClientIP = new System.Windows.Forms.TextBox();
            this.tbClientPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDB
            // 
            this.btnDB.Location = new System.Drawing.Point(139, 317);
            this.btnDB.Name = "btnDB";
            this.btnDB.Size = new System.Drawing.Size(88, 27);
            this.btnDB.TabIndex = 109;
            this.btnDB.Text = "dbAccess";
            this.btnDB.Click += new System.EventHandler(this.btnDB_Click);
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(11, 245);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(107, 20);
            this.label15.Text = "add to ignore list";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(11, 271);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(23, 13);
            this.label14.Text = "No";
            // 
            // tbIgnore
            // 
            this.tbIgnore.Location = new System.Drawing.Point(32, 268);
            this.tbIgnore.Name = "tbIgnore";
            this.tbIgnore.Size = new System.Drawing.Size(101, 23);
            this.tbIgnore.TabIndex = 104;
            this.tbIgnore.TextChanged += new System.EventHandler(this.tbIgnore_TextChanged);
            // 
            // btnSendIgnore
            // 
            this.btnSendIgnore.Location = new System.Drawing.Point(139, 264);
            this.btnSendIgnore.Name = "btnSendIgnore";
            this.btnSendIgnore.Size = new System.Drawing.Size(88, 27);
            this.btnSendIgnore.TabIndex = 105;
            this.btnSendIgnore.Text = "send";
            this.btnSendIgnore.Click += new System.EventHandler(this.btnSendIgnore_Click);
            // 
            // cbManAcc
            // 
            this.cbManAcc.Location = new System.Drawing.Point(14, 185);
            this.cbManAcc.Name = "cbManAcc";
            this.cbManAcc.Size = new System.Drawing.Size(147, 17);
            this.cbManAcc.TabIndex = 103;
            this.cbManAcc.Text = "allow manual accepting";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(8, 294);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(219, 20);
            this.label13.Text = "restart app for changes to take effect";
            // 
            // nudVol
            // 
            this.nudVol.Location = new System.Drawing.Point(144, 113);
            this.nudVol.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudVol.Name = "nudVol";
            this.nudVol.Size = new System.Drawing.Size(50, 24);
            this.nudVol.TabIndex = 100;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(99, 116);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 13);
            this.label10.Text = "volume";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(11, 116);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 13);
            this.label11.Text = "time";
            // 
            // tbTime
            // 
            this.tbTime.Location = new System.Drawing.Point(42, 113);
            this.tbTime.Name = "tbTime";
            this.tbTime.Size = new System.Drawing.Size(50, 23);
            this.tbTime.TabIndex = 99;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(11, 90);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(48, 20);
            this.label12.Text = "result";
            // 
            // lblPassError
            // 
            this.lblPassError.ForeColor = System.Drawing.Color.Red;
            this.lblPassError.Location = new System.Drawing.Point(108, 136);
            this.lblPassError.Name = "lblPassError";
            this.lblPassError.Size = new System.Drawing.Size(122, 20);
            this.lblPassError.Text = "passphrase not identical";
            this.lblPassError.Visible = false;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(42, 159);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(50, 23);
            this.tbPassword.TabIndex = 101;
            this.tbPassword.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(99, 162);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 17);
            this.label7.Text = "new";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(11, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 17);
            this.label8.Text = "new";
            // 
            // tbPassword2
            // 
            this.tbPassword2.Location = new System.Drawing.Point(144, 159);
            this.tbPassword2.Name = "tbPassword2";
            this.tbPassword2.PasswordChar = '*';
            this.tbPassword2.Size = new System.Drawing.Size(50, 23);
            this.tbPassword2.TabIndex = 102;
            this.tbPassword2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(11, 136);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(107, 20);
            this.label9.Text = "change password";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(122, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.Text = "port";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(11, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.Text = "ip";
            // 
            // tbClientIP
            // 
            this.tbClientIP.Enabled = false;
            this.tbClientIP.Location = new System.Drawing.Point(32, 67);
            this.tbClientIP.Name = "tbClientIP";
            this.tbClientIP.Size = new System.Drawing.Size(79, 23);
            this.tbClientIP.TabIndex = 97;
            // 
            // tbClientPort
            // 
            this.tbClientPort.Location = new System.Drawing.Point(153, 67);
            this.tbClientPort.Name = "tbClientPort";
            this.tbClientPort.Size = new System.Drawing.Size(42, 23);
            this.tbClientPort.TabIndex = 98;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(11, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 20);
            this.label5.Text = "client";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(11, -2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 20);
            this.label4.Text = "server";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(122, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.Text = "port";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.Text = "ip";
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(32, 21);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(79, 23);
            this.tbIP.TabIndex = 95;
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(153, 21);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(42, 23);
            this.tbPort.TabIndex = 96;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(11, 317);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(88, 27);
            this.btnExit.TabIndex = 106;
            this.btnExit.Text = "quit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(11, 350);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 27);
            this.button2.TabIndex = 107;
            this.button2.Text = "cancel";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(139, 350);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 27);
            this.btnSave.TabIndex = 108;
            this.btnSave.Text = "save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(638, 455);
            this.Controls.Add(this.btnDB);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.tbIgnore);
            this.Controls.Add(this.btnSendIgnore);
            this.Controls.Add(this.cbManAcc);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.nudVol);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tbTime);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblPassError);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbPassword2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbClientIP);
            this.Controls.Add(this.tbClientPort);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbIP);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "setup";
            this.Text = "setup";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.setup_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDB;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbIgnore;
        private System.Windows.Forms.Button btnSendIgnore;
        private System.Windows.Forms.CheckBox cbManAcc;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nudVol;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbTime;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblPassError;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbPassword2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbClientIP;
        private System.Windows.Forms.TextBox tbClientPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnSave;

    }
}