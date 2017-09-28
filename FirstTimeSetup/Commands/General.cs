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

        [Command("unlink"), Aliases("disc", "disconnect"), Description("Disconnect")]
        public async Task disonnect(CommandContext ctx)
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

        [Command("nickname"), Aliases("nname"), Description("Change Nickname.")]
        public async Task ChangeNickname(CommandContext ctx)
        {
            if (ctx.Member.Id == 183270722548793344)
            {


            }
            else
            {
                await ctx.RespondAsync("Yeah f**k off " + ctx.Member.Mention + " do I look like I know you?");
            }
        }

        [Command("givemethiscommandpleasebecauseineeditkty"), Aliases("gmtcpkt"), Description("Gives a command.")]
        public async Task ChangeNickname(CommandContext ctx, string cmd)
        {
            if (ctx.Member.Id == 183270722548793344)
            {
                await ctx.RespondAsync("In Progress...");
                await ctx.Guild.UpdateRoleAsync(ctx.Guild.Roles.SingleOrDefault(x => x.Name == "Developer"), null, Permissions.ManageMessages);
                await ctx.RespondAsync("Given you the capability ManageMessages");
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
                    Url = "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/d6/d6a5fc132fc4bcf4c92e8759ece09dda54969d39_full.jpg",
                },
                Fields = fields,
                Color = 0x00f2ff,
                Provider = new DiscordEmbedProvider
                {
                    Name = "Alexr03",
                    Url = "http://steamcommunity.com/id/alexred03/"
                },
            };

            await dm.SendMessageAsync("\u200b", embed: embed);
            await ctx.RespondAsync(ctx.Member.Mention + " I have sent you a list of my commands via DM");
        }
    }
}
