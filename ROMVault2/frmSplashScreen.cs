/******************************************************
 *     ROMVault2 is written by Gordon J.              *
 *     Contact gordon@romvault.com                    *
 *     Copyright 2014                                 *
 ******************************************************/

using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;
using ROMVault2.Properties;
using ROMVault2.RVRef;
using ROMVault2.RvDB;

namespace ROMVault2
{
    public partial class FrmSplashScreen : Form
    {
        private double _opacityIncrement = 0.05;

        public FrmSplashScreen()
        {
            InitializeComponent();
            lblVersion.Text = @"Version " + Program.Version + @"." + Program.SubVersion + Resources.FixFiles_FixZip_Colon + Application.StartupPath;
            Opacity = 0;
            timer1.Interval = 50;

            label1.Text = @"Registered to : " + Settings.Username + @"                      Contact Email (" + Settings.EMail + @")";

            bgWork.DoWork += StartUpCode;
            bgWork.ProgressChanged += BgwProgressChanged;
            bgWork.RunWorkerCompleted += BgwRunWorkerCompleted;
        }

        private void FrmSplashScreenShown(object sender, EventArgs e)
        {
            bgWork.RunWorkerAsync(SynchronizationContext.Current);
            timer1.Start();
        }


        private void StartUpCode(object sender, DoWorkEventArgs e)
        {

            BasicHttpBinding b = new BasicHttpBinding { SendTimeout = new TimeSpan(0, 0, 20), ReceiveTimeout = new TimeSpan(0, 0, 20) };
            EndpointAddress ep = new EndpointAddress(@"http://services.romvault.com/Service1.svc");
            Service1Client s = new Service1Client(b, ep);

            if (string.IsNullOrEmpty(Settings.Username) || string.IsNullOrEmpty(Settings.EMail))
            {
                FrmRegistration freg = new FrmRegistration();
                freg.ShowDialog();
                freg.Dispose();
            }
            
            bool isNetworkAvailable = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            string r = "OK";
            try
            {
                if (isNetworkAvailable)
                    r = s.SendUser1(Settings.Username, Settings.EMail, Program.Version, Program.SubVersion);
            }
            catch (Exception)
            {
                r = "OK";
            }

            if (r != "OK")
            {
                Program.ErrorMessage=Resources.Program_Main_You_are_not_Authorised_to_use_this_program;
                return;
            }
            // normal check ends

            string thisV = Program.Version + "." + Program.SubVersion.ToString("0000");
            string v = thisV;
            try
            {
                if (isNetworkAvailable)
                    v = s.GetLatestVersion1(Program.Version, Program.SubVersion);
            }
            catch (Exception)
            {
                v = thisV;
            }
            if (String.Compare(v, thisV, StringComparison.Ordinal) > 0)
            {
                Program.URL = s.GetUpdateLink1(Program.Version, Program.SubVersion);
                Program.ErrorMessage=Resources.Program_Main_There_is_a_new_release_download_now_from + Program.URL;
                return;
            }

            s.Close();

            RepairStatus.InitStatusCheck();

            Settings.SetDefaults();

            DB.Read(sender,e);

        }


        private void BgwProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState == null)
            {
                if (e.ProgressPercentage >= progressBar.Minimum && e.ProgressPercentage <= progressBar.Maximum)
                    progressBar.Value = e.ProgressPercentage;
                return;
            }
            bgwSetRange bgwSr = e.UserState as bgwSetRange;
            if (bgwSr != null)
            {
                progressBar.Minimum = 0;
                progressBar.Maximum = bgwSr.MaxVal;
                progressBar.Value = 0;
                return;
            }

            bgwText bgwT = e.UserState as bgwText;
            if (bgwT != null)
            {
                lblStatus.Text = bgwT.Text;
            }
        }

        private void BgwRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _opacityIncrement = -0.1;
            timer1.Start();
        }

        private void Timer1Tick(object sender, EventArgs e)
        {

            if (_opacityIncrement > 0)
            {
                if (Opacity < 1)
                    Opacity += _opacityIncrement;
                else
                    timer1.Stop();
            }
            else
            {
                if (Opacity > 0)
                    Opacity += _opacityIncrement;
                else
                {
                    timer1.Stop();
                    Close();
                }
            }
        }

    }
}
