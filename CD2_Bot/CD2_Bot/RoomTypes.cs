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
            "rFight",
            "rChest",
            "rRandom",
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
                default:
                    embed = Utils.QuickEmbedNormal("Room", roomtype);
                    break;
            }
            return embed;
        }
    }
 
}
