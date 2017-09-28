using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus;
using System.Timers;
using DiscordBot.API;

namespace DiscordBot.Commands
{
    public class GeneralCommands
    {
        public static CommandContext Lastcmdcontext;

        [Command("test"), Aliases("tt"), Description("Says Test back")]
        public async Task SayTest(CommandContext ctx)
        {
            Discord_Bot.ConsoleBoxText("I got ya!");
            await ctx.RespondAsync("Test");
        }

        [Command("repeat"), Aliases("rep"), Description("Says what you said.")]
        public async Task Repeat(CommandContext ctx, string Repeat)
        {
            await ctx.RespondAsync(ctx.RawArgumentString);

            await ctx.Message.DeleteAsync();
        }

        [Command("link"), Aliases("linkage", "bondage"), Description("Login to twitch.")]
        public async Task Login(CommandContext ctx)
        {
            if (ctx.Member.Id == 183270722548793344)
            {
                await ctx.RespondAsync("Linking...");

                if (TwitchAPI.Linkage())
                {
                    await ctx.RespondAsync("Connected, :D");
                }
            }
            else
            {
                await ctx.RespondAsync("Yeah f**k off " + ctx.Member.Mention + " do I look like I know you?");
            }
        }

        [Command("tstats"), Aliases("linkstats"), Description("Gets stats.")]
        public async Task Stats(CommandContext ctx)
        {
            if (ctx.Member.Id == 183270722548793344)
            {
                await ctx.RespondAsync("Getting Stats.");
                if (!TwitchAPI.instance().JoinedChannels.Any()) { TwitchAPI.instance().JoinChannel("hd_neat"); }
                await ctx.RespondAsync("RoomID: " + TwitchAPI.instance().GetJoinedChannel("hd_neat").ChannelState.RoomId);

            }
            else
            {
                await ctx.RespondAsync("Yeah f**k off " + ctx.Member.Mention + " do I look like I know you?");
            }
        }

        [Command("commands"), Aliases("cmds"), Description("Returns this list")]
        public async Task ListAllCommands(CommandContext ctx)
        {
            var dm = await ctx.Client.CreateDmAsync(ctx.Member);
            List<DiscordEmbedField> fields = new List<DiscordEmbedField>();

            foreach (Command v in DiscordBot.CommandsNextService.RegisteredCommands.Values)
            {
                fields.Add(new DiscordEmbedField
                {
                    Name = "Command: !" + v.QualifiedName,
                    Value = " -Description: " + v.Description + "\n -Usage: !" + v.QualifiedName,
                    Inline = true,
                });
            }

            var embed = new DiscordEmbed
            {
                Title = "Commands: \n",
                Image = new DiscordEmbedImage
                {
                    Url = "https://i.lyhme.net/lyhmehosting.png",
                },
                Fields = fields,
                Color = 0x00f2ff,
                Provider = new DiscordEmbedProvider
                {
                    Name = "LYHME Hosting",
                    Url = "https://lyhmhosting.com"
                },
            };

            await dm.SendMessageAsync("\u200b", embed: embed);
            await ctx.RespondAsync(ctx.Member.Mention + " I have sent you a list of my commands via DM");
        }
    }
}
