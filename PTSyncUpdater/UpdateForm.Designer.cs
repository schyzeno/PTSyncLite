namespace PTSyncUpdater
{
    partial class UpdateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.TimerLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.countDown = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // TimerLabel
            // 
            this.TimerLabel.Font = new System.Drawing.Font("Courier New", 72F, System.Drawing.FontStyle.Bold);
            this.TimerLabel.Location = new System.Drawing.Point(3, 39);
            this.TimerLabel.Name = "TimerLabel";
            this.TimerLabel.Size = new System.Drawing.Size(234, 146);
            this.TimerLabel.Text = "00";
            this.TimerLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 26F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(58, 228);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 50);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textBox1.Location = new System.Drawing.Point(3, 201);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(234, 21);
            this.textBox1.TabIndex = 2;
            // 
            // countDown
            // 
            this.countDown.Interval = 1000;
            this.countDown.Tick += new System.EventHandler(this.countDown_Tick);
            // 
            // UpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TimerLabel);
            this.Name = "UpdateForm";
            this.Text = "PTUpdater (v1.0)";
            this.Load += new System.EventHandler(this.UpdateForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label TimerLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Timer countDown;
    }
}

