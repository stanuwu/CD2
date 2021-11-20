using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    enum WeaponNames
    {
        Claymore,
        Shuriken
    }

    enum ArmorNames
    {
        Chainmail,
        Stonearmor,
    }

    enum ExtraNames
    {
        Lightbug,
        Turboshroom
    }

    enum Rarity
    {
        Common = 1,
        Uncommon = 2,
        Rare = 3,
        Epic = 4,
        Legendary = 5,
        Unstable = 6,
        Corrupted = 7,
        Unique = 8
    }

    interface IGearStats
    {
        string Name { get; set; }
        string Description { get; set; }
        Rarity Rarity { get; set; }
    }

    class Weapon : IGearStats
    {
        public WeaponNames Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Damage { get; set; }
        public Rarity Rarity { get; set; }
        public void CustomEffect(ulong playerID, Enemy enemy)
        {
            switch (this.Type)
            {
                case WeaponNames.Claymore:

                    break;
            }
        }
    }

    class Armor : IGearStats
    {
        public ArmorNames Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Resistance { get; set; }
        public Rarity Rarity { get; set; }
        public void CustomEffect(ulong playerID, Enemy enemy)
        {
            switch (this.Type)
            {
                case ArmorNames.Stonearmor:

                    break;
            }
        }
    }

    class Extra : IGearStats
    {
        public ExtraNames Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Damage { get; set; }
        public int Resistance { get; set; }
        public Rarity Rarity { get; set; }
        public void CustomEffect(ulong playerID, Enemy enemy)
        {
            switch (this.Type)
            {
                case ExtraNames.Turboshroom:

                    break;
            }
        }
    }
}
