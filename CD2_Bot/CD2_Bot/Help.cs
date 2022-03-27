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
    public static class Help
    {
        //"help" command
        public static async Task HelpAsync(SocketSlashCommand cmd)
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

            await cmd.RespondAsync(embed: helpembed, components: btn);
        }
    }
}
