namespace PTSyncClient
{
    partial class ConnectionForm
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
            this.TextBoxName = new System.Windows.Forms.TextBox();
            this.TextBoxPassword = new System.Windows.Forms.TextBox();
            this.TextBoxIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TextBoxPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ButtonSave = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.TextBoxCompanyID = new System.Windows.Forms.TextBox();
            this.TextBoxInteval = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.CheckBoxBackUp = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // TextBoxName
            // 
            this.TextBoxName.Location = new System.Drawing.Point(73, 44);
            this.TextBoxName.Name = "TextBoxName";
            this.TextBoxName.Size = new System.Drawing.Size(148, 21);
            this.TextBoxName.TabIndex = 0;
            // 
            // TextBoxPassword
            // 
            this.TextBoxPassword.Location = new System.Drawing.Point(73, 71);
            this.TextBoxPassword.Name = "TextBoxPassword";
            this.TextBoxPassword.Size = new System.Drawing.Size(148, 21);
            this.TextBoxPassword.TabIndex = 1;
            // 
            // TextBoxIP
            // 
            this.TextBoxIP.Location = new System.Drawing.Point(73, 132);
            this.TextBoxIP.Name = "TextBoxIP";
            this.TextBoxIP.Size = new System.Drawing.Size(148, 21);
            this.TextBoxIP.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.Text = "User";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 20);
            this.label2.Text = "Password";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 20);
            this.label3.Text = "IP";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 20);
            this.label4.Text = "Port";
            // 
            // TextBoxPort
            // 
            this.TextBoxPort.Location = new System.Drawing.Point(73, 161);
            this.TextBoxPort.Name = "TextBoxPort";
            this.TextBoxPort.Size = new System.Drawing.Size(148, 21);
            this.TextBoxPort.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(217, 20);
            this.label5.Text = "__________________________________";
            // 
            // ButtonSave
            // 
            this.ButtonSave.Location = new System.Drawing.Point(149, 245);
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(72, 20);
            this.ButtonSave.TabIndex = 11;
            this.ButtonSave.Text = "Save";
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(4, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 20);
            this.label6.Text = "Company ID";
            // 
            // TextBoxCompanyID
            // 
            this.TextBoxCompanyID.Location = new System.Drawing.Point(149, 17);
            this.TextBoxCompanyID.Name = "TextBoxCompanyID";
            this.TextBoxCompanyID.Size = new System.Drawing.Size(72, 21);
            this.TextBoxCompanyID.TabIndex = 19;
            // 
            // TextBoxInteval
            // 
            this.TextBoxInteval.Location = new System.Drawing.Point(73, 188);
            this.TextBoxInteval.Name = "TextBoxInteval";
            this.TextBoxInteval.Size = new System.Drawing.Size(148, 21);
            this.TextBoxInteval.TabIndex = 26;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(4, 188);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 20);
            this.label7.Text = "Interval";
            // 
            // CheckBoxBackUp
            // 
            this.CheckBoxBackUp.Location = new System.Drawing.Point(120, 219);
            this.CheckBoxBackUp.Name = "CheckBoxBackUp";
            this.CheckBoxBackUp.Size = new System.Drawing.Size(100, 20);
            this.CheckBoxBackUp.TabIndex = 29;
            this.CheckBoxBackUp.Text = "Backup Data";
            // 
            // ConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.CheckBoxBackUp);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.TextBoxInteval);
            this.Controls.Add(this.TextBoxCompanyID);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ButtonSave);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TextBoxPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBoxIP);
            this.Controls.Add(this.TextBoxPassword);
            this.Controls.Add(this.TextBoxName);
            this.Menu = this.mainMenu1;
            this.Name = "ConnectionForm";
            this.Text = "Connection";
            this.Load += new System.EventHandler(this.ConnectionForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxName;
        private System.Windows.Forms.TextBox TextBoxPassword;
        private System.Windows.Forms.TextBox TextBoxIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TextBoxPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ButtonSave;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TextBoxCompanyID;
        private System.Windows.Forms.TextBox TextBoxInteval;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox CheckBoxBackUp;
    }
}