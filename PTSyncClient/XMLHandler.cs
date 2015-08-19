using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PTSyncClient.Models;

namespace PTSyncClient
{
    public class XMLHandler
    {
        public static string SETTINGS_DIR = @"\PTSync";
        public static string TEMP_DIR = @"\PTSync\Temp";
        public static string USER_PATH = @"\PTSync\config.xml";
        public static string SUBSCRIPTION_PATH = @"\PTSync\subscriptions.xml";
        public static string SETTINGS_PATH = @"\PTSync\settings.xml";

        public static List<T> Read<T>(string path)
        {
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(List<T>));
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            List<T> list = (List<T>)reader.Deserialize(file);
            file.Close();
            return list;
        }

        public static void Write<T>(List<T> list, string path)
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(List<T>));
            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            writer.Serialize(file, list);
            file.Close();
        }

        public static User GetUser()
        {
            List<User> users = XMLHandler.Read<User>(USER_PATH);
            if (users.Count > 0)
                return users[0];
            else
                return new User();

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
