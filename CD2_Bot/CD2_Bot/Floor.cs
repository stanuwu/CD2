using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public static class Floor
    {
        //"floor" command
        public static async Task FloorAsync(SocketSlashCommand cmd)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                where user.PlayerID == cmd.User.Id
                select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("You don't have a character yet. Create one with /start!"));
                return;
            }

            if (stats.HP < stats.MaxHP / 4)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("You don't have enough health."));
                return;
            }

            int minutesago = (int)Math.Floor((DateTime.Now - stats.LastFloor).TotalMinutes);
            if (minutesago < Defaults.FLOORCOOLDOWN)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError($"You are on cooldown for {Defaults.FLOORCOOLDOWN - minutesago} minutes."));
                return;
            }

            stats.LastFloor = DateTime.Now;

            MessageComponent menu = Rooms.getRoomSelection(cmd.User.Id);
            await cmd.RespondAsync(
                embed: Utils.QuickEmbedNormal("Floor", $"{stats.CharacterName} enters a new floor and is presented with 5 rooms.\nWhat one will you open?"),
                components: menu);
            if (stats.QuestData != "none")
            {
                stats.Quest.UpdateProgress(stats, QuestActivations.EnterFloor);
            }
        }
    }
}