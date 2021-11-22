using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

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
        Unique,
        Random
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

        public static Dictionary<Rarity, int> dropchance = new Dictionary<Rarity, int> { //out of 10000
            { Rarity.Common, 6850 },
            { Rarity.Uncommon, 7850 },
            { Rarity.Rare, 8850 },
            { Rarity.Epic, 9600 },
            { Rarity.Legendary, 9850 },
            { Rarity.Unstable, 9950 },
            { Rarity.Corrupted, 10000 },
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

        public static async void RandomDrop(ulong uid, ISocketMessageChannel channel, Rarity droprarity = Rarity.Random, string type = "random")
        {
            //set rarity to random if not specified
            if (droprarity == Rarity.Random)
            {
                int roll = Defaults.GRandom.Next(0, 10000);
                if (roll < Prices.dropchance[Rarity.Common]) { droprarity = Rarity.Common; }
                else if (roll < Prices.dropchance[Rarity.Uncommon]) { droprarity = Rarity.Uncommon; }
                else if (roll < Prices.dropchance[Rarity.Rare]) { droprarity = Rarity.Rare; }
                else if (roll < Prices.dropchance[Rarity.Epic]) { droprarity = Rarity.Epic; }
                else if (roll < Prices.dropchance[Rarity.Legendary]) { droprarity = Rarity.Legendary; }
                else if (roll < Prices.dropchance[Rarity.Unstable]) { droprarity = Rarity.Unstable; }
                else { droprarity = Rarity.Corrupted; }
            }

            //determien type if not selected
            if (type == "random")
            {
                switch (Defaults.GRandom.Next(0, 2))
                {
                    case 0:
                        type = "weapon";
                        break;
                    case 1:
                        type = "armor";
                        break;
                    case 2:
                        type = "extra";
                        break;
                }
            }

            string recievedname = "";
            switch(type)
            {
                case "weapon":
                    recievedname = Gear.Weapons.FindAll(g => g.Rarity == droprarity)[Defaults.GRandom.Next(Gear.Weapons.FindAll(g => g.Rarity == droprarity).Count)].Name;
                    break;
                case "armor":
                    recievedname = Gear.Armors.FindAll(g => g.Rarity == droprarity)[Defaults.GRandom.Next(Gear.Armors.FindAll(g => g.Rarity == droprarity).Count)].Name;
                    break;
                case "extra":
                    recievedname = Gear.Extras.FindAll(g => g.Rarity == droprarity)[Defaults.GRandom.Next(Gear.Extras.FindAll(g => g.Rarity == droprarity).Count)].Name;
                    break;
            }

            var embed = new EmbedBuilder
            {
                Title = $"You found: **{droprarity.ToString()} {recievedname}**",
            };
            embed.WithColor(Color.Green);
            embed.WithFooter(Defaults.FOOTER);

            embed.Description = $"**Warning:** Claiming will replace your {type}.\n\nSell: **{Prices.sell[droprarity]}** coins.\nInfuse: **{Prices.infuse[droprarity]}** {type} exp.\nThis drop will expire after 15 minutes.";

            Embed e = embed.Build();

            ComponentBuilder btna = new ComponentBuilder()
                .WithButton("Claim", "dropaction;claim;" + uid + ";" + type + ";" + recievedname, ButtonStyle.Success)
                .WithButton("Sell", "dropaction;sell;" + uid + ";" + type + ";" + recievedname, ButtonStyle.Danger)
                .WithButton("Infuse", "dropaction;infuse;" + uid + ";" + type + ";" + recievedname, ButtonStyle.Secondary);

            await channel.SendMessageAsync(embed: e, component: btna.Build());
        }

        public static List<Weapon> Weapons = new List<Weapon>
        {
            // common
            new Weapon("Stick", "A simple wooden stick.", 5, 0, null, null, Rarity.Common),
            new Weapon("Stone Hatchet", "A tool generally used for cutting wood.", 7, 0, null, null, Rarity.Common),
            new Weapon("Shortsword", "A basic blade crafted with simple iron.", 8, 0, null, null, Rarity.Common),
            new Weapon("Wooden Wand", "Equipped to cast only the most basic of spells.", 7, 0, null, null, Rarity.Common),
            new Weapon("Dagger", "A small dagger that can be hidden away in your sleeve.", 6, 0, null, null, Rarity.Common),
            // uncommon
            new Weapon("Bladed Boomerang", "Sharp and throwable.", 19, 0, null, null, Rarity.Uncommon),
            new Weapon("Hunting Bow", "Shoots powerful steel arrows.", 17, 0, null, null, Rarity.Uncommon),
            new Weapon("Mana Catalyst", "Absorbs the mana around you and channels it into magic.", 16, 0, null, null, Rarity.Uncommon),
            new Weapon("Dreihander", "A sword so heavy that two hands aren't enough to carry it.", 18, 0, null, null, Rarity.Uncommon),
            new Weapon("Hunting Bow", "Shoots powerful steel arrows.", 17, 0, null, null, Rarity.Uncommon),
            // rare
            new Weapon("Vampire Tooth", "Steal your opponents' life by slashing them apart.", 25, 0, null, null, Rarity.Rare), // Custom Effect: Replenishes some health after a fight.
            new Weapon("Darkened Lucerne", "The blood on this halberd has turned its color to a dark red.", 31, 0, null, null, Rarity.Rare),
            new Weapon("Spiked Brass Knuckles", "Punch your enemy into the ground, leaving no room for parries.", 29, 0, null, null, Rarity.Rare),
            new Weapon("Kusarigama", "A chain sickle to wrap and slice your enemies up with.", 29, 0, null, null, Rarity.Rare),
            new Weapon("Flaming Scimitar", "This lightweight, curved blade burns your enemies with a slash.", 30, 0, null, null, Rarity.Rare),
            new Weapon("Big Cannon", "Watch out for the recoil.", 27, 0, null, null, Rarity.Rare),
            // epic
            new Weapon("Crystalrend", "The reflection of their faces on this gleaming blade is the last thing the enemy'll see.", 48, 0, null, null, Rarity.Epic),
            new Weapon("Staff of the Elements", "Channel the eternal strength of nature and force it upon your foes.", 50, 0, null, null, Rarity.Epic),
            new Weapon("Futurefinder", "A blade that is said to come from a time succeeding ours.", 44, 0, null, null, Rarity.Epic), // Custom Effect: idk lol irgndswas mit zukunft hihi
            new Weapon("Lance of the Stars", "All the stars' fury gets cast upon an opponent pierced by this weapon.", 49, 0, null, null, Rarity.Epic),
            new Weapon("Frostbite Tome", "Etches the arts of ice magic into your soul.", 42, 0, null, null, Rarity.Epic), // Custom Effect: irgendwas mit freezen von gegnern
            // legendary
            new Weapon("Link Breaker", "No trace of a connection remains once something is cut by this blade.", 75, 0, null, null, Rarity.Legendary),
            new Weapon("Ancient Root Hammer", "Birthed from an ancient, evergrowing tree, this hammer is nearly indestructible.", 71, 0, null, null, Rarity.Legendary), // Custom Effect: Chance to stun opponents. (Turn damage to zero for one turn)
            new Weapon("Runed Greatshield", "A gigantic shield to protect you from attacks - or bash in some heads.", 69, 0, null, null, Rarity.Legendary), // Custom Effect: Increases Resistance
            new Weapon("Erebus Gauntlets", "Gauntlets imbued with the power of the underworld.", 73, 0, null, null, Rarity.Legendary), // Custom Effect: Crit Chance
            new Weapon("Lightrays", "Beam your enemies away with the sun's shining light.", 74, 0, null, null, Rarity.Legendary), // Custom Effect: Chance to heal on attack
            // unstable
            new Weapon("Pulsar Breaker", "Can break a star in half with magnetic powers.", 94, 0, null, null, Rarity.Unstable),
            new Weapon("Soultracer", "Slice the edges of your opponent's soul.", 90, 0, null, null, Rarity.Unstable), // Custom Effect: Crit Chance
            new Weapon("Deep Ocean Geyser", "Command the waters and blast them at your foes.", 97, 0, null, null, Rarity.Unstable),
            new Weapon("Ebony Cloud", "Thunder and lightning are yours to control.", 95, 0, null, null, Rarity.Unstable),
            new Weapon("Sphere of the Abyss", "Absorbs the light to bring about eternal darkness.", 98, 0, null, null, Rarity.Unstable), 
            // corrupted
            new Weapon("Nocturnal Scythe", "Only those who were shortly ended by it have ever taken a glance at it.", 130, 0, null, null, Rarity.Corrupted),
            new Weapon("Syzygy", "A pair of celestial swords, the sun and the moon, aligning with your very being.", 126, 0, null, null, Rarity.Corrupted),
            new Weapon("Scepter of Cores", "The inferno of the planet's core is at your disposal.", 120, 0, null, null, Rarity.Corrupted),
            new Weapon("Dogma", "Unending, unfinite, this gauntlet of pure plasma deletes anything it grazes from existence.", 125, 0, null, null, Rarity.Corrupted),
            new Weapon("Hammer of Tabula Rasa", "Can flatten entire cities with one attack.", 122, 0, null, null, Rarity.Corrupted),
            new Weapon("Armed Railgun", "Destroy everything with gargantuan lasers.", 124, 0, null, null, Rarity.Corrupted),
            // unique
        };

        public static List<Armor> Armors = new List<Armor>
        {
            // common
            new Armor("Rags", "Really dirty and torn apart.", 100, 0, null, null, Rarity.Common),
            new Armor("Socks", "Knee-high socks to protect the wearer from dirt and nothing else.", 99, 0, null, null, Rarity.Common),
            // uncommon
            new Armor("Placeholder", "placeholder.", 100, 0, null, null, Rarity.Uncommon),
            // rare
            new Armor("Placeholder2", "placeholder.", 100, 0, null, null, Rarity.Rare),
            // epic
            new Armor("Placeholder3", "placeholder.", 100, 0, null, null, Rarity.Epic),
            // legendary
            new Armor("Placeholder4", "placeholder.", 100, 0, null, null, Rarity.Legendary),
            // unstable
            new Armor("Placeholder5", "placeholder.", 100, 0, null, null, Rarity.Unstable),
            // corrupted
            new Armor("Placeholder6", "placeholder.", 100, 0, null, null, Rarity.Corrupted),
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
