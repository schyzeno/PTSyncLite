using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using PTSyncClient.Models;
using Newtonsoft.Json.Linq;

namespace PTSyncClient
{
    public class AsyncRequestHandler
    {
        HttpWebRequest _httpRequest;
        string _webServiceURI;
        string _destination;
        string _completeFilePathSource;
        string _completeFilePathStage;
        
        public void startUploadRequest(string url, Subscription package)
        {
            _webServiceURI = url;

            string[] filePaths = Directory.GetFiles(package.Source, package.FileName);
            if(filePaths.Length>0)
                _completeFilePathSource = filePaths[0];
            else
            {
                return;
            }

            _completeFilePathStage = package.Stage + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + package.FileName;
            try
            {
                List<String> fileBuffer = new List<String>();
                using (StreamReader streamReader = new StreamReader(package.Source + "\\" + package.FileName, Encoding.Default))
                {
                    while (!streamReader.EndOfStream)
                    {
                        fileBuffer.Add(streamReader.ReadLine());
                    }
                }
                this._httpRequest = (HttpWebRequest)HttpWebRequest.Create(this._webServiceURI);
                this._httpRequest.Method = "POST";
                this._httpRequest.KeepAlive = false;
                this._httpRequest.Timeout = 30000;
                this._httpRequest.ContentType = "text/plain";
                this._httpRequest.SendChunked = true;
                
                using (var writer = new StreamWriter( _httpRequest.GetRequestStream(),Encoding.UTF8 ))
                {
                    foreach (string l in fileBuffer)
                        writer.WriteLine(l);
                }
                //async call
                IAsyncResult myAsycnCall = this._httpRequest.BeginGetResponse(new AsyncCallback(this.UploadResponseReceivedHandler), null);
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

        private void UploadResponseReceivedHandler(IAsyncResult result)
        {
            string responseData = "";
            try
            {
                HttpWebResponse response = (HttpWebResponse)this._httpRequest.EndGetResponse(result);

                Stream responseStream = response.GetResponseStream();
                StreamReader streamReader = new System.IO.StreamReader(responseStream);

                var rawJson = streamReader.ReadToEnd();
                                              
                responseStream.Close();
                streamReader.Close();

                response.Close();

                JObject jsonReponse = JObject.Parse(rawJson);
                string successResponse = jsonReponse["Response"].ToString();
                if (successResponse.Equals("\"success\""))
                {
                    if (!System.IO.File.Exists(_completeFilePathStage)){
                        //using (FileStream fs = System.IO.File.Create(_completeFilePathStage)) { }
                        System.IO.File.Move(_completeFilePathSource, 
                            _completeFilePathStage.Substring(0,_completeFilePathStage.LastIndexOf('.'))
                            +"_"+DateTime.Now.Ticks.ToString()
                            +".txt");
                    }
                    if (System.IO.File.Exists(_completeFilePathSource))
                        System.IO.File.Delete(_completeFilePathSource);
                }
            }
            catch (WebException wx)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + wx.Message);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);

            }
        }

        public void startRequest(string username, Subscription package)
        {
            string myURL = @"http://192.168.0.138:8090/api/HHDownload/";
            _webServiceURI = myURL;
            try
            {
                this._httpRequest = (HttpWebRequest)HttpWebRequest.Create(this._webServiceURI + "?username=" + username + "&subscription=" + package.Name);
                this._httpRequest.Method = "GET";
                this._httpRequest.KeepAlive = false;
                this._httpRequest.Timeout = 30000;
                this._httpRequest.ContentType = "text/plain; charset=utf-8";
                //async call
                this._destination = package.Destination +@"\"+ package.FileName;
                IAsyncResult myAsycnCall = this._httpRequest.BeginGetResponse(new AsyncCallback(this.ResponseReceivedHandler), null);
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
        /// <summary>
        /// This is the async method for the response to be get from the web service
        /// </summary>
        /// <param name="result"></param>
        private void ResponseReceivedHandler(IAsyncResult result)
        {            
            string responseData = "";
            try
            {
                HttpWebResponse response = (HttpWebResponse)this._httpRequest.EndGetResponse(result);

                System.IO.Stream streamResponse = response.GetResponseStream();
                System.IO.StreamReader streamRead = new System.IO.StreamReader(streamResponse, System.Text.Encoding.UTF8);
                
                List<String> lines = new List<String>();
                int cnt = 0;
                while(!streamRead.EndOfStream)
                {
                    responseData = streamRead.ReadLine();
                    lines.Add(responseData);
                    cnt++;
                }

                // Close the stream object
                streamResponse.Close();
                streamRead.Close();

                // Release the HttpWebResponse
                response.Close();

                //Write Data to file
                if (lines.Count > 0)
                {
                    using (StreamWriter sw = new StreamWriter(_destination, false))
                    {
                        foreach (string line in lines)
                            sw.WriteLine(line);
                        sw.Close();
                    }
                }
            }
            catch (WebException wx)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + wx.Message);
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);

            }
        }
    }
}
