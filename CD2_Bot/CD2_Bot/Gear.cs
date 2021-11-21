using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace CD2_Bot
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Unstable,
        Corrupted,
        Unique
    }

    public static class Prices
    {
        public static Dictionary<Rarity, int> sell = new Dictionary<Rarity, int> {
            { Rarity.Common, 50 },
            { Rarity.Uncommon, 100 },
            { Rarity.Rare, 1000 },
            { Rarity.Epic, 2500 },
            { Rarity.Legendary, 10000 },
            { Rarity.Unstable, 25000 },
            { Rarity.Corrupted, 500000 },
            { Rarity.Unique, 100000 }
        };
       

        public static Dictionary<Rarity, int> buy = new Dictionary<Rarity, int> {
            { Rarity.Common, 100 },
            { Rarity.Uncommon, 500 },
            { Rarity.Rare, 5000 },
            { Rarity.Epic, 10000 },
            { Rarity.Legendary, 30000 },
            { Rarity.Unstable, 50000 },
            { Rarity.Corrupted, 100000 },
        };

        public static Dictionary<Rarity, int> infuse = new Dictionary<Rarity, int> {
            { Rarity.Common, 15 },
            { Rarity.Uncommon, 25 },
            { Rarity.Rare, 250 },
            { Rarity.Epic, 650 },
            { Rarity.Legendary, 2500 },
            { Rarity.Unstable, 6500 },
            { Rarity.Corrupted, 15000 },
            { Rarity.Unique, 25000 }
        };

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
        public decimal Damage { get { return (decimal)(BaseDamage * (Level / 50 + 1.1)); } }
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
        public decimal Damage { get { return (decimal)(BaseDamage * (Level / 50 + 1.1)); } }
        public int BaseHeal { get; set; }
        public int Heal { get { return (int)(BaseHeal * (Level / 500 + 1)); } }
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

        public static Embed RandomDrop()
        {
            return null;
        }

        public static List<Weapon> Weapons = new List<Weapon>
        {
            // common
            new Weapon("Stick", "A simple wooden stick.", 5, 0, null, null, Rarity.Common),
            new Weapon("Shortsword", "A basic blade crafted with simple iron.", 8, 0, null, null, Rarity.Common),
            // uncommon

            // rare

            // epic

            // legendary

            // unstable

            // corrupted

            // unique
        };

        public static List<Armor> Armors = new List<Armor>
        {
            // common
            new Armor("Rags", "Really dirty and torn apart.", 100, 0, null, null, Rarity.Common),
            new Armor("Socks", "Knee-high socks to protect the wearer from dirt and nothing else.", 99, 0, null, null, Rarity.Common),
            // uncommon

            // rare

            // epic

            // legendary

            // unstable

            // corrupted

            // unique
        };

        public static List<Extra> Extras = new List<Extra>
        {
            // common
            new Extra("Pendant", "A small, ochre amulet.", 1, 2, 0, null, null, Rarity.Common),
            new Extra("Firefly", "Little insect buddy that seems to follow you wherever you go.", 1, 4, 0, null, null, Rarity.Common),
            new Extra("Scarf", "This classy piece of fabric makes you stronger purely by proxy of feeling cooler with it.", 2, 2, 0, null, null, Rarity.Common),
            new Extra("Water Flask", "Refreshes the soul just enough to let you fight a little longer.", 0, 6, 0, null, null, Rarity.Common),
            // uncommon
            new Extra("Blade Sharpener", "Gives you that little edge.", 5, 0, 0, null, null, Rarity.Uncommon),
            new Extra("Turbo Shroom", "Tastes horrible, but it's healthy!", 3, 3, 0, null, null, Rarity.Uncommon),
            new Extra("Honeybee", "Brings you some refreshing honey every so often.", 2, 5, 0, null, null, Rarity.Uncommon),
            new Extra("Buckler", "A small shield, only meant for quick parries.", 4, 1, 0, null, null, Rarity.Uncommon),
            new Extra("Mirror", "Blind your enemies, giving you more time to plan your next move.", 1, 7, 0, null, null, Rarity.Uncommon),
            // rare
            new Extra("Wand of Healing", "Allows you to access your intrinsic healing abilities.", 0, 15, 0, null, null, Rarity.Rare),
            new Extra("Wand of Strength", "Magically boosts your fighting prowess.", 8, 0, 0, null, null, Rarity.Rare),
            new Extra("Wand of Balance", "Every part of you feels stronger... but only slightly.", 5, 8, 0, null, null, Rarity.Rare),
            new Extra("Black Cat", "A small familiar that helps you in every fight.", 7, 4, 0, null, null, Rarity.Rare),
            new Extra("Caster Amulet", "A pendant imbued with powerful sorcery.", 6, 5, 0, null, null, Rarity.Rare),
            // epic
            new Extra("Royal Emblem", "A certain royal family's crest - wearing it makes people think you're important.", 10, 10, 0, null, null, Rarity.Epic), // Custom Effect: Decreases Shop Prices by 5%
            new Extra("Tortuga Talisman", "Made of a hard, nearly indestructible shell, this charm saves you from harm.", 5, 25, 0, null, null, Rarity.Epic), // Custom Effect: Increases Resistance
            new Extra("Ambrosia", "A meal of gods, giving you incredible health.", 0, 30, 0, null, null, Rarity.Epic ),
            new Extra("Siren's Song", "Heed the call of the sirens.", 8, 15, 0, null, null, Rarity.Epic),
            new Extra("Dreamcatcher", "Casts the evil thoughts gathered in your sleep unto your enemies.", 16, 0, 0, null, null, Rarity.Epic),
            // legendary
            new Extra("Phoenix Feather", "The plume of a mystical bird, said to be able to bring back those fallen in battle.", 0, 50, 0, null, null, Rarity.Legendary), // Custom Effect: Can rescue you from death once a fight (maybe)
            new Extra("Sliver of Darkness", "A shard of unknown origin, said to be made of pure, deep darkness.", 25, 0, 0, null, null, Rarity.Legendary), // Custom Effect: Decreases the enemies HP by a certain amount
            new Extra("Magnificent Wish", "A mystical wish for salvation embedded deeply into you.", 5, 40, 0, null, null, Rarity.Legendary), // Custom Effect: Has a chance to fully heal you after a fight
            new Extra("Fractured Gem", "A dark gem with but a single crack exposing it's beautiful, gleaming core.", 28, 0, 0, null, null, Rarity.Legendary), // Custom Effect: Increases the coins you gain by winning a fight by 50%
            new Extra("Nebula Key", "Shrouded in mystery, the owner of this key is said to be able to understand the mysteries of space.", 19, 15, 0, null, null, Rarity.Legendary), // Custom Effect: Lets you interact with a specific NPC to start a questline
            new Extra("Overgrown Horn", "The horn of a long-extinct beast, taken over by the floral wildlife.", 20, 10, 0, null, null, Rarity.Legendary), // Custom Effect: Decreases your enemies resistance by 15%
            // unstable
            new Extra("Moon Refractor", "Absorb the moon's light and utilize its might.", 50, 15, 0, null, null, Rarity.Unstable), // Custom Effect: Increases your healing by 20%
            new Extra("Spirit Replica", "A ghostly clone of your weapon, attacking in tandem with you.", 60, 0, 0, null, null, Rarity.Unstable),
            new Extra("Beckoning Cat", "A cat statue said to bring good luck to the one who holds it.", 30, 50, 0, null, null, Rarity.Unstable), // Custom Effect: Increases the money you get by 50%
            new Extra("Crescent Mask", "A Crescent-shaped Mask, said to come from worshippers of an ancient moon god.", 20, 80, 0, null, null, Rarity.Unstable),
            new Extra("Fragmented Effigy", "A once beautiful depiction of the moon god, now blemished and broken.", 25, 65, 0, null, null, Rarity.Unstable),
            // corrupted
            new Extra("Black Magic Wand", "Achieve true strength by succumbing to darkness.", 90, 0, 0, null, null, Rarity.Corrupted), // Custom Effect: Halves your HP
            new Extra("Fire Orb", "Light up the fire in your soul to burn away your enemies.", 70, 10, 0, null, null, Rarity.Corrupted),
            new Extra("Crystal Primrose", "A flower said to stand for eternal love, crystallized by an unknown force.", 10, 90, 0, null, null, Rarity.Corrupted),
            new Extra("Greater Will", "The ethos that keeps together the world given form.", 60, 40, 0, null, null, Rarity.Corrupted), // Custom Effect: Increases your HP by 10% 
            new Extra("The Fool's Errand", "Only a madman would accept the challenge.", 20, 20, 0, null, null, Rarity.Corrupted) //  Custom Effect: Doubles EXP gained by winning fights
            // unique
        };
    }
}
