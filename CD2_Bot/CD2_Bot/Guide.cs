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
    public static class Guide
    {
        //"guide" command
        public static async Task GuideAsync(SocketSlashCommand cmd)
        {
            Embed guideembed = Utils.QuickEmbedNormal("Guide", "Select a guide page.");
            ComponentBuilder guidebuttons = new ComponentBuilder()
                       .WithButton("Start", "guide;start", ButtonStyle.Primary, row:0)
                       .WithButton("Floor", "guide;floor", ButtonStyle.Primary, row: 0)
                       .WithButton("Fight", "guide;fight", ButtonStyle.Primary, row: 0)
                       .WithButton("Grinding", "guide;grinding", ButtonStyle.Primary, row: 0)
                       .WithButton("Character", "guide;character", ButtonStyle.Primary, row:1)
                       .WithButton("Gear", "guide;gear", ButtonStyle.Primary, row:1)
                       .WithButton("Quests", "guide;quests", ButtonStyle.Primary, row: 1);


            MessageComponent btn = guidebuttons.Build();

            await cmd.RespondAsync(embed: guideembed,components: btn);
        }
    }
}
