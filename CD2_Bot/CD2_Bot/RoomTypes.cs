using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public static class Rooms
    {
        static List<string> RoomTypes = new List<string>
        {
            "rMoney",
            "rChest",
            "rMerchant",
            "rTrap",
            "rQuest",
            "rCraft",
        };
        public static Tuple<Embed, Optional<MessageComponent>> ExecuteRoom(string roomtype, ulong uid, ulong gid, ISocketMessageChannel channel)
        {
            Embed embed = null;
            Optional<MessageComponent> msgc = null;
            Tuple<Embed, Optional<MessageComponent>> tosend = new Tuple<Embed, Optional<MessageComponent>>(null, null);
            CharacterStructure stats = (from user in tempstorage.characters
                                         where user.PlayerID == uid
                                         select user).SingleOrDefault();

            string biome = "";
            if (roomtype.Contains(";"))
            {
                string[] data = roomtype.Split(';');
                roomtype = data[0];
                biome = data[1];
            }
            switch (roomtype)
            {
                case "rFight":
                    Biome sbiome = Biome.Any;
                    if (biome != "")
                    {
                        sbiome = (Biome) Enum.Parse(typeof (Biome), biome);
                    }
                    Enemy opponent = EnemyGen.RandomEnemy(stats.Lvl, sbiome);

                    Tuple<Embed,MessageComponent> fr = SimulateFight.Sim(opponent, stats);
                    embed = fr.Item1;
                    msgc = fr.Item2;

                    int gearroll = Defaults.GRandom.Next(1, Defaults.GEARDROPCHANCE);;
                    //creative way to check for win (dont ask lol)
                    if (embed.Color.ToString() == "#2ECC71" && gearroll == 1)
                    {
                        Gear.RandomDrop(stats.PlayerID, channel);
                    }
                    break;
                case "rMoney":
                    int mfound = 300 + stats.Lvl * 30;
                    stats.Money += mfound;
                    embed = Utils.QuickEmbedNormal("Room", $"Lucky! You found {mfound} coins!");
                    break;
                case "rChest":
                    Rarity chestrarity = Prices.buy.Keys.ToList()[Defaults.GRandom.Next(Prices.buy.Count)];
                    string item = "";
                    int itemchance = Defaults.GRandom.Next(0, 3);
                    switch (itemchance)
                    {
                        case 0:
                            item = "weapon";
                            break;
                        case 1:
                            item = "armor";
                            break;
                        case 2:
                            item = "extra";
                            break;
                    }
                    MessageComponent btn = new ComponentBuilder()
                        .WithButton("Open", "chestopen;" + uid + ";" + chestrarity.ToString() + ";" + item, ButtonStyle.Success).Build();
                    msgc = btn;
                    embed = Utils.QuickEmbedNormal($"You found: {chestrarity.ToString()} {char.ToUpper(item[0]) + item.Substring(1)} Chest", $"You can open this chest for {Prices.buy[chestrarity]} coins.\nThis chest will expire in 15 minutes.");
                    break;
                case "rMerchant":
                    Tuple<string, int, string, int> trade = Trades.list[Defaults.GRandom.Next(Trades.list.Count)];
                    int order = Defaults.GRandom.Next(0, 1);
                    string get = trade.Item1;
                    int getam = trade.Item2;
                    string give = trade.Item3;
                    int giveam = trade.Item4;
                    if(order == 0)
                    {
                        get = trade.Item3;
                        getam = trade.Item4;
                        give = trade.Item1;
                        giveam = trade.Item2;
                    }
                    embed = Utils.QuickEmbedNormal("Room", $"You find a merchant that is willing to trade with you. \nHe wants {giveam}x {give} for his {getam}x {get}.\nThis offer will expire in 15 minutes.");
                    msgc = new ComponentBuilder()
                        .WithButton("Accept", "trade;accept;" + uid + ";" + get + ";" + getam + ";" + give + ";" + giveam, ButtonStyle.Success)
                        .WithButton("Deny", "trade;deny;"+ uid, ButtonStyle.Danger).Build();
                    break;
                case "rCraft":
                    embed = Utils.QuickEmbedNormal("Room", "Do you want to forge on of these items?");
                    SelectMenuBuilder smb = new SelectMenuBuilder()
                        .WithPlaceholder("Select an Item to craft!")
                        .WithCustomId($"craft;{uid}")
                        .WithMinValues(1)
                        .WithMaxValues(1);
                    foreach (Tuple<string, string, int, int, string> r in Crafting.list)
                    {
                        smb.AddOption(r.Item1, (r.Item1 + ";" + r.Item2 + ";" + r.Item3 + ";" + r.Item4 + ";" + r.Item5) , $"Costs: {r.Item3} {r.Item2} and {r.Item4} Coins.");
                    }
                    msgc = new ComponentBuilder().WithSelectMenu(smb).Build();
                    break;
                case "rTrap":
                    int mlost = 50 + stats.Lvl * 20;
                    if (stats.Money < mlost)
                    {
                        mlost = stats.Money;
                    }
                    stats.Money -= mlost;
                    embed = Utils.QuickEmbedNormal("Room", $"A Trap! You lost {mlost} coins.");
                    break;
                case "rQuest":
                    if (stats.QuestData == "none")
                    {
                        List<Quest> pq = Quests.questList.FindAll(q => q.LevelLimit <= stats.Lvl);
                        Quest randomQuest = pq[Defaults.GRandom.Next(pq.Count)];
                        embed = Utils.QuickEmbedNormal("Quest Offer", randomQuest.ObtainingDialogue + "\n" + randomQuest.ViewRewards() + "\nThis offer will expire after 15 minutes!");
                        msgc = new ComponentBuilder()
                            .WithButton("Accept", "questroom;accept;" + stats.PlayerID + ";" + randomQuest.Descriminator, ButtonStyle.Success)
                            .WithButton("Cancel", "questroom;deny;" + stats.PlayerID, ButtonStyle.Danger).Build();
                    } else
                    {
                        Quest q = Quests.WhatQuest(stats.QuestData);
                        if (q.isQuestExpired(stats))
                        {
                            embed = Utils.QuickEmbedNormal("Quest Failed", q.QuestFailedDialogue);
                            stats.QuestData = "none";
                        }
                        else if (q.isQuestCompleted(stats))
                        {
                            string qd = stats.QuestData;
                            embed = Utils.QuickEmbedNormal("Quest Complete!", q.CompletionDialogue);
                            EmbedBuilder b = embed.ToEmbedBuilder();
                            b.Description += "\n" + q.GenerateRewards(stats);
                            embed = b.Build();
                            if (stats.QuestData == qd)
                            {
                                stats.QuestData = "none";
                            } else
                            {
                                embed = Utils.QuickEmbedError("Your current quest is not completed.");
                            }
                        }
                    }
                    break;
                case "rRandom":
                    tosend = ExecuteRoom(RoomTypes[Defaults.GRandom.Next(RoomTypes.Count)], uid, gid, channel);
                    break;
                default:
                    embed = Utils.QuickEmbedNormal("Room", roomtype);
                    break;
            }
            //update guild stats
            if (roomtype != "rRandom")
            {
                tosend = new Tuple<Embed, Optional<MessageComponent>>(embed, msgc);
                tempstorage.guilds.Find(g => g.GuildID == gid).DoorsOpened += 1;
            }
            return tosend;
        }
        public static MessageComponent getRoomSelection(ulong uid)
        {
            SelectMenuBuilder smb = new SelectMenuBuilder()
                .WithPlaceholder("Select a Room!")
                .WithCustomId($"floorroomselect;{uid}")
                .WithMinValues(1)
                .WithMaxValues(1);
            string rType1 = RoomTypes[Defaults.GRandom.Next(RoomTypes.Count)];
            string rType2 = RoomTypes.FindAll(x => x!=rType1)[Defaults.GRandom.Next(RoomTypes.FindAll(x => x != rType1).Count)];
            Biome biome1 = BiomesScaling.randomBiome();
            List<string> rooms = new List<string> { "rRandom",  "rFight", rType1, rType2, "rFight" };
            int count = 0;
            foreach (string r in rooms)
            {
                if (r == "rFight")
                {
                    count++;
                }
                string rName = "Default Room";
                string rId = r;
                if (count == 2)
                {
                    rId += (";" + biome1.ToString());
                }
                string rDesc = "...";
                switch (r)
                {
                    case "rFight":
                        rName = "Room of Encounters";
                        rDesc = "A Monster is waiting in this room for you.";
                        if (count == 2)
                        {
                            rName = "Focused Room of Encounters";
                            rDesc = "Biome: " + biome1.ToString();
                        }
                        break;
                    case "rMoney":
                        rName = "Room of Wealth";
                        rDesc = "You will find riches in this room.";
                        break;
                    case "rChest":
                        rName = "Room of Surprises";
                        rDesc = "There is a chest in this room. What might it hold?";
                        break;
                    case "rRandom":
                        rName = "Room of Darkness";
                        rDesc = "You can not see what is inside of this room.";
                        break;
                    case "rMerchant":
                        rName = "Room of Deals";
                        rDesc = "In this room you may find one who is a master of deals.";
                        break;
                    case "rTrap":
                        rName = "Room of Traps";
                        rDesc = "This room contains a trap.";
                        break;
                    case "rQuest":
                        rName = "Room of Adventures";
                        rDesc = "This room may advance you in your Quest.";
                        break;
                    case "rCraft":
                        rName = "Room of Forging";
                        rDesc = "You can forge new gear in this room.";
                        break;
                }

                smb.AddOption(rName, rId, rDesc);
            }
            smb.Options = smb.Options.OrderBy(o => o.GetHashCode()).ToList();
            return new ComponentBuilder().WithSelectMenu(smb).Build();
        }
    }
}