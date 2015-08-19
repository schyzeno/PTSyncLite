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

            syncController.DownloadSubscriptions();
            syncController.loadSubscriptions();
            syncController.UploadOHH();
            syncController.DownloadUpdates();
            syncController.DownloadConfirmations();
            syncController.DeleteFiles();
            syncController.RenameFiles();
            syncController.Download();
            syncController.UploadMisc();
            syncController.UploadStartsWith();
            Application.Exit();
        }       


    }
}