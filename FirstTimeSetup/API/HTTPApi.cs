using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using RestSharp;
using System.Windows.Forms;

namespace DiscordBot.API
{
    public class LoginData
    {
        public string username { get; set; }
        public string password { get; set; }
        public string token { get; set; }
    }

    public class ProfileData
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string steamid { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string avatar { get; set; }
        public string address { get; set; }
        public int country_id { get; set; }
        public int role_id { get; set; }
        public string status { get; set; }
        public string birthday { get; set; }
        public string last_login { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

    public class RolesData
    {
        public string name { get; set; }
        public string display_name { get; set; }
        public string description { get; set; }
        public int id { get; set; }
        public bool removable { get; set; }
        public int users_count { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

    public class HTTPApi
    {
        public static async Task<IRestResponse<LoginData>> PostLogin(string username, string password, ulong OwnerID)
        {
            var client = new RestClient("http://panel.solarsentinels.co.uk");
            var request = new RestRequest("api/login", Method.POST);
            request.AddObject(new LoginData { username = username, password = password });
            IRestResponse<LoginData> response = client.Execute<LoginData>(request);

            DiscordBot.usrConfig.Add(new jsonDataUser
            {
                LoginData = new LoginData
                {
                    username = username,
                    password = password,
                    token = response.Data.token,
                },
                Username = username,
                Password = password,
                OwnerID = OwnerID
            });

            Discord_Bot.ConsoleBoxText(response.Content);
            await GetAllData(OwnerID);
            return response;
        }

        public static async Task GetAllData(ulong ID)
        {
            await GetProfile(ID);
            await Logout(ID);
        }

        public static async Task GetProfile(ulong OwnerID)
        {
            var usr = DiscordBot.usrConfig.FirstOrDefault(x => x.OwnerID == OwnerID);
            if(usr != null) { return; }

            var client = new RestClient("http://panel.solarsentinels.co.uk");
            client.AddDefaultHeader("Content-Type", "application/json");
            client.AddDefaultHeader("Accept", "application/json");
            client.AddDefaultParameter("Authorization", string.Format("Bearer {0}", usr.LoginData.token), ParameterType.HttpHeader);
            var request = new RestRequest("api/me", Method.GET);
            request.AddObject(new ProfileData());
            IRestResponse<ProfileData> response = client.Execute<ProfileData>(request);
            Discord_Bot.ConsoleBoxText(response.Content);

            usr.ProfileData = response.Data;
            await Task.Delay(-1);
        }

        public static async Task Logout(ulong OwnerID)
        {
            var usr = DiscordBot.usrConfig.FirstOrDefault(x => x.OwnerID == OwnerID);
            var client = new RestClient("http://panel.solarsentinels.co.uk");
            client.AddDefaultHeader("Content-Type", "application/json");
            client.AddDefaultHeader("Accept", "application/json");
            client.AddDefaultParameter("Authorization", string.Format("Bearer {0}", usr.LoginData.token), ParameterType.HttpHeader);
            var request = new RestRequest("api/logout", Method.POST);

            client.Execute(request);
            await Task.Delay(-1);
        }
    }
}
