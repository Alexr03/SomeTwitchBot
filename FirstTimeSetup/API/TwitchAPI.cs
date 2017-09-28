using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;

namespace DiscordBot.API
{
    public class TwitchAPI
    {
        private static TwitchClient client;
        public static bool Linkage()
        {
            ConnectionCredentials credentials = new ConnectionCredentials("Alexr03", "b8nv5cwrm5i8qrayuyf9lf2k8jyves");

            client = new TwitchClient(credentials, "hd_neat");
            client.OnJoinedChannel += onJoinedChannel;
            client.OnMessageReceived += onMessageReceived;
            client.OnWhisperReceived += onWhisperReceived;
            client.OnNewSubscriber += onNewSubscriber;

            if (client.IsConnected)
            {
                return true;
            }

            return false;
        }

        public static TwitchClient instance()
        {
            return client;
        }

        private static void onNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            
        }

        private static void onWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            
        }

        private static void onMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            
        }

        private static void onJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Discord_Bot.ConsoleBoxText("Joined channel: " + e.Channel + " with: " + e.BotUsername);
            var chan = DiscordBot.client.GetChannelAsync(362888040928116736).Result.SendMessageAsync("Joined channel: " + e.Channel + " with: " + e.BotUsername);
        }
    }
}
