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
                case "playerfightdetails":
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
                await btn.Message.DeleteAsync(); ;
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
                case 0:
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
                    Embed guideembed = Utils.QuickEmbedNormal("Guide", "Welcome to Custom Dungeons 2!\n In this text based RPG you can create your own character and explore a never ending dungeon, filled with monsters and riches. Level your character, slay bosses and always strive for the best loot, but be aware that enemies wil also become stronger alongside you.");
                    await btn.UpdateAsync(x => x.Embed = guideembed);
                    break;
                case "character":
                    Embed guideembedchar = Utils.QuickEmbedNormal("Guide - Character", "Character\nYour character has a custom name, a custom description, a title, a selectable class, gear (weapon, armor and an extra), money and a stat multiplier which is calculated through your current level");
                    await btn.UpdateAsync(x => x.Embed = guideembedchar);
                    break;
                case "gear":
                    Embed guideembedgear = Utils.QuickEmbedNormal("Guide - Gear", "Gear\nYour character's gear consists of a weapon, an armor and an extra. All of them have different rarities, showing how strong and valuable they are, and some even have custom effects.");
                    await btn.UpdateAsync(x => x.Embed = guideembedgear);
                    break;
                case "floor":
                    Embed guideembedfloor = Utils.QuickEmbedNormal("Guide - Floor", "Floor\nThe main gameplay aspect. You can either encounter a monster, find money or even chests, or you may be unlucky and stumble into a trap, robbing you of your hard earned money.");
                    await btn.UpdateAsync(x => x.Embed = guideembedfloor);
                    break;
                case "fight":
                    Embed guideembedfight = Utils.QuickEmbedNormal("Guide - Fight", "Fight\nA fight against a single monster. You have to defeat it for a chance of getting (crafting) item drops and always an ammount of money. If it defeats you, you won't be able to proceed into antoher floor until you have enough health again.");
                    await btn.UpdateAsync(x => x.Embed = guideembedfight);
                    break;
                case "chests":
                    Embed guideembedchests = Utils.QuickEmbedNormal("Guide - Chests", "Chests\nChests containing gear drops, which can be stumpled upon in room of surprises. Depending on rarity, the price to open them can be harsh, but even when the player isn't happy with their lot, they can sell it right away.");
                    await btn.UpdateAsync(x => x.Embed = guideembedchests);
                    break;
                case "quests":
                    Embed guideembedquests = Utils.QuickEmbedNormal("Guide - Quests", "Quests\nYou can obtain one of these tasks by entering a room of adventures and view your progress with the quest command. Be sure to complete them in time and visit the room of adventures again to recieve your reward.");
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
                    Embed helpembedchar = Utils.QuickEmbedNormal("Help - Character", "``<start [Character Name]``\n Create a character with the given name (if you do not have one already). \n\n ``<character <[UID]>``\n Views your character (or someone elses, if given an ID). \n\n ``<stats <[UID]>``\n Views your character's gear (or someone elses, if given an ID). \n\n ``<inventory``\n Shows you your inventory. \n\n ``<reset``\n Deletes your character. \n\n ``<rename [Character Name]``\n Renames your character to the given name. \n\n ``<description``\n Gives your character the given description.");
                    await btn.UpdateAsync(x => x.Embed = helpembedchar);
                    break;
                case "stats":
                    Embed helpembedstats = Utils.QuickEmbedNormal("Help - Stats", "``<weapon [Weapon Name]``\n Views the stats of the given weapon. \n\n ``<armor [Armor Name]`` \n Views the stats of the given armor. \n\n ``<extra [Extra Name]`` \n Views the stats of the given extra. \n\n ``<monster [Monster Name]`` \n Views the stats of the given monster. \n\n\n Ex.: <armor Bone");
                    await btn.UpdateAsync(x => x.Embed = helpembedstats);
                    break;
                case "dungeons":
                    Embed helpembeddungeons = Utils.QuickEmbedNormal("Help - Dungeons", "``<floor``\n Lets you choose a door to open in your server's dungeon. \n\n ``<train <[Gear Type]>``\n Lets you train with your gear, gaining XP for it. Can be done for a specific piece or every piece at once. \n\n ``<quest`` \nView your current quest, progress and how much time you have remaining.");
                    await btn.UpdateAsync(x => x.Embed = helpembeddungeons);
                    break;
                case "money":
                    Embed helpembedmoney = Utils.QuickEmbedNormal("Help - Money", "``<coinflip [Amount of money bet] <[@Opponent]>``\n Coinflip against an AI or another player for money. \n\n Ex.: <coinflip 500 @stan  \n\n ``<slots [Amount of money bet]``\n Spin a slot machine for money. \n\n <slots 2000");
                    await btn.UpdateAsync(x => x.Embed = helpembedmoney);
                    break;
                case "top":
                    Embed helpembedtop = Utils.QuickEmbedNormal("Help - Top", "``<lvltop``\n Showcases the top characters across all servers by level. \n\n ``<moneytop``\n Showcases the top characters across all servers by amount of money owned. \n\n ``<geartop``\n Showcases the top characters across all servers by gear equipped. \n\n ``<servertop``\n Showcases the top servers by various criterias.");
                    await btn.UpdateAsync(x => x.Embed = helpembedtop);
                    break;
                case "misc":
                    Embed helpembedmisc = Utils.QuickEmbedNormal("Help - Misc.", "``<guide``\n A guidebook for every part of CD2. \n\n ``<server``\n Showcases how many doors have been opened, bosses have been slain and quests have been finished on your server. \n\n ``<pvp [@player] <[wager]>`` \nChallange another player to a fight and optionally wager coins. Currently does not support custom gear abilities. \n\n ``<farm [activity]`` Farm some crafting/trading items. Current activities: fishing, mining, foraging, harvesting, hunting.");
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
            ulong userid = (ulong)Convert.ToInt64(btndata[7]);
            ulong userid2 = (ulong)Convert.ToInt64(btndata[8]);
            if (userid == btn.User.Id || userid2 == btn.User.Id)
            {
                Embed om = btn.Message.Embeds.First();
                EmbedBuilder newembed = om.ToEmbedBuilder();
                newembed.AddField($"{btndata[5]} Damage", btndata[1]);
                newembed.AddField($"{btndata[6]} Damage", btndata[2]);
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
    }
}