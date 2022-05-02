using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    static class ButtonHandler
    {
        static public async Task HandleButtonAsync(SocketMessageComponent btn)
        {
            try
            {
                //Register buttons in this switch
                switch (btn.Data.CustomId.Split(';')[0])
                {
                    case "testbtn1":
                        await BtnTest(btn);
                        break;
                    case "dropaction":
                        await DropAction(btn);
                        break;
                    case "chestopen":
                        await ChestOpen(btn);
                        break;
                    case "delchar":
                        await DelChar(btn);
                        break;
                    case "coinflip":
                        await DoCoinflip(btn);
                        break;
                    case "guide":
                        await EditGuide(btn);
                        break;
                    case "help":
                        await EditHelp(btn);
                        break;
                    case "fightdetails":
                        await FightDetails(btn);
                        break;
                    case "pvpdetails":
                        await PlayerFightDetails(btn);
                        break;
                    case "playerfight":
                        await InitPlayerFight(btn);
                        break;
                    case "questview":
                        await QuestView(btn);
                        break;
                    case "questroom":
                        await QuestOffer(btn);
                        break;
                    case "trade":
                        await Trade(btn);
                        break;
                    case "gboard":
                        await DisplayGlobalLeaderBoard(btn);
                        break;
                    case "shop":
                        await ShopButton(btn);
                        break;
                }
            }
            catch (Exception e)
            {
                await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Error, "Button Handler", e.Message));
                await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Error, "Button Handler", e.StackTrace));
            }
        }


        //add functions for buttons here (try to keep it at 1 function (async task with no return) per button)
        static public async Task BtnTest(SocketMessageComponent btn)
        {
            ulong userid = (ulong)Convert.ToInt64(btn.Data.CustomId.Split(';')[1]);
            await btn.Message.DeleteAsync();
            await btn.RespondAsync("Hi", ephemeral: true);
        }

        static public async Task DropAction(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            ulong userid = (ulong)Convert.ToInt64(btndata[2]);
            CharacterStructure statst = (from user in tempstorage.characters
                                         where user.PlayerID == userid
                                         select user).SingleOrDefault();
            if (userid == btn.User.Id)
            {
                await btn.Message.DeleteAsync();

                if ((DateTime.Now - btn.Message.Timestamp).TotalMinutes > 15)
                {
                    await btn.RespondAsync(embed: Utils.QuickEmbedError("This drop is expired."), ephemeral: true);
                    return;
                }

                string dmsg = "";
                switch (btndata[1])
                {
                    case "claim":
                        dmsg = "Successfully Claimed!";
                        switch (btndata[3])
                        {
                            case "weapon":
                                statst.Weapon = Gear.Weapons.Find(w => w.Name == btndata[4]);
                                break;
                            case "armor":
                                statst.Armor = Gear.Armors.Find(w => w.Name == btndata[4]);
                                break;
                            case "extra":
                                statst.Extra = Gear.Extras.Find(w => w.Name == btndata[4]);
                                break;
                        }
                        break;

                    case "sell":
                        dmsg = "Successfully Sold!";
                        switch (btndata[3])
                        {
                            case "weapon":
                                statst.Money += Prices.sell[Gear.Weapons.Find(w => w.Name == btndata[4]).Rarity];
                                break;
                            case "armor":
                                statst.Money += Prices.sell[Gear.Armors.Find(w => w.Name == btndata[4]).Rarity];
                                break;
                            case "extra":
                                statst.Money += Prices.sell[Gear.Extras.Find(w => w.Name == btndata[4]).Rarity];
                                break;
                        }
                        break;

                    case "infuse":
                        dmsg = "Successfully Infused!";
                        switch (btndata[3])
                        {
                            case "weapon":
                                statst.WeaponXP += Prices.infuse[Gear.Weapons.Find(w => w.Name == btndata[4]).Rarity];
                                break;
                            case "armor":
                                statst.ArmorXP += Prices.infuse[Gear.Armors.Find(w => w.Name == btndata[4]).Rarity];
                                break;
                            case "extra":
                                statst.ExtraXP += Prices.infuse[Gear.Extras.Find(w => w.Name == btndata[4]).Rarity];
                                break;
                        }
                        break;
                }
                await btn.RespondAsync(embed: Utils.QuickEmbedNormal("Drop", dmsg), ephemeral: true);
            }
            else
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This is not your drop."), ephemeral: true);
            }
        }

        static public async Task ChestOpen(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            ulong userid = (ulong)Convert.ToInt64(btndata[1]);
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == userid
                                        select user).SingleOrDefault();
            if (userid == btn.User.Id)
            {
                if ((DateTime.Now - btn.Message.Timestamp).TotalMinutes > 15)
                {
                    await btn.Message.DeleteAsync();
                    await btn.RespondAsync(embed: Utils.QuickEmbedError("This chest is expired."), ephemeral: true);
                    return;
                }
                Rarity drarity = (Rarity)Enum.Parse(typeof(Rarity), btndata[2]);
                if (stats.Money >= Prices.buy[drarity])
                {
                    await btn.Message.DeleteAsync();
                    stats.Money -= Prices.buy[drarity];
                    Gear.RandomDrop(userid, btn.Channel, drarity, btndata[3]);
                }
                else
                {
                    await btn.RespondAsync(embed: Utils.QuickEmbedError("You can not afford this chest."), ephemeral: true);
                }
            }
            else
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This is not your chest."), ephemeral: true);
            }
        }

        static public async Task DelChar(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            ulong userid = (ulong)Convert.ToInt64(btndata[2]);
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == userid
                                        select user).SingleOrDefault();
            if (userid == btn.User.Id)
            {
                if ((DateTime.Now - btn.Message.Timestamp).TotalMinutes > 5)
                {
                    await btn.Message.DeleteAsync();
                    await btn.RespondAsync(embed: Utils.QuickEmbedError("This prompt is expired."), ephemeral: true);
                    return;
                }
                Utils.DebugLog(Convert.ToString(btn.Message.Id));
                await btn.Message.DeleteAsync();
                if (btndata[1] == "confirm")
                {
                    stats.Deleted = true;
                    await btn.RespondAsync(embed: Utils.QuickEmbedNormal("Success", "Your character was deleted!"), ephemeral: true);
                }
            }
            else
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This is not your prompt."), ephemeral: true);
            }
        }

        static public async Task DoCoinflip(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            ulong userid = (ulong)Convert.ToInt64(btndata[2]);
            ulong userid2 = (ulong)Convert.ToInt64(btndata[3]);

            if ((DateTime.Now - btn.Message.Timestamp).TotalMinutes > 15)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This request is expired."), ephemeral: true);
                return;
            }

            if (btndata[1] == "cancel" && (btn.User.Id == userid || btn.User.Id == userid2))
            {
                await btn.Message.DeleteAsync();
                return;
            }

            if (btn.User.Id != userid2)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This is not your coinflip request."), ephemeral: true);
                return;
            }

            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == userid
                                        select user).SingleOrDefault();
            CharacterStructure stats2 = (from user in tempstorage.characters
                                         where user.PlayerID == userid2
                                         select user).SingleOrDefault();

            if (stats == null || stats2 == null)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("One of you does not have a character."));
                await btn.Message.DeleteAsync();
                return;
            }

            if (userid == userid2)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("Can not coinflip with yourself."), ephemeral: true);
                return;
            }

            int wager = Convert.ToInt32(btndata[4]);

            if (stats.Money < wager || stats2.Money < wager)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("One of you can not afford this coinflip."));
                return;
            }

            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Coinflip"
            };
            embed.WithFooter(Defaults.FOOTER);
            int cfw = Defaults.GRandom.Next(0, 2);
            embed.WithColor(Color.Green);
            switch (cfw)
            {
                case 1:
                    stats.Money += wager;
                    stats2.Money -= wager;
                    embed.Description = $"<@{userid}> takes {wager} coins from <@{userid2}>!";
                    break;

                default:
                    stats.Money -= wager;
                    stats2.Money += wager;
                    embed.Description = $"<@{userid}> looses {wager} coins to <@{userid2}>!";
                    break;
            }
            await btn.Message.DeleteAsync();
            await btn.RespondAsync(embed: embed.Build());
        }
        public static async Task EditGuide(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            switch (btndata[1])
            {
                case "start":
                    Embed guideembed = Utils.QuickEmbedNormal("Guide", Text.guide_page_start);
                    await btn.UpdateAsync(x => x.Embed = guideembed);
                    break;
                case "character":
                    Embed guideembedchar = Utils.QuickEmbedNormal("Guide - Character", Text.guide_page_character);
                    await btn.UpdateAsync(x => x.Embed = guideembedchar);
                    break;
                case "gear":
                    Embed guideembedgear = Utils.QuickEmbedNormal("Guide - Gear", Text.guide_page_gear);
                    await btn.UpdateAsync(x => x.Embed = guideembedgear);
                    break;
                case "floor":
                    Embed guideembedfloor = Utils.QuickEmbedNormal("Guide - Floor", Text.guide_page_floors);
                    await btn.UpdateAsync(x => x.Embed = guideembedfloor);
                    break;
                case "fight":
                    Embed guideembedfight = Utils.QuickEmbedNormal("Guide - Fight", Text.guide_page_fights);
                    await btn.UpdateAsync(x => x.Embed = guideembedfight);
                    break;
                case "grinding":
                    Embed guideembedchests = Utils.QuickEmbedNormal("Guide - Grinding", Text.guide_page_grinding);
                    await btn.UpdateAsync(x => x.Embed = guideembedchests);
                    break;
                case "quests":
                    Embed guideembedquests = Utils.QuickEmbedNormal("Guide - Quests", Text.guide_page_quests);
                    await btn.UpdateAsync(x => x.Embed = guideembedquests);
                    break;
            }
        }

        public static async Task EditHelp(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            switch (btndata[1])
            {
                case "character":
                    Embed helpembedchar = Utils.QuickEmbedNormal("Help - Character", Text.help_page_character);
                    await btn.UpdateAsync(x => x.Embed = helpembedchar);
                    break;
                case "stats":
                    Embed helpembedstats = Utils.QuickEmbedNormal("Help - Stats", Text.help_page_stats);
                    await btn.UpdateAsync(x => x.Embed = helpembedstats);
                    break;
                case "play":
                    Embed helpembeddungeons = Utils.QuickEmbedNormal("Help - Play", Text.help_page_play);
                    await btn.UpdateAsync(x => x.Embed = helpembeddungeons);
                    break;
                case "money":
                    Embed helpembedmoney = Utils.QuickEmbedNormal("Help - Money", Text.help_page_money);
                    await btn.UpdateAsync(x => x.Embed = helpembedmoney);
                    break;
                case "rooms":
                    Embed helpembedtop = Utils.QuickEmbedNormal("Help - Rooms", Text.help_page_rooms);
                    await btn.UpdateAsync(x => x.Embed = helpembedtop);
                    break;
                case "misc":
                    Embed helpembedmisc = Utils.QuickEmbedNormal("Help - Misc.", Text.help_page_misc);
                    await btn.UpdateAsync(x => x.Embed = helpembedmisc);
                    break;
            }
        }
        public static async Task FightDetails(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            ulong userid = (ulong)Convert.ToInt64(btndata[5]);
            if (userid == btn.User.Id)
            {
                Embed om = btn.Message.Embeds.First();
                EmbedBuilder newembed = om.ToEmbedBuilder();
                newembed.AddField("Your Damage", btndata[1]);
                newembed.AddField("Enemy Damage", btndata[2]);
                if (newembed.Color.ToString() == "#2ECC71")
                {
                    newembed.AddField("Rounds", btndata[3]);
                } else
                {
                    newembed.AddField("Rounds", btndata[4]);
                }

                await btn.UpdateAsync(x => { 
                    x.Embed = newembed.Build();
                    x.Components = null;
                });
            }
            else
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This is not your fight."), ephemeral: true);
                return;
            }
        }
        public static async Task PlayerFightDetails(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            ulong userid = (ulong)Convert.ToInt64(btndata[5]);
            ulong userid2 = (ulong)Convert.ToInt64(btndata[6]);
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == userid
                                        select user).SingleOrDefault();
            CharacterStructure stats2 = (from user in tempstorage.characters
                                         where user.PlayerID == userid2
                                         select user).SingleOrDefault();
            if (userid == btn.User.Id || userid2 == btn.User.Id)
            {
                Embed om = btn.Message.Embeds.First();
                EmbedBuilder newembed = om.ToEmbedBuilder();
                newembed.AddField($"{stats.CharacterName} Damage", btndata[1]);
                newembed.AddField($"{stats2.CharacterName} Damage", btndata[2]);
                if (newembed.Color.ToString() == "#2ECC71")
                {
                    newembed.AddField("Rounds", btndata[3]);
                }
                else
                {
                    newembed.AddField("Rounds", btndata[4]);
                }

                await btn.UpdateAsync(x => {
                    x.Embed = newembed.Build();
                    x.Components = null;
                });
            }
            else
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This is not your fight."), ephemeral: true);
                return;
            }
        }
        static public async Task InitPlayerFight(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            ulong userid = (ulong)Convert.ToInt64(btndata[2]);
            ulong userid2 = (ulong)Convert.ToInt64(btndata[3]);

            if ((DateTime.Now - btn.Message.Timestamp).TotalMinutes > 15)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This request is expired."), ephemeral: true);
                return;
            }

            if(userid == userid2)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("Can not fight yourself."), ephemeral: true);
                return;
            }

            if (btndata[1] == "cancel" && (btn.User.Id == userid || btn.User.Id == userid2))
            {
                await btn.Message.DeleteAsync();
                return;
            }

            if (btn.User.Id != userid2)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This is not your fight request."), ephemeral: true);
                return;
            }

            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == userid
                                        select user).SingleOrDefault();
            CharacterStructure stats2 = (from user in tempstorage.characters
                                         where user.PlayerID == userid2
                                         select user).SingleOrDefault();

            if (stats == null || stats2 == null)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("One of you does not have a character."));
                await btn.Message.DeleteAsync();
                return;
            }

            int wager = Convert.ToInt32(btndata[4]);

            if (stats.Money < wager || stats2.Money < wager)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("One of you can not afford this fight."));
                return;
            }

            Tuple<Embed, MessageComponent> simresult = SimulatePlayerFight.Sim(stats, stats2, wager);

            await btn.Message.DeleteAsync();
            await btn.RespondAsync(embed: simresult.Item1, components: simresult.Item2);
        }

        static public async Task QuestView(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            ulong userid = (ulong)Convert.ToInt64(btndata[2]);
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == userid
                                        select user).SingleOrDefault();

            if (userid != btn.User.Id)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This is not your quest."), ephemeral: true);
                return;
            }

            if (stats == null)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("You do not have a character."), ephemeral: true);
                return;
            }

            if (btndata[1] == "cancel")
            {
                stats.QuestData = "none";
                await btn.RespondAsync(embed: Utils.QuickEmbedNormal("Quest", "Quest Cancelled!"));
                await btn.Message.DeleteAsync();
                return;
            }
        }

        static public async Task QuestOffer(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            ulong userid = (ulong)Convert.ToInt64(btndata[2]);
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == userid
                                        select user).SingleOrDefault();

            if (userid != btn.User.Id)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This is not your room."), ephemeral: true);
                return;
            }

            if ((DateTime.Now - btn.Message.Timestamp).TotalMinutes > 15)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This offer is expired."), ephemeral: true);
                return;
            }

            if (stats == null)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("You do not have a character."), ephemeral: true);
                return;
            }

            if (btndata[1] == "accept")
            {
                Quests.QuestFromDisc(btndata[3]).generateQuest(stats);
                await btn.Message.DeleteAsync();
                await btn.RespondAsync(embed: Utils.QuickEmbedNormal("Quest", "Quest Accepted!"));
                return;
            } else if (btndata[1] == "deny")
            {
                await btn.Message.DeleteAsync();
                await btn.RespondAsync(embed: Utils.QuickEmbedNormal("Quest", "Quest Denied!"), ephemeral: true);
                return;
            }
        }

        static public async Task Trade(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            ulong userid = (ulong)Convert.ToInt64(btndata[2]);
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == userid
                                        select user).SingleOrDefault();

            if (userid != btn.User.Id)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This is not your room."), ephemeral: true);
                return;
            }

            if ((DateTime.Now - btn.Message.Timestamp).TotalMinutes > 15)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This offer is expired."), ephemeral: true);
                return;
            }

            if (stats == null)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("You do not have a character."), ephemeral: true);
                return;
            }

            if (btndata[1] == "accept")
            {
                string get = btndata[3];
                int getam = Convert.ToInt32(btndata[4]);
                string give = btndata[5];
                int giveam = Convert.ToInt32(btndata[6]);
                Dictionary<string, int> inv = Utils.InvAsDict(stats);
                if (inv.ContainsKey(give) && inv[give] >= giveam)
                {
                    inv[give] -= giveam;
                    if (inv[give] < 1)
                    {
                        inv.Remove(give);
                    }
                    if (inv.ContainsKey(get))
                    {
                        inv[get] += getam;
                    } else
                    {
                        inv.Add(get, getam);
                    }
                    Utils.SaveInv(stats, inv);
                    await btn.Message.DeleteAsync();
                    await btn.RespondAsync(embed: Utils.QuickEmbedNormal("Trade", $"Quest Accepted!\n-{giveam}x {give}\n+{getam}x {get}"));
                    if (stats.QuestData != "none")
                    {
                        stats.Quest.UpdateProgress(stats, QuestActivations.TradeComplete);
                    }
                    return;
                }
                else
                {
                    await btn.RespondAsync(embed: Utils.QuickEmbedError("You can not afford this trade!"), ephemeral: true);
                    return;
                }
            }
            else if (btndata[1] == "deny")
            {
                await btn.Message.DeleteAsync();
                await btn.RespondAsync(embed: Utils.QuickEmbedNormal("Trade", "Trade Denied!"), ephemeral: true);
                return;
            }
        }

        static public async Task DisplayGlobalLeaderBoard(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            ulong userid = (ulong)Convert.ToInt64(btndata[2]);
            
            switch(btndata[1])
            {
                case "lvl":
                    if (tempstorage.characters.Count < 3)
                    {
                        await btn.UpdateAsync(x => {
                            x.Components = null;
                            x.Embed = Utils.QuickEmbedError("There is not enough players to do this.");
                        });
                        return;
                    }

                    EmbedBuilder embed = new EmbedBuilder
                    {
                        Title = "Global Level Leaderboard"
                    };

                    List<CharacterStructure> tco = tempstorage.characters.Where(x => x.Deleted == false).OrderByDescending(c => c.EXP).ToList();
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
                                                where user.PlayerID == userid
                                                select user).SingleOrDefault();

                    if (stats != null && stats.Deleted == false && !t3c.Any(x => x.PlayerID == stats.PlayerID))
                    {
                        embed.Description += $"**Your Place:** {tco.IndexOf(stats) + 1}";
                    }

                    embed.WithColor(Color.DarkMagenta);
                    embed.WithFooter(Defaults.FOOTER);
                    await btn.UpdateAsync(x => { x.Embed = embed.Build(); x.Components = null; });

                    break;

                case "money":
                    EmbedBuilder embedm = new EmbedBuilder
                    {
                        Title = "Global Money Leaderboard"
                    };

                    if (tempstorage.characters.Count < 3)
                    {
                        SocketMessage m = btn.Message;
                        await btn.UpdateAsync(x => {
                            x.Components = null;
                            x.Embed = Utils.QuickEmbedError("There is not enough players to do this.");
                            });
                        return;
                    }

                    List<CharacterStructure> tcom = tempstorage.characters.Where(x => x.Deleted == false).OrderByDescending(c => c.Money).ToList();
                    List<CharacterStructure> t3cm = tcom.Take(3).ToList();

                    string avatarurlm = "";
                    IUser founduserm = await Defaults.CLIENT.GetUserAsync(t3cm[0].PlayerID);
                    if (founduserm != null)
                    {
                        avatarurlm = founduserm.GetAvatarUrl();
                    }
                    else
                    {
                        avatarurlm = Defaults.CLIENT.CurrentUser.GetDefaultAvatarUrl();
                    }

                    embedm.ThumbnailUrl = avatarurlm;

                    embedm.Description = "\n" +
                        $"**1: {t3cm[0].CharacterName}**\n" +
                        $"Money: {t3cm[0].Money} coins\n\n" +
                        $"**2: {t3cm[1].CharacterName}**\n" +
                        $"Money: {t3cm[1].Money} coins\n\n" +
                        $"**3: {t3cm[2].CharacterName}**\n" +
                        $"Money: {t3cm[2].Money} coins\n\n";

                    CharacterStructure statsm = (from user in tempstorage.characters
                                                where user.PlayerID == userid
                                                select user).SingleOrDefault();

                    if (statsm != null && statsm.Deleted == false && !t3cm.Any(x => x.PlayerID == statsm.PlayerID))
                    {
                        embedm.Description += $"**Your Place:** {tcom.IndexOf(statsm) + 1}";
                    }

                    embedm.WithColor(Color.DarkMagenta);
                    embedm.WithFooter(Defaults.FOOTER);
                    await btn.UpdateAsync(x => { x.Embed = embedm.Build(); x.Components = null; });

                    break;

                case "gear":
                    EmbedBuilder embedg = new EmbedBuilder
                    {
                        Title = "Global Money Leaderboard"
                    };

                    if (tempstorage.characters.Count < 3)
                    {
                        SocketMessage m = btn.Message;
                        await btn.UpdateAsync(x => {
                            x.Components = null;
                            x.Embed = Utils.QuickEmbedError("There is not enough players to do this.");
                        });
                        return;
                    }

                    List<CharacterStructure> tcog = tempstorage.characters.Where(x => x.Deleted == false).OrderByDescending(c => Prices.sell[c.Weapon.Rarity] + Prices.sell[c.Armor.Rarity] + Prices.sell[c.Extra.Rarity]).ToList();
                    List<CharacterStructure> t3cg = tcog.Take(3).ToList();

                    string avatarurlg = "";
                    IUser founduserg = await Defaults.CLIENT.GetUserAsync(t3cg[0].PlayerID);
                    if (founduserg != null)
                    {
                        avatarurlg = founduserg.GetAvatarUrl();
                    }
                    else
                    {
                        avatarurlg = Defaults.CLIENT.CurrentUser.GetDefaultAvatarUrl();
                    }

                    embedg.ThumbnailUrl = avatarurlg;

                    embedg.Description = "\n" +
                        $"**1: {t3cg[0].CharacterName}**\n" +
                        $"Weapon: {t3cg[0].Weapon.Name}\nArmor: {t3cg[0].Armor.Name}\nExtra: {t3cg[0].Extra.Name} \n\n" +
                        $"**2: {t3cg[1].CharacterName}**\n" +
                        $"Weapon: {t3cg[1].Weapon.Name}\nArmor: {t3cg[1].Armor.Name}\nExtra: {t3cg[1].Extra.Name}\n\n" +
                        $"**3: {t3cg[2].CharacterName}**\n" +
                        $"Weapon: {t3cg[2].Weapon.Name}\nArmor: {t3cg[2].Armor.Name}\nExtra: {t3cg[2].Extra.Name}\n\n";

                    CharacterStructure statsg = (from user in tempstorage.characters
                                                where user.PlayerID == userid
                                                select user).SingleOrDefault();

                    if (statsg != null && statsg.Deleted == false && !t3cg.Any(x => x.PlayerID == statsg.PlayerID))
                    {
                        embedg.Description += $"**Your Place:** {tcog.IndexOf(statsg) + 1}";
                    }

                    embedg.WithColor(Color.DarkMagenta);
                    embedg.WithFooter(Defaults.FOOTER);
                    await btn.UpdateAsync(x => { x.Embed = embedg.Build(); x.Components = null; });

                    break;
            }
        }

        static public async Task ShopButton(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            switch(btndata[1])
            {
                case "buy":
                    await Shop.ShopBuy(btn, btndata[2]);
                    break;
            }
        }
    }
}