using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;
using System.Drawing;
using DSharpPlus.CommandsNext;
//using DiscordBot.API;

namespace DiscordBot
{
    public class Functions
    {
        private static string directory = System.IO.Directory.GetCurrentDirectory();
        public static string dir = System.IO.Directory.GetParent(directory).FullName + "/";

        public static bool ServiceControls(string ServiceName, string type = "start")
        {
            ServiceController sv = new ServiceController(ServiceName);

            if (type == "start".ToLower())
            {
                sv.Start();
                sv.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 10));
                if (sv.Status == ServiceControllerStatus.Running)
                {
                    return true;
                }
            }

            else if (type == "stop".ToLower())
            {
                sv.Stop();
                sv.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 10));
                if (sv.Status == ServiceControllerStatus.Stopped)
                {
                    return true;
                }
            }

            return false;

        }

        public static bool DoesServiceExist(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            var service = services.FirstOrDefault(s => s.ServiceName == serviceName);
            return service != null;
        }

        public static void SendRCON(string server, string command, string user = "Administrator")
        {
            var request = (HttpWebRequest)WebRequest.Create(server);

            var postData = "command=" + command;
            postData += "&user=" + user;
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        public static void GetLatestFile(string DownloadURL, string Username, string Password, string SaveAsName, bool DeleteIfExist = true)
        {
            if (DeleteIfExist)
            {
                if (File.Exists(SaveAsName))
                {
                    Console.WriteLine("Deleting file: " + dir + SaveAsName);
                    File.Delete(SaveAsName);
                }
            }

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + DownloadURL);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            request.Credentials = new NetworkCredential(Username, Password);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();

            string content;
            using (StreamReader reader = new StreamReader(responseStream))
            {
                content = reader.ReadToEnd();
                reader.Dispose();
                reader.Close();
            }

            response.Close();

            using (StreamWriter w = File.AppendText(SaveAsName))
            {
                w.WriteLine(content);
                w.Dispose();
                w.Close();
            }
        }

        public static void UploadToFTP(string URLToPerm, string Username, string Password, string File = "Permissions.config.xml")
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + URLToPerm);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential(Username, Password);

            StreamReader sourceStream = new StreamReader(File);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Console.WriteLine("Upload File Complete, status: " + response.StatusDescription);

            response.Close();
        }

        public static string GetNewsJson()
        {
            return new WebClient().DownloadString("http://api.steampowered.com/ISteamNews/GetNewsForApp/v0002/?appid=304930&count=1&maxlength=1000000&format=json");
        }

        public static string Get64ID(string communityName, string APIKey)
        {
            WebClient client = new WebClient();
            string json = client.DownloadString("http://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key=" + APIKey + "&vanityurl=" + communityName);
            return json;
        }

        public static string GetURLID(string url)
        {
            Uri uri = new Uri(url);
            string lastSegment = uri.Segments.Last();
            if (lastSegment.EndsWith("/"))
            {
                string last = lastSegment.TrimEnd('/');
                return last;
            }
            return lastSegment;
        }

        //public static ServerInfo GetServerInfo(string arg)
        //{
        //    var count = DiscordBot.config.Servers.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        Servers server = DiscordBot.config.Servers[i];
        //        if (server.ServerNumber == arg)
        //        {
        //            var sv = ServerQuery.GetServerInstance(EngineType.Source, server.ServerIP, Convert.ToUInt16(server.Port + 1));
        //            return sv.GetInfo();
        //        }
        //    }
        //    return null;
        //}

        //public static void GetPlayers(string server, CommandEventArgs e)
        //{
        //    ServerInfo svinfo = GetServerInfo(server);
        //    e.Context.Channel.SendMessageAsync("Name: " + svinfo.Name);
        //    e.Context.Channel.SendMessageAsync("Players: " + svinfo.Players + "/" + svinfo.MaxPlayers);
        //}

        //public static QueryMasterCollection<PlayerInfo> GetPlayersInfo(string arg, CommandEventArgs e)
        //{
        //    var count = DiscordBot.config.Servers.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        Servers server = DiscordBot.config.Servers[i];
        //        if (server.ServerNumber == arg)
        //        {
        //            var sv = ServerQuery.GetServerInstance(EngineType.Source, server.ServerIP, Convert.ToUInt16(server.Port + 1));
        //            return sv.GetPlayers();
        //        }
        //    }
        //    return null;
        //}
    }
}
