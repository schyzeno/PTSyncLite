using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PTSyncClient
{
    public partial class SyncForm : Form
    {
        private static int MINIMUM_INTERVAL = 600000;
        private static string DEFAULT_COMPLETE_STATEMENT = "Complete!\n";
        SyncController syncController;
        class SyncOperation{
            public Action SyncAction{get;set;}
            public string BeginText { get; set; }
            public string EndText { get; set; }
            public SyncOperation() { }
            public SyncOperation(Action action, string begin, string end)
            {
                SyncAction = action;
                BeginText = begin;
                EndText = end;
            }
        }
       
        public SyncForm()
        {
            InitializeComponent();
        }

        private void SyncForm_Load(object sender, EventArgs e)
        {
            //new SetUpController().firstTimeSetup();
            syncController = new SyncController();
        }


        private void SyncForm_Activated(object sender, EventArgs e)
        {
            syncController.loadSettings();
            syncController.loadUser();
        }


        private void ButtonSync_Click(object sender, EventArgs e)
        {
            List<SyncOperation> ops = new List<SyncOperation>();
            ops.Add(new SyncOperation(syncController.DownloadSubscriptions, "Downloading Subs...", DEFAULT_COMPLETE_STATEMENT));
            ops.Add(new SyncOperation(syncController.loadSubscriptions, "Loading Subs...", DEFAULT_COMPLETE_STATEMENT));
            ops.Add(new SyncOperation(syncController.UploadOHH, "Uploading Orders...", DEFAULT_COMPLETE_STATEMENT));
            ops.Add(new SyncOperation(syncController.DownloadUpdates, "Downloading Updates...", DEFAULT_COMPLETE_STATEMENT));
            ops.Add(new SyncOperation(syncController.DownloadConfirmations, "Downloading Confirmations...", DEFAULT_COMPLETE_STATEMENT));
            ops.Add(new SyncOperation(syncController.DeleteFiles, "Deleting Files...", DEFAULT_COMPLETE_STATEMENT));
            ops.Add(new SyncOperation(syncController.RenameFiles, "Renaming Files...", DEFAULT_COMPLETE_STATEMENT));
            ops.Add(new SyncOperation(syncController.Download, "Downloading Misc...", DEFAULT_COMPLETE_STATEMENT));
            ops.Add(new SyncOperation(syncController.UploadMisc, "Uploading Misc...", DEFAULT_COMPLETE_STATEMENT));
            ops.Add(new SyncOperation(syncController.UploadStartsWith, "Uploading Startswith...", DEFAULT_COMPLETE_STATEMENT));
            ActionWithStatus(ops);
        }


        private void ButtonDLSubs_Click(object sender, EventArgs e)
        {
            List<SyncOperation> ops = new List<SyncOperation>();
            ops.Add(new SyncOperation(syncController.DownloadSubscriptions, "Downloading Subs...", DEFAULT_COMPLETE_STATEMENT));
            ops.Add(new SyncOperation(syncController.loadSubscriptions, "Loading Subs...", DEFAULT_COMPLETE_STATEMENT));
            ActionWithStatus(ops);
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            ActionWithStatus(new SyncOperation(syncController.UploadOHH, "Uploading Orders...", DEFAULT_COMPLETE_STATEMENT));
        }

        private void ButtonUpdates_Click(object sender, EventArgs e)
        {
            ActionWithStatus(new SyncOperation(syncController.DownloadUpdates, "Downloading Updates...", DEFAULT_COMPLETE_STATEMENT));
        }


        private void ButtonConfirmation_Click(object sender, EventArgs e)
        {
            ActionWithStatus(new SyncOperation(syncController.DownloadConfirmations, "Downloading Confirmations...", DEFAULT_COMPLETE_STATEMENT));
        }

        private void ButtonDLMisc_Click(object sender, EventArgs e)
        {
            ActionWithStatus(new SyncOperation(syncController.Download, "Downloading Misc...", DEFAULT_COMPLETE_STATEMENT));
        }

        private void ButtonUpMisc_Click(object sender, EventArgs e)
        {
            ActionWithStatus(new SyncOperation(syncController.UploadMisc, "Uploading Misc...", DEFAULT_COMPLETE_STATEMENT));
        }        

        private void SyncTimer_Tick(object sender, EventArgs e)
        {
            if (syncController.Settings.SyncInterval > MINIMUM_INTERVAL)
                SyncTimer.Interval = syncController.Settings.SyncInterval;
            else
                SyncTimer.Interval = MINIMUM_INTERVAL;
            syncController.IsSyncing = true;

            syncController.IsSyncing = false;


        }

        private void ButtonSubscription_Click(object sender, EventArgs e)
        {
            MessageBox.Show(syncController.User.Name + " \n" +
                            syncController.User.Password + " \n" +
                            syncController.Settings.IP + " \n" +
                            syncController.Settings.Port);
            StringBuilder sb = new StringBuilder();
            foreach (Models.Subscription subscription in syncController.Subscriptions)
            {
                sb.Append(subscription.Name + "\n");
            }
            label1.Text = sb.ToString();
        }


        private void ButtonConnection_Click(object sender, EventArgs e)
        {
            ConnectionForm connectionForm = new ConnectionForm();
            connectionForm.Show();
        }


        private void ActionWithStatus(List<SyncOperation> operations)
        {
            SyncStatusForm syncStatusForm = new SyncStatusForm();
            syncStatusForm.Show();
            this.Hide();
            foreach (SyncOperation operation in operations)
            {
                syncStatusForm.appendText(operation.BeginText); syncStatusForm.Update(); syncStatusForm.Refresh();
                operation.SyncAction();
                syncStatusForm.appendText(operation.EndText); syncStatusForm.Update(); syncStatusForm.Refresh();
            }
            syncStatusForm.clearText();
            syncStatusForm.Close();
            this.Show();
        }

        private void ActionWithStatus(SyncOperation operation)
        {
            List<SyncOperation> ops = new List<SyncOperation>();
            ops.Add(operation);
            ActionWithStatus(ops);
        }

    }
}