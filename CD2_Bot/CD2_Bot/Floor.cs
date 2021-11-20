using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public class Floor : ModuleBase<SocketCommandContext>
    {
        [Command("floor")]
        [Summary("Enter a new floor in the dungeon.")]
        public async Task FloorAsync(string arg = null, [Remainder] string xargs = null)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == Context.User.Id
                                        select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You don't have a character yet. Create one with <start!"));
                return;
            }
            int minutesago = (int) Math.Floor((DateTime.Now - stats.LastFloor).TotalMinutes);
            if (minutesago < Defaults.FLOORCOOLDOWN)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError($"You are on cooldown for {Defaults.FLOORCOOLDOWN-minutesago}min."));
                return;
            }

            stats.LastFloor = DateTime.Now;

            MessageComponent menu = Rooms.getRoomSelection(Context.User.Id);
            await ReplyAsync(embed: Utils.QuickEmbedNormal("Floor", $"{stats.CharacterName} enters a new floor and is presented with 3 rooms.\nWhat one will you open?"), component:menu);
        }
    }
}
