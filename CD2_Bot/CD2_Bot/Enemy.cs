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
            new Enemy("Goblin Bandit", "Hostile Goblins wielding melee weapons. A common, but ", 0, 0, 0, 0, 0, null, null, null)
        };
        }
}
