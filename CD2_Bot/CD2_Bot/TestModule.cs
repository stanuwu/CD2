using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace CD2_Bot
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        [Summary("Test if the bot is working.")]
        public async Task TestAsync(string arg = null, [Remainder] string xargs = null)
        {
            await ReplyAsync("The bot is working!");
            switch (arg)
            {
                case null:
                    break;
                default:
                    await Context.Channel.SendMessageAsync(arg);
                    break;
            }
        }
    }
}