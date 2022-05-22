using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public static class MiscStats
    {
        //"weapon" command
        public static async Task ViewWeaponAsync(SocketSlashCommand cmd)
        {
            if (cmd.Data.Options.Count < 1)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("No weapon name entered."));
                return;
            }

            string tv = (string)cmd.Data.Options.First().Value;

            Weapon thisweapon = (from w in Gear.Weapons
                where w.Name.ToLower().Contains(tv.ToLower())
                select w).FirstOrDefault();
            if (thisweapon == null)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("No weapon with this name was found."));
            }
            else
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Title = thisweapon.Name,
                    Description = thisweapon.Description,
                };

                embed.Description += "\n\n" +
                                     $"**Base Damage:** {thisweapon.BaseDamage}\n" +
                                     $"**Rarity:** {thisweapon.Rarity.ToString()}\n" +
                                     $"**Type:** {thisweapon.Category.ToString()}\n" +
                                     $"**Sell Price:** {Prices.sell[thisweapon.Rarity]}\n" +
                                     $"**Infusion Worth:** {Prices.infuse[thisweapon.Rarity]}\n";

                if (thisweapon.CustomEffectName != null)
                {
                    embed.Description += $"**Effect Name:** {thisweapon.CustomEffectName}\n" +
                                         $"**Effect:** {thisweapon.CustomEffectDescription}\n";
                }

                embed.WithColor(Color.DarkMagenta);
                embed.WithFooter(Defaults.FOOTER);
                await cmd.RespondAsync(embed: embed.Build());
            }
        }

        //"armor" command
        public static async Task ViewArmorAsync(SocketSlashCommand cmd)
        {
            if (cmd.Data.Options.Count < 1)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("No armor name entered."));
                return;
            }

            string tv = (string)cmd.Data.Options.First().Value;

            Armor thisarmor = (from w in Gear.Armors
                where w.Name.ToLower().Contains(tv.ToLower())
                select w).FirstOrDefault();
            if (thisarmor == null)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("No armor with this name was found."));
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
                await cmd.RespondAsync(embed: embed.Build());
            }
        }

        //"extra" command
        public static async Task ViewExtraAsync(SocketSlashCommand cmd)
        {
            if (cmd.Data.Options.Count < 1)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("No extra name entered."));
                return;
            }

            string tv = (string)cmd.Data.Options.First().Value;

            Extra thisextra = (from w in Gear.Extras
                where w.Name.ToLower().Contains(tv.ToLower())
                select w).FirstOrDefault();
            if (thisextra == null)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("No extra with this name was found."));
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
                await cmd.RespondAsync(embed: embed.Build());
            }
        }

        //"monster" command
        public static async Task ViewMonsterAsync(SocketSlashCommand cmd)
        {
            if (cmd.Data.Options.Count < 1)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("No monster name entered."));
                return;
            }

            string tv = (string)cmd.Data.Options.First().Value;

            Enemy thisenemy = (from w in EnemyGen.Enemies
                where w.Type.ToLower().Contains(tv.ToLower())
                select w).FirstOrDefault();

            if (thisenemy == null)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("No enemy with this name was found."));
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
                await cmd.RespondAsync(embed: embed.Build());
            }
        }

        //"bosses" command
        public static async Task ViewBossesAsync(SocketSlashCommand cmd)
        {
            if (cmd.Data.Options.Count < 1)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("No boss name entered."));
                return;
            }

            string tv = (string)cmd.Data.Options.First().Value;

            Boss thisboss = (from w in BossFights.Bosses
                where w.Type.ToLower().Contains(tv.ToLower())
                select w).FirstOrDefault();

            if (thisboss == null)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("No boss with this name was found."));
            }
            else
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Title = thisboss.Type,
                    Description = thisboss.Description,
                };

                embed.Description += "\n\n" +
                                     $"**Base HP:** {thisboss.HP}\n" +
                                     $"**Base Damage:** {thisboss.Damage}\n" +
                                     $"**Resistance:** {thisboss.Resistance}%\n" +
                                     $"**Minimum level:** {thisboss.Minlevel}\n" +
                                     $"**Summon Cost:** {thisboss.Cost.MoneyCost} coins";
                if (thisboss.Cost.ItemType != null)
                {
                    embed.Description += $", {thisboss.Cost.ItemCost}x {thisboss.Cost.ItemType}";
                }

                embed.Description += "\n**Rewards:**\n";
                embed.Description += thisboss.Drops.PreviewDrops();
                embed.WithColor(Color.DarkMagenta);
                embed.WithFooter(Defaults.FOOTER);
                await cmd.RespondAsync(embed: embed.Build());
            }
        }

        //"class" command
        public static async Task ViewClassAsync(SocketSlashCommand cmd)
        {
            if (cmd.Data.Options.Count < 1)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("No class name entered."));
                return;
            }

            string tv = (string)cmd.Data.Options.First().Value;

            Class thisclass = (from w in Class.Classes
                where w.Name.ToLower().Contains(tv.ToLower())
                select w).FirstOrDefault();
            if (thisclass == null)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("No class with this name was found."));
            }
            else
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Title = thisclass.Name,
                    Description = thisclass.Description,
                };

                embed.Description += "\n\n" +
                                     $"**Damage:** {thisclass.DamageMultiplier * 100}%\n" +
                                     $"**Resistance:** {(thisclass.ResistanceBonus >= 0 ? "+" : "")}{thisclass.ResistanceBonus}\n" +
                                     $"**Heal:** {thisclass.HealMultiplier * 100}%\n" +
                                     $"**Level:** {thisclass.Level}";
                embed.WithColor(Color.DarkMagenta);
                embed.WithFooter(Defaults.FOOTER);
                await cmd.RespondAsync(embed: embed.Build());
            }
        }

        //"lvltop" command
        public static async Task LevelTopAsync(SocketSlashCommand cmd)
        {
            List<IGuildUser> tgp = (await (((IGuildChannel)cmd.Channel).Guild.GetUsersAsync())).ToList()
                .FindAll(u => tempstorage.characters.Exists(x => x.PlayerID == u.Id));
            List<CharacterStructure> tgc = new List<CharacterStructure>() { };
            tgp.ForEach(x => tgc.Add(tempstorage.characters.Find(p => p.PlayerID == x.Id)));

            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Level Leaderboard"
            };

            MessageComponent btn = new ComponentBuilder().WithButton("Global", "gboard;lvl;" + cmd.User.Id, ButtonStyle.Primary).Build();

            if (tgp.Count < 3)
            {
                embed.Description = "There is not enough players in this guild. You can view the global leaderboards instead.";
                await cmd.RespondAsync(embed: embed.Build(), components: btn);
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
                where user.PlayerID == cmd.User.Id
                select user).SingleOrDefault();

            if (stats != null && stats.Deleted == false && !t3c.Any(x => x.PlayerID == stats.PlayerID))
            {
                embed.Description += $"**Your Place:** {tco.IndexOf(stats) + 1}";
            }

            embed.WithColor(Color.DarkMagenta);
            embed.WithFooter(Defaults.FOOTER);
            await cmd.RespondAsync(embed: embed.Build(), components: btn);
        }

        //"moneytop" command
        public static async Task MoneyTopAsync(SocketSlashCommand cmd)
        {
            List<IGuildUser> tgp = (await (((IGuildChannel)cmd.Channel).Guild.GetUsersAsync())).ToList()
                .FindAll(u => tempstorage.characters.Exists(x => x.PlayerID == u.Id));
            List<CharacterStructure> tgc = new List<CharacterStructure>() { };
            tgp.ForEach(x => tgc.Add(tempstorage.characters.Find(p => p.PlayerID == x.Id)));

            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Money Leaderboard"
            };

            MessageComponent btn = new ComponentBuilder().WithButton("Global", "gboard;money;" + cmd.User.Id, ButtonStyle.Primary).Build();

            if (tgp.Count < 3)
            {
                embed.Description = "There is not enough players in this guild. You can view the global leaderboards instead.";
                await cmd.RespondAsync(embed: embed.Build(), components: btn);
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
                where user.PlayerID == cmd.User.Id
                select user).SingleOrDefault();

            if (stats != null && stats.Deleted == false && !t3c.Any(x => x.PlayerID == stats.PlayerID))
            {
                embed.Description += $"**Your Place:** {tco.IndexOf(stats) + 1}";
            }

            embed.WithColor(Color.DarkMagenta);
            embed.WithFooter(Defaults.FOOTER);
            await cmd.RespondAsync(embed: embed.Build(), components: btn);
        }

        //"geartop" command
        public static async Task GearTopAsync(SocketSlashCommand cmd)
        {
            List<IGuildUser> tgp = (await (((IGuildChannel)cmd.Channel).Guild.GetUsersAsync())).ToList()
                .FindAll(u => tempstorage.characters.Exists(x => x.PlayerID == u.Id));
            List<CharacterStructure> tgc = new List<CharacterStructure>() { };
            tgp.ForEach(x => tgc.Add(tempstorage.characters.Find(p => p.PlayerID == x.Id)));

            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Gear Leaderboard"
            };

            MessageComponent btn = new ComponentBuilder().WithButton("Global", "gboard;gear;" + cmd.User.Id, ButtonStyle.Primary).Build();

            if (tgp.Count < 3)
            {
                embed.Description = "There is not enough players in this guild. You can view the global leaderboards instead.";
                await cmd.RespondAsync(embed: embed.Build(), components: btn);
            }

            //List<CharacterStructure> tco = tempstorage.characters.Where(x => x.Deleted == false).OrderByDescending(c => Prices.sell[c.Weapon.Rarity] + Prices.sell[c.Armor.Rarity] + Prices.sell[c.Extra.Rarity]).ToList();
            List<CharacterStructure> tco = tgc.Where(x => x.Deleted == false)
                .OrderByDescending(c => Prices.sell[c.Weapon.Rarity] + Prices.sell[c.Armor.Rarity] + Prices.sell[c.Extra.Rarity]).ToList();
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
                where user.PlayerID == cmd.User.Id
                select user).SingleOrDefault();

            if (stats != null && stats.Deleted == false && !t3c.Any(x => x.PlayerID == stats.PlayerID))
            {
                embed.Description += $"**Your Place:** {tco.IndexOf(stats) + 1}";
            }

            embed.WithColor(Color.DarkMagenta);
            embed.WithFooter(Defaults.FOOTER);
            await cmd.RespondAsync(embed: embed.Build(), components: btn);
        }

        //"servertop" command
        public static async Task ServerTopAsync(SocketSlashCommand cmd)
        {
            if (tempstorage.guilds.Count < 3)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("There is not enough servers to do this."));
                return;
            }

            List<GuildStructure> tco = tempstorage.guilds.OrderByDescending(c => c.DoorsOpened + (c.BossesSlain * 10) + (c.QuestsFinished * 5)).ToList();
            List<GuildStructure> t3c = tco.Take(3).ToList();

            string avatarurl = "";
            List<SocketGuild> guilds = Defaults.CLIENT.Guilds.ToList();
            IGuild foundguild = guilds.Find(x => cmd.Id == t3c[0].GuildID);
            IGuild foundguild2 = guilds.Find(x => cmd.Id == t3c[1].GuildID);
            IGuild foundguild3 = guilds.Find(x => cmd.Id == t3c[2].GuildID);
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

            if (!t3c.Any(x => x.GuildID == ((IGuildChannel)cmd.Channel).Guild.Id))
            {
                embed.Description += $"**Your Place:** {tco.IndexOf(tco.Where(x => x.GuildID == ((IGuildChannel)cmd.Channel).Guild.Id).FirstOrDefault()) + 1}";
            }

            embed.WithColor(Color.DarkMagenta);
            embed.WithFooter(Defaults.FOOTER);
            await cmd.RespondAsync(embed: embed.Build());
        }
    }
}