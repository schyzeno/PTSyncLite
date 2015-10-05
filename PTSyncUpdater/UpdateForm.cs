using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PTSyncUpdater
{
    public partial class UpdateForm : Form
    {
        private int START_TIME = 30;
        private static string UPDATE_SCRIPT_DIRECTORY = @"\Program Files\PTSyncAll\UpdateProTrack.exe";
        private static string UPDATING_MESSAGE = "Closing ProTrack";
        private static string CONTINUE_MESSAGE = "Press Ok To Continue Updating...";
        public UpdateForm()
        {
            InitializeComponent();
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            countDown.Enabled = true;
            textBox1.Text = UPDATING_MESSAGE;
            button1.Enabled = false;
        }

        private void countDown_Tick(object sender, EventArgs e)
        {
            TimerLabel.Text = "" +(--START_TIME);
            if (START_TIME == 0)
            {
                button1.Enabled = true;
                button1.Focus();
                textBox1.Text = CONTINUE_MESSAGE;
                countDown.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(UPDATE_SCRIPT_DIRECTORY, "");
            Application.Exit();
        }
    }
}