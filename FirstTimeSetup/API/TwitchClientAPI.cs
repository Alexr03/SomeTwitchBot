using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;
using System.Windows.Forms;
using DSharpPlus;

namespace DiscordBot.API
{
    public class TwitchAPI
    {
        private static TwitchClient client;
        public static bool Linkage()
        {
            ConnectionCredentials credentials = new ConnectionCredentials("Alexr03", "b8nv5cwrm5i8qrayuyf9lf2k8jyves");

            client = new TwitchClient(credentials, "hd_neat");
            client.Connect();

            client.OnModeratorJoined += onModJoined;
            client.OnConnectionError += onConnectionError;
            client.OnConnected += onConnected;
            client.OnJoinedChannel += onJoinedChannel;
            client.OnMessageReceived += onMessageReceived;
            client.OnHostingStarted += onHostingStarted;
            client.OnNewSubscriber += onNewSubscriber;

            if (client.IsConnected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void onModJoined(object sender, OnModeratorJoinedArgs e)
        {
            DiscordEmbed embed = new DiscordEmbed
            {
                Color = 0x00ffff,
                Image = new DiscordEmbedImage
                {
                    Url = "https://static-cdn.jtvnw.net/jtv_user_pictures/59b24d4b225f7df6-profile_image-300x300.jpeg"
                },
                Author = new DiscordEmbedAuthor
                {
                    Name = DiscordBot.client.CurrentUser.Username,
                    IconUrl = DiscordBot.client.CurrentUser.AvatarUrl
                },
                Title = "Watchout, Moderator " + e.Username + " entered the stream.",
                Url = "https://www.twitch.tv/hd_neat",
            };

            var chan = DiscordBot.client.GetChannelAsync(362888040928116736).Result.SendMessageAsync("\n", embed: embed);
        }

        private static void onHostingStarted(object sender, OnHostingStartedArgs e)
        {
            DiscordEmbed embed = new DiscordEmbed
            {
                Color = 0x00ffff,
                Image = new DiscordEmbedImage
                {
                    Url = "https://static-cdn.jtvnw.net/jtv_user_pictures/59b24d4b225f7df6-profile_image-300x300.jpeg"
                },
                Author = new DiscordEmbedAuthor
                {
                    Name = DiscordBot.client.CurrentUser.Username,
                    IconUrl = DiscordBot.client.CurrentUser.AvatarUrl
                },
                Title = "HD_Neat has started hosting " + e.TargetChannel + " over at: https://www.twitch.tv/hd_neat",
                Url = "https://www.twitch.tv/hd_neat",
            };

            var chan = DiscordBot.client.GetChannelAsync(362888040928116736).Result.SendMessageAsync("\n", embed: embed);
        }

        private static void onConnected(object sender, OnConnectedArgs e)
        {
            Discord_Bot.ConsoleBoxText("Connected. " + e.BotUsername);
        }

        private static void onConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Discord_Bot.ConsoleBoxText("ERROR WHEN CONNECTING.... " + e.Error + "\n Name: " + e.BotUsername);
        }

        public static TwitchClient instance()
        {
            return client;
        }

        private static void onNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            DiscordEmbed embed = new DiscordEmbed
            {
                Color = 0x00ffff,
                Author = new DiscordEmbedAuthor
                {
                    Name = DiscordBot.client.CurrentUser.Username,
                    IconUrl = DiscordBot.client.CurrentUser.AvatarUrl
                },
                Title = e.Subscriber.DisplayName + " has subscribed to " + e.Channel,
                Url = "https://www.twitch.tv/hd_neat",
            };

            var chan = DiscordBot.client.GetChannelAsync(362888040928116736).Result.SendMessageAsync("\n", embed: embed);
        }

        private static void onWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            
        }

        private static void onMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            Discord_Bot.ConsoleBoxText(string.Format("{0}: {1}", e.ChatMessage.Username, e.ChatMessage.Message));

            DiscordEmbed embed = new DiscordEmbed
            {
                Color = 0x00ffff,
                Title = "-LIVE CHAT-",
                Url = "https://www.twitch.tv/hd_neat",
                Fields = new List<DiscordEmbedField>
                {
                    new DiscordEmbedField
                    {
                        Name = e.ChatMessage.DisplayName,
                        Value = e.ChatMessage.Message + "\n | Subscribed: " + e.ChatMessage.IsSubscriber + ", Moderator: " + e.ChatMessage.IsModerator,
                    }
                }
            };
            var chan = DiscordBot.client.GetChannelAsync(362888040928116736).Result.SendMessageAsync("\n", embed: embed);
        }

        private static void onJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Discord_Bot.ConsoleBoxText("Joined channel: " + e.Channel + " with: " + e.BotUsername);
            var chan = DiscordBot.client.GetChannelAsync(362888040928116736).Result.SendMessageAsync("Joined channel: " + e.Channel + " with bot: " + e.BotUsername);
        }
    }
}
