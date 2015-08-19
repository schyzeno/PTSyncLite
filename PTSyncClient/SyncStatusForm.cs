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
    public partial class SyncStatusForm : Form
    {
        public SyncStatusForm()
        {
            InitializeComponent();
        }

        private void SyncStatusForm_Load(object sender, EventArgs e)
        {

        }

        public void updateText(string text)
        {
            StatusWindow.Text = text;
        }

        public void appendText(string text)
        {
            StatusWindow.Text += text;
        }

        public void clearText()
        {
            StatusWindow.Text = "";
        }


    }
}