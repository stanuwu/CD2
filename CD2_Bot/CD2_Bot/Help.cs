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
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Summary("Opens the help menu.")]
        public async Task HelpAsync([Remainder] string xargs = null)
        {
            Embed helpembed = Utils.QuickEmbedNormal("Help", "Select a help page.");
            ComponentBuilder helpbuttons = new ComponentBuilder()
                       .WithButton("Character", "help;character", ButtonStyle.Primary, row: 0)
                       .WithButton("Stats", "help;stats", ButtonStyle.Primary, row: 0)
                       .WithButton("Play", "help;play", ButtonStyle.Primary, row: 0)
                       .WithButton("Money", "help;money", ButtonStyle.Primary, row: 1)
                       .WithButton("Rooms", "help;rooms", ButtonStyle.Primary, row: 1)
                       .WithButton("Misc.", "help;misc", ButtonStyle.Primary, row: 1);

            MessageComponent btn = helpbuttons.Build();

            await ReplyAsync(embed: helpembed, components: btn);
        }
    }
}
