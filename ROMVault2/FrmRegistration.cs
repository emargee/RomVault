/******************************************************
 *     ROMVault2 is written by Gordon J.              *
 *     Contact gordon@romvault.com                    *
 *     Copyright 2014                                 *
 ******************************************************/

using System;
using System.Windows.Forms;

namespace ROMVault2
{
    public partial class FrmRegistration : Form
    {
        public FrmRegistration()
        {
            InitializeComponent();
            txtName.Text = Settings.Username;
            txtEmail.Text = Settings.EMail;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Settings.Username = txtName.Text;
            Settings.EMail = txtEmail.Text;
            Close();
        }
    }
}
