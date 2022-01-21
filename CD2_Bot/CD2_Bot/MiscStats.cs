using Discord;
using Discord.Commands;
using Npgsql;
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
                                 select w).FirstOrDefault();
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
                                 select w).FirstOrDefault();
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
                                 select w).FirstOrDefault();
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
                               select w).FirstOrDefault();

            if (thisenemy == null)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("No enemy with this name was found."));
            }
            else
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Title = thisenemy.Type,
                    Description = thisenemy.Description,
                };

                embed.Description += "\n\n" +
                    $"**Biome:** {thisenemy.Biome.ToString()}\n" +
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

        [Command("lvltop")]
        [Summary("View the level leaderboard.")]
        public async Task LevelTopAsync([Remainder] string xargs = null)
        {
            List<IGuildUser> tgp = (await Context.Guild.GetUsersAsync().FlattenAsync()).ToList().FindAll(u => tempstorage.characters.Exists(x => x.PlayerID == u.Id));
            List<CharacterStructure> tgc = new List<CharacterStructure>() { };
            tgp.ForEach(x => tgc.Add(tempstorage.characters.Find(p => p.PlayerID == x.Id)));

            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Level Leaderboard"
            };

            MessageComponent btn = new ComponentBuilder().WithButton("Global", "gboard;lvl;" + Context.User.Id , ButtonStyle.Primary).Build();

            if (tgp.Count < 3)
            {
                embed.Description = "There is not enough players in this guild. You can view the global leaderboards instead.";
                await ReplyAsync(embed: embed.Build(), components: btn);
            }

            List<CharacterStructure> tco = tgc.Where(x => x.Deleted == false).OrderByDescending(c => c.EXP).ToList();
            List<CharacterStructure> t3c = tco.Take(3).ToList();

            string avatarurl = "";
            IUser founduser = await Defaults.CLIENT.GetUserAsync(t3c[0].PlayerID);
            if (founduser != null)
            {
                avatarurl = founduser.GetAvatarUrl();
            }
            else
            {
                avatarurl = Defaults.CLIENT.CurrentUser.GetDefaultAvatarUrl();
            }

            embed.ThumbnailUrl = avatarurl;

            embed.Description = "\n" +
                $"**1: {t3c[0].CharacterName}**\n" +
                $"Level: {t3c[0].Lvl} ({t3c[0].EXP}exp)\n\n" +
                $"**2: {t3c[1].CharacterName}**\n" +
                $"Level: {t3c[1].Lvl} ({t3c[1].EXP}exp)\n\n" +
                $"**3: {t3c[2].CharacterName}**\n" +
                $"Level: {t3c[2].Lvl} ({t3c[2].EXP}exp)\n\n";

            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == Context.User.Id
                                        select user).SingleOrDefault();

            if (stats != null && stats.Deleted == false && !t3c.Any(x => x.PlayerID == stats.PlayerID))
            {
                embed.Description += $"**Your Place:** {tco.IndexOf(stats) + 1}";
            }

            embed.WithColor(Color.DarkMagenta);
            embed.WithFooter(Defaults.FOOTER);
            await ReplyAsync(embed: embed.Build(), components: btn);
        }

        [Command("moneytop")]
        [Summary("View the money leaderboard.")]
        public async Task MoneyTopAsync([Remainder] string xargs = null)
        {
            List<IGuildUser> tgp = (await Context.Guild.GetUsersAsync().FlattenAsync()).ToList().FindAll(u => tempstorage.characters.Exists(x => x.PlayerID == u.Id));
            List<CharacterStructure> tgc = new List<CharacterStructure>() { };
            tgp.ForEach(x => tgc.Add(tempstorage.characters.Find(p => p.PlayerID == x.Id)));

            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Money Leaderboard"
            };

            MessageComponent btn = new ComponentBuilder().WithButton("Global", "gboard;money;" + Context.User.Id, ButtonStyle.Primary).Build();

            if (tgp.Count < 3)
            {
                embed.Description = "There is not enough players in this guild. You can view the global leaderboards instead.";
                await ReplyAsync(embed: embed.Build(), components: btn);
            }

            List<CharacterStructure> tco = tgc.Where(x => x.Deleted == false).OrderByDescending(c => c.Money).ToList();
            List<CharacterStructure> t3c = tco.Take(3).ToList();

            string avatarurl = "";
            IUser founduser = await Defaults.CLIENT.GetUserAsync(t3c[0].PlayerID);
            if (founduser != null)
            {
                avatarurl = founduser.GetAvatarUrl();
            }
            else
            {
                avatarurl = Defaults.CLIENT.CurrentUser.GetDefaultAvatarUrl();
            }

            embed.ThumbnailUrl = avatarurl;

            embed.Description = "\n" +
                $"**1: {t3c[0].CharacterName}**\n" +
                $"Money: {t3c[0].Money} coins\n\n" +
                $"**2: {t3c[1].CharacterName}**\n" +
                $"Money: {t3c[1].Money} coins\n\n" +
                $"**3: {t3c[2].CharacterName}**\n" +
                $"Money: {t3c[2].Money} coins\n\n";

            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == Context.User.Id
                                        select user).SingleOrDefault();

            if (stats != null && stats.Deleted == false && !t3c.Any(x => x.PlayerID == stats.PlayerID))
            {
                embed.Description += $"**Your Place:** {tco.IndexOf(stats) + 1}";
            }

            embed.WithColor(Color.DarkMagenta);
            embed.WithFooter(Defaults.FOOTER);
            await ReplyAsync(embed: embed.Build(), components: btn);
        }

        [Command("geartop")]
        [Summary("View the gear leaderboard.")]
        public async Task GearTopAsync([Remainder] string xargs = null)
        {
            List<IGuildUser> tgp = (await Context.Guild.GetUsersAsync().FlattenAsync()).ToList().FindAll(u => tempstorage.characters.Exists(x => x.PlayerID == u.Id));
            List<CharacterStructure> tgc = new List<CharacterStructure>() { };
            tgp.ForEach(x => tgc.Add(tempstorage.characters.Find(p => p.PlayerID == x.Id)));

            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Gear Leaderboard"
            };

            MessageComponent btn = new ComponentBuilder().WithButton("Global", "gboard;gear;" + Context.User.Id, ButtonStyle.Primary).Build();

            if (tgp.Count < 3)
            {
                embed.Description = "There is not enough players in this guild. You can view the global leaderboards instead.";
                await ReplyAsync(embed: embed.Build(), components: btn);
            }

            //List<CharacterStructure> tco = tempstorage.characters.Where(x => x.Deleted == false).OrderByDescending(c => Prices.sell[c.Weapon.Rarity] + Prices.sell[c.Armor.Rarity] + Prices.sell[c.Extra.Rarity]).ToList();
            List<CharacterStructure> tco = tgc.Where(x => x.Deleted == false).OrderByDescending(c => Prices.sell[c.Weapon.Rarity] + Prices.sell[c.Armor.Rarity] + Prices.sell[c.Extra.Rarity]).ToList();
            List<CharacterStructure> t3c = tco.Take(3).ToList();

            string avatarurl = "";
            IUser founduser = await Defaults.CLIENT.GetUserAsync(t3c[0].PlayerID);
            if (founduser != null)
            {
                avatarurl = founduser.GetAvatarUrl();
            }
            else
            {
                avatarurl = Defaults.CLIENT.CurrentUser.GetDefaultAvatarUrl();
            }

            embed.ThumbnailUrl = avatarurl;

            embed.Description = "\n" +
                $"**1: {t3c[0].CharacterName}**\n" +
                $"Weapon: {t3c[0].Weapon.Name}\nArmor: {t3c[0].Armor.Name}\nExtra: {t3c[0].Extra.Name} \n\n" +
                $"**2: {t3c[1].CharacterName}**\n" +
                $"Weapon: {t3c[1].Weapon.Name}\nArmor: {t3c[1].Armor.Name}\nExtra: {t3c[1].Extra.Name}\n\n" +
                $"**3: {t3c[2].CharacterName}**\n" +
                $"Weapon: {t3c[2].Weapon.Name}\nArmor: {t3c[2].Armor.Name}\nExtra: {t3c[2].Extra.Name}\n\n";

            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == Context.User.Id
                                        select user).SingleOrDefault();

            if (stats != null && stats.Deleted == false && !t3c.Any(x => x.PlayerID == stats.PlayerID))
            {
                embed.Description += $"**Your Place:** {tco.IndexOf(stats) + 1}";
            }

            embed.WithColor(Color.DarkMagenta);
            embed.WithFooter(Defaults.FOOTER);
            await ReplyAsync(embed: embed.Build(), components: btn);
        }

        [Command("servertop")]
        [Summary("View the server leaderboard.")]
        public async Task ServerTopAsync([Remainder] string xargs = null)
        {
            if (tempstorage.guilds.Count < 3)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("There is not enough servers to do this."));
                return;
            }

            List<GuildStructure> tco = tempstorage.guilds.OrderByDescending(c => c.DoorsOpened + (c.BossesSlain*10) + (c.QuestsFinished*5)).ToList();
            List<GuildStructure> t3c = tco.Take(3).ToList();

            string avatarurl = "";
            IGuild foundguild = Defaults.CLIENT.GetGuild(t3c[0].GuildID);
            IGuild foundguild2 = Defaults.CLIENT.GetGuild(t3c[1].GuildID);
            IGuild foundguild3 = Defaults.CLIENT.GetGuild(t3c[2].GuildID);
            if (foundguild != null)
            {
                avatarurl = foundguild.IconUrl;
            }

            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Server Leaderboard",
                ThumbnailUrl = avatarurl,
            };

            embed.Description = "\n" +
                $"**1: {foundguild.Name}**\n" +
                $"Doors: {t3c[0].DoorsOpened}\nBosses: {t3c[0].BossesSlain}\nQuests: {t3c[0].QuestsFinished} \n\n" +
                $"**2: {foundguild2.Name}**\n" +
                $"Doors: {t3c[1].DoorsOpened}\nBosses: {t3c[1].BossesSlain}\nQuests: {t3c[1].QuestsFinished}\n\n" +
                $"**3: {foundguild3.Name}**\n" +
                $"Doors: {t3c[2].DoorsOpened}\nBosses: {t3c[2].BossesSlain}\nQuests: {t3c[2].QuestsFinished}\n\n";

            if (!t3c.Any(x => x.GuildID == Context.Guild.Id))
            {
                embed.Description += $"**Your Place:** {tco.IndexOf(tco.Where(x => x.GuildID == Context.Guild.Id).FirstOrDefault()) + 1}";
            }

            embed.WithColor(Color.DarkMagenta);
            embed.WithFooter(Defaults.FOOTER);
            await ReplyAsync(embed: embed.Build());
        }
    }
}
