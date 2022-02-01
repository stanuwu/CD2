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
        Random,
        Handmade,
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
            { Rarity.Unique, 100000 },
            { Rarity.Handmade, 50 },
        };
       

        public static Dictionary<Rarity, int> buy = new Dictionary<Rarity, int> {
            { Rarity.Common, 100 },
            { Rarity.Uncommon, 500 },
            { Rarity.Rare, 5000 },
            { Rarity.Epic, 10000 },
            { Rarity.Legendary, 30000 },
            { Rarity.Unstable, 50000 },
            { Rarity.Corrupted, 100000 },
            { Rarity.Handmade, int.MaxValue },
        };

        public static Dictionary<Rarity, int> infuse = new Dictionary<Rarity, int> {
            { Rarity.Common, 15 },
            { Rarity.Uncommon, 25 },
            { Rarity.Rare, 250 },
            { Rarity.Epic, 650 },
            { Rarity.Legendary, 2500 },
            { Rarity.Unstable, 6500 },
            { Rarity.Corrupted, 15000 },
            { Rarity.Unique, 25000 },
            { Rarity.Handmade, 15 },
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
        public Weapon(string name, string description, int damage, int exp, string customeffectname, string customeffectdesc, Rarity rarity, bool canDrop = true)
        {
            Name = name;
            Description = description;
            BaseDamage = damage;
            EXP = exp;
            CustomEffectName = customeffectname;
            CustomEffectDescription = customeffectdesc;
            Rarity = rarity;
            CanDrop = canDrop;
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
        public bool CanDrop { get; set; }
        public double CustomEffect(CharacterStructure character, Enemy enemy, Armor armor, Extra extra)
        {
            switch (this.CustomEffectName)
            {
                case "Lifesteal":
                    character.HP += (int)this.Damage;
                    break;

                case "Future Sight":
                    enemy.Damage -= enemy.Damage/15;
                    break;

                case "Freeze":
                    enemy.Damage -= enemy.Damage/25;
                    enemy.Resistance += 3;
                    break;

                case "Stun":
                    enemy.Damage -= enemy.Damage/10;
                    break;

                case "Heavy Stun":
                    enemy.Damage -= enemy.Damage/20;
                    break;

                case "Fortified":
                    armor.Resistance -= 5;
                    break;

                case "Critical Eye":
                    if (Defaults.GRandom.Next(20) == 1)
                    {
                        this.BaseDamage = this.BaseDamage * 2;
                    }
                    break;

                case "Weakness Exploit":
                    enemy.Resistance += 5;
                    break;
                
                case "Guide to the Afterlife":
                    if (Defaults.GRandom.Next(100) == 1)
                    {
                        return int.MaxValue;
                    }
                    break;

                case "Motor Function":
                    if (Defaults.GRandom.Next(0, 3) == 0)
                    {
                        this.BaseDamage = 55;
                    }
                    else if (Defaults.GRandom.Next(0, 3) == 1)
                    {
                        this.BaseDamage = 75;
                    }
                    else if (Defaults.GRandom.Next(0, 3) == 2)
                    {
                        this.BaseDamage = 90;
                    }
                    break;

                case "Blessing Light":
                    character.HP += (int)(((double)this.Damage)*1.5);
                    break;

                case "Worldpoison":
                    return enemy.HP / 20;
            }

            return 0;
        }
        public Weapon Clone()
        {
            return (Weapon)this.MemberwiseClone();
        }
    }

    public class Armor : IGearStats
    {
        public Armor(string name, string description, int resistance, int exp, string customeffectname, string customeffectdesc, Rarity rarity, bool canDrop = true)
        {
            Name = name;
            Description = description;
            Resistance = resistance;
            EXP = exp;
            CustomEffectName = customeffectname;
            CustomEffectDescription = customeffectdesc;
            Rarity = rarity;
            CanDrop = canDrop;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Resistance { get; set; }
        public int Level { get { return (int)Math.Floor((double)this.EXP / 250); } }
        public int EXP { get; set; }
        public string CustomEffectName { get; set; }
        public string CustomEffectDescription { get; set; }
        public Rarity Rarity { get; set; }
        public bool CanDrop { get; set; }
        public double CustomEffect(CharacterStructure character, Enemy enemy, Weapon weapon, Extra extra)
        {
            switch (this.CustomEffectName)
            {
                case "Light Burn":
                    enemy.Damage -= enemy.Damage/20;
                    return enemy.HP/50;

                case "Strength":
                    weapon.BaseDamage += 10;
                    break;

                case "Archer's Blessing":
                    if (weapon.Name.Contains("Bow") || weapon.Name.Contains("Crossbow") || weapon.Name.Contains("Bowgun"))
                    {
                        weapon.BaseDamage += weapon.BaseDamage/5;
                    }
                    break;

                case "Healthy Herbs":
                    extra.BaseHeal += extra.BaseHeal/10;
                    break;

                case "Calming Waters":
                    character.HP += character.MaxHP/20;
                    break;

                case "Berserker's Fury":
                    character.HP = character.MaxHP/2;
                    weapon.BaseDamage += character.HP/20;
                    break;

                case "Chilly":
                    character.HP -= 10;
                    break;

                case "Sage Magic":
                    extra.BaseHeal += extra.BaseHeal/5;
                    break;

                case "Moneymaker":
                    character.Money += enemy.Level*10;
                    break;

                case "Fault in the System":
                    this.Resistance = Defaults.GRandom.Next(70, 90);
                    weapon.BaseDamage += Defaults.GRandom.Next(-5, 10);
                    break;

                case "Comfy Cotton":
                    extra.BaseHeal += extra.BaseHeal/4;
                    break;

                case "Safety Hazard":
                    if (Defaults.GRandom.Next(10) == 1)
                    {
                        character.HP -= (character.HP/100)*Defaults.GRandom.Next(10);
                    }
                    break;

                case "Power of Hatred":
                    weapon.BaseDamage += weapon.BaseDamage/4;
                    break;

                case "Core's Heat":
                    weapon.BaseDamage += weapon.BaseDamage/4;
                    break;

                case "Modifier":
                    switch (weapon.Rarity)
                    {
                        case Rarity.Common:
                            weapon.BaseDamage += 4;
                            break;

                        case Rarity.Uncommon:
                            weapon.BaseDamage += 8;
                            break;

                        case Rarity.Rare:
                            weapon.BaseDamage += 12;
                            break;

                        case Rarity.Epic:
                            weapon.BaseDamage += 16;
                            break;

                        case Rarity.Legendary:
                            weapon.BaseDamage += 20;
                            break;

                        case Rarity.Unstable:
                            weapon.BaseDamage += 24;
                            break;

                        case Rarity.Corrupted:
                            weapon.BaseDamage += 28;
                            break;

                        case Rarity.Unique:
                            weapon.BaseDamage += 24;
                            break;
                    }

                    switch (extra.Rarity)
                    {
                        case Rarity.Common:
                            character.HP += 6;
                            break;

                        case Rarity.Uncommon:
                            character.HP += 12;
                            break;

                        case Rarity.Rare:
                            character.HP += 18;
                            break;

                        case Rarity.Epic:
                            character.HP += 24;
                            break;

                        case Rarity.Legendary:
                            character.HP += 30;
                            break;

                        case Rarity.Unstable:
                            character.HP += 36;
                            break;

                        case Rarity.Corrupted:
                            character.HP += 42;
                            break;

                        case Rarity.Unique:
                            character.HP += 36;
                            break;
                    }
                    break;

                case "Sylph's Flight":
                    enemy.Damage = enemy.Damage / 2;
                    break;

                case "Lethal Gamble":
                    this.Resistance = Defaults.GRandom.Next(25,125);
                    break;
            }

            return 0;
        }
        public Armor Clone()
        {
            return (Armor)this.MemberwiseClone();
        }
    }
    
    public class Extra : IGearStats
    {
        public Extra(string name, string description, int damage, int heal, int exp, string customeffectname, string customeffectdesc, Rarity rarity, bool canDrop = true)
        {
            Name = name;
            Description = description;
            BaseDamage = damage;
            BaseHeal = heal;
            EXP = exp;
            CustomEffectName = customeffectname;
            CustomEffectDescription = customeffectdesc;
            Rarity = rarity;
            CanDrop = canDrop;
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
        public bool CanDrop { get; set; }
        public double CustomEffect(CharacterStructure character, Enemy enemy, Weapon weapon, Armor armor)
        {
            switch (this.CustomEffectName)
            {
                case "TestExtra":
                    break;

                case "Turtle Spirit":
                    armor.Resistance -= 5;
                    break;

                case "Phoenix' Embrace":
                    if (Defaults.GRandom.Next(200) == 1)
                    {
                        character.HP += 500;
                    }
                    break;

                case "Life Decrease":
                    enemy.HP -= enemy.HP/10;
                    break;

                case "Lifelight":
                    character.HP = character.MaxHP;
                    break;

                case "Gemstone Delight":
                    character.Money += 1000;
                    break;

                case "Piercing Defenses":
                    enemy.Resistance += 5;
                    break;

                case "Moonshine":
                    this.BaseHeal += this.BaseHeal / 5;
                    break;

                case "Money Machine":
                    if (Defaults.GRandom.Next(50) == 1)
                    {
                        character.Money += 3000;
                    }
                    break;

                case "Valorous Offensive":
                    if (character.HP / character.MaxHP > 80)
                    {
                        weapon.BaseDamage += weapon.BaseDamage / 20;
                    }
                    else if (character.HP / character.MaxHP > 60 && character.HP / character.MaxHP <= 80)
                    {
                        weapon.BaseDamage += weapon.BaseDamage / 10;
                    }
                    else if (character.HP / character.MaxHP > 40 && character.HP / character.MaxHP <= 60)
                    {
                        weapon.BaseDamage += (int)(weapon.BaseDamage / 7);
                    }
                    else if (character.HP / character.MaxHP > 20 && character.HP / character.MaxHP <= 40)
                    {
                        weapon.BaseDamage += weapon.BaseDamage / 5;
                    }
                    break;

                case "Black Magic":
                    weapon.BaseDamage = weapon.BaseDamage * 2;
                    character.HP = character.HP / 2;
                    break;

                case "Guts":
                    character.EXP += Prices.infuse[Rarity.Epic];
                    break;

                case "Radiation":
                    character.HP -= character.HP/10;
                    enemy.HP -= enemy.HP/10;
                    break;
            }

            return 0;
        }
        public Extra Clone()
        {
            return (Extra)this.MemberwiseClone();
        }
    }

    public static class Gear
    {

        public static async void RandomDrop(ulong uid, ISocketMessageChannel channel, Rarity droprarity = Rarity.Random, string type = "random", string ovr = null, string ovrtype = null)
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

            //determine type if not selected
            if (type == "random")
            {
                switch (Defaults.GRandom.Next(0, 3))
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
            if (ovr == null)
            {
                switch (type)
                {
                    case "weapon":
                        recievedname = Gear.Weapons.FindAll(g => g.Rarity == droprarity && g.CanDrop == true)[Defaults.GRandom.Next(Gear.Weapons.FindAll(g => g.Rarity == droprarity && g.CanDrop == true).Count)].Name;
                        break;
                    case "armor":
                        recievedname = Gear.Armors.FindAll(g => g.Rarity == droprarity && g.CanDrop == true)[Defaults.GRandom.Next(Gear.Armors.FindAll(g => g.Rarity == droprarity && g.CanDrop == true).Count)].Name;
                        break;
                    case "extra":
                        recievedname = Gear.Extras.FindAll(g => g.Rarity == droprarity && g.CanDrop == true)[Defaults.GRandom.Next(Gear.Extras.FindAll(g => g.Rarity == droprarity && g.CanDrop == true).Count)].Name;
                        break;
                }
            }
            else
            {
                recievedname = ovr;
                switch (ovrtype)
                {
                    case "weapon":
                        droprarity = Gear.Weapons.Find(g => g.Name == ovr).Rarity;
                        break;
                    case "armor":
                        droprarity = Gear.Armors.Find(g => g.Name == ovr).Rarity;
                        break;
                    case "extra":
                        droprarity = Gear.Extras.Find(g => g.Name == ovr).Rarity;
                        break;
                }
                type = ovrtype;
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

            await channel.SendMessageAsync(embed: e, components: btna.Build());
        }

        public static List<Weapon> Weapons = new List<Weapon>
        {
            // common
            new Weapon("Stick", "A simple wooden stick.", 3, 0, null, null, Rarity.Common),
            new Weapon("Stone Hatchet", "A tool generally used for cutting wood.", 7, 0, null, null, Rarity.Common),
            new Weapon("Shortsword", "A basic blade crafted with simple iron.", 8, 0, null, null, Rarity.Common),
            new Weapon("Wooden Wand", "Equipped to cast only the most basic of spells.", 7, 0, null, null, Rarity.Common),
            new Weapon("Dagger", "A small dagger that can be hidden away in your sleeve.", 6, 0, null, null, Rarity.Common),
            new Weapon("Claymore", "A large but heavy blade.", 11, 0, null, null, Rarity.Common),
            new Weapon("Miniature Lance", "A shortened lance for short range stabbing.", 8, 0, null, null, Rarity.Common),
            // uncommon
            new Weapon("Bladed Boomerang", "Sharp and throwable.", 19, 0, null, null, Rarity.Uncommon),
            new Weapon("Hunting Bow", "Shoots powerful steel arrows.", 17, 0, null, null, Rarity.Uncommon),
            new Weapon("Mana Catalyst", "Absorbs the mana around you and channels it into magic.", 16, 0, null, null, Rarity.Uncommon),
            new Weapon("Dreihander", "A sword so heavy that two hands aren't enough to carry it.", 18, 0, null, null, Rarity.Uncommon),
            new Weapon("Big Cannon", "Watch out for the recoil.", 17, 0, null, null, Rarity.Uncommon),
            new Weapon("Morning Star", "A big spikey ball on a stick.", 21, 0, null, null, Rarity.Uncommon),
            // rare
            new Weapon("Vampire Tooth", "Steal your opponents' life by slashing them apart.", 25, 0, "Lifesteal", "Heals for twice your damage before the start of the battle.", Rarity.Rare),
            new Weapon("Darkened Lucerne", "The blood on this halberd has turned its color to a dark red.", 31, 0, null, null, Rarity.Rare),
            new Weapon("Spiked Brass Knuckles", "Punch your enemy into the ground, leaving no room for parries.", 29, 0, null, null, Rarity.Rare),
            new Weapon("Kusarigama", "A chain sickle to wrap and slice your enemies up with.", 29, 0, null, null, Rarity.Rare),
            new Weapon("Flaming Scimitar", "This lightweight, curved blade burns your enemies with a slash.", 30, 0, null, null, Rarity.Rare),
            new Weapon("Electric Crossbow", "Shoots electrically-charged, hefty bolts.", 27, 0, null, null, Rarity.Rare),
            // epic
            new Weapon("Crystalrend", "The reflection of their faces on this gleaming blade is the last thing the enemy'll see.", 48, 0, null, null, Rarity.Epic),
            new Weapon("Staff of the Elements", "Channel the eternal strength of nature and force it upon your foes.", 47, 0, null, null, Rarity.Epic),
            new Weapon("Futurefinder", "A blade that is said to come from a time succeeding ours.", 45, 0, "Future Sight", "Has a chance to predict an enemy's attack, completely avoiding it.", Rarity.Epic), 
            new Weapon("Lance of the Stars", "All the stars' fury gets cast upon an opponent pierced by this weapon.", 49, 0, null, null, Rarity.Epic),
            new Weapon("Frostbite Tome", "Etches the arts of ice magic into your soul.", 44, 0, "Freeze", "Decreases damage and resistance of the enemy.", Rarity.Epic), 
            new Weapon("Long-sunken Anchor", "Reclaimed by oceanic wildlife, but still hits like a truck.", 48, 0, null, null, Rarity.Epic),
            new Weapon("Darkwood Staff", "You can feel the forest's power flowing through it with every hit.", 47, 0, null, null, Rarity.Epic),
            // legendary
            new Weapon("Link Breaker", "No trace of a connection remains once something is cut by this blade.", 75, 0, null, null, Rarity.Legendary),
            new Weapon("Ancient Root Hammer", "Birthed from an ancient, evergrowing tree, this hammer is nearly indestructible.", 71, 0, "Stun", "Small chance for the enemy to not attack you.", Rarity.Legendary),
            new Weapon("Runed Greatshield", "A gigantic shield to protect you from attacks - or bash in some heads.", 69, 0, "Fortified", "Increases your Resistance.", Rarity.Legendary),
            new Weapon("Erebus Gauntlets", "Gauntlets imbued with the power of the underworld.", 73, 0, "Critical Eye", "Chance to critically hit an enemy.", Rarity.Legendary),
            new Weapon("Lightray Bowgun", "Beam your enemies away with the sun's shining light.", 74, 0, "Blessing Light", "Heals you on attack.", Rarity.Legendary),
            new Weapon("Propeller Blade", "Has three modes. Can be really strong ONLY if you're lucky.", 75, 0, "Motor Function", "Chance on hit to either do 55, 75 or 90 damage on hit (Each 33.3% chance).", Rarity.Legendary),
            new Weapon("Scythe of the Young Reaper", "Used to take away the souls of the deceased - but doesn't always work.", 20, 0, "Guide to the Afterlife", "Tiny chance to instantly kill an enemy.", Rarity.Legendary),
            // unstable
            new Weapon("Pulsar Breaker", "Can break a star in half with magnetic powers.", 94, 0, null, null, Rarity.Unstable),
            new Weapon("Soultracer", "Slice the edges of your opponent's soul.", 90, 0, "Critical Eye", "Chance to critically hit an enemy.", Rarity.Unstable), 
            new Weapon("Heartstopper", "These claws are infamous for making opponents freeze in fear merely at a glance.", 90, 0, "Weakness Exploit", "Weakens the enemies Resistance.", Rarity.Unstable),
            new Weapon("Bodybreaker", "Pulverize your opponent's bones through mere punches.", 90, 0, "Stun", "Small chance for the enemy to not attack you.", Rarity.Unstable),
            new Weapon("Deep Ocean Geyser", "Command the waters and blast them at your foes.", 97, 0, null, null, Rarity.Unstable),
            new Weapon("Ebony Cloud", "Thunder and lightning are yours to control.", 95, 0, null, null, Rarity.Unstable),
            new Weapon("Sphere of the Abyss", "Absorbs the light to bring about eternal darkness.", 98, 0, null, null, Rarity.Unstable), 
            // corrupted
            new Weapon("Nocturnal Scythe", "Can taint the entire sky a deep, abyssal dark.", 130, 0, null, null, Rarity.Corrupted),
            new Weapon("Syzygy", "A pair of celestial swords, the sun and the moon, aligning with your very being.", 126, 0, null, null, Rarity.Corrupted),
            new Weapon("Scepter of Cores", "The inferno of the planet's core is at your disposal.", 120, 0, null, null, Rarity.Corrupted),
            new Weapon("Dogma", "Gauntlets of gleaming plasma with destructive capabilities.", 125, 0, "Heavy Stun", "Medium chance for the enemy to not attack you.", Rarity.Corrupted),
            new Weapon("Hammer of Tabula Rasa", "Said to be able to flatten mountains.", 122, 0, null, null, Rarity.Corrupted),
            new Weapon("Astral Binding Bow", "Bound to the stars, this weapon can light up the night sky.", 124, 0, null, null, Rarity.Corrupted),
            new Weapon("Jormungandr", "A giant longsword, unmatched in length, and an undying snake wrapped around its blade.", 121, 0, "Worldpoison", "Heavily poisons your enemy, dealing damage over time.", Rarity.Corrupted),
            // unique

            //handmade
            new Weapon("Wooden Sword", "Only made for combat training.", 5, 0, null, null, Rarity.Common, false),
            new Weapon("Restored Sword", "An old sword, restored to its former glory.", 13, 0, null, null, Rarity.Common, false),
            new Weapon("Diamond Tip Sword", "Made out of an incredible amount of stone and one diamond.", 50, 0, null, null, Rarity.Epic, false),
        };

        public static List<Armor> Armors = new List<Armor>
        {
            // common
            new Armor("Rags", "Really dirty and torn apart.", 100, 0, null, null, Rarity.Common),
            new Armor("Socks", "Knee-high socks to protect the wearer from dirt and nothing else.", 99, 0, null, null, Rarity.Common),
            new Armor("Top Hat", "Stylish, but doesn't really shield all that much.", 98, 0, null, null, Rarity.Common),
            new Armor("Cape", "Makes you look like a superhero, a naked superhero...", 99, 0, null, null, Rarity.Common),
            new Armor("\"Heavy Plated Armor\"", "Armor made out of paper and painted - no wonder it was so cheap.", 97, 0, null, null, Rarity.Common),
            // uncommon
            new Armor("Chainmail", "A little rusty, but it does the job.", 95, 0, null, null, Rarity.Uncommon),
            new Armor("Rock Helmet", "Good against concussions.", 94, 0, null, null, Rarity.Uncommon),
            new Armor("Brass Armor", "This gilden Chestplate is prettier than it's actually useful.", 94, 0, null, null, Rarity.Uncommon),
            new Armor("Hardened Clay Armor", "A layer burnt onto your body in exactly the right shape.", 93, 0, null, null, Rarity.Uncommon), 
            // rare
            new Armor("Thin Crystal Armor", "Shiny AND quite sturdy!", 85, 0, null, null, Rarity.Rare),
            new Armor("Volcano Rock Cuirass", "Made of volcanic rocks, lava veins are still visible.", 87, 0, "Light Burn", "Causes slight extra damage depending on the strength of the enemy and decreases their accuracy.", Rarity.Rare),
            new Armor("Dragon Crown", "The horns of a young dragon.", 88, 0, "Strength", "Increases your damage by 10.", Rarity.Rare),
            new Armor("Archer's Vest", "Makes you swift, but lacking in defense.", 90, 0, "Archer's Blessing", "If your weapon is a bow, a crossbow or a bowgun, increase damage by 20%.", Rarity.Rare),
            new Armor("Fogglass Guard", "Vanish in the fog.", 86, 0, null, null, Rarity.Rare),
            // epic
            new Armor("Golem Skin", "Made from the body of a rock creature.", 82, 0, null, null, Rarity.Epic),
            new Armor("Bone Armor", "An armor-set made of skeleton bones.", 84, 0, null, null, Rarity.Epic),
            new Armor("Moth Pelt Vest", "Makes you feel attracted to light.", 83, 0, null, null, Rarity.Epic),
            new Armor("Deepforest Cloak", "Protective, but still meant for mobility. Comes with healing herbs.", 86, 0, "Healthy Herbs", "Increases your healing by 10%.", Rarity.Epic),
            new Armor("Aquatic Robe", "Cloak yourself in flowing waters.", 85, 0, "Calming Waters", "Increases HP by 5%.", Rarity.Epic),
            // legendary
            new Armor("Berserker's Guard", "Focus fully on fighting with little regard for your survival.", 75, 0, "Berserker's Fury", "Decreases your HP by 50%, but increases your damage by 5% of that.", Rarity.Legendary),
            new Armor("Mask of the Purveyor", "Spread the truth you believe in.", 74, 0, null, null, Rarity.Legendary),
            new Armor("Diamond Dust Cuirass", "You'll freeze a little bit, but it's worth the good defense.", 71, 0, "Chilly", "The cold hurts you a little bit every fight.", Rarity.Legendary),
            new Armor("Sage's Robe", "Really comfy. Strenghtens your natural healing powers.", 76, 0, "Sage Magic", "Increase your healing by 20%.", Rarity.Legendary),
            new Armor("Goldwitch Overcoat", "Enchanted by witches, this piece of armor is sure to bring you financial luck.", 78, 0, "Moneymaker", "Increases gained money per fight.", Rarity.Legendary),
            // unstable
            new Armor("Gambler's Ruin", "It's all or nothing.", 100, 0, "Lethal Gamble", "Sets the resistanceto a random value between 25 and 125.", Rarity.Unstable), // Custom Effect: 50% chance to fully negate damage, returns 200% if negated. // Bro Wie?
            new Armor("§!#+$=ERR", "This isn't supposed to happen...", 100, 0, "Fault in the System", "Gives you a random resistance and a random damage buff every fight", Rarity.Unstable),
            new Armor("Pima Longcoat", "The fluffiest, most comfortable piece of clothing around.", 68, 0, "Comfy Cotton", "Increases your healing by 25%.", Rarity.Unstable),
            new Armor("Blazing Fireguard", "The flames of this chestplate keep foes at bay.", 69, 0, null, null, Rarity.Unstable),
            new Armor("Mecha Suit", "Technological advancements allow you to enhance yourself with this robotic suit. Prone to malfunction.", 66, 0, "Safety Hazard", "Chance to hurt you every fight.", Rarity.Unstable),
            // corrupted
            new Armor("Fibre of Hatred", "Focus on hurting your foes rather than defending.", 75, 0, "Power of Hatred", "Increases your weapon's damage by a quarter.", Rarity.Corrupted),
            new Armor("Core Smelter Armor", "Made of the innermost layer of earth.", 65, 0, "Core's Heat", "Increases your weapon's damage by 25.", Rarity.Corrupted),
            new Armor("Wyvern's Aegis", "Able to withstand even meteorites.", 55, 0, null, null, Rarity.Corrupted),
            new Armor("Adaptive Bioarmor", "Hi-tech, adapts to every situation that comes your way.", 75, 0, "Modifier", "Increases your damage and your healing depending on the rarity of your weapon and your extra.", Rarity.Corrupted),
            new Armor("Sylphid Wings", "Once belonging to a wind elemental, you can now freely fly with them.", 75, 0, "Sylph's Flight", "Dodges every other attack.", Rarity.Corrupted),
            // unique

            //handmade
            new Armor("Fish Scale Mail", "Really shiny, not sturdy.", 99, 0, null, null, Rarity.Common, false),
            new Armor("Paper Armor", "A surprisingly sturdy armor made out of paper.", 94, 0, null, null, Rarity.Uncommon, false),
            new Armor("Steel Armor", "Quite hefty - good defense, but you look sort of stupid with it.", 89, 0, null, null, Rarity.Rare, false),

        };

        public static List<Extra> Extras = new List<Extra>
        {
            // common
            new Extra("Pendant", "A small, ochre amulet.", 1, 2, 0, null, null, Rarity.Common),
            new Extra("Firefly", "Little insect buddy that seems to follow you wherever you go.", 1, 4, 0, null, null, Rarity.Common),
            new Extra("Scarf", "This classy piece of fabric makes you stronger purely by proxy of feeling cooler with it.", 2, 2, 0, null, null, Rarity.Common),
            new Extra("Water Flask", "Refreshes the soul just enough to let you fight a little longer.", 0, 6, 0, null, null, Rarity.Common),
            new Extra("Magic Powder", "Just a few grains of this can enhance your physical ability beyond normal levels. About 1% beyond normal levels.", 2, 3, 0, null, null, Rarity.Common),
            new Extra("Tiny Horn", "Its sound prepares you for battle.", 3, 0, 0, null, null, Rarity.Common),
            new Extra("Memory Fragment", "A little bit of knowledge on how to fight, hidden in your own mind.", 2, 1, 0, null, null, Rarity.Common),
            // uncommon
            new Extra("Blade Sharpener", "Gives you that little edge.", 5, 0, 0, null, null, Rarity.Uncommon),
            new Extra("Turbo Shroom", "Tastes horrible, but it's healthy!", 3, 3, 0, null, null, Rarity.Uncommon),
            new Extra("Honeybee", "Brings you some refreshing honey every so often.", 2, 5, 0, null, null, Rarity.Uncommon),
            new Extra("Buckler", "A small shield, only meant for quick parries.", 4, 1, 0, null, null, Rarity.Uncommon),
            new Extra("Mirror", "Blind your enemies, giving you more time to plan your next move.", 1, 7, 0, null, null, Rarity.Uncommon),
            new Extra("Modular Holster", "A little contraption that allows you to pull your weapon faster than ever.", 4, 0, 0, null, null, Rarity.Uncommon),
            // rare
            new Extra("Wand of Healing", "Allows you to access your intrinsic healing abilities.", 0, 15, 0, null, null, Rarity.Rare),
            new Extra("Wand of Strength", "Magically boosts your fighting prowess.", 9, 0, 0, null, null, Rarity.Rare),
            new Extra("Wand of Balance", "Every part of you feels stronger... but only slightly.", 5, 8, 0, null, null, Rarity.Rare),
            new Extra("Black Cat", "A small familiar that helps you in every fight.", 7, 4, 0, null, null, Rarity.Rare),
            new Extra("Caster Amulet", "A pendant imbued with powerful sorcery.", 6, 5, 0, null, null, Rarity.Rare),
            new Extra("Friendly Wisp", "Keeps flying around you and making little noises. Kind of cute.", 4, 10, 0, null, null, Rarity.Rare),
            // epic
            new Extra("Royal Emblem", "A certain royal family's crest - wearing it makes people think you're important.", 10, 10, 0, null, null, Rarity.Epic),
            new Extra("Tortuga Talisman", "Made of a hard, nearly indestructible shell, this charm saves you from harm.", 5, 20, 0, "Turtle Spirit", "Increases your Resistance.", Rarity.Epic),
            new Extra("Ambrosia", "A meal of gods, giving you incredible health.", 0, 30, 0, null, null, Rarity.Epic ),
            new Extra("Siren's Song", "Heed the call of the sirens.", 8, 15, 0, null, null, Rarity.Epic),
            new Extra("Dreamcatcher", "Casts the evil thoughts gathered in your sleep unto your enemies.", 16, 0, 0, null, null, Rarity.Epic),
            new Extra("Petrified Leaf", "You can still see a little of its auburn color. ", 9, 12, 0, null, null, Rarity.Epic),
            // legendary
            new Extra("Phoenix Feather", "The plume of a mystical bird, said to be able to bring back those fallen in battle.", 0, 50, 0, "Phoenix' Embrace", "Miniscule Chance to give you enough HP to completely survive a fight.", Rarity.Legendary),
            new Extra("Sliver of Darkness", "A shard of unknown origin, said to be made of pure, deep darkness.", 25, 0, 0, "Life Decrease", "Decreases the enemies' HP.", Rarity.Legendary),
            new Extra("Magnificent Wish", "A mystical wish for salvation embedded deeply into you.", 5, 40, 0, "Lifelight", "Tiny chance to completely heal you before a fight starts.", Rarity.Legendary),
            new Extra("Fractured Gem", "A dark gem with but a single crack exposing it's beautiful, gleaming core.", 28, 0, 0, "Gemstone Delight", "Gives you some extra money after a fight.", Rarity.Legendary),
            new Extra("Nebula Key", "Shrouded in mystery, the owner of this key is said to be able to understand the mysteries of space.", 19, 15, 0, null, null, Rarity.Legendary), // Custom Effect: Lets you interact with a specific NPC to start a questline
            new Extra("Overgrown Horn", "The horn of a long-extinct beast, taken over by the floral wildlife.", 20, 10, 0, "Piercing Defenses", "Decreases your enemies' Resistance.", Rarity.Legendary),
            // unstable
            new Extra("Moon Refractor", "Absorb the moon's light and utilize its might.", 50, 15, 0, "Moonshine", "Increases your healing by 20%.", Rarity.Unstable),
            new Extra("Spirit Replica", "A ghostly clone of your weapon, attacking in tandem with you.", 60, 0, 0, null, null, Rarity.Unstable),
            new Extra("Beckoning Cat", "A cat statue said to bring good luck to the one who holds it.", 30, 50, 0, "Money Machine", "Chance to give you quite a bit of extra money after a fight.", Rarity.Unstable),
            new Extra("Crescent Mask", "A Crescent-shaped Mask, said to come from worshippers of an ancient moon god.", 20, 80, 0, null, null, Rarity.Unstable),
            new Extra("Fragmented Effigy", "A once beautiful depiction of the moon god, now blemished and broken.", 25, 65, 0, null, null, Rarity.Unstable),
            new Extra("Sash of Valor", "A sash meant for those with great courage and incredible strength.", 40, 30, 0, "Valorous Offensive", "The lower your HP currently is, the more damage you deal.", Rarity.Unstable),
            // corrupted
            new Extra("Black Magic Wand", "Achieve true strength by succumbing to darkness.", 10, 0, 0, "Black Magic", "Doubles your damage, halves your health.", Rarity.Corrupted),
            new Extra("Fire Orb", "Light up the fire in your soul to burn away your enemies.", 70, 10, 0, null, null, Rarity.Corrupted),
            new Extra("Crystal Primrose", "A flower said to stand for eternal love, crystallized by an unknown force.", 10, 90, 0, null, null, Rarity.Corrupted),
            new Extra("Greater Will", "The ethos that keeps together the world given form.", 40, 100, 0, null, null, Rarity.Corrupted),
            new Extra("The Fool's Errand", "Only a madman would accept the challenge.", 20, 20, 0, "Guts", "Doubles EXP gained from fights.", Rarity.Corrupted),
            new Extra("Nuclear Fishing Pole", "There probably aren't many sea creatures willing to be caught with this.", 55, 55, 0, "Radiation", "Deals damage to you and your enemy.", Rarity.Corrupted),
            // unique

            // handmade
            new Extra("Bunny Ears", "A ridiculous headwear, only working because your opponent will not expect any damage from you at all.", 3, 0, 0, null, null, Rarity.Common, false),
            new Extra("Rabbit Foot", "A regular lucky charm.", 0, 3, 0, null, null, Rarity.Common, false),


        };
    }
}
