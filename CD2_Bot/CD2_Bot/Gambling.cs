using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public class Gambling : ModuleBase<SocketCommandContext>
    {
        [Command("coinflip")]
        [Summary("Coinflip for money against the computer or another player.")]
        public async Task CoinflipAsync(int wager = 0, SocketUser opponent = null, [Remainder] string xargs = null)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == Context.User.Id
                                        select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You do not have a character."));
                return;
            }

            if (wager < 1)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("Enter a valid wager."));
                return;
            }

            if (opponent == null)
            {
                if (stats.Money < wager)
                {
                    await ReplyAsync(embed: Utils.QuickEmbedError("You can not afford this."));
                    return;
                }
                EmbedBuilder embed = new EmbedBuilder
                {
                    Title = "Coinflip"
                };
                embed.WithFooter(Defaults.FOOTER);
                int cfw = Defaults.GRandom.Next(0, 3);
                switch(cfw)
                {
                    case 0:
                        stats.Money += wager;
                        embed.WithColor(Color.Green);
                        embed.Description = $"You win {wager} coins!";
                        break;

                    default:
                        stats.Money -= wager;
                        embed.WithColor(Color.DarkRed);
                        embed.Description = $"You loose {wager} coins!";
                        break;
                }
                await ReplyAsync(embed: embed.Build());
            } else
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Title = "Coinflip",
                    Description = $"{stats.CharacterName} has invited <@{opponent.Id}> to a coinflip for {wager} coins!\nThis expires after 15 minutes."
                };
                embed.WithFooter(Defaults.FOOTER);
                embed.WithColor(Color.Magenta);

                ComponentBuilder btnb = new ComponentBuilder()
                        .WithButton("Accept", "coinflip;ask;" + Context.User.Id.ToString() + ";" + opponent.Id.ToString() + ";" + wager.ToString(), ButtonStyle.Success)
                        .WithButton("Cancel", "coinflip;cancel;" + Context.User.Id.ToString() + ";" + opponent.Id.ToString(), ButtonStyle.Danger);

                MessageComponent btn = btnb.Build();

                await ReplyAsync(embed: embed.Build(), component:btn);
            }
        }

        [Command("slots")]
        [Summary("Take a spin at a slot machine.")]
        public async Task SlotsAsync(int wager = 0, [Remainder] string xargs = null)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == Context.User.Id
                                        select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You do not have a character."));
                return;
            }

            if (wager < 1)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("Enter a valid wager."));
                return;
            }

            if (stats.Money < wager)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You can not afford this."));
                return;
            }

            string[] sp = new string[] {"🔪", "🔪", "🍇", "🍇", "🍒", "🍒", "💚"};

            string pick1 = sp[Defaults.GRandom.Next(sp.Length)];
            string pick2 = sp[Defaults.GRandom.Next(sp.Length)];
            string pick3 = sp[Defaults.GRandom.Next(sp.Length)];

            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Slots",
                Description = $":diamonds::red_square::red_square::red_square::diamonds:\n:red_square::small_red_triangle_down::small_red_triangle_down::small_red_triangle_down::red_square:\n:red_square:{pick1}{pick2}{pick3}:red_square:\n:red_square::small_red_triangle::small_red_triangle::small_red_triangle::red_square:\n:diamonds::red_square::red_square::red_square::diamonds:\n"
            };
            embed.WithFooter(Defaults.FOOTER);
            int win = wager*-1;
            if (pick1 == pick2 && pick2 == pick3)
            {
                if (pick1 == "💚")
                {
                    win = wager * 25;
                } else
                {
                    win = wager * 10;
                }
                embed.Description += $"You win {win} coins!";
                embed.Color = Color.Green;
            } else
            {
                embed.Description += $"You loose {win} coins!";
                embed.Color = Color.Red;
            }
            stats.Money += win;
            await ReplyAsync(embed: embed.Build());
        }
    }
}
