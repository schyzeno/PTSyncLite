using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PTSync.Models;
using System.IO;

namespace PTSync
{
    class SetUpController
    {
        private static string SETUP_PATH = @"\PTSyncTemp";
        private static string CHECK_FILE = @"setupready.txt";
        public void firstTimeSetup()
        {
            if(!File.Exists(Path.Combine(SETUP_PATH,CHECK_FILE)))
            {
                CheckDirectoryStructure(RequestHandler.BACKUP_DIR);
                CheckDirectoryStructure(XMLHandler.SETTINGS_DIR);
                CheckDirectoryStructure(SETUP_PATH);
                CheckSettingsFiles();
                using (StreamWriter sw = new StreamWriter(Path.Combine(SETUP_PATH,CHECK_FILE)))
                {
                    sw.WriteLine("");
                }
            }
        }

        private void CheckDirectoryStructure(string path)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            file.Directory.Create();
        }

        private void CheckSettingsFiles()
        {
            if (!System.IO.File.Exists(XMLHandler.SETTINGS_PATH))
            {
                System.IO.File.Create(XMLHandler.SETTINGS_PATH);
                //List<Settings> settings = new List<Settings>();
                //settings.Add(new Settings());
                //XMLHandler.Write<Settings>(settings, XMLHandler.SETTINGS_PATH);
            }
            if (!System.IO.File.Exists(XMLHandler.SUBSCRIPTION_PATH))
            {
                System.IO.File.Create(XMLHandler.SUBSCRIPTION_PATH);
                //List<Subscription> subs = new List<Subscription>();
                //subs.Add(new Subscription());
                //XMLHandler.Write<Subscription>(subs, XMLHandler.SUBSCRIPTION_PATH);
            }
            if (!System.IO.File.Exists(XMLHandler.USER_PATH))
            {
                System.IO.File.Create(XMLHandler.USER_PATH);
                //List<User> users = new List<User>();
                //users.Add(new User());
                //XMLHandler.Write<User>(users, XMLHandler.USER_PATH);
            }

        }

    }
}
