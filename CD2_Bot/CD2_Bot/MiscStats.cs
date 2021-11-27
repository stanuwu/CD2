using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public class MiscStats : ModuleBase<SocketCommandContext>
    {
        [Command("weapon")]
        [Summary("View a weapons stats.")]
        public async Task ViewWeaponAsync([Remainder] string tv = null)
        {
            if (tv == null)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("No weapon name entered."));
                return;
            }

            Weapon thisweapon = (from w in Gear.Weapons
                                 where w.Name.ToLower().Contains(tv.ToLower())
                                 select w).SingleOrDefault();
            if (thisweapon == null)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("No weapon with this name was found."));
            } else
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Title = thisweapon.Name,
                    Description = thisweapon.Description,
                };

                embed.Description += "\n\n" +
                    $"**Base Damage:** {thisweapon.BaseDamage}\n" +
                    $"**Rarity:** {thisweapon.Rarity.ToString()}\n" +
                    $"**Sell Price:** {Prices.sell[thisweapon.Rarity]}\n" +
                    $"**Infusion Worth:** {Prices.infuse[thisweapon.Rarity]}\n";

                if (thisweapon.CustomEffectName != null)
                {
                    embed.Description += $"**Effect Name:** {thisweapon.CustomEffectName}\n" +
                    $"**Effect:** {thisweapon.CustomEffectDescription}\n";
                }
                embed.WithColor(Color.DarkMagenta);
                embed.WithFooter(Defaults.FOOTER);
                await ReplyAsync(embed: embed.Build());
            }
        }

        [Command("armor")]
        [Summary("View an armors stats.")]
        public async Task ViewArmorAsync([Remainder] string tv = null)
        {
            if (tv == null)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("No armor name entered."));
                return;
            }

            Armor thisarmor = (from w in Gear.Armors
                                 where w.Name.ToLower().Contains(tv.ToLower())
                                 select w).SingleOrDefault();
            if (thisarmor == null)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("No armor with this name was found."));
            }
            else
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Title = thisarmor.Name,
                    Description = thisarmor.Description,
                };

                embed.Description += "\n\n" +
                    $"**Resistance:** {thisarmor.Resistance}\n" +
                    $"**Rarity:** {thisarmor.Rarity.ToString()}\n" +
                    $"**Sell Price:** {Prices.sell[thisarmor.Rarity]}\n" +
                    $"**Infusion Worth:** {Prices.infuse[thisarmor.Rarity]}\n";

                if (thisarmor.CustomEffectName != null)
                {
                    embed.Description += $"**Effect Name:** {thisarmor.CustomEffectName}\n" +
                    $"**Effect:** {thisarmor.CustomEffectDescription}\n";
                }
                embed.WithColor(Color.DarkMagenta);
                embed.WithFooter(Defaults.FOOTER);
                await ReplyAsync(embed: embed.Build());
            }
        }

        [Command("extra")]
        [Summary("View an extras stats.")]
        public async Task ViewExtraAsync([Remainder] string tv = null)
        {
            if (tv == null)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("No extra name entered."));
                return;
            }

            Extra thisextra = (from w in Gear.Extras
                                 where w.Name.ToLower().Contains(tv.ToLower())
                                 select w).SingleOrDefault();
            if (thisextra == null)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("No extra with this name was found."));
            }
            else
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Title = thisextra.Name,
                    Description = thisextra.Description,
                };

                embed.Description += "\n\n" +
                    $"**Base Damage:** {thisextra.BaseDamage}\n" +
                    $"**Base Heal:** {thisextra.BaseHeal}\n" +
                    $"**Rarity:** {thisextra.Rarity.ToString()}\n" +
                    $"**Sell Price:** {Prices.sell[thisextra.Rarity]}\n" +
                    $"**Infusion Worth:** {Prices.infuse[thisextra.Rarity]}\n";

                if (thisextra.CustomEffectName != null)
                {
                    embed.Description += $"**Effect Name:** {thisextra.CustomEffectName}\n" +
                    $"**Effect:** {thisextra.CustomEffectDescription}\n";
                }
                embed.WithColor(Color.DarkMagenta);
                embed.WithFooter(Defaults.FOOTER);
                await ReplyAsync(embed: embed.Build());
            }
        }

        [Command("monster")]
        [Summary("View a monsters stats.")]
        public async Task ViewmonsterAsync([Remainder] string tv = null)
        {
            if (tv == null)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("No monster name entered."));
                return;
            }

            Enemy thisenemy = (from w in EnemyGen.Enemies
                               where w.Type.ToLower().Contains(tv.ToLower())
                               select w).SingleOrDefault();
            if (thisenemy == null)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("No enemy with this name was found."));
            }
            else
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Title = thisenemy.Type,
                    Description =thisenemy.Description,
                };

                embed.Description += "\n\n" +
                    $"**Base HP:** {thisenemy.HP}\n" +
                    $"**Base Damage:** {thisenemy.Damage}\n" +
                    $"**Resistance:** {thisenemy.Resistance}\n" +
                    $"**Appears at level:** {thisenemy.Minlevel}\n";

                if (thisenemy.CustomEffectName != null)
                {
                    embed.Description += $"**Effect Name:** {thisenemy.CustomEffectName}\n" +
                    $"**Effect:** {thisenemy.CustomEffectDescription}\n";
                }

                if (thisenemy.Drops != null)
                {
                    string dropam = thisenemy.Drops.DropAmount.ToString();
                    if (thisenemy.Drops.DropVariation > 0)
                    {
                        dropam = $"{thisenemy.Drops.DropAmount - thisenemy.Drops.DropVariation}-{thisenemy.Drops.DropAmount + thisenemy.Drops.DropVariation}";
                    }
                    embed.Description += $"**Drop:** {thisenemy.Drops.Drop}\n" +
                    $"**Amount:** {dropam}\n" +
                    $"**Chance:** {thisenemy.Drops.DropChance}%\n";
                }
                embed.WithColor(Color.DarkMagenta);
                embed.WithFooter(Defaults.FOOTER);
                await ReplyAsync(embed: embed.Build());
            }
        }
    }
}
