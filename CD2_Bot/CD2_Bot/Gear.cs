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
        public Weapon Clone()
        {
            return (Weapon)this.MemberwiseClone();
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
        public Armor Clone()
        {
            return (Armor)this.MemberwiseClone();
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
        public Extra Clone()
        {
            return (Extra)this.MemberwiseClone();
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
            // commons
            new Extra("Pendant", "A small, ochre amulet.", 1, 2, 0, null, null, Rarity.Common),
            new Extra("Firefly", "Little insect buddy that seems to follow you wherever you go.", 1, 4, 0, null, null, Rarity.Common),
            new Extra("Scarf", "This classy piece of fabric makes you stronger purely by proxy of feeling cooler with it.", 2, 2, 0, null, null, Rarity.Common),
            new Extra("Water Flask", "Refreshes the soul just enough to let you fight a little longer.", 0, 6, 0, null, null, Rarity.Common),
            // uncommons
            new Extra("Blade Sharpener", "Gives you that little edge.", 5, 0, 0, null, null, Rarity.Uncommon),
            new Extra("Turbo Shroom", "Tastes horrible, but it's healthy!", 3, 3, 0, null, null, Rarity.Uncommon),
            new Extra("Honeybee", "Brings you some refreshing honey every so often.", 2, 5, 0, null, null, Rarity.Uncommon),
            new Extra("Buckler", "A small shield, only meant for quick parries.", 4, 1, 0, null, null, Rarity.Uncommon),
            new Extra("Mirror", "Blind your enemies, giving you more time to plan your next move.", 1, 7, 0, null, null, Rarity.Uncommon),
            // rares
            new Extra("Wand of Healing", "Allows you to access your intrinsic healing abilities.", 0, 14, 0, null, null, Rarity.Rare),
            new Extra("Wand of Strength", "Magically boosts your fighting prowess.", 8, 0, 0, null, null, Rarity.Rare),
            new Extra("Wand of Balance", "Every part of you feels stronger... but only slightly.", 5, 6, 0, null, null, Rarity.Rare),
            new Extra("Black Cat", "A small familiar, helping you in every fight.", 7, 4, 0, null, null, Rarity.Rare),

            // epics

            // legendarys

            // unstables

            // corrupteds

            // uniques

        };
    }
}
