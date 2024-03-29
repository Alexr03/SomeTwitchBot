﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.API;

namespace DiscordBot
{
    public class jsonDataUser
    {
        public ulong OwnerID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public LoginData LoginData { get; set; }
        public ProfileData ProfileData { get; set; }
    }

    public class Configuration
    {
        public bool Master { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public ulong Owner { get; set; }
        public string FTPHost { get; set; }
        public string FTPUser { get; set; }
        public string FTPPass { get; set; }
        public IList<ulong> AdminIDs { get; set; }
        public bool Debug { get; set; }
        public bool FirstTimeSetup { get; set; }
    }
}
