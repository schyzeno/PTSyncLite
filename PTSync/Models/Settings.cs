using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace PTSync.Models
{
    public class Settings
    {
        public String IP { get; set; }
        public String Port { get; set; }
        public int SyncInterval { get; set; }
        public Boolean BackupData { get; set; }
        public Settings()
        {
            IP = "";
            Port = "";
            SyncInterval = 60000;
            BackupData = false;
            //minimum interval is 1 minute || 60000 milliseconds

        }
        public Settings(string ip, string port,int syncInterval,bool backup)
        {
            IP = ip;
            Port = port;
            SyncInterval = syncInterval;
            BackupData = backup;
        }
    }
}
