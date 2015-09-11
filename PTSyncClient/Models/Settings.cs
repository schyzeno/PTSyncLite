using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace PTSyncClient.Models
{
    public class Settings
    {
        public String IP { get; set; }
        public String Port { get; set; }
        public int SyncInterval { get; set; }
        public Boolean BackupData { get; set; }
        public int DailySyncTime { get; set; }
        public Settings()
        {
            IP = "";
            Port = "";
            SyncInterval = 5;
            BackupData = false;
            DailySyncTime = 0400;
            //minimum interval is 1 minute || 60000 milliseconds

        }
        public Settings(string ip, string port, int syncInterval, bool backup, int dailySyncTime)
        {
            IP = ip;
            Port = port;
            SyncInterval = syncInterval;
            BackupData = backup;
            DailySyncTime = dailySyncTime;
        }
    }
}
