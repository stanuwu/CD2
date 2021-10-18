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
    }
}