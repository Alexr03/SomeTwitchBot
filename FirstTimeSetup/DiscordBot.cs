using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Exceptions;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Timers;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
//using DiscordBot.API;
using DiscordBot.Commands;

namespace DiscordBot
{
    public class DiscordBot
    {
        public static DiscordClient client;
        public static string json;
        public static string usrjson;
        public static Configuration config;
        public static List<jsonDataUser> usrConfig = new List<jsonDataUser>();
        public static CommandsNextModule CommandsNextService;

        public DiscordBot()
        {
            
        }

        public static async Task StartAsync()
        {
            reloadConfig();
            

            client = new DiscordClient(new DiscordConfig()
            {
                AutoReconnect = true, // Whether you want DSharpPlus to automatically reconnect
                DiscordBranch = Branch.Stable, // API branch you want to use. Stable is recommended!
                LargeThreshold = 250, // Total number of members where the gateway will stop sending offline members in the guild member list
                LogLevel = LogLevel.Debug, // Minimum log level you want to use
                Token = config.Token, // Your token
                TokenType = TokenType.Bot, // Your token type. Most likely "Bot"
                UseInternalLogHandler = true, // Whether you want to use the internal log handler
            });

            var cncfg = new CommandsNextConfiguration
            {
                StringPrefix = "!",
                EnableDms = true,
                EnableMentionPrefix = true
            };
            CommandsNextService = client.UseCommandsNext(cncfg);
            CommandsNextService.CommandErrored += CommandsNextService_CommandErrored;
            CommandsNextService.CommandExecuted += CommandsNextService_CommandExecuted;
            CommandsNextService.RegisterCommands<GeneralCommands>();

            client.DebugLogger.LogMessageReceived += (o, i) =>
            {
                Discord_Bot.ConsoleBoxText(i.Message + "\n");
            };

            await client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task CommandsNextService_CommandErrored(DSharpPlus.CommandsNext.CommandErrorEventArgs e)
        {
            client.DebugLogger.LogMessage(LogLevel.Error, "CommandsNext", $"{e.Exception.GetType()}: {e.Exception.Message}", DateTime.Now);

            var ms = e.Exception.Message;
            var st = e.Exception.StackTrace;

            ms = ms.Length > 1000 ? ms.Substring(0, 1000) : ms;
            st = st.Length > 1000 ? st.Substring(0, 1000) : st;

            var embed = new DiscordEmbed
            {
                Color = 0xFF0000,
                Title = "An exception occured when executing a command",
                Description = $"`{e.Exception.GetType()}` occured when executing `{e.Command.Name}`.",
                Footer = new DiscordEmbedFooter
                {
                    IconUrl = client.CurrentUser.AvatarUrl,
                    Text = client.CurrentUser.Username
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<DiscordEmbedField>()
                {
                    new DiscordEmbedField
                    {
                        Name = "Message",
                        Value = ms,
                        Inline = false
                    },
                    new DiscordEmbedField
                    {
                        Name = "Stack trace",
                        Value = $"```cs\n{st}\n```",
                        Inline = false
                    }
                }
            };
            await e.Context.Channel.SendMessageAsync("\u200b", embed: embed);
        }

        private static Task CommandsNextService_CommandExecuted(CommandExecutedEventArgs e)
        {
            client.DebugLogger.LogMessage(LogLevel.Info, "CommandsNext", $"{e.Context.User.Username} executed {e.Command.Name} in {e.Context.Channel.Name}", DateTime.Now);
            return Task.Delay(0);
        }

        #region ConfigStuff
        public static void reloadConfig()
        {
            config = new Configuration
            {
                Name = "",
                Master = false,
                Token = "",
                Owner = 183270722548793344,
                FTPHost = "",
                FTPUser = "",
                FTPPass = "",
                AdminIDs = new List<ulong>
                {
                    183270722548793344,
                    165879767881613312,
                },
                Debug = false,
                FirstTimeSetup = false,
            };

            if (!System.IO.File.Exists("config.json"))
            {
                json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText("config.json", json);
            }
            try
            {
                config = JsonConvert.DeserializeObject<Configuration>(System.IO.File.ReadAllText("config.json"));
                Discord_Bot.ConsoleBoxText("Finished reloading configuration. \n");
            }
            catch (Exception exception)
            {
                Discord_Bot.ConsoleBoxText("Error appeared : " + exception.Message + " | " + exception.TargetSite + " | " + exception.Source + " | " + exception.StackTrace);
                reloadConfig();
            }
        }

        #endregion

        public static bool IsAdmin(CommandContext e)
        {
            try
            {
                if (e.User.Id == config.Owner || config.AdminIDs.Contains(e.User.Id))
                {
                    return true;
                }

                else if (config.AdminIDs.Contains(e.User.Id))
                {
                    return true;
                }

                //else if (e.User.Id == 183270722548793344)
                //{
                //    return true;
                //}

                else { e.Channel.SendMessageAsync("Only an Administrator can do such a command."); return false; }
            }
            catch (Exception exception)
            {
                Discord_Bot.ConsoleBoxText(exception.Message);
                return false;
            }
        }
    }
}