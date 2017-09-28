using MaterialSkin.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
//using DiscordBot.API;

namespace DiscordBot
{
    public partial class Discord_Bot : MaterialForm
    {
        public static Configuration config;
        public static IList<ulong> admins = new List<ulong>();
        public static string json;
        public static string usrjson;
        public static Discord_Bot instance;

        public Discord_Bot()
        {
            InitializeComponent();
            instance = this;
            materialTabControl1.SelectedTab = GeneralPage;
            this.MaximumSize = new System.Drawing.Size(1623, 580);
            this.MinimumSize = new System.Drawing.Size(1623, 580);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            LoadDefaults();

            FormClosed += (o, i) =>
            {
                if (i.CloseReason == CloseReason.UserClosing)
                {
                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Logs"))
                    {
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Logs");
                    }
                    ConsoleBox.SaveFile(string.Format(AppDomain.CurrentDomain.BaseDirectory + "Logs\\Bot Logs - {0}", DateTime.Now.ToString("dd.MM.yyyy___hhmmss") + ".rtf"));
                }
            };
        }

        private async void FirstTimeSetup_Load(object sender, EventArgs e)
        {
            ConsoleBoxText(string.Format("========================================{0}========================================\n", DateTime.Now.ToString("hh:mm:ss_dd.MM.yyyy")));
            await Startsvr();
        }

        delegate void ConsoleBoxCallback(string text);
        public static void ConsoleBoxText(string text)
        {
            if (instance.ConsoleBox.InvokeRequired)
            {
                ConsoleBoxCallback d = new ConsoleBoxCallback(ConsoleBoxText);
                instance.ConsoleBox.Invoke(d, new object[] { text });
            }
            else
            {
                instance.ConsoleBox.AppendText("[" + DateTime.Now.ToString("hh:mm:ss") + "] " +  text);
            }
        }

        private void SubmitAdminsID_Click(object sender, EventArgs e)
        {
            admins.Clear();
            foreach (string v in AdminListBox.Lines)
            {
                admins.Add(Convert.ToUInt64(v));
            }
        }

        private void PreviewConfigButton_Click(object sender, EventArgs e)
        {
            GenerateConfigPreview();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            reloadConfig();
            var t = MessageBox.Show("Are you sure you want to submit these changes?", "ARE YOU SUREE!?!?!?!?", MessageBoxButtons.YesNo);
            if (t == DialogResult.Yes)
            {
                if (File.Exists("config.json"))
                {
                    if (instance.OverwriteCheckBox.Checked)
                    {
                        reloadConfig();
                        MessageBox.Show("Updated the configuration file with the new details. After any changes please do !reload in discord to the bot for changes to take affect.");
                    }
                    else { MessageBox.Show("Configuration already found, please tick 'Overwrite if exists'"); }
                }
                else
                {
                    reloadConfig();
                }
            }
            else
            {
                MessageBox.Show("Aborted.", "", MessageBoxButtons.OK);
            }
        }

        public static void reloadConfig()
        {
            config = new Configuration
            {
                Name = instance.BotNameBox.Text,
                Master = Convert.ToBoolean(instance.MasterBox.Text),
                Token = instance.DiscordTokenBox.Text,
                Owner = Convert.ToUInt64(instance.OwnerIDBox.Text),
                FTPHost = instance.FTPHostBox.Text,
                FTPUser = instance.FTPUserBox.Text,
                FTPPass = instance.FTPPassBox.Text,
                AdminIDs = admins,
                Debug = Convert.ToBoolean(instance.DebugBox.Text),
                FirstTimeSetup = true,
            };

            if (!System.IO.File.Exists("config.json"))
            {
                json = "";
                json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText("config.json", json);
            }
            else if (instance.OverwriteCheckBox.Checked == true)
            {
                File.Delete("config.json");
                json = "";
                json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText("config.json", json);
            }
            try
            {
                config = JsonConvert.DeserializeObject<Configuration>(System.IO.File.ReadAllText("config.json"));
                instance.PreviewBox.Text = json;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.StackTrace);
                reloadConfig();
            }
            instance.PreviewBox.Text = json;
        }

        public static void GenerateConfigPreview()
        {
            config = new Configuration
            {
                Name = instance.BotNameBox.Text,
                Master = Convert.ToBoolean(instance.MasterBox.Text),
                Token = instance.DiscordTokenBox.Text,
                Owner = Convert.ToUInt64(instance.OwnerIDBox.Text),
                FTPHost = instance.FTPHostBox.Text,
                FTPUser = instance.FTPUserBox.Text,
                FTPPass = instance.FTPPassBox.Text,
                AdminIDs = admins,
                Debug = true,
                FirstTimeSetup = true,
            };

            json = "";
            json = JsonConvert.SerializeObject(config, Formatting.Indented);
            instance.PreviewBox.Text = json;
        }

        public static void LoadDefaults()
        {
            if (!File.Exists("config.json"))
            {
                return;
            }
            config = JsonConvert.DeserializeObject<Configuration>(System.IO.File.ReadAllText("config.json"));

            if (config == null) { return; }

            foreach (var l in config.AdminIDs)
            {
                instance.AdminListBox.AppendText(l.ToString() + Environment.NewLine);
                admins.Add(l);
            }

            instance.BotNameBox.Text = config.Name;
            instance.MasterBox.Text = config.Master.ToString();

            instance.FTPHostBox.Text = config.FTPHost;
            instance.FTPUserBox.Text = config.FTPUser;
            instance.FTPPassBox.Text = config.FTPPass;
            instance.DiscordTokenBox.Text = config.Token;
            instance.OwnerIDBox.Text = config.Owner.ToString();

        }

        private async void materialFlatButton1_Click(object sender, EventArgs e)
        {
            instance.materialFlatButton1.Enabled = false;
            materialTabControl1.SelectTab(2);
            //await Task.Factory.StartNew(() => new UptimeRobot());
            GenerateConfigPreview();
            await DiscordBot.StartAsync();
        }

        public async Task Startsvr()
        {
            instance.materialFlatButton1.Enabled = false;
            materialTabControl1.SelectTab(2);
            //await Task.Factory.StartNew(() => new UptimeRobot());
            GenerateConfigPreview();
            await DiscordBot.StartAsync();
        }

        private void GeneralButton_Click(object sender, EventArgs e)
        {
            materialTabControl1.SelectedIndex = 0;
        }

        private void PermissionsButton_Click(object sender, EventArgs e)
        {
            materialTabControl1.SelectedIndex = 1;
        }

        private void materialFlatButton2_Click(object sender, EventArgs e)
        {
            materialTabControl1.SelectedIndex = 2;
        }
    }
}
