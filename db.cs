using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BarcodeReader_ce_CF2
{
    public partial class db : Form
    {
        sql sqldb = new sql();
        tcp tcpSend = new tcp();
        fs FS = new fs();

        List<dbData> gridData = new List<dbData>();

        #region gridDeclaration
        List<TextBox> tbDBRowpn = new List<TextBox>();
        List<TextBox> tbDBRowdt = new List<TextBox>();
        List<CheckBox> tbDBRowcb = new List<CheckBox>();

        #endregion

        public db()
        {
            InitializeComponent();

            FS.readConfig();
            if (FS.getHall == 3)
                nudY.Maximum = 55;
            else
                if (FS.getHall == 2)
                    nudY.Maximum = 41;
                else
                    MessageBox.Show("Wrong hallnumber ("+ nudY+")","Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);


            for (int i = 0; i < 70; i++)//upraviť počet pre viac ako 6pack
            {
                tbDBRowdt.Add(new TextBox());
                tbDBRowcb.Add(new CheckBox());
                tbDBRowpn.Add(new TextBox(){ /*Width = 200*/});

                this.panel1.Controls.Add(tbDBRowcb[i]);
                this.panel1.Controls.Add(tbDBRowdt[i]);
                this.panel1.Controls.Add(tbDBRowpn[i]);

                this.tbDBRowpn[i].Location = new System.Drawing.Point(0, 0 + (i * 38));
                this.tbDBRowpn[i].Name = "tbDBRowpn" + i.ToString();
                this.tbDBRowpn[i].Size = new System.Drawing.Size(155, 46);
                this.tbDBRowpn[i].TabIndex = 1100 + i;
                this.tbDBRowpn[i].Text = "123456789";
                this.tbDBRowpn[i].Visible = false;

                this.tbDBRowdt[i].Location = new System.Drawing.Point(138, 0 + (i * 38));
                this.tbDBRowdt[i].Name = "tbDBRowdt" + i.ToString();
                this.tbDBRowdt[i].Size = new System.Drawing.Size(250, 46);
                this.tbDBRowdt[i].TabIndex = 900 + i;
                this.tbDBRowdt[i].Text = "2016-12-20 23:59:59";
                this.tbDBRowdt[i].Visible = false;

                this.tbDBRowcb[i].Location = new System.Drawing.Point(395, 0 + (i * 38));
                this.tbDBRowcb[i].Name = "checkBox" + i.ToString();
                this.tbDBRowcb[i].Size = new System.Drawing.Size(40, 40);
                this.tbDBRowcb[i].TabIndex = 700 + i;
                this.tbDBRowcb[i].Visible = false;

            }


            tbPalNr.Text = "";
            tbX.Text = "";
            tbY.Text = "";
            tbPN.Text = "";
            tbChannel.Text = "";
            tbPack.Text = "";
            tbZ.Text = "";

            tbDBChannel.Text = "";
            tbDBPack.Text = "";
            tbDBCount.Text = "";
            tbDBPn.Text = "";
            btnDbg.Visible = false;
            tbDbg.Visible = false;

            btnSave.Visible = false;
            btnSaveP.Visible = false;

        }
        private class dbData
        {
            public dbData(int ident, string pnr, DateTime dt)
            {
                this.id = ident;
                this.palletNr = pnr;
                this.FIFODatetime = dt;
            }
            public int id { get; set; }
            //string partNr { get; set; }
            //string partNrFixed { get; set; }
            //int channel { get; set; }
            public string palletNr { get; set; }
            //int posX { get; set; }
            //int posY { get; set; }
            //int posZ { get; set; }
            //int section { get; set; }
            public DateTime FIFODatetime { get; set; }

        }

        private void btnSrch_Click(object sender, EventArgs e)
        {
            tbX.Text = "";
            tbY.Text = "";
            tbPN.Text = "";
            tbChannel.Text = "";
            tbPack.Text = "";
            tbZ.Text = "";

            if (tbPalNr.Text.Length == 7)
                tbPalNr.Text = "0" + tbPalNr.Text;

            try
            {
                Int64 i = Convert.ToInt64(tbPalNr.Text);
                if (tbPalNr.Text.Length != 10 && tbPalNr.Text.Length != 8) {
                    MessageBox.Show("the format of pallet number not valid");
                    FS.logData("btnSrch error:" + tbPalNr.Text + " the format of pallet number not valid");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("not a number");
                FS.logData("btnSrch error:" + tbPalNr.Text + " is not a number");
                return;
            }
            try
            {

                object[] o = sqldb.searchRecord(tbPalNr.Text);
                if (o[0] == null)
                {
                    MessageBox.Show("record not found");
                    FS.logData("data not found:" + tbPalNr.Text);
                    return;
                }
                int row = Convert.ToInt32(o[2]), pack = Convert.ToInt32(o[5]), pos = 0;
                tbX.Text = o[0].ToString();
                tbY.Text = o[1].ToString();
                tbPN.Text = o[3].ToString();
                tbChannel.Text = o[4].ToString();
                tbPack.Text = pack.ToString();
                if (pack > 1)
                {
                    pos = ((row - 1) / pack) + 1;
                }
                else
                    pos = row;
                tbZ.Text = pos.ToString();

            }
            catch (Exception ex)
            {
                FS.logData("searchRecord(" + tbPalNr.Text + ")" + ex.ToString());
            }
            finally
            {

            }

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            gridData.Clear();
            int x = (int)nudX.Value, y = (int)nudY.Value, p = 0;
            object[] id = sqldb.getPos("id", x.ToString(), y.ToString());
            if (id == null)
            {
                MessageBox.Show("no pallet on position");
                return;
            }
            object[] palettNr = sqldb.getPos("paletteNr", x.ToString(), y.ToString());
            object[] dt = sqldb.getPos("FIFODatetime", x.ToString(), y.ToString());

            object[] pos = sqldb.getPos(x.ToString(), y.ToString());
            if (pos == null || pos[0] == null)
            {
                MessageBox.Show("no data for pos");
                return;
            }
            
            try
            {
                p = Convert.ToInt32(pos[2].ToString());
            }
            catch
            {
                p = 1;
            }
            if (p == 0)
                p = 1;

            for (int i = 0; i < tbDBRowpn.Count; i++)
            {

                if (i < id.Length)
                {
                    gridData.Add(new dbData(Convert.ToInt32(id[i]), palettNr[i].ToString(), Convert.ToDateTime(dt[i])));
                    tbDBRowpn[i].Text = gridData[i].palletNr;
                    tbDBRowdt[i].Text = gridData[i].FIFODatetime.ToString("yyyy-MM-dd HH:mm:ss");
                    tbDBRowcb[i].Visible = true;
                    tbDBRowpn[i].Visible = true;
                    tbDBRowdt[i].Visible = true;
                    if ((i % p) == 0)
                    {
                        tbDBRowpn[i].ForeColor = SystemColors.Highlight;
                        tbDBRowdt[i].ForeColor = SystemColors.Highlight;
                    }
                    else
                    {
                        tbDBRowpn[i].ForeColor = SystemColors.ControlText;
                        tbDBRowdt[i].ForeColor = SystemColors.ControlText;
                    }
                }
                else
                {
                    tbDBRowcb[i].Visible = false;
                    tbDBRowpn[i].Visible = false;
                    tbDBRowdt[i].Visible = false;
                }
            }

            nudX.ForeColor = System.Drawing.SystemColors.WindowText;
            nudY.ForeColor = System.Drawing.SystemColors.WindowText;
            btnSave.Visible = true;
        }

        private void btnClr_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < tbDBRowpn.Count; i++)
            {

                if (tbDBRowcb[i].Checked)
                {
                    sqldb.clearRecord(gridData[i].palletNr);
                    FS.logData("clearRecord(" + gridData[i].palletNr + ")");
                    tbDBRowcb[i].Checked = false;

                }
            }
            btnShow_Click(null, null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < tbDBRowpn.Count; i++)
            {

                if (tbDBRowcb[i].Checked)
                {
                    sqldb.updateRecordByPalletNr(gridData[i].palletNr, tbDBRowpn[i].Text, tbDBRowdt[i].Text);
                    FS.logData("updateRecord(" + gridData[i].palletNr + "," + tbDBRowpn[i].Text + "," + tbDBRowdt[i].Text + ")");
                    tbDBRowcb[i].Checked = false;

                }
            }
            
            btnShow_Click(null, null);
        }

        private void nudX_ValueChanged(object sender, EventArgs e)
        {
            nudX.ForeColor = Color.Red;
        }

        private void nudY_ValueChanged(object sender, EventArgs e)
        {
            nudY.ForeColor = Color.Red;
        }

        private void tbPalNr_TextChanged(object sender, EventArgs e)
        {
            if (tbPalNr.Text == "nasask")
            {
                btnDbg.Visible = true;
                tbDbg.Visible = true;
                FS.logData("debugEnabled");

            }
            else
            {
                btnDbg.Visible = false;
                tbDbg.Visible = false;
            }
        }

        private void btnDbg_Click(object sender, EventArgs e)
        {

            tcpSend.send(tbDbg.Text);
            FS.logData("sent:" + tbDbg.Text);
        }

        private void nudXP_ValueChanged(object sender, EventArgs e)
        {
            nudXP.ForeColor = Color.Red;
        }

        private void nudYP_ValueChanged(object sender, EventArgs e)
        {
            nudYP.ForeColor = Color.Red;
        }

        private void btnShowP_Click(object sender, EventArgs e)
        {
            int x = (int)nudXP.Value, y = (int)nudYP.Value, p = 0, count = 0;

            object[] pos = sqldb.getPos(x.ToString(), y.ToString());
            if (pos == null || pos[0] == null)
            {
                MessageBox.Show("no data for pos");
                return;
            }
            tbDBPn.Text = pos[0].ToString().Trim();
            tbDBChannel.Text = pos[1].ToString();
            tbDBPack.Text = pos[2].ToString();
            
            //cbAgeCheck.Checked = Convert.ToBoolean(pos[4]);
            //tbAgeLimit.Text = pos[3].ToString();
            //tbAgeBypassTime.Text = pos[5].ToString();

            try
            {
                p = Convert.ToInt32(tbDBPack.Text);
                count = Convert.ToInt32(pos[8].ToString());
            }
            catch
            {
                p = 1;
                count = 6;
            }
            if (p == 0)
                p = 1;


            tbDBCount.Text = (count / p).ToString();
            

            nudXP.ForeColor = System.Drawing.SystemColors.WindowText;
            nudYP.ForeColor = System.Drawing.SystemColors.WindowText;
            btnSaveP.Visible = true;
        }

        private void btnSaveP_Click(object sender, EventArgs e)
        {
            if (nudXP.ForeColor == SystemColors.WindowText && nudYP.ForeColor == SystemColors.WindowText)
            {
                try
                {
                    int p = Convert.ToInt32(tbDBPack.Text);
                    int count = Convert.ToInt32(tbDBCount.Text);
                    sqldb.updatePosByPos(nudXP.Value.ToString(), nudYP.Value.ToString(), tbDBPn.Text, tbDBChannel.Text, tbDBPack.Text, (count * p).ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("save Failed!\nukladanie zlyhalo\n" + ex.ToString());
                }


            }
            else 
            {
                MessageBox.Show("position changed, can not save!\npozícia zmenená zmeny sa neuložia\n" + "");
            }
        }

        private void btnSetBP_Click(object sender, EventArgs e)
        {
            sqldb.alBPUpdate(cbPN.Text, "True");
            MessageBox.Show("bypass enabled");
        }

        private void btnClearBP_Click(object sender, EventArgs e)
        {
            sqldb.alBPUpdate(cbPN.Text, "False");
            MessageBox.Show("bypass disabled");
        }

        private void btnCheckBP_Click(object sender, EventArgs e)
        {
            string rv = sqldb.aLBPRemainingTime(cbPN.Text);
            if (rv == null)
                MessageBox.Show("bypass disabled");
            else
                MessageBox.Show("remaining time: " + rv + " minutes");
        }

        private void tpPos_GotFocus(object sender, EventArgs e)
        {
            //load pn list from db
            //assign pn list to combobox
            //set/reset/check data where pn = combobox.value
        }

        private void setPartsData()
        {
            string[] parts = sqldb.getPnList();
            List<string> partsList = new List<string>();
            foreach (string part in parts)
            {
                string[] partSplit = part.Split(new char[] { ',', '.' });
                if (partSplit.Length > 1 && !partsList.Contains(partSplit[0]))
                {
                    partsList.Add(partSplit[0]);
                }
            }
            foreach(string part in parts)
            {
                partsList.Add(part);
            }
            cbPN.DataSource = partsList;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            setPartsData();
        }

        private void tpAge_GotFocus(object sender, EventArgs e)
        {
            string[] parts = sqldb.getPnList();
            cbPN.DataSource = parts;
        }

        private void btnASave_Click(object sender, EventArgs e)
        {
            sqldb.updatePosAgeByPn(cbPN.Text, tbAgeLimit.Text, cbAgeCheck.Checked, tbAgeBypassTime.Text);
        }

        private void db_Load(object sender, EventArgs e)
        {
            setPartsData();
            cbPN_SelectedIndexChanged(null, null);
        }

        private void cbPN_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                object[] pos = sqldb.getPosAge(cbPN.Text);
                cbAgeCheck.Checked = Convert.ToBoolean(pos[2]);
                tbAgeLimit.Text = pos[1].ToString();
                tbAgeBypassTime.Text = pos[0].ToString();

            }
            catch (Exception ex) 
            {
                FS.logData("cbPN_SelectedIndexChanged error:" + cbPN.Text + " ");
 
            }
            
        }

        


    }
}