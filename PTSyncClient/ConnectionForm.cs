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
    public partial class ConnectionForm : Form
    {
        SyncController syncController;
        public ConnectionForm()
        {
            InitializeComponent();
        }

        private void ConnectionForm_Load(object sender, EventArgs e)
        {
            syncController = new SyncController();
            TextBoxCompanyID.Text = syncController.User.CompanyID;
            TextBoxName.Text = syncController.User.Name;
            TextBoxPassword.Text = syncController.User.Password;
            TextBoxIP.Text = syncController.Settings.IP;
            TextBoxPort.Text = syncController.Settings.Port;
            TextBoxInteval.Text = syncController.Settings.SyncInterval.ToString();
            CheckBoxBackUp.Checked = syncController.Settings.BackupData;

        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            List<User> users = new List<User>();
            List<Settings> settings = new List<Settings>();

            users.Add(new User(TextBoxCompanyID.Text,TextBoxName.Text,TextBoxPassword.Text));
            try
            {
                
                settings.Add(new Settings(TextBoxIP.Text, TextBoxPort.Text, Int32.Parse(TextBoxInteval.Text), CheckBoxBackUp.Checked));
            }
            catch (Exception ex)
            {
                settings.Add(new Settings(TextBoxIP.Text, TextBoxPort.Text, 180000, false));
            }

            XMLHandler.SaveSettings(settings);
            XMLHandler.SaveUser(users);
            this.Close();
        }
       
        
    }
}