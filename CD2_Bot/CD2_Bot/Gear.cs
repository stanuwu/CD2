using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public enum Rarity
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
        int Level { get; }
        int EXP { get; set; }
        Rarity Rarity { get; set; }
        string CustomEffectName { get; set; }
        string CustomEffectDescription { get; set; }
    }

    public class Weapon : IGearStats
    {
        public Weapon(string name, string description, int damage, int exp, string customeffectname, string customeffectdesc, Rarity rarity)
        {
            Name = name;
            Description = description;
            BaseDamage = damage;
            EXP = exp;
            CustomEffectName = customeffectname;
            CustomEffectDescription = customeffectdesc;
            Rarity = rarity;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BaseDamage { get; set; }
        public int Damage { get { return BaseDamage * (Level/50 + 1); } }
        public int Level { get { return (int)Math.Floor((double)this.EXP / 250); } }
        public int EXP { get; set; }
        public string CustomEffectName { get; set; }
        public string CustomEffectDescription { get; set; }
        public Rarity Rarity { get; set; }
        public void CustomEffect(ulong playerID, Enemy enemy)
        {
            switch (this.Name)
            {
                case "TestWaffa":

                    break;
            }
        }
    }

    public class Armor : IGearStats
    {
        public Armor(string name, string description, int resistance, int exp, string customeffectname, string customeffectdesc, Rarity rarity)
        {
            Name = name;
            Description = description;
            Resistance = resistance;
            EXP = exp;
            CustomEffectName = customeffectname;
            CustomEffectDescription = customeffectdesc;
            Rarity = rarity;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Resistance { get; set; }
        public int Level { get { return (int)Math.Floor((double)this.EXP / 250); } }
        public int EXP { get; set; }
        public string CustomEffectName { get; set; }
        public string CustomEffectDescription { get; set; }
        public Rarity Rarity { get; set; }
        public void CustomEffect(ulong playerID, Enemy enemy)
        {
            switch (this.Name)
            {
                case "TestRusta":

                    break;
            }
        }
    }
    
    public class Extra : IGearStats
    {
        public Extra(string name, string description, int damage, int heal, int exp, string customeffectname, string customeffectdesc, Rarity rarity)
        {
            Name = name;
            Description = description;
            BaseDamage = damage;
            BaseHeal = heal;
            EXP = exp;
            CustomEffectName = customeffectname;
            CustomEffectDescription = customeffectdesc;
            Rarity = rarity;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int BaseDamage { get; set; }
        public int Damage { get { return BaseDamage * (Level / 50 + 1); } }
        public int BaseHeal { get; set; }
        public int Heal { get { return BaseHeal * (Level / 500 + 1); } }
        public int Level { get { return (int)Math.Floor((double)this.EXP / 250); } }
        public int EXP { get; set; }
        public string CustomEffectName { get; set; }
        public string CustomEffectDescription { get; set; }
        public Rarity Rarity { get; set; }
        public void CustomEffect(ulong playerID, Enemy enemy)
        {
            switch (this.Name)
            {
                case "TestExtra":

                    break;
            }
        }
    }

    public static class Gear
    {
        public static List<Weapon> Weapons = new List<Weapon>
        {
            new Weapon("Stick", "A simple wooden stick.", 5, 0, null, null, Rarity.Common),
            new Weapon("Shortsword", "A basic blade crafted with simple iron.", 8, 0, null, null, Rarity.Common),
        };

        public static List<Armor> Armors = new List<Armor>
        {
            new Armor("Rags", "Really dirty and torn apart.", 100, 0, null, null, Rarity.Common),
            new Armor("Socks", "Knee-high socks to protect the wearer from dirt and nothing else.", 99, 0, null, null, Rarity.Common),
        };

        public static List<Extra> Extras = new List<Extra>
        {
            new Extra("Pendant", "A small, ochre amulet.", 1, 2, 0, null, null, Rarity.Common),
            new Extra("Firefly", "Little insect buddy that seems to follow you wherever you go.", 1, 5, 0, null, null, Rarity.Common),
        };
    }
}
