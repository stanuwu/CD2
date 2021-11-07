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
                await ReplyAsync("Please enter a name for your character!");
            }
            else if(tempstorage.characters.Any(x => x.PlayerID == Context.User.Id))
            {
                await ReplyAsync("You already have a character! \n If you want to delete this character and create a new one, use <reset.");
            }
            else
            {
                if(charname.Length > 20)
                {
                    charname = charname.Substring(0, 20);
                }

                NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO public.\"Character\" VALUES (@id, '', 'Player', '', 0, 0, '', 0, '', '', '', ARRAY[]::varchar[], 1);", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)Context.User.Id);
                db.CommandVoid(cmd);

                tempstorage.characters.Add(new CharacterStructure(Context.User.Id));
                CharacterStructure stats = (from user in tempstorage.characters
                                            where user.PlayerID == Context.User.Id
                                            select user).SingleOrDefault();
                stats.CharacterName = charname;
                await ReplyAsync($"Character {charname} created!");
            }

        }

        [Command("character")]
        [Summary("View your character.")]
        public async Task CharacterAsync([Remainder] string xargs = null)
        {

            CharacterStructure stats = (from user in tempstorage.characters
                                       where user.PlayerID == Context.User.Id
                                       select user).SingleOrDefault();

            if (stats == null)
            {
                await ReplyAsync("You don't have a character! Create one with <start.");
            }
            else
            {
                await ReplyAsync($"Name: {stats.CharacterName} \n" +
                    $"Title: {stats.Title} \n" +
                    $"Description: {stats.Description} \n" +
                    $"Class: {stats.CharacterClass} \n" +
                    $"Money: {stats.Money} \n" +
                    $"EXP: {stats.EXP} \n" +
                    $"HP: {stats.HP} \n" +
                    $"Weapon: {stats.Weapon} \n" +
                    $"Armor: {stats.Armor} \n" +
                    $"Extra: {stats.Extra} \n" +
                    $"Inventory: {stats.Inventory} \n" +
                    $"Stat Multiplier: {stats.StatMultiplier} \n" +
                    $"Image: {Context.User.GetAvatarUrl()}");
            }
        }

        [Command("reset")]
        [Summary("Deletes your character after asking for confirmation.")]
        public async Task ResetAsync([Remainder] string xargs = null)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == Context.User.Id
                                        select user).SingleOrDefault();

            if (stats == null)
            {
                await ReplyAsync("You don't have a character to delete.");
                return;
            }

            IUserMessage msg = (IUserMessage) await ReplyAsync("Are you sure you want to reset your character? **This will delete ALL your data.** \nTo confirm, react with :white_check_mark: (Expires in 5 seconds)");
            await msg.AddReactionAsync(new Emoji("✅"));
            await Task.Delay(5000);
            List<IUser> reacted = (await msg.GetReactionUsersAsync(new Emoji("✅"), 5).FlattenAsync()).ToList();
            if (reacted.Select(user => user.Id).Contains(Context.User.Id))
            {
                NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM public.\"Character\" WHERE \"UserID\" = @id", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)Context.User.Id);
                db.CommandVoid(cmd);
                tempstorage.characters.RemoveAll(c => c.PlayerID == Context.User.Id);
                await ReplyAsync("Successfully deleted character.");
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
                await ReplyAsync("Please enter a name for your character!");
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
                await ReplyAsync($"Your character has been renamed to {charname}!");
            }
            else
            {
                await ReplyAsync("You don't have a character yet! Create one with <start.");
            }
        }

        [Command("description")]
        [Summary("Edit your character's description.")]
        public async Task DescriptionAsync([Remainder] string chardesc = null)
        {
            if (string.IsNullOrWhiteSpace(chardesc))
            {
                await ReplyAsync("Please enter a description!");
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
                await ReplyAsync("A new description has been set!");
            }
            else
            {
                await ReplyAsync("You don't have a character yet! Create one with <start.");
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
                        stats.HP = Convert.ToInt32(xargs);
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
                    else
                    {
                        await ReplyAsync("Invalid argument");
                        return;
                    }
                    await ReplyAsync("Successfully changed");
                }
                else
                {
                    await ReplyAsync("Invalid User");
                }
            }
            else
            {
                await ReplyAsync("Missing Arguments");
            }
        }
    }
}
