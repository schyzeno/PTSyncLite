using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PTSync.Models;
using System.Collections.Specialized;

namespace PTSync
{
    public class SyncController
    {
        public User User { get; set; }
        public Settings Settings { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        private const string waitFilePath = @"\HHUpdate\Wait.Txt";
        private const string readyFilePath = @"\HHUpdate\Ready.Txt";

        public SyncController()
        {
            loadUser();
            Subscriptions = new List<Subscription>();
            loadSubscriptions();
            loadSettings();
        }

        public void loadUser()
        {
            User = XMLHandler.GetUser();
        }

        public void loadSubscriptions()
        {
            Subscriptions.Clear();
            foreach (Subscription p in XMLHandler.GetSubscriptions())
            {
                Subscriptions.Add(p);
            }
        }

        public bool isDailySync()
        {
            Int32 currentTime = Convert.ToInt32(DateTime.Now.ToString("HHmm"));
            //Int32 dailyFloor = Settings.DailySyncTime - Settings.SyncInterval;
            //Int32 dailyCeil = Settings.DailySyncTime + Settings.SyncInterval;
            return currentTime >= (Settings.DailySyncTime - Settings.SyncInterval)
                    && currentTime <= (Settings.DailySyncTime + Settings.SyncInterval);
        }

        public void loadSettings()
        {
            Settings = XMLHandler.GetSettings();
        }

        public void DownloadUpdates(string cycle)
        {
            if (File.Exists(waitFilePath))
            {
                return;
            }
            //Delete All Files

            Dictionary<String, bool> updateCheckList = new Dictionary<string, bool>();
            //Loop through subs for HHDownload set, download
            foreach (Subscription subscription in Subscriptions)
            {
                if (subscription.Type.Equals("DownloadSet") && HasCurrentCycle(subscription.Cycle, cycle))
                {
                    if (File.Exists(Path.Combine(subscription.Destination, subscription.FileName)))
                        File.Delete(Path.Combine(subscription.Destination, subscription.FileName));
                    bool fileDownloaded = RequestHandler.StartDownloadSetRequest(
                        ServiceAddress.GetDownloadUpdateURL(Settings, User, subscription)
                        , subscription);
                    updateCheckList.Add(subscription.Name, fileDownloaded);
                }
            }

        }

        public void UploadOHH(string cycle)
        {
            foreach (Subscription subscription in Subscriptions)
            {
                if (subscription.Type.Equals("Upload") && HasCurrentCycle(subscription.Cycle, cycle))
                {
                    RequestHandler.startUploadOHH(ServiceAddress.GetUploadURL(Settings, User, subscription), subscription, Settings.BackupData);
                }
            }
        }

        public void DownloadConfirmations(string cycle)
        {
            foreach (Subscription subscription in Subscriptions)
            {
                if (subscription.Type.Equals("DownloadStartsWith") && HasCurrentCycle(subscription.Cycle, cycle))
                {
                    RequestHandler.StartDownloadStartsWithRequest(ServiceAddress.GetDownloadConfirmationURL(Settings, User, subscription), subscription);
                }
            }
        }

        public void DownloadSubscriptions()
        {
            RequestHandler.DownloadSubscriptions(ServiceAddress.GetSubscriptionURL(Settings, User));
        }

        public void ConfirmDownloads(string cycle)
        {

        }

        public void DeleteFiles(string cycle)
        {
            foreach (Subscription subscription in Subscriptions)
            {
                if (subscription.Type.Equals("Delete") && HasCurrentCycle(subscription.Cycle, cycle))
                {
                    string sourcePath = Path.Combine(subscription.Source, subscription.FileName);
                    if (File.Exists(sourcePath))
                        File.Delete(sourcePath);
                }
            }
        }

        public void RenameFiles(string cycle)
        {
            foreach (Subscription subscription in Subscriptions)
            {
                if (subscription.Type.Equals("Rename") && HasCurrentCycle(subscription.Cycle, cycle))
                {
                    string sourcePath = Path.Combine(subscription.Source, subscription.FileName);
                    string destinationPath = Path.Combine(subscription.Destination, subscription.Stage);
                    if (File.Exists(sourcePath))
                    {
                        if (File.Exists(destinationPath))
                            File.Delete(destinationPath);
                        File.Move(sourcePath, destinationPath);
                    }
                }
            }
        }

        public void UploadMisc(string cycle)
        {
            foreach (Subscription subscription in Subscriptions)
            {
                if (subscription.Type.Equals("UploadMisc") && HasCurrentCycle(subscription.Cycle, cycle))
                {
                    string url = ServiceAddress.GetUploadMiscURL(Settings, User, subscription);
                    string path = Path.Combine(subscription.Source, subscription.FileName);
                    string mime = Util.MimeTypeMap.GetMimeType(Path.GetExtension(path));
                    if (File.Exists(path))
                    {
                        RequestHandler.HttpUploadFile(
                            url
                            , subscription.Source
                            , subscription.FileName
                            , "fieldNamehere"
                            , mime
                            , Settings.BackupData
                        );
                    }
                }
            }
        }

        public void UploadStartsWith(string cycle)
        {
            try
            {
                foreach (Subscription subscription in Subscriptions)
                {
                    if (subscription.Type.Equals("UploadStartsWith") && HasCurrentCycle(subscription.Cycle, cycle))
                    {
                        //Find all files that
                        string fileNameRegEx = subscription.FileName + "*";

                        List<string> fileNames = new List<string>();
                        foreach (string filePath in Directory.GetFiles(subscription.Source, fileNameRegEx))
                            fileNames.Add(Path.GetFileName(filePath));

                        string url = ServiceAddress.GetUploadStartsWithURL(Settings, User, subscription);
                        RequestHandler.HttpUploadDirectory(
                                    url
                                    , subscription.Source
                                    , fileNames
                                    , Settings.BackupData
                                );

                    }
                }
            }
            catch (DirectoryNotFoundException dex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + dex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
            }
        }

        public void Download(string cycle)
        {
            foreach (Subscription subscription in Subscriptions)
            {
                if (subscription.Type.Equals("Download") && HasCurrentCycle(subscription.Cycle, cycle))
                {
                    if (!File.Exists(Path.Combine(subscription.Destination, subscription.FileName)))
                    {
                        string url = ServiceAddress.GetDownloadURL(Settings, User, subscription);
                        RequestHandler.Download(url, subscription);
                    }
                }
            }
        }

        public void DeleteStartsWith(string cycle)
        {
            try
            {
                foreach (Subscription subscription in Subscriptions)
                {
                    if (subscription.Type.Equals("DeleteStartsWith") && HasCurrentCycle(subscription.Cycle, cycle))
                    {
                        string fileNameRegEx = subscription.FileName + "*";
                        foreach (string filePath in Directory.GetFiles(subscription.Source, fileNameRegEx))
                        {
                            if (File.Exists(filePath))
                                File.Delete(filePath);
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException dex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + dex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
            }
        }

        private bool HasCurrentCycle(string subscriptionCycle, string currentCycle)
        {
            if (currentCycle.Equals(Cycle.Any))
                return true;
            else
                return subscriptionCycle.Equals(currentCycle);
        }

    }
}
