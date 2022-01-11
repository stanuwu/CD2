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
        };
    }
    public static class Crafting
    {
        public static List<Tuple<string, string, int, int, string>> list = new List<Tuple<string, string, int, int, string>> {
            new Tuple<string, string, int, int, string> ("Hardened Stone Sword", "Stone", 500, 1000, "weapon"),
            new Tuple<string, string, int, int, string> ("Wooden Sword", "Wood", 5, 100, "weapon"),
        };
    }
}
