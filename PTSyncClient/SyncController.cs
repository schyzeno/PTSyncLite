using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PTSyncClient.Models;
using System.Collections.Specialized;

namespace PTSyncClient
{
    public class SyncController
    {
        public Boolean IsSyncing { get; set; }
        public User User { get; set; }
        public Settings Settings { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        private const string waitFilePath=@"\HHUpdate\Wait.Txt";
        private const string readyFilePath=@"\HHUpdate\Ready.Txt";

        public SyncController()
        {
            loadUser();
            Subscriptions = new List<Subscription>();
            loadSubscriptions();
            loadSettings();
            IsSyncing = false;
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

        public void loadSettings()
        {
            Settings = XMLHandler.GetSettings();
        }

        public void DownloadUpdates()
        {
            if(File.Exists(waitFilePath))
            {
                return;
            }
            //Delete All Files

            Dictionary<String,bool> updateCheckList = new Dictionary<string,bool>();
            //Loop through subs for HHDownload set, download
            foreach(Subscription subscription in Subscriptions)
            {
                if(subscription.Type.Equals("DownloadSet"))
                {
                    if (File.Exists(Path.Combine(subscription.Destination, subscription.FileName)))
                        File.Delete(Path.Combine(subscription.Destination, subscription.FileName));
                    bool fileDownloaded = RequestHandler.StartDownloadSetRequest(
                        ServiceAddress.GetDownloadUpdateURL(Settings,User,subscription)
                        ,subscription);
                    updateCheckList.Add(subscription.Name,fileDownloaded);
                }
            }
            
        }

        public void UploadOHH()
        {
            foreach (Subscription subscription in Subscriptions)
            {
                if (subscription.Type.Equals("Upload"))
                {
                    RequestHandler.startUploadOHH(ServiceAddress.GetUploadURL(Settings, User, subscription), subscription,Settings.BackupData);
                }
            }
        }

        public void DownloadConfirmations()
        {
            foreach (Subscription subscription in Subscriptions)
            {
                if (subscription.Type.Equals("DownloadStartsWith"))
                {
                    RequestHandler.StartDownloadStartsWithRequest(ServiceAddress.GetDownloadConfirmationURL(Settings, User, subscription), subscription);
                }
            }
        }

        public void DownloadSubscriptions()
        {
            RequestHandler.DownloadSubscriptions(ServiceAddress.GetSubscriptionURL(Settings,User));
        }

        public void ConfirmDownloads()
        {
            
        }

        public void DeleteFiles()
        {
            foreach (Subscription subscription in Subscriptions)
            {
                if (subscription.Type.Equals("Delete"))
                {
                    string sourcePath = Path.Combine(subscription.Source, subscription.FileName);
                    if (File.Exists(sourcePath))
                        File.Delete(sourcePath);
                }
            }
        }

        public void RenameFiles()
        {
            foreach (Subscription subscription in Subscriptions)
            {
                if (subscription.Type.Equals("Rename"))
                {
                    string sourcePath = Path.Combine(subscription.Source, subscription.FileName);
                    string destinationPath = Path.Combine(subscription.Destination,subscription.Stage);
                    if (File.Exists(sourcePath))
                    {
                        if (File.Exists(destinationPath))
                            File.Delete(destinationPath);
                        File.Move(sourcePath, destinationPath);
                    }
                }
            }
        }

        public void UploadMisc()
        {
            foreach (Subscription subscription in Subscriptions)
            {
                if (subscription.Type.Equals("UploadMisc"))
                {
                    string url = ServiceAddress.GetUploadMiscURL(Settings, User, subscription);
                    string path = Path.Combine(subscription.Source,subscription.FileName);
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

        public void UploadStartsWith()
        {
            foreach (Subscription subscription in Subscriptions)
            {
                if (subscription.Type.Equals("UploadStartsWith"))
                {
                    //Find all files that
                    string fileNameRegEx = subscription.FileName + "*";

                    List<string> fileNames = new List<string>();
                    foreach(string filePath in Directory.GetFiles(subscription.Source, fileNameRegEx))
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

        public void Download()
        {
            foreach (Subscription subscription in Subscriptions)
            {
                if (subscription.Type.Equals("Download"))
                {
                    if (!File.Exists(Path.Combine(subscription.Destination, subscription.FileName)))
                    {
                        string url = ServiceAddress.GetDownloadURL(Settings, User, subscription);
                        RequestHandler.Download(url, subscription);
                    }
                }
            }
        }

    }
}
