using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    class Quests
    {
    }

    public class Quest
    {
        public string Name { get; set; }
        public string Descriminator { get; set; }
        public string Description { get; set; }
        public string Progress { get; set; }
        public string ProgressMargin { get; set; }
        public int MoneyReward { get; set; }
        public int XpReward { get; set; }
        public int WeaponXpReward { get; set; }
        public int ArmorXpReward { get; set; }
        public int ExtraXpReward { get; set; }
        public Quest QuestReward { get; set; }
        public int CompletionTimeInMinutes { get; set; }
        public void UpdateProgress ()
        {
            throw new NotImplementedException();
        }
        public void GenerateRewards ()
        {
            throw new NotImplementedException();
        }
        public void SaveProgress (CharacterStructure stats)
        {
            throw new NotImplementedException();
        }
        public void LoadProgress(CharacterStructure stats)
        {
            throw new NotImplementedException();
        }
    }
}
