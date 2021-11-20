using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CD2_Bot
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        [Summary("Test if the bot is working.")]
        public async Task TestAsync(string arg = null, [Remainder] string xargs = null)
        {
            switch (arg)
            {
                case null:
                    await ReplyAsync(embed: Utils.QuickEmbedError("Missing arguments"));
                    break;
                default:
                    ComponentBuilder btnb = new ComponentBuilder().WithButton("test", "testbtn1;"+ Context.User.Id.ToString(), ButtonStyle.Primary);
                    
                    MessageComponent btn = btnb.Build();
                    await ReplyAsync(embed: Utils.QuickEmbedNormal("Success", $"Arg: {arg}, Xargs: {xargs}"), component: btn);
                    break;
            }
        }
    }
}