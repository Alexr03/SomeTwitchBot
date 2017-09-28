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
        public static void Test()
        {
            TwitchLib.TwitchAPI.Settings.ClientId = "pz6o9dsl95pbb85vgfb8ggf9vwmuee";
            TwitchLib.TwitchAPI.Settings.AccessToken = "vjazqh3f42xx4tn29u6bufjkrmrlkj";

            ConnectionCredentials credentials = new ConnectionCredentials("Alexr03", "oauth:ssxt2tcpz1ngw9jnacmh6h7rcet3sd");

            client = new TwitchClient(credentials, "hd_neat");
            client.OnJoinedChannel += onJoinedChannel;
            client.OnMessageReceived += onMessageReceived;
            client.OnWhisperReceived += onWhisperReceived;
            client.OnNewSubscriber += onNewSubscriber;
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
            DiscordBot.client.GetChannelAsync
        }
    }
}
