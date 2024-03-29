﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace PTSync.Models
{
    public class ServiceAddress
    {
        private const string FSLASH = "/";

        public static string GetSubscriptionURL(Settings settings, User user)
        {
            string route = "/Subscription/user";
            return BaseURL(settings) + route + FSLASH + user.CompanyID + FSLASH + user.Name;
        }

        public static string GetDownloadUpdateURL(Settings settings, User user, Subscription subscription)
        {
            string route = "/HHDownloadSet/Download";
            return BaseURL(settings) + route + FSLASH + user.CompanyID + FSLASH + user.Name + FSLASH + subscription.Name;
        }

        public static string GetUploadURL(Settings settings, User user, Subscription subscription)
        {
            string route = "/HHUpload/Upload";
            return BaseURL(settings) + route + FSLASH + user.CompanyID + FSLASH + user.Name + FSLASH + subscription.Name;
        }

        public static string GetUploadMiscURL(Settings settings, User user, Subscription subscription)
        {
            string route = "/HHUpload/UploadFile";
            return BaseURL(settings) + route + FSLASH + user.CompanyID + FSLASH + user.Name + FSLASH + subscription.Name;
        }

        public static string GetUploadStartsWithURL(Settings settings, User user, Subscription subscription)
        {
            string route = "/HHUpload/UploadFileStartsWith";
            return BaseURL(settings) + route + FSLASH + user.CompanyID + FSLASH + user.Name + FSLASH + subscription.Name;
        }

        public static string GetDownloadURL(Settings settings, User user, Subscription subscription)
        {
            string route = "/HHDownload/Download";
            return BaseURL(settings) + route + FSLASH + user.CompanyID + FSLASH + user.Name + FSLASH + subscription.Name;
        }


        public static string GetConfirmationReceiptURL(Settings settings, User user)
        {
            string route = "/HHDownload/Confirm";
            return BaseURL(settings) + route + FSLASH + user.CompanyID + FSLASH + user.Name;
        }

        public static string GetDownloadConfirmationURL(Settings settings, User user, Subscription subscription)
        {
            string route = "/HHDownloadStartsWith/Download";
            return BaseURL(settings) + route + FSLASH + user.CompanyID + FSLASH + user.Name + FSLASH + subscription.Name;
        }

        private static string BaseURL(Settings settings)
        {
            return "http://" + settings.IP + ":" + settings.Port;
        }
    }
}

