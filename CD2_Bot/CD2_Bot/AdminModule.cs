using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace CD2_Bot
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("guilds")]
        [Summary("List the number of guilds (servers) the bot is in.")]
        public async Task GuildsAsync([Remainder] string xargs = null)
        {
            if (!Defaults.STAFF.Contains(Context.User.Id)) { return; }
            await ReplyAsync(Convert.ToString(await Utils.GuildCount(Context)));
        }

        [Command("users")]
        [Summary("List the number of unique users in all the bots guilds.")]
        public async Task UsersAsync([Remainder] string xargs = null)
        {
            if (!Defaults.STAFF.Contains(Context.User.Id)) { return; }
            await ReplyAsync(Convert.ToString(await Utils.UniqueUserCount(Context)));
        }

        [Command("reload")]
        [Summary("Reconnect the client to discord to potentially solve issues.")]
        public async Task ReloadAsync([Remainder] string xargs = null)
        {
            if (!Defaults.STAFF.Contains(Context.User.Id)) { return; }
            Utils.RestartClient(Context);
            await ReplyAsync("Reloaded!");
        }

        [Command("status")]
        [Summary("Set the GameActivity status to a string.")]
        public async Task StatusAsync([Remainder] string xargs = null)
        {
            if (!Defaults.STAFF.Contains(Context.User.Id)) { return; }
            if (String.IsNullOrEmpty(xargs))
            {
                await ReplyAsync("Provide a status to set!");
            } else
            {
                Utils.UpdateStatus(xargs);
                await ReplyAsync("Status Updated!");
            }
            
        }

        [Command("broadcast")]
        [Summary("Send message to every server the bot is in.")]
        public async Task BroadcastAsync([Remainder] string xargs = null)
        {
            if (!Defaults.STAFF.Contains(Context.User.Id)) { return; }
            if (String.IsNullOrEmpty(xargs))
            {
                await ReplyAsync("Enter a message to send out!");
            }
            else
            {
                Utils.SendBroadcast(Context, xargs);
                await ReplyAsync("Sent Broadcast!");
            }

        }
    }
}