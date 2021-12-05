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
                drops = new KeyValuePair<string, int>(this.Drop, Math.Abs(Defaults.GRandom.Next(this.DropVariation * -1, this.DropVariation) + this.DropAmount));
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
            public Enemy(string type, string description, int hP, int level, int resistance, int minlevel, int damage, string customEffectName, string customEffectDescription, EnemyDrops drops)
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

            public virtual void CustomEffect(CharacterStructure character, Weapon weapon, Armor armor, Extra extra)
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
                    base.Damage -= base.Damage/20;
                    return character.MaxHP/50;
                    
                    break;

                 case "Moderate Burn":
                    base.Damage -= base.Damage/20;
                    return character.MaxHP/25;

                    break;

                 case "Severe Burn":
                    base.Damage -= base.Damage/20;
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
                    base.Damage -= base.Damage/10;

                    break;

                case "Paralysis":
                    armor.Resistance += 10;

                    break;

                case "Freeze":
                    base.Damage -= base.Damage/25;
                    armor.Resistance += 3;

                    break;

                case "Lifesteal":
                    this.HP += this.Damage*2;

                    break;

                case "Disgusting Appearance":
                    base.Damage += base.Damage+5;
                   
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
            }
            } 

            
        }
        
        public static class EnemyGen
        {
            public static Enemy RandomEnemy(int level)
            {
            List<Enemy> possibleEnemies = (from e in Enemies where e.Minlevel <= level select e).ToList();
            Enemy selectedEnemy = possibleEnemies[Defaults.GRandom.Next(possibleEnemies.Count())];
            if (level > 6)
            {
                level = Math.Abs(level + Defaults.GRandom.Next(-5, 5));
            }
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
                damageadded = ((int)Math.Floor(selectedEnemy.Damage + 0.003 * Math.Pow(50, 2.5))) + (5 * (level - 50));
            }
            else
            {
                damageadded = (int)Math.Floor(selectedEnemy.Damage + 0.003 * Math.Pow(level, 2.5));
            }
            Enemy generatedEnemy = new Enemy(selectedEnemy.Type, selectedEnemy.Description, hpadded, level, selectedEnemy.Resistance, selectedEnemy.Minlevel, damageadded, selectedEnemy.CustomEffectName, selectedEnemy.CustomEffectDescription, selectedEnemy.Drops);
            return generatedEnemy;
            }

        public static List<Enemy> Enemies = new List<Enemy>
        {
            new Enemy("Slime", "A mindless creature, formed through strange goo flowing out of the walls of certain caves. Comes in a variety of colours.", 50, 0, 100, 0, 7 , null, null, null),
            new Enemy("Gnawing Rat", "A rat who attacks and eats everything it sees, even other rats.", 20, 0, 90, 0, 5, "Light Bleeding", "Causes sligth extra damage depending on the monster's strength, negating armor.", null),
            new Enemy("Crazy Monk", "A monk whose mind has been corrupted by only the sins of their own clergy. Attacks random travelers to cleanse their soul.", 25, 0, 100, 0, 12 , null, null, new EnemyDrops("Torn Bible", 1, 0, 10)),
            new Enemy("Cursed Rock", "A rock that has been cursed to roll forever, crushing everything in its path.", 70, 0, 80, 0, 5, null, null, new EnemyDrops("Stone", 2, 1, 40)),
            new Enemy("Wild Hound", "A stray dog, craving only food, not affection.", 50, 0, 100, 0, 10, null, null, null),
            new Enemy("Red Lizard", "A common reptile, often mistaken for a young dragon.", 40, 0, 100, 0, 9, null, null, new EnemyDrops("Scale", 1, 0, 20)),
            new Enemy("Blorb", "Bloated, fallen off flesh from another monster, which somehow formed a life of its own. Sometimes it even grows limbs, wobbling around and being a disgusting sight to behold.", 40, 0, 100, 0, 7, "Disgusting Appearance", "Increases the damage the player deals", null),
            new Enemy("Black-Gilled Piranha", "A predator found in many rivers and streams. There black gills give off an awful stench.", 30, 0, 85, 0, 8, null, null, new EnemyDrops("Fish Scale", 1, 0, 20)),
            new Enemy("Eye-Eating Crow", "A crow who lives around cemeterys, attacking the grieving when there is no corpse to eat.", 35, 0, 85, 0, 7, null, null, new EnemyDrops("Feather", 1, 0, 20)),
            new Enemy("Buzzing Dragonfly", "Bigger and more aggressive than normal dragonflys. Easy to kill, but their bites can cause grave wounds.", 20, 0, 150, 0, 30, null, null, null),
            new Enemy("Wooden Mimic", "A wooden chest corrupted by evil, engaging everyone who tries to open it", 50, 0, 95, 0, 9, "Small Money Drop", "The player gets a small amount of money", new EnemyDrops("Wood", 1, 0, 20)),
            new Enemy("Goblin Bandit", "Hostile goblin wielding a melee weapon.", 50, 0, 100, 5, 15, null, null, null),
            new Enemy("Stabbing Goblin ", "A goblin with murderous intent, enjoying the sounds of their shiv.", 45, 0, 100, 6, 15, "Light Bleeding", "Causes sligth extra damage depending on the monster's strength, negating armor.", null),
            new Enemy("Skeleton Warrior", "The remains of a fallen warrior whose desire for blood hasn't even been quenched by their demise.", 35, 0, 80, 5, 25, null, null, new EnemyDrops("Rusty Sword", 1, 0, 15)),
            new Enemy("Skeleton Archer", "Robbed of their eyes through their decay, they began to fire at everything the sense.", 35, 0, 100, 5, 30, null, null, new EnemyDrops("Arrow", 1, 0, 20)),
            new Enemy("Roaming Thundercloud", "The creation of a wizard's spell, forgotten by its caster.", 30, 0, 150, 5, 50, null, null, null),
            new Enemy("Brown Wolf", "The smallest species of wolf who dares to attack humans. Only slightly bigger than a fox.", 60, 0, 100, 5, 20, null, null, new EnemyDrops("Small Wolf Pelt", 1, 0, 10)),
            new Enemy("Brown Alpha Wolf", "The leader of a pack of brown wolves. Can hold their own against larger wolf species.", 70, 0, 100, 6, 25, null, null, new EnemyDrops("Small Wolf Pelt", 1, 0, 10)),
            new Enemy("Poacher", "A hunter without a license or ethics. Doesn't take it lightly when someones marches into their hunting grounds.", 55, 0, 100, 5, 20, null, null, new EnemyDrops("Small Wolf Pelt", 1, 0, 5)),
            new Enemy("White-Eyed Scorpion", "A scorpion who is rougly the size of a pig. Lives in caves because the sunlight hurts its eyes.", 40, 0, 85, 5, 18, null, null, null),
            new Enemy("Steely Mimic", "A chest made of steel corrupted by evil, engaging everyone who tries to open it", 70, 0, 90, 5, 17, "Moderate Money Drop", "The player gets a moderate amount of money", new EnemyDrops("Steel", 1, 0, 20)),
            new Enemy("Green Ogre", "A giant monster armed with an even larger club. Protective of its territory.", 75, 0, 80, 10, 20, null, null, null),
            new Enemy("Bandit", "A human whose path led them to a life of crime.", 65, 0, 100, 10, 25, null, null, new EnemyDrops("Small Pouch", 1, 0, 10)),
            new Enemy("Bandit Leader", "The leader of a small group of bandits. Some are indepent, some are lieutenants of a larger bandit group.", 75, 0, 100, 11, 30, null, null, new EnemyDrops("Small Pouch", 1, 0, 15)),
            new Enemy("Wandering Tree", "A tree whose roots started to become legs to hunt the fiends who destroyed their habitat.", 80, 0, 90, 10, 20, null, null, new EnemyDrops("Wood", 3, 1, 30)),
            new Enemy("Stone Golem", "A giant humanoid covered in a armor of stones and minerals. Often thought to be a defense mechanism of the mountains they roam.", 100, 0, 70, 10, 20, null, null, new EnemyDrops("Stone", 4, 1, 20)),
            new Enemy("Toxic Smoke", "A cloud o poisonous smoke which is controlled by an otherwise weak spirit.", 60, 0, 150, 10, 10, "Light Posion", "Causes slight extra damage and decreases the resistance of the player", null),
            new Enemy("Ice Wizard of the Order", "A member of an evil organisation who specializes in ice spells.", 55, 0, 100, 10, 20, "Freeze", "Decreases damage and resistance of the player.", null),
            new Enemy("Fire Wizard of the Order", "A member of an evil organisation who specializes in fire spells.", 55, 0, 120, 10, 30, "Light Burn", "Causes slight extra damage depending on the strength of the player and decreases their accuracy.", null),
            new Enemy("Grand Wizard of the Order", "An officer of an evil organisation who specializes in a variety of spells.", 70, 0, 110, 11, 25, null, null, null),
            new Enemy("Black Wolf", "The wolf species people instantly think about when you talk about wolves. The rarely attack humans.", 70, 0, 100, 10, 27, null, null, new EnemyDrops("Medium Wolf Pelt", 1, 0, 10)),
            new Enemy("Black Alpha Wolf", "The leader of a pack of black wolves. Often has scars around it eyes gained in battle over the leadership.", 80, 0, 100, 11, 30, null, null, new EnemyDrops("Medium Wolf Pelt", 1, 0, 10)),
            new Enemy("Scarred Grizzly", "A big bear who lost its cubs to a hunting party. The scars show the battle which soon followed after.", 85, 0, 100, 15, 40, null, null, new EnemyDrops("Tattered Bear Pelt", 1, 0, 5)),
            new Enemy("Fire Golem", "They look Like their cousin the stone golem, but are covered in fire and found in volcanoes rather than normal mountains.", 110, 0, 80, 15, 40, "Moderate Burn", "Causes moderate extra damage depending on the strength of the player and decreases their accuracy.", null),
            new Enemy("Wolverine Spider", "An agressive arachne, crawling in the deepest of caves. They got their name from its deadly bite strength.", 50, 0, 100, 15, 50, null, null, null),
            new Enemy("Lost Spirit", "The ghost of a human who found a horrifying death, trapped in the world of the living until the kill another unfortunate soul.", 50, 0, 50, 15, 40, null, null, null),
            new Enemy("Ancient Spirit", "The ghost of a human who has forgotten their own death. Some say the ony kill in order to remember how they died.", 40, 0, 50, 16, 45, null, null, null),
            new Enemy("Hungry Vampire", "A vampire who has lost their mind through starving for too long. They may never recover again, even if they manage to drink blood.", 50, 0, 120, 15, 50, "Lifesteal", "Heals itself for twice its damage before the start of the battle.", null),
            new Enemy("Lion Shark", "A medium sized shark. Known for their yellow color.", 70, 0, 100, 15, 35, null, null, new EnemyDrops("Fish Scale", 3, 1, 15)),
            new Enemy("Silvery Mimic", "A chest made of silver corrupted by evil, engaging everyone who tries to open it", 90, 0, 85, 15, 30, "Large Money Drop", "The player gets a large amount of money", new EnemyDrops("Silver", 1, 0, 20)),
            new Enemy("Orc Bandit", "Hostile orc wielding a melee weapon, though they only use their fists sometimes.", 90, 0, 90, 20, 35, null, null, null),
            new Enemy("Black Dragon", "A dragon who has been ousted by its family when it was young. It survived, only motiviated by its anger.", 150, 0, 70, 20, 40, "Moderate Burn", "Causes moderate extra damage depending on the strength of the player and decreases their accuracy.", new EnemyDrops("Dragon Scale", 3, 1, 10)),
            new Enemy("Banished Reaper", "A taker of souls who enjoyed their profession for the wrong reasons.", 100, 0, 100, 20, 50, null, null, null),
            new Enemy("Fallen Angel", "Deserters, who could not longer bear the ignorance of their maker. They were granted to roam the world of the living, but only to be punished with ill thoughts after arriving.", 120, 0, 100, 20, 40, null, null, null),
            new Enemy("Devil's Minion", "Asked and granted to bring fear into the lives of the living by the devil himself.", 90, 0, 80, 25, 45, null, null, null),
            new Enemy("Corrupted Spirit", "The soul of a person who killed before when they were alive. Sadly, the don't use their second chance for a betterment.", 100, 0, 50, 25, 60, null, null, null),
            new Enemy("Magma Golem", "Only found in mountain ranges including volcanoes, this golem seems to be a rare fusion of a stone and a fire golem.", 130, 0, 70, 30, 50, "Moderate Burn", "Causes moderate extra damage depending on the strength of the player and decreases their accuracy.", new EnemyDrops("Stone", 4, 2, 5)),
            new Enemy("Necromancer of the Order", "A member of an evil organisation who specializes in necromancy.", 80, 0, 120, 30, 40, null, null, null),
            new Enemy("Golden Mimic", "A chest made of gold corrupted by evil, engaging everyone who tries to open it", 100, 0, 80, 0, 50, "Massive Money Drop", "The player gets a massive amount of money", new EnemyDrops("Gold", 1, 0, 20)),
            new Enemy("Platinum Mimic", "A chest made of platinum corrupted by evil, engaging everyone who tries to open it", 130, 0, 75, 40, 50, "Tremendous Money Drop", "The player gets a tremendous amount of money", new EnemyDrops("Platinum", 1, 0, 20)),

            // new Enemy("", "", 0, 0, 0, 0, 0, null, null, null)
        };
        }
}
