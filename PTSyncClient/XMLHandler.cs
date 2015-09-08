using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PTSyncClient.Models;

namespace PTSyncClient
{
    public class XMLHandler
    {
        public static string BACKUP_DIR = @"\HHBackup";
        public static string SETTINGS_DIR = @"\PTSync";
        public static string TEMP_DIR = @"\PTSync\Temp";
        public static string NO_USER_FOUND = "NO_USER_FOUND";
        public static string DEFAULT_USER_PATH = @"\Program Files\PTSyncAll\config.xml";
        public static string DEFAULT_SUBSCRIPTION_PATH = @"\Program Files\PTSyncAll\\subscriptions.xml";
        public static string DEFAULT_SETTINGS_PATH = @"\Program Files\PTSyncAll\settings.xml";

        public static string USER_PATH = @"\PTSync\config.xml";
        public static string SUBSCRIPTION_PATH = @"\PTSync\subscriptions.xml";
        public static string SETTINGS_PATH = @"\PTSync\settings.xml";

        public static List<T> Read<T>(string path)
        {
            List<T> list;
            try
            {
                System.Xml.Serialization.XmlSerializer reader =
                    new System.Xml.Serialization.XmlSerializer(typeof(List<T>));
                System.IO.StreamReader file = new System.IO.StreamReader(path);
                list = (List<T>)reader.Deserialize(file);
                file.Close();
            }
            catch (System.IO.FileNotFoundException e)
            {
                list = new List<T>();
            }
            catch(Exception e)
            {
                list = new List<T>();
            }
            return list;
        }

        public static void Write<T>(List<T> list, string path)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer writer =
                    new System.Xml.Serialization.XmlSerializer(typeof(List<T>));
                System.IO.StreamWriter file = new System.IO.StreamWriter(path);
                writer.Serialize(file, list);
                file.Close();
            }
            catch (System.IO.FileNotFoundException e)
            {
            }
            catch(Exception e)
            {
            }
        }

        public static void CreateMissingDirectories()
        {
            if (!System.IO.Directory.Exists(BACKUP_DIR))
                System.IO.Directory.CreateDirectory(BACKUP_DIR);
            if (!System.IO.Directory.Exists(SETTINGS_DIR))
                System.IO.Directory.CreateDirectory(SETTINGS_DIR);
            if (!System.IO.File.Exists(USER_PATH))
                System.IO.File.Copy(DEFAULT_USER_PATH, USER_PATH);
            if (!System.IO.File.Exists(SETTINGS_PATH))
                System.IO.File.Copy(DEFAULT_SETTINGS_PATH, SETTINGS_PATH);
            if (!System.IO.File.Exists(SUBSCRIPTION_PATH))
                System.IO.File.Copy(DEFAULT_SUBSCRIPTION_PATH, SUBSCRIPTION_PATH);
        }

        public static User GetUser()
        {
            List<User> users = XMLHandler.Read<User>(USER_PATH);
            if (users.Count > 0)
                return users[0];
            else
                return new User("",NO_USER_FOUND,"");

        }

        public static Settings GetSettings()
        {
            List<Settings> settings = XMLHandler.Read<Settings>(SETTINGS_PATH);
            if (settings.Count > 0)
                return settings[0];
            else
                return new Settings();

        }

        public static List<Subscription> GetSubscriptions()
        {
            List<Subscription> subs = XMLHandler.Read<Subscription>(SUBSCRIPTION_PATH);
            if (subs.Count > 0)
                return subs;
            else
                return new List<Subscription>();
        }

        public static void SaveSettings(List<Settings> list)
        {
            Write<Settings>(list, SETTINGS_PATH);
        }

        public static void SaveUser(List<User> list)
        {
            Write<User>(list, USER_PATH);
        }

        //usage
        //public static string[] getRouteNames()
        //{
        //    List<DeliveryRoute> dRoutes = XMLHandler.Read<DeliveryRoute>(routeNamePath);
        //    List<DeliveryRoute> todaysRoutes = new List<DeliveryRoute>();
        //    DayOfWeek today = DateTime.Today.DayOfWeek;
        //    foreach (DeliveryRoute deliveryRoute in dRoutes)
        //    {
        //        if (deliveryRoute.HasDay(today))
        //            todaysRoutes.Add(deliveryRoute);
        //    }
        //    string[] todaysRouteNames = new string[todaysRoutes.Count];
        //    for (int index = 0; index < todaysRoutes.Count; index++)
        //    {
        //        todaysRouteNames[index] = todaysRoutes[index].Name;
        //    }
        //    return todaysRouteNames;

        //}

    }
}
