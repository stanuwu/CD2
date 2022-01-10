﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public class PlayerCommands : ModuleBase<SocketCommandContext>
    {
        [Command("pvp")]
        [Summary("Play against another user to test your strength.")]
        public async Task PlayerFightRequestAsync(SocketUser opponent = null, int wager = 0, [Remainder] string xargs = null)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == Context.User.Id
                                        select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You do not have a character."));
                return;
            }

            if (wager < 0)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("Enter a valid wager."));
                return;
            }

            if (opponent == null)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("Please tag an opponent."));
                return;
            }
            else
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Title = "Player Fight",
                    Description = $"{stats.CharacterName} has invited <@{opponent.Id}> to a fight for {wager} coins!\nThis expires after 15 minutes."
                };
                embed.WithFooter(Defaults.FOOTER);
                embed.WithColor(Color.Magenta);

                ComponentBuilder btnb = new ComponentBuilder()
                        .WithButton("Accept", "playerfight;ask;" + Context.User.Id.ToString() + ";" + opponent.Id.ToString() + ";" + wager.ToString(), ButtonStyle.Success)
                        .WithButton("Cancel", "playerfight;cancel;" + Context.User.Id.ToString() + ";" + opponent.Id.ToString(), ButtonStyle.Danger);

                MessageComponent btn = btnb.Build();

                await ReplyAsync(embed: embed.Build(), components: btn);
            }
        }
    }
}
