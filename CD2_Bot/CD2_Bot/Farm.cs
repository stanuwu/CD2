﻿using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public class Farm
    {
        //"farm" command
        public static async Task FarmAsync(SocketSlashCommand cmd)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                where user.PlayerID == cmd.User.Id
                select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("You don't have a character yet. Create one with /start!"));
                return;
            }

            string arg = (string)cmd.Data.Options.First().Value;

            if (arg == "")
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("Please enter a category of farming."));
            }
            else
            {
                int minutesago = (int)Math.Floor((DateTime.Now - stats.LastFarm).TotalMinutes);
                if (minutesago < Defaults.FARMINGCOOLDOWN)
                {
                    await cmd.RespondAsync(embed: Utils.QuickEmbedError($"You are on cooldown for {Defaults.FARMINGCOOLDOWN - minutesago} minutes."));
                    return;
                }

                string sreward = "";
                string rreward = "";
                string gtext = "";
                switch (arg)
                {
                    case "fishing":
                        gtext = "You go fishing...";
                        sreward = "Fish Scale";
                        rreward = "Hydra Scale";
                        break;
                    case "mining":
                        gtext = "You go mining...";
                        sreward = "Stone";
                        rreward = "Diamond";
                        break;
                    case "foraging":
                        gtext = "You fell some trees...";
                        sreward = "Wood";
                        rreward = "Tropical Wood";
                        break;
                    case "harvesting":
                        gtext = "You search the forest for food...";
                        sreward = "Mushroom";
                        rreward = "Rare Root";
                        break;
                    case "hunting":
                        gtext = "You go hunting...";
                        sreward = "Small Game Pelt";
                        rreward = "Bear Pelt";
                        break;
                    default:
                        await cmd.RespondAsync(embed: Utils.QuickEmbedError("Please enter a valid category of farming."));
                        return;
                }

                if (stats.QuestData != "none")
                {
                    stats.Quest.UpdateProgress(stats, QuestActivations.FarmCompleteAny, arg);
                }

                stats.LastFarm = DateTime.Now;

                int sam = Defaults.GRandom.Next(1, 3);
                string rewards = "";

                Dictionary<string, int> inv = Utils.InvAsDict(stats);
                if (inv.ContainsKey(sreward))
                {
                    inv[sreward] += sam;
                }
                else
                {
                    inv.Add(sreward, sam);
                }

                rewards += $"+{sam}x {sreward}\n";
                if (Defaults.GRandom.Next(1, 10) == 5)
                {
                    if (inv.ContainsKey(rreward))
                    {
                        inv[rreward] += 1;
                    }
                    else
                    {
                        inv.Add(rreward, 1);
                    }

                    rewards += $"+1x {rreward}";
                }

                Utils.SaveInv(stats, inv);

                await cmd.RespondAsync(embed: Utils.QuickEmbedNormal("Farm", gtext + "\n" + rewards));
            }
        }
    }
}