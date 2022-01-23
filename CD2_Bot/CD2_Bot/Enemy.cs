using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{

    interface IEnemyStats
    {
        string Type { get; set; }
        string Description { get; set; }
        int HP { get; set; }
        int Level { get; set; }
        int Resistance { get; set; }
        int Minlevel { get; set; }
        int Damage { get; set; }

    }

    public enum Biome
    {
     Any,
     Plains,
     Forest,
     Jungle,
     Cave,
     Crypt,
     Coast,
     Sea,
     Mountains,
     Volcano,
     Desert,
     Tundra,
     Swamp,
     Settlement
    }

    public static class BiomesScaling
    {
        public static Dictionary<Biome, int> levels = new Dictionary<Biome, int>
        {
            { Biome.Any, 0 },
            { Biome.Plains, -2 },
            { Biome.Forest, -1 },
            { Biome.Jungle, 1 },
            { Biome.Cave, 1 },
            { Biome.Crypt, 3 },
            { Biome.Coast, 0 },
            { Biome.Sea, 1 },
            { Biome.Mountains, 1 },
            { Biome.Volcano, 4 },
            { Biome.Desert, 2 },
            { Biome.Tundra, 0 },
            { Biome.Swamp, 1 },
            { Biome.Settlement, 0 },
        };

        public static Biome randomBiome()
        {
            List<Biome> biomes = ((Biome[]) (Enum.GetValues(typeof (Biome)))).ToList();
            biomes.Remove(Biome.Any);
            return biomes[Defaults.GRandom.Next(biomes.Count)];
        }
    }

    public class EnemyDrops
    {
        public EnemyDrops(string drop, int dropAmount, int dropVariation, int dropChance)
        {
            Drop = drop;
            DropAmount = dropAmount;
            DropVariation = dropVariation;
            DropChance = dropChance;
        }

        public string Drop { get; set; }
        public int DropAmount { get; set; }
        public int DropVariation { get; set; }
        public int DropChance { get; set; }




        public KeyValuePair<string, int> getDrops()
        {
            KeyValuePair<string, int> drops;
            if (Defaults.GRandom.Next(0, 100) <= this.DropChance)
            {
                drops = new KeyValuePair<string, int>(this.Drop, Math.Abs(Defaults.GRandom.Next(this.DropVariation * -1, this.DropVariation+1) + this.DropAmount));
            }
            else
            {
                drops = new KeyValuePair<string, int>(this.Drop, 0);
            }
            return drops;
        }
    }
        public class Enemy : IEnemyStats
        {
            public Enemy(string type, string description, int hP, int level, int resistance, int minlevel, int damage, string customEffectName, string customEffectDescription, EnemyDrops drops, Biome biome)
            {
                Type = type;
                Description = description;
                HP = hP;
                Level = level;
                Resistance = resistance;
                Minlevel = minlevel;
                Damage = damage;
                CustomEffectName = customEffectName;
                CustomEffectDescription = customEffectDescription;
                Drops = drops;
                Biome = biome;
            }

            public string Type { get; set; }
            public string Description { get; set; }
            public int HP { get; set; }
            public int Level { get; set; }
            public int Resistance { get; set; }
            public int Minlevel { get; set; }
            public int Damage { get; set; }
            public string CustomEffectName { get; set; }
            public string CustomEffectDescription { get; set; }
            public EnemyDrops Drops { get; set; }
            public Biome Biome { get; set; } 

            public virtual double CustomEffect(CharacterStructure character, Weapon weapon, Armor armor, Extra extra)
            {
                switch (this.CustomEffectName)
            {

                case "Light Bleeding":
                    return this.Damage/10;

                    break;

                case "Moderate Bleeding":
                    return this.Damage/8;

                    break;

                case "Severe Bleeding":
                    return this.Damage/5;

                    break;

                case "Light Burn":
                    weapon.BaseDamage -= weapon.BaseDamage / 20;
                    return character.MaxHP/50;
                    
                    break;

                 case "Moderate Burn":
                    weapon.BaseDamage -= weapon.BaseDamage / 20;
                    return character.MaxHP/25;

                    break;

                 case "Severe Burn":
                    weapon.BaseDamage -= weapon.BaseDamage / 20;
                    return character.MaxHP/15;

                    break;

                 case "Light Poison":
                    armor.Resistance += 5;
                    return character.MaxHP/70;

                    break;

                 case "Moderate Poison":
                    armor.Resistance += 5;
                    return this.Damage/50;

                    break;

                 case "Severe Poison":
                    armor.Resistance += 5;
                    return this.Damage/25;

                    break;

                 case "Stun":
                    weapon.BaseDamage -= weapon.BaseDamage / 10;

                    break;

                case "Paralysis":
                    armor.Resistance += 10;

                    break;

                case "Freeze":
                    weapon.BaseDamage -= weapon.BaseDamage / 25;
                    armor.Resistance += 3;

                    break;

                case "Lifesteal":
                    this.HP += this.Damage*2;

                    break;

                case "Disgusting Appearance":
                    weapon.BaseDamage += weapon.BaseDamage + 5;
                   
                    break;

                case "Small Money Drop":
                    character.Money += Prices.buy[Rarity.Common];

                    break;

                case "Moderate Money Drop":
                    character.Money += Prices.buy[Rarity.Uncommon];

                    break;
                
                case "Large Money Drop":
                    character.Money += Prices.buy[Rarity.Rare];

                    break;

                case "Massive Money Drop":
                    character.Money += Prices.buy[Rarity.Epic];

                    break;

                case "Tremendous Money Drop":
                    character.Money += Prices.buy[Rarity.Legendary];

                    break;
                case "Melting":
                    if (weapon.CustomEffectName != null && weapon.CustomEffectName.Contains("Burn"))
                    {
                        this.Resistance = this.Resistance*2;
                    }
                    break;
                case "Confusing Appearance":
                    weapon.BaseDamage -= weapon.BaseDamage/2;
                    break;
            }
            return 0;
            } 

            
        }
        
        public static class EnemyGen
        {
            public static Enemy RandomEnemy(int level, Biome b = Biome.Any)
            {
                List<Enemy> possibleEnemies = new List<Enemy> { };
                if (b != Biome.Any)
                {
                    possibleEnemies = (from e in Enemies where e.Minlevel <= level select e).ToList().FindAll(e => e.Biome == b);
                } else
                {
                    possibleEnemies = (from e in Enemies where e.Minlevel <= level select e).ToList();
                }
                
                Enemy selectedEnemy = possibleEnemies[Defaults.GRandom.Next(possibleEnemies.Count())];
                if (level > 6)
                {
                    level = Math.Abs(level + Defaults.GRandom.Next(-2, 3));
                }
                level += BiomesScaling.levels[b];
                if (level < 0) level = 0;
                int hpadded = 0;
                if (level > 50)
                {
                    hpadded = ((int)Math.Floor(selectedEnemy.HP + 0.05 * Math.Pow(50, 2.5))) + (5 * (level - 50));
                }
                else
                {
                    hpadded = (int)Math.Floor(selectedEnemy.HP + 0.05 * Math.Pow(level, 2.5));
                }
                int damageadded = 0;
                if (level > 50)
                {
                    damageadded = ((int)Math.Floor(selectedEnemy.Damage + 0.004 * Math.Pow(50, 2.5))) + (5 * (level - 50));
                }
                else
                {
                    damageadded = (int)Math.Floor(selectedEnemy.Damage + 0.004 * Math.Pow(level, 2.5));
                }
                Enemy generatedEnemy = new Enemy(selectedEnemy.Type, selectedEnemy.Description, hpadded, level, selectedEnemy.Resistance, selectedEnemy.Minlevel, damageadded, selectedEnemy.CustomEffectName, selectedEnemy.CustomEffectDescription, selectedEnemy.Drops, selectedEnemy.Biome);
                return generatedEnemy;
            }

            public static List<Enemy> Enemies = new List<Enemy>
            {
                //Any
                new Enemy("Slime", "A mindless creature, formed through strange goo flowing out of the walls of certain caves. Comes in a variety of colours.", 50, 0, 100, 0, 7 , null, null, null, Biome.Any),
                new Enemy("Wild Hound", "A stray dog, craving only food, not affection.", 50, 0, 100, 0, 10, null, null, null, Biome.Any),
                new Enemy("Red Lizard", "A common reptile, often mistaken for a young dragon.", 40, 0, 100, 0, 9, null, null, new EnemyDrops("Scale", 1, 0, 20), Biome.Any),
                new Enemy("Blorb", "Bloated, fallen off flesh from another monster, which somehow formed a life of its own. Sometimes it even grows limbs, wobbling around and being a disgusting sight to behold.", 40, 0, 100, 0, 7, "Disgusting Appearance", "Increases the damage the player deals", null, Biome.Any),
                new Enemy("Eye-Eating Crow", "A crow who lives around cemeterys, attacking the grieving when there is no corpse to eat.", 35, 0, 85, 0, 7, null, null, new EnemyDrops("Feather", 1, 0, 20), Biome.Any),
                new Enemy("Wooden Mimic", "A wooden chest corrupted by evil, engaging everyone who tries to open it", 50, 0, 95, 0, 9, "Small Money Drop", "The player gets a small amount of money", new EnemyDrops("Wood", 1, 0, 20), Biome.Any),
                new Enemy("Goblin Bandit", "Hostile goblin wielding a melee weapon.", 50, 0, 100, 5, 15, "Small Money Drop", "The player gets a small amount of money", null, Biome.Any),
                new Enemy("Stabbing Goblin ", "A goblin with murderous intent, enjoying the sounds of their shiv.", 45, 0, 100, 6, 15, "Light Bleeding", "Causes sligth extra damage depending on the monster's strength, negating armor.", null, Biome.Any), 
                new Enemy("Roaming Thundercloud", "The creation of a wizard's spell, forgotten by its caster.", 30, 0, 150, 5, 50, null, null, null, Biome.Any),
                new Enemy("Bandit", "A human whose path led them to a life of crime.", 65, 0, 100, 10, 25, "Small Money Drop", "The player gets a small amount of money", new EnemyDrops("Small Pouch", 1, 0, 10), Biome.Any),
                new Enemy("Bandit Leader", "The leader of a small group of bandits. Some are indepent, some are lieutenants of a larger bandit group.", 75, 0, 100, 11, 30, "Moderate Money Drop", "The player gets a moderate amount of money", new EnemyDrops("Small Pouch", 1, 0, 15), Biome.Any),
                new Enemy("Grand Wizard of the Order", "An officer of an evil organisation who specializes in a variety of spells.", 70, 0, 110, 11, 25, null, null, null, Biome.Settlement),
                new Enemy("Conjoined Blorbs", "Two blorbs who conjoined completely by accident", 80, 0, 100, 15, 21, "Disgusting Appearance", "Increases the damage the player deals", null, Biome.Any),
                new Enemy("Lost Spirit", "The ghost of a human who found a horrifying death, trapped in the world of the living until the kill another unfortunate soul.", 50, 0, 50, 15, 40, null, null, null, Biome.Any),
                new Enemy("Hungry Vampire", "A vampire who has lost their mind through starving for too long. They may never recover again, even if they manage to drink blood.", 50, 0, 120, 15, 50, "Lifesteal", "Heals itself for twice its damage before the start of the battle.", null, Biome.Any),
                new Enemy("Silvery Mimic", "A chest made of silver corrupted by evil, engaging everyone who tries to open it", 90, 0, 85, 15, 30, "Large Money Drop", "The player gets a large amount of money", new EnemyDrops("Silver", 1, 0, 20), Biome.Any),
                new Enemy("Goblin Bandit Squad", "A squad of weak goblin bandits, but they come in numbers.", 80, 0, 90, 15, 37, "Small Money Drop", "The player gets a small amount of money", null, Biome.Any),
                new Enemy("Goblin Bandit Squad Leader", "A dwarfish orc posing as a goblin, serving as a leader of a bandit squad.", 85, 0, 95, 16, 33, "Small Money Drop", "The player gets a small amount of money", null, Biome.Any),
                new Enemy("Orc Bandit", "Hostile orc wielding a melee weapon, though they only use their fists sometimes.", 90, 0, 90, 20, 35, "Small Money Drop", "The player gets a small amount of money", null, Biome.Any),
                new Enemy("Black Dragon", "A dragon who has been ousted by its family when it was young. It survived, only motiviated by its anger.", 150, 0, 70, 20, 40, "Moderate Burn", "Causes moderate extra damage depending on the strength of the player and decreases their accuracy.", new EnemyDrops("Dragon Scale", 3, 1, 10), Biome.Any),
                new Enemy("Fallen Angel", "Deserters, who could not longer bear the ignorance of their maker. They were granted to roam the world of the living, but only to be punished with ill thoughts after arriving.", 120, 0, 100, 20, 40, null, null, null, Biome.Any),
                new Enemy("Corrupted Spirit", "The soul of a person who killed before when they were alive. Sadly, the don't use their second chance for a betterment.", 100, 0, 50, 25, 60, null, null, null, Biome.Any),
                new Enemy("Golden Mimic", "A chest made of gold corrupted by evil, engaging everyone who tries to open it", 100, 0, 80, 30, 50, "Massive Money Drop", "The player gets a massive amount of money", new EnemyDrops("Gold", 1, 0, 20), Biome.Any),
                new Enemy("Platinum Mimic", "A chest made of platinum corrupted by evil, engaging everyone who tries to open it", 130, 0, 75, 40, 50, "Tremendous Money Drop", "The player gets a tremendous amount of money", new EnemyDrops("Platinum", 1, 0, 20), Biome.Any),
                new Enemy("Black Mother Dragon", "A dragon who has been ousted by its family when it was young, who started a family of her own after killing her parents.", 300, 0, 70, 50, 80, "Severe Burn", "Causes severe extra damage depending on the strength of the player and decreases their accuracy.", new EnemyDrops("Dragon Scale", 5, 1, 10), Biome.Any),

                //Plains
                new Enemy("Plain Slime", "A slime so plain it's often not even considered as a threat.", 25, 0, 100, 0, 5, null, null, null, Biome.Plains),
                new Enemy("Brown Wolf", "The smallest species of wolf who dares to attack humans. Only slightly bigger than a fox.", 60, 0, 100, 5, 20, null, null, new EnemyDrops("Small Wolf Pelt", 1, 0, 10), Biome.Plains),
                new Enemy("Brown Alpha Wolf", "The leader of a pack of brown wolves. Can hold their own against larger wolf species.", 70, 0, 100, 6, 25, null, null, new EnemyDrops("Small Wolf Pelt", 1, 0, 10), Biome.Plains),
                new Enemy("Smalltime Poacher", "A hunter without a license or ethics. Doesn't take it lightly when someones marches into their hunting grounds.", 55, 0, 100, 5, 20, null, null, new EnemyDrops("Small Wolf Pelt", 1, 0, 5), Biome.Plains),
                new Enemy("Green Ogre", "A giant monster armed with an even larger club. Protective of its territory.", 75, 0, 80, 10, 20, null, null, null, Biome.Plains),
                new Enemy("Banished Reaper", "A taker of souls who enjoyed their profession for the wrong reasons.", 100, 0, 100, 20, 50, null, null, null, Biome.Plains),



                //Cave
                new Enemy("Gnawing Rat", "A rat who attacks and eats everything it sees, even other rats.", 20, 0, 90, 0, 5, "Light Bleeding", "Causes sligth extra damage depending on the monster's strength, negating armor.", null, Biome.Cave),
                new Enemy("Glowing Bug", "A flesh-eating insect, using its yellowish skin to reflect light.", 35, 0, 150, 0, 10, "Stun", "Causes a small chance for the player to miss.", null, Biome.Cave),
                new Enemy("White-Eyed Scorpion", "A scorpion who is rougly the size of a pig. Lives in caves because the sunlight hurts its eyes.", 40, 0, 85, 5, 18, "Paralysis", "Decreases the resistance of the player.", null, Biome.Cave),
                new Enemy("Wolverine Spider", "An agressive arachne, crawling in the deepest of caves. They got their name from its deadly bite strength.", 50, 0, 100, 15, 50, null, null, null, Biome.Cave),


                //Coast
                new Enemy("Hungry Drowned", "An undead who met their demise at the bottom of the sea.", 30, 0, 110, 0, 8, null, null, null, Biome.Coast),
                new Enemy("Washed Up Pirate", "A pirate who was nearly killed in a storm, roaming the shores.", 60, 0, 100, 10, 28, "Small Money Drop", "The player gets a small amount of money", new EnemyDrops("Small Pouch", 1, 0, 10), Biome.Coast),


                //Crypt
                new Enemy("Roaming Skeleton", "A soul cursed to rot in its already rotten body.", 30, 0, 100, 0, 10, null, null, new EnemyDrops("Bone", 2, 1, 10), Biome.Crypt),
                new Enemy("Skeleton Warrior", "The remains of a fallen warrior whose desire for blood hasn't even been quenched by their demise.", 35, 0, 80, 5, 25, null, null, new EnemyDrops("Rusty Sword", 1, 0, 15), Biome.Crypt),
                new Enemy("Skeleton Archer", "Robbed of their eyes through their decay, they began to fire at everything the sense.", 35, 0, 100, 5, 30, null, null, new EnemyDrops("Arrow", 1, 0, 20), Biome.Crypt),
                new Enemy("Ancient Spirit", "The ghost of a human who has forgotten their own death. Some say the ony kill in order to remember how they died.", 40, 0, 50, 16, 45, null, null, null, Biome.Crypt),
                new Enemy("Necromancer of the Order", "A member of an evil organisation who specializes in necromancy.", 80, 0, 120, 30, 40, null, null, null, Biome.Crypt),


                //Desert
                new Enemy("Sandspewer", "An aggressive reptile who lunges sand to attack its prey.", 40, 0, 100, 0, 8, null, null, null, Biome.Desert),
                new Enemy("Yellow-Skinned Scorpion", "A subspecies of the White-Eyed Scorpion. A nocturnal threat found in most deserts.", 40, 0, 85, 5, 18, "Paralysis", "Decreases the resistance of the player.", null, Biome.Desert),


                //Forest
                new Enemy("Convict in Hiding", "A criminal who made the woods their new home.", 50, 0, 100, 0, 7, null, null, null, Biome.Forest),
                new Enemy("Wandering Tree", "A tree whose roots started to become legs to hunt the fiends who destroyed their habitat.", 80, 0, 90, 10, 20, null, null, new EnemyDrops("Wood", 3, 1, 30), Biome.Forest),
                new Enemy("Black Wolf", "The wolf species people instantly think about when you talk about wolves. The rarely attack humans.", 70, 0, 100, 10, 27, null, null, new EnemyDrops("Medium Wolf Pelt", 1, 0, 10), Biome.Forest),
                new Enemy("Black Alpha Wolf", "The leader of a pack of black wolves. Often has scars around it eyes gained in battle over the leadership.", 80, 0, 100, 11, 30, null, null, new EnemyDrops("Medium Wolf Pelt", 1, 0, 10), Biome.Forest),
                new Enemy("Scarred Grizzly", "A big bear who lost its cubs to a hunting party. The scars show the battle which soon followed after.", 85, 0, 100, 15, 40, null, null, new EnemyDrops("Tattered Bear Pelt", 1, 0, 5), Biome.Forest),


                //Jungle
                new Enemy("Black-Gilled Piranha", "A predator found in many rivers and streams. There black gills give off an awful stench.", 30, 0, 85, 0, 8, null, null, new EnemyDrops("Fish Scale", 1, 0, 20), Biome.Jungle),


                //Tundra
                new Enemy("Cursed Snowman", "The favorite joke of ice wizards in the area.", 30, 0, 100, 0, 3, "Melting", "Receives more damage when hit with fire.", null, Biome.Tundra),
                new Enemy("Ice Wizard of the Order", "A member of an evil organisation who specializes in ice spells.", 55, 0, 100, 10, 20, "Freeze", "Decreases damage and resistance of the player.", null, Biome.Tundra),
                new Enemy("Snow Golem", "A giant humanoid made out of strange hardened snow.", 80, 0, 90, 10, 20, "Melting", "Receives more damage when hit with fire.", null, Biome.Tundra),

                //Mountains
                new Enemy("Cursed Rock", "A rock that has been cursed to roll forever, crushing everything in its path.", 70, 0, 80, 0, 5, null, null, new EnemyDrops("Stone", 2, 1, 40), Biome.Mountains),
                new Enemy("Stone Golem", "A giant humanoid covered in a armor of stones and minerals. Often thought to be a defense mechanism of the mountains they roam.", 100, 0, 70, 10, 20, null, null, new EnemyDrops("Stone", 4, 1, 20), Biome.Mountains),


                //Volcano
                new Enemy("Optimistic Pyromaniac", "An aspirirng fire wizard wannabe, for all the wrong reasons", 50, 0, 100, 0, 10, null, null, null, Biome.Volcano),
                new Enemy("Fire Wizard of the Order", "A member of an evil organisation who specializes in fire spells.", 55, 0, 120, 10, 30, "Light Burn", "Causes slight extra damage depending on the strength of the player and decreases their accuracy.", null, Biome.Volcano),
                new Enemy("Fire Golem", "They look Like their cousin the stone golem, but are covered in fire and found in volcanoes rather than normal mountains.", 110, 0, 80, 15, 40, "Moderate Burn", "Causes moderate extra damage depending on the strength of the player and decreases their accuracy.", null, Biome.Volcano),
                new Enemy("Devil's Minion", "Asked and granted to bring fear into the lives of the living by the devil himself.", 90, 0, 80, 25, 45, null, null, null, Biome.Volcano),
                new Enemy("Magma Golem", "Only found in mountain ranges including volcanoes, this golem seems to be a rare fusion of a stone and a fire golem.", 130, 0, 70, 30, 50, "Moderate Burn", "Causes moderate extra damage depending on the strength of the player and decreases their accuracy.", new EnemyDrops("Stone", 4, 2, 5), Biome.Volcano),



                //Swamp
                new Enemy("Buzzing Dragonfly", "Bigger and more aggressive than normal dragonflys. Easy to kill, but their bites can cause grave wounds.", 20, 0, 150, 0, 30, null, null, null, Biome.Swamp),
                new Enemy("Toxic Smoke", "A cloud o poisonous smoke which is controlled by an otherwise weak spirit.", 60, 0, 150, 10, 10, "Light Posion", "Causes slight extra damage and decreases the resistance of the player", null, Biome.Swamp),


                //Sea
                new Enemy("Neukenvis", "The most hated kind of fish off all fishermen, known for chewing on rods and fingers.", 30, 0, 100, 0, 8, null, null, new EnemyDrops("Fish Scale", 1, 0, 5), Biome.Sea),
                new Enemy("Crying Siren", "A sea creature who shapeshifts into the most loved person of it's prey to lure them in.", 40, 0, 100, 10, 20, "Confusing Appearance", "Decreases the damage dealt by the the player", null, Biome.Sea),
                new Enemy("Lion Shark", "A medium sized shark. Known for their yellow color.", 70, 0, 100, 15, 35, null, null, new EnemyDrops("Fish Scale", 3, 1, 15), Biome.Sea),


                //Settlement
                new Enemy("Crazy Monk", "A monk whose mind has been corrupted by only the sins of their own clergy. Attacks random travelers to cleanse their soul.", 25, 0, 100, 0, 12 , null, null, new EnemyDrops("Torn Bible", 1, 0, 10), Biome.Settlement),
                new Enemy("Paranoid Escapee", "A prisoner who managed to flee from their cell, using a rather extreme method to ensure that there will be no witnesses.", 50, 0, 100, 5, 18, null, null, null, Biome.Settlement),
                new Enemy("Corrupt Guard", "A corrupt member of the city guard.", 70, 0, 90, 10, 22, null, null, null, Biome.Settlement),
                new Enemy("Corrupt Guard Officer", "A corrupt officer of the city guard.", 75, 0, 90, 11, 25, null, null, null, Biome.Settlement),


                // new Enemy("", "", 0, 0, 0, 0, 0, null, null, null, null)
            };
        }
}
