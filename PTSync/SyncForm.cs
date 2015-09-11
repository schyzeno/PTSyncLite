using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PTSync
{
    public partial class SyncForm : Form
    {
        private static int MINIMUM_INTERVAL = 600000;
        SyncController syncController;
       
        public SyncForm()
        {
            InitializeComponent();
        }

        private void SyncForm_Load(object sender, EventArgs e)
        {
            syncController = new SyncController();

            SyncAll(Models.Cycle.Any);
            Application.Exit();
        }
        private void SyncAll(string cycle)
        {
            //SyncAll(Cycle.Interval);
            syncController.DownloadSubscriptions();
            syncController.loadSubscriptions();
            syncController.UploadOHH(cycle);
            syncController.DownloadUpdates(cycle);
            syncController.DownloadConfirmations(cycle);
            syncController.DeleteFiles(cycle);
            syncController.DeleteStartsWith(cycle);
            syncController.RenameFiles(cycle);
            syncController.Download(cycle);
            syncController.UploadMisc(cycle);
            syncController.UploadStartsWith(cycle);
        }


    }
}