using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace CD2_Bot
{
    public static class Utils
    {
        public static async void DebugLog(string msg)
        {
            await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Debug, "Debugging", msg));
        }

        public static async Task<int> GuildCount(ICommandContext ctx)
        {
            IReadOnlyCollection<Discord.IGuild> guilds = await ctx.Client.GetGuildsAsync();
            return guilds.Count;

        }

        public static async Task<int> UniqueUserCount(ICommandContext ctx)
        {
            IReadOnlyCollection<Discord.IGuild> guilds = await ctx.Client.GetGuildsAsync();
            List<IUser> uusers = new List<IUser> { };

            foreach (IGuild g in guilds)
            {
                IReadOnlyCollection<Discord.IGuildUser> tusers = await g.GetUsersAsync();
                foreach (IGuildUser gu in tusers)
                {
                    if (!uusers.Any(x=> x.Id==gu.Id))
                    {
                        uusers.Add(gu);
                    }
                }
            }

            return uusers.Count;
        }

        public static async void RestartClient(ICommandContext ctx)
        {
            await ctx.Client.StopAsync();
            await ctx.Client.StartAsync();
        }

        public static async void UpdateStatus(string msg)
        {
            await Defaults.CLIENT.SetGameAsync(msg);
        }

        public static async void SendBroadcast(ICommandContext ctx, string msg)
        {
            IReadOnlyCollection<Discord.IGuild> guilds = await ctx.Client.GetGuildsAsync();
            foreach (IGuild g in guilds)
            {
                ITextChannel channel = await g.GetDefaultChannelAsync();
                await channel.SendMessageAsync(msg);
            }

        }

        public static Embed QuickEmbedNormal(string title, string description)
        {
            var embed = new EmbedBuilder
            {
                Title = title,
                Description = description
            };

            embed.WithColor(Color.Teal);
            embed.WithFooter(Defaults.FOOTER);

            return embed.Build();
        }

        public static Embed QuickEmbedError(string description)
        {
            var embed = new EmbedBuilder
            {
                Title = "Error",
                Description = description
            };

            embed.WithColor(Color.DarkRed);
            embed.WithFooter(Defaults.FOOTER);

            return embed.Build();
        }

        public static Embed QuickEmbedBotinfo()
        {
            var embed = new EmbedBuilder
            {
                Title = "CustomDungeons 2 - Info",
                Description = "This bot.",
                ThumbnailUrl = Defaults.BOTIMG
            };

            embed.WithColor(Color.Purple);
            embed.WithFooter(Defaults.FOOTER);

            return embed.Build();
        }
        public static Embed QuickEmbedMenu(Dictionary<string, string> menuitems)
        {
            var embed = new EmbedBuilder
            {
                Title = "Menu",
                Description = "Basic menu"
            };

            foreach (string k in menuitems.Keys)
            {
                embed.AddField(k, menuitems[k], true);
            }

            embed.WithColor(Color.Gold);
            embed.WithFooter(Defaults.FOOTER);

            return embed.Build();
        }

        public static Embed QuickEmbedList(List<string> listitems)
        {
            var embed = new EmbedBuilder
            {
                Title = "List"
            };

            foreach (string l in listitems)
            {
                embed.Description += $"- {l}\n";
            }

            embed.WithColor(Color.Magenta);
            embed.WithFooter(Defaults.FOOTER);

            return embed.Build();
        }
    }
}