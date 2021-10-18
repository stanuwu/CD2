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
            else
            {
                tempstorage.characters.Add(new CharacterStructure(charname.Substring(0, 20), Context.User.Id));
                Utils.DebugLog(charname.Substring(0, 20));
                await ReplyAsync($"Character {charname.Substring(0, 20)} created!");
            }
        }
    }
}
