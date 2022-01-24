using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public static class Trades
    {
        public static List<Tuple<string, int, string, int>> list = new List<Tuple<string, int, string, int>>
        {
            new Tuple<string, int, string, int> ("Stone", 5, "Wood", 5),
            new Tuple<string, int, string, int> ("Stone", 20, "Diamond", 1),
            new Tuple<string, int, string, int> ("Stone", 100, "Diamond", 10),
            new Tuple<string, int, string, int> ("Wood", 20, "Tropical Wood", 1),
            new Tuple<string, int, string, int> ("Wood", 100, "Tropical Wood", 10),
            new Tuple<string, int, string, int> ("Wood", 1, "Paper", 10),
            new Tuple<string, int, string, int> ("Wood", 10, "Paper", 100),
            new Tuple<string, int, string, int> ("Torn Bible", 1, "Paper", 3),
            new Tuple<string, int, string, int> ("Scale", 5, "Fish Scale", 5),
            new Tuple<string, int, string, int> ("Feather", 10, "Small Game Pelt", 3),
            new Tuple<string, int, string, int> ("Fish Scale", 20, "Hydra Scale", 1),
            new Tuple<string, int, string, int> ("Fish Scale", 100, "Hydra Scale", 5),
            new Tuple<string, int, string, int> ("Small Game Pelt", 20, "Bear Pelt", 1),
            new Tuple<string, int, string, int> ("Small Game Pelt", 100, "Bear Pelt", 5),
            new Tuple<string, int, string, int> ("Small Game Pelt", 100, "Exotic Pelt", 1),
            new Tuple<string, int, string, int> ("Bear Pelt", 5, "Bear Pelt", 1),
            new Tuple<string, int, string, int> ("Mushroom", 20, "Rare Root", 1),
            new Tuple<string, int, string, int> ("Mushroom", 100, "Rare Root", 10),
            new Tuple<string, int, string, int> ("Small Pouch", 10, "Diamond", 1),
            new Tuple<string, int, string, int> ("Diamond", 10, "Steel", 5),
            new Tuple<string, int, string, int> ("Steel", 10, "Silver", 5),
            new Tuple<string, int, string, int> ("Silver", 5, "Gold", 1),
            new Tuple<string, int, string, int> ("Gold", 10, "Platinum", 1),
            new Tuple<string, int, string, int> ("Hydra Scale", 25, "Dragon Scale", 1),
            new Tuple<string, int, string, int> ("Arrow", 5, "Rusty Sword", 1),
            new Tuple<string, int, string, int> ("Small Wolf Pelt", 3, "Medium Wolf Pelt", 1),

        };
    }
    public static class Crafting
    {
        public static List<Tuple<string, Dictionary<string, int>, int, string>> list = new List<Tuple<string, Dictionary<string, int>, int, string>> {
            
            //Weapons
            new Tuple<string, Dictionary<string, int>, int, string> ("Wooden Sword", new Dictionary<string, int> { { "Wood", 5 } }, 100, "weapon"),
            new Tuple<string, Dictionary<string, int>, int, string> ("Restored Sword", new Dictionary<string, int> { { "Rusty Sword", 1 }, {"Steel", 1 } }, 250, "weapon"),
            new Tuple<string, Dictionary<string, int>, int, string> ("Diamond Tip Sword", new Dictionary<string, int> { { "Stone", 200 }, { "Diamond", 1 } }, 1000, "weapon"),

            //Armor
            new Tuple<string, Dictionary<string, int>, int, string> ("Fish Scale Armor", new Dictionary<string, int> { { "Fish Scale", 5 } }, 100, "armor"),
            new Tuple<string, Dictionary<string, int>, int, string> ("Paper Armor", new Dictionary<string, int> { { "Paper", 100 } }, 500, "armor"),
            new Tuple<string, Dictionary<string, int>, int, string> ("Steel Armor", new Dictionary<string, int> { { "Steel", 10 } }, 1000, "armor"),

            //Extra
            new Tuple<string, Dictionary<string, int>, int, string> ("Bunny Ears", new Dictionary<string, int> { { "Small Game Pelt", 5 } }, 100, "extra"),
            new Tuple<string, Dictionary<string, int>, int, string> ("Rabbit Foot", new Dictionary<string, int> { { "Small Game Pelt", 5 } }, 100, "extra"),

            // new Tuple<string, Dictionary<string, int>, int, string> ("", new Dictionary<string, int> { { "", 0 } }, 0, ""),

        };
    }
}
