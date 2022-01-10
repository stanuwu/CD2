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
            //"rMerchant",
            "rTrap",
            //"rQuest",
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
                    embed = Utils.QuickEmbedNormal("Room", "Merchants not Implemented yet.");
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
                    embed = Utils.QuickEmbedNormal("Room", "Quests not Implemented yet.");
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
            Biome biome1 = BiomesScaling.randomBiome();
            Biome biome2 = BiomesScaling.randomBiome();
            List<string> rooms = new List<string> { "rRandom", "rFight", rType1, "rFight", "rFight" };
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
                else if (count == 3)
                {
                    rId += (";" + biome2.ToString());
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
                        } else if (count == 3)
                        {
                            rName = "Focused Room of Encounters";
                            rDesc = "Biome: " + biome2.ToString();
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
                }

                smb.AddOption(rName, rId, rDesc);
            }
            smb.Options = smb.Options.OrderBy(o => o.GetHashCode()).ToList();
            return new ComponentBuilder().WithSelectMenu(smb).Build();
        }
    }
}