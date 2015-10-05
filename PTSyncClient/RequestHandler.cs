using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using PTSyncClient.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;

namespace PTSyncClient
{
    public class RequestHandler
    {
        public static string BACKUP_DIR = @"\HHBackup";
        public static void StartDownloadStartsWithRequest(string url, Subscription package)
        {
            string _webServiceURI = url;
            string _destination;
            string _backup;
            try
            {
                WebRequest wrGETURL = WebRequest.Create(_webServiceURI);
                List<String> streamBuffer = new List<String>();
                int numberOfFiles = 0;

                using (Stream responseStream = wrGETURL.GetResponse().GetResponseStream())
                using (StreamReader streamReader = new System.IO.StreamReader(responseStream))
                {
                    if (!streamReader.EndOfStream)
                    {
                        string firstLine = streamReader.ReadLine();
                        if (firstLine.ToUpper().Equals("[0]") || firstLine.ToUpper().Equals("NO_FILE_FOUND"))
                        {
                            return;
                        }
                        else
                        {
                            numberOfFiles = Convert.ToInt32(firstLine.Substring(1, firstLine.Length - 2));
                        }
                    }
                    else
                    {
                        return;
                    }

                    string lineBuffer = "";
                    string fileName = "";
                    for (int i = 0; i < numberOfFiles; i++)
                    {
                        streamBuffer.Clear();
                        lineBuffer = streamReader.ReadLine();
                        if (!lineBuffer.Equals("[START]"))
                            return;
                        else
                            lineBuffer = streamReader.ReadLine();
                        fileName = lineBuffer;
                        lineBuffer = streamReader.ReadLine();
                        while (!lineBuffer.Equals("[END]"))
                        {
                            if (lineBuffer.Equals("NO_FILE_FOUND"))
                                break;
                            streamBuffer.Add(lineBuffer);
                            lineBuffer = streamReader.ReadLine();
                        }
                        if (lineBuffer.Equals("NO_FILE_FOUND"))
                            return;
                        if (streamBuffer.Count > 0)
                        {
                            _destination = package.Destination + "\\" + fileName;
                            using (StreamWriter sw = new StreamWriter(_destination, false))
                            {
                                for (int j = 0; j < streamBuffer.Count; j++)
                                    sw.WriteLine(streamBuffer[j]);
                            }                            
                        }
                    }

                }
            }
            catch (WebException wx)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + wx.Message);
            }
            catch (Exception wx)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + wx.Message);
            }
        }

        public static bool StartDownloadSetRequest(string url, Subscription package)
        {
            bool downloadSuccess = false;
            string _webServiceURI = url;
            string _destination = Path.Combine(package.Destination, package.FileName);


            try
            {
                WebRequest wrGETURL = WebRequest.Create(_webServiceURI);
                List<String> streamBuffer = new List<String>();

                using (Stream responseStream = wrGETURL.GetResponse().GetResponseStream())
                using (StreamReader streamReader = new System.IO.StreamReader(responseStream))
                {

                    if (!streamReader.EndOfStream)
                    {
                        string firstLine = streamReader.ReadLine();
                        if (!firstLine.ToUpper().Equals("[START]"))
                        {
                            return downloadSuccess;
                        }
                    }
                    else
                    {
                        return downloadSuccess;
                    }
                    while (!streamReader.EndOfStream)
                    {
                        streamBuffer.Add(streamReader.ReadLine());
                    }
                }

                if (streamBuffer.Count > 0)
                {
                    if (DownloadedCompletely(streamBuffer[streamBuffer.Count - 1]))
                    {
                        using (StreamWriter sw = new StreamWriter(_destination, false))
                        {
                            for (int i = 0; i < streamBuffer.Count - 1; i++)
                                sw.WriteLine(streamBuffer[i]);
                            downloadSuccess = true;
                        }
                    }
                }
            }
            catch (WebException wx)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + wx.Message);
                downloadSuccess = false;
            }
            catch (Exception wx)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + wx.Message);
                downloadSuccess = false;
            }
            return downloadSuccess;
        }

        private static bool DownloadedCompletely(string lastLine)
        {
            return lastLine.ToUpper().StartsWith("[END]") || lastLine.ToUpper().StartsWith("[EOF");
        }

        public static void DownloadSubscriptions(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();

                List<Subscription> subscriptions = new List<Subscription>();
                JArray arrJson = JArray.Parse(rawJson);
                foreach (JObject jobj in arrJson)
                {
                    Subscription subscription = new Subscription();
                    /*
                    subscription.Destination = jobj.Value<String>("Destination");
                    subscription.Source = jobj["Source"].ToString();
                    subscription.FileName = jobj["FileName"].ToString();
                    subscription.Name = jobj["Name"].ToString();
                    subscription.Stage = jobj["Stage"].ToString();
                    subscription.Type = jobj["Type"].ToString();
                    */
                    subscription.Destination = jobj.Value<String>("Destination");
                    subscription.Source = jobj.Value<String>("Source");
                    subscription.FileName = jobj.Value<String>("FileName");
                    subscription.Name = jobj.Value<String>("Name");
                    subscription.Stage = jobj.Value<String>("Stage");
                    subscription.Type = jobj.Value<String>("Type");
                    subscription.Cycle = jobj.Value<String>("Cycle");
                    subscriptions.Add(subscription);
                }
                XMLHandler.Write<Subscription>(subscriptions, XMLHandler.SUBSCRIPTION_PATH);
            }
            catch (Exception e)
            {
                string debug = e.Message;
            }

        }

        public static void HttpUploadFile(string url, string fileDir, string fileName, string paramName, string contentType, bool backup)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, fileName, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            string filePath = Path.Combine(fileDir, fileName);
            string _completeFilePathStage = Path.Combine(BACKUP_DIR, Path.GetFileNameWithoutExtension(filePath)
                                                        + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")
                                                        + Path.GetExtension(filePath));
            FileInfo fileInfo = new FileInfo(filePath);
            long totalRequestBodySize = boundarybytes.Length + headerbytes.Length + trailer.Length + fileInfo.Length;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.AllowWriteStreamBuffering = false;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webRequest.Method = "POST";
            webRequest.KeepAlive = false;

            //webRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            //webRequest.SendChunked = true;
            webRequest.ContentLength = totalRequestBodySize;

            WebResponse wresp = null;
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                    requestStream.Write(headerbytes, 0, headerbytes.Length);
                    int loopCount = 0;

                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        requestStream.Write(buffer, 0, bytesRead);
                        loopCount++;
                    }
                    requestStream.Write(trailer, 0, trailer.Length);
                }
                wresp = webRequest.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                string response2 = reader2.ReadToEnd();
                JObject jsonReponse = JObject.Parse(response2);

                string successResponse = jsonReponse["success"].ToString();
                if (successResponse.Equals("true"))
                {
                    if (System.IO.File.Exists(filePath) && backup)
                    {
                        //using (FileStream fs = System.IO.File.Create(_completeFilePathStage)) { }
                        System.IO.File.Move(filePath, _completeFilePathStage);
                    }
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

            }
            catch (Exception ex)
            {
                string mess = ex.Message;
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                webRequest = null;
            }

        }

        public static void HttpUploadDirectory(string url, string fileDir, List<string> fileNames, bool backup)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";

            long totalHeaderLength = 0;
            long totalBoundaryLength = 0;
            long totalFileLength = 0;
            foreach (string fileName in fileNames)
            {
                string contentType = Util.MimeTypeMap.GetMimeType(Path.GetExtension(Path.Combine(fileDir, fileName)));
                string header = string.Format(headerTemplate, fileName, fileName, contentType);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                totalHeaderLength += headerbytes.Length;
                totalHeaderLength += boundarybytes.Length;
                totalFileLength += new FileInfo(Path.Combine(fileDir, fileName)).Length;
            }
            long totalRequestBodySize = totalBoundaryLength + totalHeaderLength + trailer.Length + totalFileLength;



            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.AllowWriteStreamBuffering = false;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webRequest.Method = "POST";
            webRequest.KeepAlive = false;
            webRequest.ContentLength = totalRequestBodySize;

            WebResponse webResponse = null;

            byte[] buffer = new byte[4096];
            try
            {
                using (Stream requestStream = webRequest.GetRequestStream())
                {

                    foreach (string fileName in fileNames)
                    {
                        string contentType = Util.MimeTypeMap.GetMimeType(Path.GetExtension(Path.Combine(fileDir, fileName)));
                        string header = string.Format(headerTemplate, fileName, fileName, contentType);
                        byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                        requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                        requestStream.Write(headerbytes, 0, headerbytes.Length);

                        FileStream fileStream = new FileStream(Path.Combine(fileDir, fileName), FileMode.Open, FileAccess.Read);
                        int bytesRead = 0;
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            requestStream.Write(buffer, 0, bytesRead);
                        }
                        fileStream.Close();
                    }

                    requestStream.Write(trailer, 0, trailer.Length);

                }
                webResponse = webRequest.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader reader2 = new StreamReader(responseStream);
                string response2 = reader2.ReadToEnd();
                JObject jsonReponse = JObject.Parse(response2);

                string successResponse = jsonReponse["success"].ToString();
                if (successResponse.Equals("true"))
                {
                    string filePath = "";
                    foreach (string fileName in fileNames)
                    {
                        filePath = Path.Combine(fileDir, fileName);
                        string _completeFilePathStage = Path.Combine(BACKUP_DIR, Path.GetFileNameWithoutExtension(filePath)
                                                    + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")
                                                    + Path.GetExtension(filePath));
                        if (System.IO.File.Exists(filePath) && backup)
                        {
                            //using (FileStream fs = System.IO.File.Create(_completeFilePathStage)) { }
                            System.IO.File.Move(filePath, _completeFilePathStage);
                        }
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                    }
                }

            }
            catch (Exception ex)
            {
                string mess = ex.Message;
                if (webResponse != null)
                {
                    webResponse.Close();
                    webResponse = null;
                }
            }
            finally
            {
                webRequest = null;
            }

        }

        public static void Download(String url, Subscription package)
        {
            string destinationPath = Path.Combine(package.Destination, package.FileName);
            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                byte[] result = null;
                byte[] buffer = new byte[4096];
                int bytesRead;
                int count = 0;

                using (Stream responseStream = webRequest.GetResponse().GetResponseStream())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    do
                    {
                        count = responseStream.Read(buffer, 0, buffer.Length);
                        memoryStream.Write(buffer, 0, count);

                        if (count == 0)
                        {
                            break;
                        }
                    }
                    while (true);

                    result = memoryStream.ToArray();

                    using (FileStream fs = new FileStream(destinationPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        fs.Write(result, 0, result.Length);
                }
            }
            catch (WebException wx)
            {
                String ex = wx.Message;
                ex = ex + " ";
                System.Diagnostics.Debug.WriteLine("Exception: " + wx.Message);
            }
            catch (Exception wx)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + wx.Message);
            }
        }

        public static void sendFileconfirmations(string url, List<Subscription> packages)
        {

            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.KeepAlive = false;
                httpRequest.Timeout = 30000;
                httpRequest.ContentType = "text/json";

                using (var writer = new StreamWriter(httpRequest.GetRequestStream(), Encoding.UTF8))
                {
                    JArray filesConfirmed = new JArray(packages);
                    writer.Write(filesConfirmed);
                    writer.Flush();
                    writer.Close();
                }

                JObject jsonReponse = new JObject();
                using (HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new System.IO.StreamReader(responseStream))
                    {
                        var rawJson = streamReader.ReadToEnd();
                        jsonReponse = JObject.Parse(rawJson);
                    }
                }

                string successResponse = (string)jsonReponse["Response"];
                if (successResponse.Equals("success"))
                {
                    
                }
            }
            catch (WebException wx)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + wx.Message);
            }
            catch (Exception wx)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + wx.Message);
            }
        }
    

        public static void startUploadOHH(string url, Subscription package, bool backup)
        {

            try
            {
                string _completeFilePathSource;


                string[] filePaths = Directory.GetFiles(package.Source, package.FileName);
                if (filePaths.Length > 0)
                    _completeFilePathSource = filePaths[0];
                else
                {
                    return;
                }
                string _completeFilePathStage = Path.Combine(BACKUP_DIR, Path.GetFileNameWithoutExtension(_completeFilePathSource)
                                                            + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")
                                                            + Path.GetExtension(_completeFilePathSource));

                List<String> fileBuffer = new List<String>();
                using (StreamReader streamReader = new StreamReader(package.Source + "\\" + package.FileName, Encoding.Default))
                {
                    while (!streamReader.EndOfStream)
                    {
                        fileBuffer.Add(streamReader.ReadLine());
                    }
                }
                HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.KeepAlive = false;
                httpRequest.Timeout = 30000;
                httpRequest.ContentType = "text/plain";
                httpRequest.SendChunked = true;

                using (var writer = new StreamWriter(httpRequest.GetRequestStream(), Encoding.UTF8))
                {
                    foreach (string l in fileBuffer)
                        writer.WriteLine(l);
                }

                JObject jsonReponse = new JObject();
                using (HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new System.IO.StreamReader(responseStream))
                    {
                        var rawJson = streamReader.ReadToEnd();
                        jsonReponse = JObject.Parse(rawJson);
                    }
                }

                string successResponse = (string)jsonReponse["Response"];
                if (successResponse.Equals("success"))
                {
                    if (System.IO.File.Exists(_completeFilePathSource) && backup)
                    {
                        //using (FileStream fs = System.IO.File.Create(_completeFilePathStage)) { }
                        System.IO.File.Move(_completeFilePathSource, _completeFilePathStage);
                    }
                    if (System.IO.File.Exists(_completeFilePathSource))
                        System.IO.File.Delete(_completeFilePathSource);
                }
            }
            catch (WebException wx)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + wx.Message);
            }
            catch (Exception wx)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + wx.Message);
            }
        }
    }
}
