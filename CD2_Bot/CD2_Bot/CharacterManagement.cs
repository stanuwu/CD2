﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Npgsql;

namespace CD2_Bot
{
    public class CharacterManagement : ModuleBase<SocketCommandContext>
    {
        [Command("start")]
        [Summary("Creates a character to start the game with.")]
        public async Task StartAsync([Remainder] string charname = null)
        {
            if (string.IsNullOrWhiteSpace(charname))
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("Please enter a name for your character. \n(usage: `<start [Name]`)"));
                return;
            }
            else if(tempstorage.characters.Any(x => x.PlayerID == Context.User.Id))
            {
                CharacterStructure statst = (from user in tempstorage.characters
                                            where user.PlayerID == Context.User.Id
                                            select user).SingleOrDefault();
                if (statst.Deleted == true)
                {
                    NpgsqlCommand cmd2 = new NpgsqlCommand("DELETE FROM public.\"Character\" WHERE \"UserID\" = @id", db.dbc);
                    cmd2.Parameters.AddWithValue("@id", (Int64)Context.User.Id);
                    db.CommandVoid(cmd2);
                    tempstorage.characters.RemoveAll(c => c.PlayerID == Context.User.Id);
                }
                else
                {
                    await ReplyAsync(embed: Utils.QuickEmbedError("You already have a character! \n If you want to delete this character and create a new one, use <reset."));
                    return;
                }
            }
             if(charname.Length > 20)
             {
                 charname = charname.Substring(0, 20);
             }

             NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO public.\"Character\" VALUES (@id, '', 'Player', 'Commoner', 0, 0, '', 0, 'Stick', 'Rags', 'Pendant', ARRAY[]::varchar[], 0, false, 100, 0, 0, 0, @dt, 'none');", db.dbc);
             cmd.Parameters.AddWithValue("@id", (Int64)Context.User.Id);
             cmd.Parameters.AddWithValue("@dt", DateTime.Now.ToString());
             db.CommandVoid(cmd);

             tempstorage.characters.Add(new CharacterStructure(Context.User.Id));
             CharacterStructure stats = (from user in tempstorage.characters
                                         where user.PlayerID == Context.User.Id
                                         select user).SingleOrDefault();
             stats.CharacterName = charname;
             await ReplyAsync(embed: Utils.QuickEmbedNormal("Character created", $"Your character {charname} has been created!"));
         }


        [Command("character")]
        [Summary("View your character.")]
        public async Task CharacterAsync(ulong uid = 0, [Remainder] string xargs = null)
        {
            if (uid == 0)
            {
                uid = Context.User.Id;
            } 
            CharacterStructure stats = (from user in tempstorage.characters
                                            where user.PlayerID == uid
                                            select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You don't have a character! Create one with <start."));
            }
            else
            {
                using (MemoryStream ms = await ImageGenerator.MakeCharacterImage(stats))
                {
                    ms.Position = 0;
                    await Context.Channel.SendFileAsync(ms, stats.PlayerID + "_character.png");
                }
            }
        }


        [Command("stats")]
        [Summary("View your gear.")]
        public async Task StatsAsync(ulong uid = 0, [Remainder] string xargs = null)
        {
            string avatarurl = "";

            if (uid == 0)
            {
                avatarurl = Context.User.GetAvatarUrl();
                uid = Context.User.Id;
            }
            else
            {
                IUser founduser = Defaults.CLIENT.GetUser(uid);
                if (founduser != null)
                {
                    avatarurl = founduser.GetAvatarUrl();
                }
                else
                {
                    avatarurl = Defaults.CLIENT.CurrentUser.GetDefaultAvatarUrl();
                }
            }

            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == uid
                                        select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You don't have a character! Create one with <start."));
            }
            else
            {
                var embed = new EmbedBuilder
                {
                    Title = $"{stats.CharacterName} - Gear",
                    Description = "",
                    ThumbnailUrl = avatarurl,
                };

                embed.Description += "**Weapon**:\n" +
                    $"Name: {stats.Weapon.Name}\n" +
                    $"EXP: {stats.Weapon.EXP}\n" +
                    $"Level: {stats.Weapon.Level}\n" +
                    $"Damage: {stats.Weapon.Damage}\n\n";

                embed.Description += "**Armor**:\n" +
                    $"Name: {stats.Armor.Name}\n" +
                    $"EXP: {stats.Armor.EXP}\n" +
                    $"Level: {stats.Armor.Level}\n" +
                    $"Resistance: {stats.Armor.Resistance}\n\n";

                embed.Description += "**Extra**:\n" +
                    $"Name: {stats.Extra.Name}\n" +
                    $"EXP: {stats.Extra.EXP}\n" +
                    $"Level: {stats.Extra.Level}\n" +
                    $"Damage: {stats.Extra.Damage}\n" +
                    $"Heal: {stats.Extra.Heal}";
                embed.WithColor(Color.DarkMagenta);
                embed.WithFooter(Defaults.FOOTER);
                await ReplyAsync(embed: embed.Build());
            }
        }


        [Command("server")]
        [Summary("View ythe server you are currently in.")]
        public async Task ServerAsync([Remainder] string xargs = null)
        {
            GuildStructure gstats = tempstorage.guilds.Find(g => g.GuildID == Context.Guild.Id);

            var embed = new EmbedBuilder
            {
                Title = $"{Context.Guild.Name} - Stats",
                Description = $"Doors Opened: {gstats.DoorsOpened}\nBosses Slain: {gstats.BossesSlain}\nQuests Finished: {gstats.QuestsFinished}",
                ThumbnailUrl = Context.Guild.IconUrl,
            };
            embed.WithColor(Color.DarkMagenta);
            embed.WithFooter(Defaults.FOOTER);
            await ReplyAsync(embed: embed.Build());
        }


        [Command("inventory")]
        [Summary("View your inventory.")]
        public async Task InventoryAsync([Remainder] string xargs = null)
        {
            string avatarurl = Context.User.GetAvatarUrl();
            ulong uid = Context.User.Id;

            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == uid
                                        select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You don't have a character! Create one with <start."));
            }
            else
            {
                var embed = new EmbedBuilder
                {
                    Title = $"{stats.CharacterName} - Inventory",
                    Description = "",
                    ThumbnailUrl = avatarurl,
                };
                Dictionary<string, int> inv = Utils.InvAsDict(stats);
                foreach (string k in inv.Keys)
                {
                    embed.Description += $"{inv[k]}x {k}\n";
                }
                embed.WithColor(Color.DarkMagenta);
                embed.WithFooter(Defaults.FOOTER);
                await ReplyAsync(embed: embed.Build());
            }
        }


        [Command("reset")]
        [Summary("Deletes your character after asking for confirmation.")]
        public async Task ResetAsync([Remainder] string xargs = null)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == Context.User.Id
                                        select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You don't have a character to delete."));
                return;
            }
            MessageComponent btnb = new ComponentBuilder()
                        .WithButton("Delete", "delchar;confirm;" + Context.User.Id.ToString(), ButtonStyle.Danger)
                        .WithButton("Cancel", "delchar;cancel;" + Context.User.Id.ToString(), ButtonStyle.Secondary)
                        .Build();

            await ReplyAsync(embed: Utils.QuickEmbedNormal("Confirmation", "Are you sure you want to reset your character?\n" +
                "**This will delete ALL your data!**" +
                "\nExpires in 5 minutes."), components: btnb);
        }

        [Command("rename")]
        [Summary("Renames your character.")]
        public async Task RenameAsync([Remainder] string charname = null)
        {
            if (string.IsNullOrWhiteSpace(charname))
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("Please enter a name for your character!"));
            }
            else if (tempstorage.characters.Any(x => x.PlayerID == Context.User.Id))
            {
                if (charname.Length > 20)
                {
                    charname = charname.Substring(0, 20);
                }

                CharacterStructure stats = (from user in tempstorage.characters
                                            where user.PlayerID == Context.User.Id
                                            select user).SingleOrDefault();
                stats.CharacterName = charname;
                await ReplyAsync(embed: Utils.QuickEmbedNormal("Success", $"Your character has been renamed to {charname}!"));
            }
            else
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You don't have a character yet! Create one with <start."));
            }
        }

        [Command("description")]
        [Summary("Edit your character's description.")]
        public async Task DescriptionAsync([Remainder] string chardesc = null)
        {
            if (string.IsNullOrWhiteSpace(chardesc))
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("Please enter a description."));
            }
            else if (tempstorage.characters.Any(x => x.PlayerID == Context.User.Id))
            {
                if (chardesc.Length > 50)
                {
                    chardesc = chardesc.Substring(0, 50);
                }

                CharacterStructure stats = (from user in tempstorage.characters
                                            where user.PlayerID == Context.User.Id
                                            select user).SingleOrDefault();
                stats.Description = chardesc;
                await ReplyAsync(embed: Utils.QuickEmbedNormal("Success", "A new description has been set!"));
            }
            else
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You don't have a character yet! Create one with <start."));
            }
        }

        [Command("editcharacter")]
        [Summary("Change a character's stats.")]
        public async Task EditcharacterAsync(ulong userid = 0, string toedit = null, [Remainder] string xargs = null)
        {
            if (!Defaults.STAFF.Contains(Context.User.Id)) { return; }
            if (userid != 0 && toedit != null && xargs != null)
            {
                CharacterStructure stats = (from user in tempstorage.characters
                                            where user.PlayerID == userid
                                            select user).SingleOrDefault();

                if (stats != null)
                {
                    if(toedit == "name")
                    {
                        if (xargs.Length > 20)
                        {
                            xargs = xargs.Substring(0, 20);
                        }
                        stats.CharacterName = xargs;
                    }
                    else if (toedit == "title")
                    {
                        stats.Title = xargs;
                    }
                    else if (toedit == "description")
                    {
                        if (xargs.Length > 50)
                        {
                            xargs = xargs.Substring(0, 50);
                        }
                        stats.Description = xargs;
                    }
                    else if (toedit == "money")
                    {
                        stats.Money = Convert.ToInt32(xargs);
                    }
                    else if (toedit == "exp")
                    {
                        stats.EXP = Convert.ToInt32(xargs);
                    }
                    else if (toedit == "hp")
                    {
                        stats.MaxHP = Convert.ToInt32(xargs);
                    }
                    // else if (toedit == "weapon")
                    // {
                    //     stats.Weapon = xargs;
                    // }
                    // else if (toedit == "armor")
                    // {
                    //     stats.Armor = xargs;
                    // }
                    // else if (toedit == "extra")
                    // {
                    //     stats.Extra = xargs;
                    // }
                    else if (toedit == "statmultiplier")
                    {
                        stats.StatMultiplier = Convert.ToDouble(xargs);
                    }
                    else if (toedit == "deleted")
                    {
                        stats.Deleted = Convert.ToBoolean(xargs);
                    }
                    else if (toedit == "currhp")
                    {
                        stats.HP = Convert.ToInt32(xargs);
                    }
                    else
                    {
                        await ReplyAsync(embed: Utils.QuickEmbedError("Invalid argument"));
                        return;
                    }
                    await ReplyAsync(embed: Utils.QuickEmbedNormal("Success", "Stat changed"));
                }
                else
                {
                    await ReplyAsync(embed: Utils.QuickEmbedError("Invalid User"));
                }
            }
            else
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("Missing Arguments"));
            }
        }
    }
}
