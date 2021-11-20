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
    public class CharacterManagement : ModuleBase<SocketCommandContext>
    {
        [Command("start")]
        [Summary("Creates a character to start the game with.")]
        public async Task StartAsync([Remainder] string charname = null)
        {
            if (string.IsNullOrWhiteSpace(charname))
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("Please enter a name for your character."));
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

             NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO public.\"Character\" VALUES (@id, '', 'Player', 'Commoner', 0, 0, '', 0, 'Stick', 'None', 'None', ARRAY[]::varchar[], 0, false, 100);", db.dbc);
             cmd.Parameters.AddWithValue("@id", (Int64)Context.User.Id);
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
            string avatarurl = "";

            if (uid == 0) {
                avatarurl = Context.User.GetAvatarUrl();
                uid = Context.User.Id;
            }
            else
            {
                IUser founduser = Defaults.CLIENT.GetUser(uid);
                avatarurl = founduser.GetAvatarUrl();
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
                    Title = $"{stats.CharacterName} [LVL {stats.Lvl.ToString()}]",
                    Description = stats.Description,
                    ThumbnailUrl = avatarurl,
                };

                embed.AddField("Title", Convert.ToString(stats.Title));
                embed.AddField("Class", $"{Convert.ToString(stats.CharacterClass)} ");
                embed.AddField("Money", Convert.ToString(stats.Money),true);
                embed.AddField("EXP", Convert.ToString(stats.EXP), true);
                embed.AddField("HP", Convert.ToString(stats.HP) + "/" + Convert.ToString(stats.MaxHP), true);
                embed.AddField("Weapon", $"{Convert.ToString(stats.Weapon)} ", true);
                embed.AddField("Armor", $"{Convert.ToString(stats.Armor)} ", true);
                embed.AddField("Extra", $"{Convert.ToString(stats.Extra)} ", true);
                embed.AddField("Stat Multiplier", Convert.ToString(stats.StatMultiplier));
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

            IUserMessage msg = await ReplyAsync(embed: Utils.QuickEmbedNormal("Confirmation", "Are you sure you want to reset your character? **This will delete ALL your data.** \nTo confirm, react with :white_check_mark: (Expires in 5 seconds)"));
            await msg.AddReactionAsync(new Emoji("✅"));
            await Task.Delay(5000);
            List<IUser> reacted = (await msg.GetReactionUsersAsync(new Emoji("✅"), 5).FlattenAsync()).ToList();
            if (reacted.Select(user => user.Id).Contains(Context.User.Id))
            {
                stats.Deleted = true;
                await ReplyAsync(embed: Utils.QuickEmbedNormal("Success", "Successfully deleted character."));
            }
            else
            {
                await msg.RemoveReactionAsync(new Emoji("✅"), Defaults.CLIENT.CurrentUser);
            }
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
                    else if (toedit == "weapon")
                    {
                        stats.Weapon = xargs;
                    }
                    else if (toedit == "armor")
                    {
                        stats.Armor = xargs;
                    }
                    else if (toedit == "extra")
                    {
                        stats.Extra = xargs;
                    }
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
