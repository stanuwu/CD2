using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace CD2_Bot
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        [Summary("Test if the bot is working.")]
        public async Task TestAsync(string arg = null, [Remainder] string xargs = null)
        {
            switch (arg)
            {
                case null:
                    await ReplyAsync(embed: Utils.QuickEmbedError("Missing arguments"));
                    break;
                case "bot":
                    await ReplyAsync(embed: Utils.QuickEmbedBotinfo());
                    break;
                case "menu":
                    Dictionary<string, string> tempdict = new Dictionary<string, string>();
                    tempdict.Add("1", "This is the first field");
                    tempdict.Add("2", "This is the second field");
                    await ReplyAsync(embed: Utils.QuickEmbedMenu(tempdict));
                    break;
                case "list":
                    List<string> templist = new List<string>
                    {
                        "Servus",
                        "Ich",
                        "bin",
                        "ne",
                        "Liste"
                    };
                    await ReplyAsync(embed: Utils.QuickEmbedList(templist));
                    break;
                default:
                    await ReplyAsync(embed: Utils.QuickEmbedNormal("Success", $"Arg: {arg}, Xargs: {xargs}"));
                    break;
            }
        }
    }
}