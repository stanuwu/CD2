using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace CD2_Bot
{
    class CharacterManagement : ModuleBase<SocketCommandContext>
    {
        [Command("start")]
        [Summary("Creates a character to start the game with.")]
        public async Task StartAsync(string charname = null, [Remainder] string xargs = null)
        {
            switch (string.IsNullOrWhiteSpace(charname))
            {
                case true:
                    await ReplyAsync("Please enter a name for your character!");
                    break;
                default:
                    tempstorage.characters.Add(new CharacterManagement())
                    break;
            }
        }
    }
}
