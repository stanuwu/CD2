using Discord;
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
        };
        public static Embed ExecuteRoom(string roomtype, ulong uid, ulong gid)
        {
            Embed embed;
            CharacterStructure stats = (from user in tempstorage.characters
                                         where user.PlayerID == uid
                                         select user).SingleOrDefault();
            switch (roomtype)
            {
                case "rFight":
                    Enemy opponent = EnemyGen.RandomEnemy(stats.Lvl);

                    embed = SimulateFight.Sim(opponent, stats);
                    break;
                case "rMoney":
                    int mfound = 300 + stats.Lvl * 30;
                    stats.Money += mfound;
                    embed = Utils.QuickEmbedNormal("Room", $"Lucky! You found {mfound} coins!");
                    break;
                case "rChest":
                    embed = Utils.QuickEmbedNormal("Room", "Chests not Implemented yet.");
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
                    embed = ExecuteRoom(RoomTypes[Defaults.GRandom.Next(RoomTypes.Count)], uid, gid);
                    break;
                default:
                    embed = Utils.QuickEmbedNormal("Room", roomtype);
                    break;
            }
            //update guild stats
            if (roomtype != "rRandom")
            {
                tempstorage.guilds.Find(g => g.GuildID == gid).DoorsOpened += 1;
            }
            return embed;
        }
        public static MessageComponent getRoomSelection(ulong uid)
        {
            SelectMenuBuilder smb = new SelectMenuBuilder()
                .WithPlaceholder("Select a Room!")
                .WithCustomId($"floorroomselect;{uid}")
                .WithMinValues(1)
                .WithMaxValues(1);
            string rType1 = RoomTypes[Defaults.GRandom.Next(RoomTypes.Count)];
            List<string> rooms = new List<string> { "rRandom", "rFight", rType1 };
            foreach (string r in rooms)
            {
                string rName = "Default Room";
                string rId = r;
                string rDesc = "...";
                switch (r)
                {
                    case "rFight":
                        rName = "Room of Encounters";
                        rDesc = "A Monster is waiting in this room for you.";
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
            return new ComponentBuilder().WithSelectMenu(smb).Build();
        }
    }
}