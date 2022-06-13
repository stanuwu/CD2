using System;
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
    public static class CharacterManagement
    {
        //"start" command
        public static async Task StartAsync(SocketSlashCommand cmd)
        {
            string charname = "";
            if (cmd.Data.Options.Count < 1)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("Please enter a name for your character. \n(usage: `/start [Name]`)"));
                return;
            }
            else
            {
                charname = (string)cmd.Data.Options.First().Value;
            }

            if (tempstorage.characters.Any(x => x.PlayerID == cmd.User.Id))
            {
                CharacterStructure statst = (from user in tempstorage.characters
                    where user.PlayerID == cmd.User.Id
                    select user).SingleOrDefault();
                if (statst.Deleted == true)
                {
                    NpgsqlCommand cmd2 = new NpgsqlCommand("DELETE FROM public.\"Character\" WHERE \"UserID\" = @id", db.dbc);
                    cmd2.Parameters.AddWithValue("@id", (Int64)cmd.User.Id);
                    db.CommandVoid(cmd2);
                    tempstorage.characters.RemoveAll(c => c.PlayerID == cmd.User.Id);
                }
                else
                {
                    await cmd.RespondAsync(
                        embed: Utils.QuickEmbedError(
                            "You already have a character! \n If you want to delete this character and create a new one, use <reset."));
                    return;
                }
            }

            if (charname.Length > 20)
            {
                charname = charname.Substring(0, 20);
            }

            NpgsqlCommand dbcmd =
                new NpgsqlCommand(
                    "INSERT INTO public.\"Character\" VALUES (@id, '', 'Player', 'Commoner', 0, 0, '', 0, 'Stick', 'Rags', 'Pendant', ARRAY[]::varchar[], 0, false, 100, 0, 0, 0, @dt, 'none');",
                    db.dbc);
            dbcmd.Parameters.AddWithValue("@id", (Int64)cmd.User.Id);
            dbcmd.Parameters.AddWithValue("@dt", DateTime.Now.ToString());
            db.CommandVoid(dbcmd);

            tempstorage.characters.Add(new CharacterStructure(cmd.User.Id));
            CharacterStructure stats = (from user in tempstorage.characters
                where user.PlayerID == cmd.User.Id
                select user).SingleOrDefault();
            stats.CharacterName = charname;
            await cmd.RespondAsync(embed: Utils.QuickEmbedNormal("Character created", $"Your character {charname} has been created!"));
        }


        //"character" command
        public static async Task CharacterAsync(SocketSlashCommand cmd)
        {
            ulong uid = 0;
            if (cmd.Data.Options.Count < 1)
            {
                uid = cmd.User.Id;
            }
            else
            {
                uid = ((SocketUser)cmd.Data.Options.First().Value).Id;
            }

            CharacterStructure stats = (from user in tempstorage.characters
                where user.PlayerID == uid
                select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("You don't have a character! Create one with /start."));
            }
            else
            {
                using (MemoryStream ms = await ImageGenerator.MakeCharacterImage(stats))
                {
                    Utils.SendFileAsyncFast(cmd, ms.ToArray(), stats.PlayerID + "_character.png");
                }
            }
        }


        //"stats" command
        public static async Task StatsAsync(SocketSlashCommand cmd)
        {
            string avatarurl = "";
            ulong uid = 0;
            if (cmd.Data.Options.Count < 1)
            {
                avatarurl = cmd.User.GetAvatarUrl();
                uid = cmd.User.Id;
            }
            else
            {
                uid = ((SocketUser)cmd.Data.Options.First().Value).Id;
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
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("You don't have a character! Create one with /start."));
            }
            else
            {
                using (MemoryStream ms = await ImageGenerator.MakeStatsImage(stats))
                {
                    Utils.SendFileAsyncFast(cmd, ms.ToArray(), stats.PlayerID + "_stats.png");
                }
            }
        }


        //"server" command
        public static async Task ServerAsync(SocketSlashCommand cmd)
        {
            GuildStructure gstats = tempstorage.guilds.Find(g => g.GuildID == ((IGuildChannel)cmd.Channel).Guild.Id);

            var embed = new EmbedBuilder
            {
                Title = $"{((IGuildChannel)cmd.Channel).Guild.Name} - Stats",
                Description = $"Doors Opened: {gstats.DoorsOpened}\nBosses Slain: {gstats.BossesSlain}\nQuests Finished: {gstats.QuestsFinished}",
                ThumbnailUrl = ((IGuildChannel)cmd.Channel).Guild.IconUrl,
            };
            embed.WithColor(Color.DarkMagenta);
            embed.WithFooter(Defaults.FOOTER);
            await cmd.RespondAsync(embed: embed.Build());
        }


        //"inventory" command
        public static async Task InventoryAsync(SocketSlashCommand cmd)
        {
            string avatarurl = cmd.User.GetAvatarUrl();
            ulong uid = cmd.User.Id;

            CharacterStructure stats = (from user in tempstorage.characters
                where user.PlayerID == uid
                select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("You don't have a character! Create one with /start."));
            }
            else
            {
                using (MemoryStream ms = await ImageGenerator.MakeInventoryImage(stats))
                {
                    Utils.SendFileAsyncFast(cmd, ms.ToArray(), stats.PlayerID + "_inventory.png");
                }
            }
        }


        //"reset" command
        public static async Task ResetAsync(SocketSlashCommand cmd)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                where user.PlayerID == cmd.User.Id
                select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("You don't have a character to delete."));
                return;
            }

            MessageComponent btnb = new ComponentBuilder()
                .WithButton("Delete", "delchar;confirm;" + cmd.User.Id.ToString(), ButtonStyle.Danger)
                .WithButton("Cancel", "delchar;cancel;" + cmd.User.Id.ToString(), ButtonStyle.Secondary)
                .Build();

            await cmd.RespondAsync(embed: Utils.QuickEmbedNormal("Confirmation", "Are you sure you want to reset your character?\n" +
                                                                                 "**This will delete ALL your data!**" +
                                                                                 "\nExpires in 5 minutes."), components: btnb);
        }

        //"rename" command
        public static async Task RenameAsync(SocketSlashCommand cmd)
        {
            if (cmd.Data.Options.Count < 1)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("Please enter a name for your character!"));
            }
            else if (tempstorage.characters.Any(x => x.PlayerID == cmd.User.Id))
            {
                string charname = (string)cmd.Data.Options.First().Value;
                if (charname.Length > 20)
                {
                    charname = charname.Substring(0, 20);
                }

                CharacterStructure stats = (from user in tempstorage.characters
                    where user.PlayerID == cmd.User.Id
                    select user).SingleOrDefault();
                stats.CharacterName = charname;
                await cmd.RespondAsync(embed: Utils.QuickEmbedNormal("Success", $"Your character has been renamed to {charname}!"));
            }
            else
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("You don't have a character yet! Create one with /start."));
            }
        }

        //"description" command
        public static async Task DescriptionAsync(SocketSlashCommand cmd)
        {
            if (cmd.Data.Options.Count < 1)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("Please enter a description."));
            }
            else if (tempstorage.characters.Any(x => x.PlayerID == cmd.User.Id))
            {
                string chardesc = (string)cmd.Data.Options.First().Value;
                if (chardesc.Length > 50)
                {
                    chardesc = chardesc.Substring(0, 50);
                }

                CharacterStructure stats = (from user in tempstorage.characters
                    where user.PlayerID == cmd.User.Id
                    select user).SingleOrDefault();
                stats.Description = chardesc;
                await cmd.RespondAsync(embed: Utils.QuickEmbedNormal("Success", "A new description has been set!"));
            }
            else
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("You don't have a character yet! Create one with /start."));
            }
        }

        //"editcharacter" command
        public static async Task EditcharacterAsync(SocketSlashCommand cmd)
        {
            if (!Defaults.STAFF.Contains(cmd.User.Id))
            {
                return;
            }

            if (cmd.Data.Options.Count >= 3)
            {
                ulong userid = Convert.ToUInt64((string)cmd.Data.Options.First().Value);
                string toedit = (string)cmd.Data.Options.ToList()[1].Value;
                string xargs = (string)cmd.Data.Options.ToList()[2].Value;
                CharacterStructure stats = (from user in tempstorage.characters
                    where user.PlayerID == userid
                    select user).SingleOrDefault();

                if (stats != null)
                {
                    if (toedit == "name")
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
                        await cmd.RespondAsync(embed: Utils.QuickEmbedError("Invalid argument"));
                        return;
                    }

                    await cmd.RespondAsync(embed: Utils.QuickEmbedNormal("Success", "Stat changed"));
                }
                else
                {
                    await cmd.RespondAsync(embed: Utils.QuickEmbedError("Invalid User"));
                }
            }
            else
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("Missing Arguments"));
            }
        }
    }
}