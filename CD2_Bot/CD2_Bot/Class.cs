using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public class Class
    {
        public Class(string name, string description, double damageMultiplier, int resistanceBonus, double healMultiplier, int level)
        {
            Name = name;
            Description = description;
            DamageMultiplier = damageMultiplier;
            ResistanceBonus = resistanceBonus;
            HealMultiplier = healMultiplier;
            Level = level;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public double DamageMultiplier { get; set; }
        public int ResistanceBonus { get; set; }
        public double HealMultiplier { get; set; }
        public int Level { get; set; }

        public static void beforeFight(CharacterStructure player, Enemy enemy, Weapon weapon, Armor armor, Extra extra)
        {
            switch (player.CharacterClass.Name)
            {
                case "Berserker":
                    if (weapon.Category == Category.Axe)
                    {
                        weapon.BaseDamage = (int)(weapon.BaseDamage * 1.2);
                    }

                    break;

                case "Thief":
                    if (weapon.Category == Category.Dagger)
                    {
                        weapon.BaseDamage = (int)(weapon.BaseDamage * 1.2);
                    }

                    break;
            }
        }

        public static void winFight(CharacterStructure player, int money, int exp, int gearexp, Enemy enemy)
        {
            switch (player.CharacterClass.Name)
            {
                case "Adventurer":
                    player.EXP += (int)(exp * 0.1);
                    break;

                case "Thief":
                    player.Money += (int)(money * 0.3);
                    break;
            }
        }

        public static void looseFight(CharacterStructure player, int money, Enemy enemy)
        {
        }

        public static List<Class> Classes = new List<Class>()
        {
            new Class("Commoner", "A normal guy.", 1, 0, 1, 0),
            new Class("Adventurer", "Gets 10% bonus XP from fights.", 1, 0, 1, 0),
            new Class("Berserker", "Brute that does bonus damage with axe type weapons.", 1.15, -10, 1, 5),
            new Class("Healer", "Heals faster but does less damage.", 0.9, 0, 1.3, 5),
            new Class("Thief", "More money from fights and does more damage with dagger type weapons.", 1, 0, 1, 15),
        };

        public static async Task SetClassAsync(SocketSlashCommand cmd)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                where user.PlayerID == cmd.User.Id
                select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("You don't have a character yet. Create one with /start!"));
                return;
            }

            string arg = (string)cmd.Data.Options.First().Value;

            if (arg == "")
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("Please enter a class name."));
            }
            else
            {
                Class charclass = Class.Classes.Find(x => x.Name.ToLower() == arg.ToLower());
                if (charclass == null)
                {
                    await cmd.RespondAsync(embed: Utils.QuickEmbedError("Class not found."));
                }
                else if (stats.Lvl < charclass.Level)
                {
                    await cmd.RespondAsync(embed: Utils.QuickEmbedError($"You need to be level {charclass.Level} to use this class."));
                }
                else
                {
                    stats.CharacterClass = charclass;
                    await cmd.RespondAsync(embed: Utils.QuickEmbedNormal("Class", $"Set your class to {charclass.Name}!"));
                }
            }
        }
    }
}