using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    class CharacterStructure
    {
        public CharacterStructure(string characterName, string title, string description, string characterClass, int money, int eXP, int hP, string weapon, string armor, string extra, Dictionary<string, int> inventory, double statMultiplier)
        {
            CharacterName = characterName;
            Title = title;
            Description = description;
            CharacterClass = characterClass;
            Money = money;
            EXP = eXP;
            HP = hP;
            Weapon = weapon;
            Armor = armor;
            Extra = extra;
            Inventory = inventory;
            StatMultiplier = statMultiplier;
        }

        public string CharacterName { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string CharacterClass { get; private set; }
        public int Money { get; private set; }
        public int EXP { get; private set; }
        public int HP { get; private set; }
        public string Weapon { get; private set; }
        public string Armor { get; private set; }
        public string Extra { get; private set; }
        public Dictionary<string, int> Inventory { get; private set; }
        public double StatMultiplier { get; private set; }
    }
}
