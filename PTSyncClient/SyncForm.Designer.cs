namespace PTSyncClient
{
    partial class SyncForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.ButtonSync = new System.Windows.Forms.Button();
            this.ButtonSubscription = new System.Windows.Forms.Button();
            this.UploadButton = new System.Windows.Forms.Button();
            this.ButtonUpdates = new System.Windows.Forms.Button();
            this.ButtonConfirmation = new System.Windows.Forms.Button();
            this.ButtonConnection = new System.Windows.Forms.Button();
            this.ButtonDLSubs = new System.Windows.Forms.Button();
            this.ButtonDLMisc = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.menuItem2);
            this.menuItem1.Text = "Menu";
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "";
            // 
            // ButtonSync
            // 
            this.ButtonSync.Location = new System.Drawing.Point(0, 1);
            this.ButtonSync.Name = "ButtonSync";
            this.ButtonSync.Size = new System.Drawing.Size(139, 47);
            this.ButtonSync.TabIndex = 0;
            this.ButtonSync.Text = "Sync";
            this.ButtonSync.Click += new System.EventHandler(this.ButtonSync_Click);
            // 
            // ButtonSubscription
            // 
            this.ButtonSubscription.Location = new System.Drawing.Point(145, 196);
            this.ButtonSubscription.Name = "ButtonSubscription";
            this.ButtonSubscription.Size = new System.Drawing.Size(92, 20);
            this.ButtonSubscription.TabIndex = 1;
            this.ButtonSubscription.Text = "Subscriptions";
            this.ButtonSubscription.Click += new System.EventHandler(this.ButtonSubscription_Click);
            // 
            // UploadButton
            // 
            this.UploadButton.Location = new System.Drawing.Point(0, 107);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(139, 47);
            this.UploadButton.TabIndex = 2;
            this.UploadButton.Text = "Upload";
            this.UploadButton.Click += new System.EventHandler(this.UploadButton_Click);
            // 
            // ButtonUpdates
            // 
            this.ButtonUpdates.Location = new System.Drawing.Point(0, 150);
            this.ButtonUpdates.Name = "ButtonUpdates";
            this.ButtonUpdates.Size = new System.Drawing.Size(139, 47);
            this.ButtonUpdates.TabIndex = 3;
            this.ButtonUpdates.Text = "Download Updates";
            this.ButtonUpdates.Click += new System.EventHandler(this.ButtonUpdates_Click);
            // 
            // ButtonConfirmation
            // 
            this.ButtonConfirmation.Location = new System.Drawing.Point(0, 196);
            this.ButtonConfirmation.Name = "ButtonConfirmation";
            this.ButtonConfirmation.Size = new System.Drawing.Size(139, 47);
            this.ButtonConfirmation.TabIndex = 4;
            this.ButtonConfirmation.Text = "Download Replenish";
            this.ButtonConfirmation.Click += new System.EventHandler(this.ButtonConfirmation_Click);
            // 
            // ButtonConnection
            // 
            this.ButtonConnection.Location = new System.Drawing.Point(145, 223);
            this.ButtonConnection.Name = "ButtonConnection";
            this.ButtonConnection.Size = new System.Drawing.Size(92, 20);
            this.ButtonConnection.TabIndex = 5;
            this.ButtonConnection.Text = "Connection";
            this.ButtonConnection.Click += new System.EventHandler(this.ButtonConnection_Click);
            // 
            // ButtonDLSubs
            // 
            this.ButtonDLSubs.Location = new System.Drawing.Point(0, 55);
            this.ButtonDLSubs.Name = "ButtonDLSubs";
            this.ButtonDLSubs.Size = new System.Drawing.Size(139, 20);
            this.ButtonDLSubs.TabIndex = 7;
            this.ButtonDLSubs.Text = "DL subscriptions";
            this.ButtonDLSubs.Click += new System.EventHandler(this.ButtonDLSubs_Click);
            // 
            // ButtonDLMisc
            // 
            this.ButtonDLMisc.Location = new System.Drawing.Point(0, 81);
            this.ButtonDLMisc.Name = "ButtonDLMisc";
            this.ButtonDLMisc.Size = new System.Drawing.Size(139, 20);
            this.ButtonDLMisc.TabIndex = 9;
            this.ButtonDLMisc.Text = "DLmisc";
            this.ButtonDLMisc.Click += new System.EventHandler(this.ButtonDLMisc_Click);
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(140, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(100, 184);
            this.listBox1.TabIndex = 11;
            // 
            // SyncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.ButtonDLMisc);
            this.Controls.Add(this.ButtonDLSubs);
            this.Controls.Add(this.ButtonConnection);
            this.Controls.Add(this.ButtonConfirmation);
            this.Controls.Add(this.ButtonUpdates);
            this.Controls.Add(this.UploadButton);
            this.Controls.Add(this.ButtonSubscription);
            this.Controls.Add(this.ButtonSync);
            this.Menu = this.mainMenu1;
            this.Name = "SyncForm";
            this.Text = "PTSync (v0.2.9)";
            this.Load += new System.EventHandler(this.SyncForm_Load);
            this.Activated += new System.EventHandler(this.SyncForm_Activated);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonSync;
        private System.Windows.Forms.Button ButtonSubscription;
        private System.Windows.Forms.Button UploadButton;
        private System.Windows.Forms.Button ButtonUpdates;
        private System.Windows.Forms.Button ButtonConfirmation;
        private System.Windows.Forms.Button ButtonConnection;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.Button ButtonDLSubs;
        private System.Windows.Forms.Button ButtonDLMisc;
        private System.Windows.Forms.ListBox listBox1;
    }
}

