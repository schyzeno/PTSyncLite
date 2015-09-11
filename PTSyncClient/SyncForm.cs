using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PTSyncClient.Models;

namespace PTSyncClient
{
    public partial class SyncForm : Form
    {
        private static int MINIMUM_INTERVAL = 600000;
        private static string DEFAULT_COMPLETE_STATEMENT = "Complete!\n";
        SyncController syncController;
        List<string> subscriptions = new List<string>();
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
            XMLHandler.CreateMissingDirectories();
            syncController = new SyncController();
        }


        private void SyncForm_Activated(object sender, EventArgs e)
        {
            syncController.loadSettings();
            syncController.loadUser();
            CycleTimer.Enabled = true;
            CycleTimer.Interval = syncController.Settings.SyncInterval;
        }
        
        private void ButtonSync_Click(object sender, EventArgs e)
        {
            SyncAllWithStatus(Cycle.Any);
        }

        private void SyncAllWithStatus(string cycle)
        {
            List<SyncOperation> operations = new List<SyncOperation>();
            operations.Add(new SyncOperation(syncController.DownloadSubscriptions, "Downloading Subs...", DEFAULT_COMPLETE_STATEMENT));
            operations.Add(new SyncOperation(syncController.loadSubscriptions, "Loading Subs...", DEFAULT_COMPLETE_STATEMENT));
            operations.Add(new SyncOperation(new Action(delegate() { syncController.UploadOHH(cycle); }), "Uploading Orders...", DEFAULT_COMPLETE_STATEMENT));
            operations.Add(new SyncOperation(new Action(delegate() { syncController.DownloadUpdates(cycle); }), "Downloading Updates...", DEFAULT_COMPLETE_STATEMENT));
            operations.Add(new SyncOperation(new Action(delegate() { syncController.DownloadConfirmations(cycle); }), "Downloading Confirmations...", DEFAULT_COMPLETE_STATEMENT));
            operations.Add(new SyncOperation(new Action(delegate() { syncController.DeleteFiles(cycle); }), "Deleting Files...", DEFAULT_COMPLETE_STATEMENT));
            operations.Add(new SyncOperation(new Action(delegate() { syncController.DeleteStartsWith(cycle); }), "Del Files Starts W/...", DEFAULT_COMPLETE_STATEMENT));
            operations.Add(new SyncOperation(new Action(delegate() { syncController.RenameFiles(cycle); }), "Renaming Files...", DEFAULT_COMPLETE_STATEMENT));
            operations.Add(new SyncOperation(new Action(delegate() { syncController.Download(cycle); }), "Downloading Misc...", DEFAULT_COMPLETE_STATEMENT));
            operations.Add(new SyncOperation(new Action(delegate() { syncController.UploadMisc(cycle); }), "Uploading Misc...", DEFAULT_COMPLETE_STATEMENT));
            operations.Add(new SyncOperation(new Action(delegate() { syncController.UploadStartsWith(cycle); }), "Uploading Startswith...", DEFAULT_COMPLETE_STATEMENT));
            ActionWithStatus(operations);
            RefreshSubscriptionList();
        }


        private void ButtonDLSubs_Click(object sender, EventArgs e)
        {
            List<SyncOperation> ops = new List<SyncOperation>();
            ops.Add(new SyncOperation(syncController.DownloadSubscriptions, "Downloading Subs...", DEFAULT_COMPLETE_STATEMENT));
            ops.Add(new SyncOperation(syncController.loadSubscriptions, "Loading Subs...", DEFAULT_COMPLETE_STATEMENT));
            ActionWithStatus(ops);
            RefreshSubscriptionList();
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            List<SyncOperation> ops = new List<SyncOperation>();
            ops.Add(new SyncOperation(new Action(delegate() { syncController.UploadOHH(Cycle.Any); }), "Uploading Orders...", DEFAULT_COMPLETE_STATEMENT));
            ops.Add(new SyncOperation(new Action(delegate() { syncController.UploadMisc(Cycle.Any); }), "Uploading Misc...", DEFAULT_COMPLETE_STATEMENT));
            ops.Add(new SyncOperation(new Action(delegate() { syncController.UploadStartsWith(Cycle.Any); }), "Uploading Startswith...", DEFAULT_COMPLETE_STATEMENT));
            ActionWithStatus(ops);
        }

        private void ButtonUpdates_Click(object sender, EventArgs e)
        {
            ActionWithStatus(new SyncOperation(new Action(delegate() { syncController.DownloadUpdates(Cycle.Any); }), "Downloading Updates...", DEFAULT_COMPLETE_STATEMENT));
        }


        private void ButtonConfirmation_Click(object sender, EventArgs e)
        {
            ActionWithStatus(new SyncOperation(new Action(delegate() { syncController.DownloadConfirmations(Cycle.Any); }), "Downloading Confirmations...", DEFAULT_COMPLETE_STATEMENT));
        }

        private void ButtonDLMisc_Click(object sender, EventArgs e)
        {
            ActionWithStatus(new SyncOperation(new Action(delegate() { syncController.Download(Cycle.Any); }), "Downloading Misc...", DEFAULT_COMPLETE_STATEMENT));
        }

        private void ButtonUpMisc_Click(object sender, EventArgs e)
        {
            ActionWithStatus(new SyncOperation(new Action(delegate() { syncController.UploadMisc(Cycle.Any); }), "Uploading Misc...", DEFAULT_COMPLETE_STATEMENT));
        }        

        private void ButtonSubscription_Click(object sender, EventArgs e)
        {
            MessageBox.Show(syncController.User.Name + " \n" +
                            syncController.User.Password + " \n" +
                            syncController.Settings.IP + " \n" +
                            syncController.Settings.Port);
            RefreshSubscriptionList();
        }

        private void RefreshSubscriptionList()
        {
            subscriptions.Clear();
            foreach (Models.Subscription subscription in syncController.Subscriptions)
            {
                subscriptions.Add(subscription.Name);
            }
            listBox1.DataSource = null;
            listBox1.DataSource = subscriptions;
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

        private void CycleTimer_Tick(object sender, EventArgs e)
        {
            SyncAll(Cycle.Interval);
            if (syncController.isDailySync())
                SyncAll(Cycle.Daily);
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