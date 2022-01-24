using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public class Train : ModuleBase<SocketCommandContext>
    {
        [Command("train")]
        [Summary("Train with your gear to increase its level.")]
        public async Task TrainAsync(string arg = "", [Remainder] string xargs = null)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == Context.User.Id
                                        select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You don't have a character yet. Create one with <start!"));
                return;
            }
            int minutesago = (int)Math.Floor((DateTime.Now - stats.LastTrain).TotalMinutes);
            if (minutesago < Defaults.FLOORCOOLDOWN / 2)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError($"You are on cooldown for {Defaults.FLOORCOOLDOWN / 2 - minutesago} minutes."));
                return;
            }

            stats.LastTrain = DateTime.Now;

            int trainchance = Defaults.GRandom.Next(0, 100);

            if (stats.QuestData != "none")
            {
                stats.Quest.UpdateProgress(stats, QuestActivations.TrainCompleteAny, arg.ToLower());
            }
            switch (arg.ToLower())
            {
                case "weapon":
                    if (trainchance >= 80)
                    {
                        stats.WeaponXP += 25;
                        await ReplyAsync(embed: Utils.QuickEmbedNormal("Train", $"Great success! Your {stats.Weapon.Name} earns 25 XP!"));
                    }
                    else if (trainchance < 90 && trainchance >= 10)
                    {
                        stats.WeaponXP += 15;
                        await ReplyAsync(embed: Utils.QuickEmbedNormal("Train", $"Success! Your {stats.Weapon.Name} earns 15 XP!"));
                    }
                    else
                    {
                        stats.WeaponXP += 7;
                        await ReplyAsync(embed: Utils.QuickEmbedNormal("Train", $"That didn't go so well... Your {stats.Weapon.Name} earns 7 XP!"));
                    }
                    break;

                case "armor":
                    if (trainchance >= 80)
                    {
                        stats.ArmorXP += 25;
                        await ReplyAsync(embed: Utils.QuickEmbedNormal("Train", $"Great success! Your {stats.Armor.Name} earns 25 XP!"));
                    }
                    else if (trainchance < 90 && trainchance >= 10)
                    {
                        stats.ArmorXP += 15;
                        await ReplyAsync(embed: Utils.QuickEmbedNormal("Train", $"Success! Your {stats.Armor.Name} earns 15 XP!"));
                    }
                    else
                    {
                        stats.ArmorXP += 7;
                        await ReplyAsync(embed: Utils.QuickEmbedNormal("Train", $"That didn't go so well... Your {stats.Armor.Name} earns 7 XP!"));
                    }
                    break;

                case "extra":
                    if (trainchance >= 80)
                    {
                        stats.ExtraXP += 25;
                        await ReplyAsync(embed: Utils.QuickEmbedNormal("Train", $"Great success! Your {stats.Extra.Name} earns 25 XP!"));
                    }
                    else if (trainchance < 90 && trainchance >= 10)
                    {
                        stats.ExtraXP += 15;
                        await ReplyAsync(embed: Utils.QuickEmbedNormal("Train", $"Success! Your {stats.Weapon.Name} earns 15 XP!"));
                    }
                    else
                    {
                        stats.ExtraXP += 7;
                        await ReplyAsync(embed: Utils.QuickEmbedNormal("Train", $"That didn't go so well... Your {stats.Extra.Name} earns 7 XP!"));
                    }
                    break;

                default:
                    if (trainchance >= 90)
                    {
                        stats.WeaponXP += 16;
                        stats.ArmorXP += 16;
                        stats.ExtraXP += 16;
                        await ReplyAsync(embed: Utils.QuickEmbedNormal("Train", $"Great success! Your gear earns 16 XP!"));
                    }
                    else if (trainchance < 90 && trainchance >= 10)
                    {
                        stats.WeaponXP += 10;
                        stats.ArmorXP += 10;
                        stats.ExtraXP += 10;
                        await ReplyAsync(embed: Utils.QuickEmbedNormal("Train", $"Success! Your gear earns 10 XP!"));
                    }
                    else
                    {
                        stats.WeaponXP += 5;
                        stats.ArmorXP += 5;
                        stats.ExtraXP += 5;
                        await ReplyAsync(embed: Utils.QuickEmbedNormal("Train", $"That didn't go so well... Your gear earns 5 XP!"));
                    }
                    break;
            }
        }
    }
}