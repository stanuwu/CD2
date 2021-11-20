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
        public static Embed ExecuteRoom(string roomtype)
        {
            Embed embed;
            switch (roomtype)
            {
                case "rFight":
                    embed = Utils.QuickEmbedNormal("Fight", "The room contains a Monster!");
                    break;
                case "rMoney":
                    embed = Utils.QuickEmbedNormal("Lucky!", "You found some coins!");
                    break;
                case "rRandom":
                    embed = ExecuteRoom(RoomTypes[Defaults.GRandom.Next(RoomTypes.Count)]);
                    break;
                default:
                    embed = Utils.QuickEmbedNormal("Room", roomtype);
                    break;
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