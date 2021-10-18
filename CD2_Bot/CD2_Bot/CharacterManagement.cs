using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

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
                await ReplyAsync("You already have a character!");
            }
            else
            {
                if(charname.Length > 20)
                {
                    charname = charname.Substring(0, 20);
                }
                tempstorage.characters.Add(new CharacterStructure(charname, Context.User.Id));
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
            Utils.DebugLog("Servus");

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
    }
}
