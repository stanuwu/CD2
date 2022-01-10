using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Npgsql;

namespace CD2_Bot
{
    public class Guide : ModuleBase<SocketCommandContext>
    {
        [Command("guide")]
        [Summary("opens the guide.")]
        public async Task GuideAsync([Remainder] string xargs = null)
        {
            Embed guideembed = Utils.QuickEmbedNormal("Guide", "Select a guide page.");
            ComponentBuilder guidebuttons = new ComponentBuilder()
                       .WithButton("Start", "guide;start", ButtonStyle.Primary, row:0)
                       .WithButton("Character", "guide;character", ButtonStyle.Primary, row:0)
                       .WithButton("Gear", "guide;gear", ButtonStyle.Primary, row:0)
                       .WithButton("Floor", "guide;floor", ButtonStyle.Primary, row:1)
                       .WithButton("Fight", "guide;fight", ButtonStyle.Primary, row:1)
                       .WithButton("Chests", "guide;chests", ButtonStyle.Primary, row:1);


            MessageComponent btn = guidebuttons.Build();

            await ReplyAsync(embed: guideembed,components: btn);
        }
    }
}
