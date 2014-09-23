/******************************************************
 *     ROMVault2 is written by Gordon J.              *
 *     Contact gordon@romvault.com                    *
 *     Copyright 2014                                 *
 ******************************************************/

using System;
using System.Windows.Forms;

namespace ROMVault2
{
    public partial class frmHelpAbout : Form
    {
        public frmHelpAbout()
        {

            InitializeComponent();
            Text = "Version "+Program.Version+"." + Program.SubVersion + " : " + Application.StartupPath;
            lblVersion.Text = "Version "+Program.Version+"." + Program.SubVersion;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            string url = "http://www.romvault.com/";
            System.Diagnostics.Process.Start(url);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "mailto:support@romvault.com?subject=Support " + Program.Version + "." + Program.SubVersion;
                System.Diagnostics.Process.Start(url);
            }
            catch
            { }
        }

    }
}
