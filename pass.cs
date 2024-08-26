using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BarcodeReader_ce_CF2
{
    public partial class pass : Form
    {
        fs FS = new fs();
        public pass()
        {
            InitializeComponent();

            FS.readConfig();

        }

        private void tbPass_TextChanged(object sender, EventArgs e)
        {
            if (tbPass.Text == FS.config[FS.userPass] || tbPass.Text == FS.config[FS.adminPass])
                btnOK.DialogResult = DialogResult.OK;

            else
                btnOK.DialogResult = DialogResult.None;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!(tbPass.Text == FS.config[FS.userPass] || tbPass.Text == FS.config[FS.adminPass]))
            {
                MessageBox.Show("wrong password");

            }
        }

        private void pass_Load(object sender, EventArgs e)
        {
            tbPass.Focus();
        }
    }
}