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

        string Drop { get; set; }
        int DropAmount { get; set; }
        int DropVariation { get; set; }
        int DropChance { get; set; }




        public KeyValuePair<string, int> getDrops()
        {
            KeyValuePair<string, int> drops;
            if (Defaults.GRandom.Next(0, 100) <= this.DropChance)
            {
                drops = new KeyValuePair<string, int>(this.Drop, Defaults.GRandom.Next(this.DropVariation * -1, this.DropVariation) + this.DropAmount);
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

            public virtual void CustomEffect(ulong playerID)
            {
                switch (this.Type)
            {
                case "Test":

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
            level = level + Defaults.GRandom.Next(-5, 5);
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
            new Enemy("Crazy Monk", "A monk whose mind has been corrupted by only the sins of their own clergy. Attacks random travelers to cleanse their souls.", 25, 0, 100, 0, 12 , null, null, new EnemyDrops("Torn Bible", 1, 0, 10)),
            new Enemy("Cursed Rock", "A rock that has been cursed to roll forever, crushing everything in its path", 70, 0, 80, 0, 5, null, null, new EnemyDrops("Stone", 2, 1, 40)),
            new Enemy("Wild Hound", "A stray dog, craving only food, not affection.", 50, 0, 100, 0, 10, null, null, null),
            new Enemy("Red Lizard", "A common reptile, often mistaken for a young dragon.", 40, 0, 100, 0, 9, null, null, new EnemyDrops("Scale", 1, 0, 20)),
            new Enemy("Goblin Bandit", "Hostile Goblins wielding melee weapons.", 70, 0, 90, 5, 15, null, null, null),
            new Enemy("Buzzing Dragonfly", "Bigger and more aggressive than normal dragonflys. Easy to kill, but their bites can cause grave wounds.", 35, 0, 150, 5, 20, null, null, null),
            new Enemy("Skeleton Warrior", "The remains of a fallen warrior whose desire for blood hasn't even been quenched by their demise.", 45, 0, 80, 5, 25, null, null, new EnemyDrops("Rusty Sword", 1, 0, 15)),
            new Enemy("Roaming Thundercloud", "The creation of a wizard's spell, forgotten by its caster.", 30, 0, 100, 5, 50, null, null, null),
            new Enemy("Green Ogre", "A giant monster armed with an even larger club. Protective of its territory.", 100, 0, 75, 5, 20, null, null, null),
            new Enemy("Stone Golem", "A giant humanoid covered in a armor of stones and minerals. Often thought to be a defense mechanism of the mountains they roam.", 100, 0, 70, 10, 20, null, null,  new EnemyDrops("Stone", 4, 1, 20)),
            new Enemy("Toxic Smoke", "A cloud o poisonous smoke which is controlled by an otherwise weak spirit.", 60, 0, 150, 10, 10, null, null, null),
            new Enemy("Ice Wizard of the Order", "A member of an evil organisation who specializes in ice spells.", 55, 0, 120, 10, 20, null, null, null),
            new Enemy("Fire Wizard of the Order", "A member of an evil organisation who specializes in fire spells.", 55, 0, 100, 10, 30, null, null, null),
            new Enemy("Grand Wizard of the Order", "An officer of an evil organisation who specializes in a variety of spells.", 70, 0, 110, 12, 25, null, null, null),
            new Enemy("Fire Golem", "They look Like their cousin the Stone Golem, but are covered in fire and found in volcanoes rather than normal mountains", 110, 0, 80, 15, 40, null, null, null),
            new Enemy("Wolverine Spider", "An agressive arachne, crawling in the deepest of caves. They got their name from its deadly bite bite strenght", 50, 0, 100, 15, 50, null, null, null),
            new Enemy("Lost Spirit", "The ghost of a human who found a horrifying death, trapped in the world of the living until the kill another unfortunate soul", 50, 0, 50, 15, 40, null, null, null),
            new Enemy("Ancient Spirit", "The ghost of a human who has forgotten their own death. Some say the ony kill in order to remember how they died", 40, 0, 50, 16, 45, null, null, null),
            new Enemy("Hungry Vampire", "A vampire who has lost their mind through starving for too long. They may never recover again, even if they manage to drink blood.", 50, 0, 120, 15, 50, null, null, null),
            new Enemy("Black Dragon", "A dragon who has been ousted by its family when it was young. It survived, only motiviated by its anger.", 150, 0, 70, 20, 40, null, null, new EnemyDrops("Scale", 3, 1, 10)),
            new Enemy("Banished Reaper", "A taker of souls who enjoyed their profession for the wrong reasons.", 100, 0, 100, 20, 50, null, null, null),
            new Enemy("Fallen Angel", "Deserters, who could not longer bear the ignorance of their maker. They were granted to roam the world of the living, but only to be punished with ill thoughts after arriving", 120, 0, 100, 20, 40, null, null, null),
            new Enemy("Devil's Minion", "Asked and granted to bring fear into the lives of the living by the devil himsel.", 90, 0, 80, 0, 45, null, null, null),
            new Enemy("Corrupted Spirit", "The soul of a person who killed before when they were alive. Sadly, the don't use their second chance for a ", 100, 0, 50, 25, 60, null, null, null)
        };
        }
}
