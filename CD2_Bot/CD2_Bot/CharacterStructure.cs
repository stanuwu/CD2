using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    class CharacterStructure
    {
        public CharacterStructure(string characterName, ulong playerID)
        {
            CharacterName = characterName;
            Title = "Player";
            Description = "";
            CharacterClass = "";
            Money = 0;
            EXP = 0;
            HP = 0;
            Weapon = "";
            Armor = "";
            Extra = "";
            Inventory = new Dictionary<string, int> { };
            StatMultiplier = 1;
            PlayerID = playerID;
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
        public ulong PlayerID { get; private set; }
    }
}
