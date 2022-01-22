﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public static class Quests
    {
        //add quests here
        public static List<Quest> questList = new List<Quest>
        {
            //kill any monster quest
            new KillAnyMonstersQuest("Kill Random Monsters", "KRM", "Kill any monster in any place.", 9, 1, 1000, 250, 0, 0, 0, null, null, 2160, "These monsters have been annoying me, please kill some of any monster for me!", "Thanks for killing those monsters! Here is your reward!", "You were not able to kill the monsters? Sorry, but I can not offer you any reward.", "You have killed %current%/%goal% monsters so far.", 0, QuestActivations.DefeatMonster),
            //biome kill monster quests
            new KillBiomeMonstersQuest("Kill Plains Monsters", "KPLM", "Kill any monster from the plains.", 7, 2, 900, 220, 0, 0, 0, null, null, 2880, "The plains are crawling with monsters. Please kill some of them!", "You've done well. Here is your reward!", "You were not able to kill the monsters? Sorry, but I can not offer you any reward.", "You have killed %current%/%goal% monsters so far.", 0, QuestActivations.DefeatMonster, Biome.Plains),
            new KillBiomeMonstersQuest("Kill Forest Monsters", "KFRM", "Kill any monster from the forest.", 7, 2, 990, 240, 0, 0, 0, null, null, 2880, "The forest is crawling with monsters. Please kill some of them!", "You've done well. Here is your reward!", "You were not able to kill the monsters? Sorry, but I can not offer you any reward.", "You have killed %current%/%goal% monsters so far.", 0, QuestActivations.DefeatMonster, Biome.Forest),
            new KillBiomeMonstersQuest("Kill Jungle Monsters", "KJGM", "Kill any monster from the jungle.", 7, 2, 1500, 350, 0, 0, 0, null, null, 2880, "The jungle is crawling with monsters. Please kill some of them!", "You've done well. Here is your reward!", "You were not able to kill the monsters? Sorry, but I can not offer you any reward.", "You have killed %current%/%goal% monsters so far.", 2, QuestActivations.DefeatMonster, Biome.Jungle),
            new KillBiomeMonstersQuest("Kill Cave Monsters", "KCVM", "Kill any monster from the cave.", 7, 2, 1500, 350, 0, 0, 0, null, null, 2880, "The cave is crawling with monsters. Please kill some of them!", "You've done well. Here is your reward!", "You were not able to kill the monsters? Sorry, but I can not offer you any reward.", "You have killed %current%/%goal% monsters so far.", 2, QuestActivations.DefeatMonster, Biome.Cave),
            new KillBiomeMonstersQuest("Kill Crypt Monsters", "KCPM", "Kill any monster from the crpyt.", 5, 2, 2500, 550, 0, 0, 0, null, null, 2880, "The crypt is crawling with monsters. Please kill some of them!", "You've done well. Here is your reward!", "You were not able to kill the monsters? Sorry, but I can not offer you any reward.", "You have killed %current%/%goal% monsters so far.", 8, QuestActivations.DefeatMonster, Biome.Crypt),
            new KillBiomeMonstersQuest("Kill Coast Monsters", "KCOM", "Kill any monster from the coast.", 7, 2, 900, 220, 0, 0, 0, null, null, 2880, "The coast is crawling with monsters. Please kill some of them!", "You've done well. Here is your reward!", "You were not able to kill the monsters? Sorry, but I can not offer you any reward.", "You have killed %current%/%goal% monsters so far.", 0, QuestActivations.DefeatMonster, Biome.Coast),
            new KillBiomeMonstersQuest("Kill Sea Monsters", "KSEM", "Kill any monster from the sea.", 7, 2, 1500, 350, 0, 0, 0, null, null, 2880, "The sea is crawling with monsters. Please kill some of them!", "You've done well. Here is your reward!", "You were not able to kill the monsters? Sorry, but I can not offer you any reward.", "You have killed %current%/%goal% monsters so far.", 2, QuestActivations.DefeatMonster, Biome.Sea),
            new KillBiomeMonstersQuest("Kill Mountain Monsters", "KMOM", "Kill any monster from the mountains.", 7, 2, 1500, 350, 0, 0, 0, null, null, 2880, "The mountains are crawling with monsters. Please kill some of them!", "You've done well. Here is your reward!", "You were not able to kill the monsters? Sorry, but I can not offer you any reward.", "You have killed %current%/%goal% monsters so far.", 2, QuestActivations.DefeatMonster, Biome.Mountains),
            new KillBiomeMonstersQuest("Kill Volcano Monsters", "KVOM", "Kill any monster from the volcano.", 5, 2, 2800, 650, 0, 0, 0, null, null, 2880, "The volcano is crawling with monsters. Please kill some of them!", "You've done well. Here is your reward!", "You were not able to kill the monsters? Sorry, but I can not offer you any reward.", "You have killed %current%/%goal% monsters so far.", 10, QuestActivations.DefeatMonster, Biome.Volcano),
            new KillBiomeMonstersQuest("Kill Desert Monsters", "KDEM", "Kill any monster from the desert.", 4, 2, 1750, 400, 0, 0, 0, null, null, 2880, "The desert is crawling with monsters. Please kill some of them!", "You've done well. Here is your reward!", "You were not able to kill the monsters? Sorry, but I can not offer you any reward.", "You have killed %current%/%goal% monsters so far.", 5, QuestActivations.DefeatMonster, Biome.Desert),
            new KillBiomeMonstersQuest("Kill Tundra Monsters", "KTUM", "Kill any monster from the plains.", 7, 2, 900, 220, 0, 0, 0, null, null, 2880, "The tundra is crawling with monsters. Please kill some of them!", "You've done well. Here is your reward!", "You were not able to kill the monsters? Sorry, but I can not offer you any reward.", "You have killed %current%/%goal% monsters so far.", 0, QuestActivations.DefeatMonster, Biome.Tundra),
            new KillBiomeMonstersQuest("Kill Swamp Monsters", "KSWM", "Kill any monster from the swamp.", 7, 2, 1500, 350, 0, 0, 0, null, null, 2880, "The swamp is crawling with monsters. Please kill some of them!", "You've done well. Here is your reward!", "You were not able to kill the monsters? Sorry, but I can not offer you any reward.", "You have killed %current%/%goal% monsters so far.", 2, QuestActivations.DefeatMonster, Biome.Swamp),
            new KillBiomeMonstersQuest("Kill Settlement Monsters", "KSEM", "Kill any monster from the settlement.", 7, 2, 900, 220, 0, 0, 0, null, null, 2880, "The settlement is crawling with monsters. Please kill some of them!", "You've done well. Here is your reward!", "You were not able to kill the monsters? Sorry, but I can not offer you any reward.", "You have killed %current%/%goal% monsters so far.", 0, QuestActivations.DefeatMonster, Biome.Settlement),

            //new Quest("", "", "", 0, 0, 0, 0, 0, 0, 0, null, null, 0, "", "", "", "", 0, null),

        };

        //get what quest the player has from stored quest data value
        public static Quest WhatQuest(string data)
        {
            string disc = data.Split(';')[0];
            return questList.Find(x => x.Descriminator == disc);
        }

        public static Quest QuestFromDisc(string disc)
        {
            return questList.Find(x => x.Descriminator == disc);
        }
    }

    //if you add something here you will need to implement it actually firing UpdateProgess in the right place with the right information... (like event triggers)
    public enum QuestActivations
    { 
        DefeatMonster,
    }

    public class Quest {
        public Quest(string name, string descriminator, string description, int progress, int progressMargin, int moneyReward, int xpReward, int weaponXpReward, int armorXpReward, int extraXpReward, Quest questReward, EnemyDrops dropReward, int completionTimeInMinutes, string obtainingDialogue, string completionDialogue, string questFailedDialogue, string questGoalDialogue, int levelLimit, QuestActivations activate)
        {
            Name = name;
            Descriminator = descriminator;
            Description = description;
            Progress = progress;
            ProgressMargin = progressMargin;
            MoneyReward = moneyReward;
            XpReward = xpReward;
            WeaponXpReward = weaponXpReward;
            ArmorXpReward = armorXpReward;
            ExtraXpReward = extraXpReward;
            QuestReward = questReward;
            DropReward = dropReward;
            CompletionTimeInMinutes = completionTimeInMinutes;
            ObtainingDialogue = obtainingDialogue;
            CompletionDialogue = completionDialogue;
            QuestFailedDialogue = questFailedDialogue;
            QuestGoalDialogue = questGoalDialogue;
            LevelLimit = levelLimit;
            Activate = activate;
        }

        public string Name { get; set; }
        public string Descriminator { get; set; }
        public string Description { get; set; }
        public int Progress { get; set; }
        public int ProgressMargin { get; set; }
        public int MoneyReward { get; set; }
        public int XpReward { get; set; }
        public int WeaponXpReward { get; set; }
        public int ArmorXpReward { get; set; }
        public int ExtraXpReward { get; set; }
        public Quest QuestReward { get; set; }
        public EnemyDrops DropReward { get; set; }
        public int CompletionTimeInMinutes { get; set; }
        public string ObtainingDialogue { get; set; }
        public string CompletionDialogue { get; set; }
        public string QuestFailedDialogue { get; set; }
        public string QuestGoalDialogue { get; set; }
        public int LevelLimit { get; set; }
        public QuestActivations Activate { get; set; }

        public virtual bool UpdateProgress (CharacterStructure stats, QuestActivations source, Object param = null)
        {
            return this.Activate == source;
        }

        public virtual string ViewRewards()
        {
            string tell = "";
            if (this.MoneyReward > 0)
            {
                tell += $"{this.MoneyReward} Coins\n";
            }

            if (this.XpReward > 0)
            {
                tell += $"{this.XpReward} XP\n";
            }

            if (this.WeaponXpReward > 0)
            {
                tell += $"{this.WeaponXpReward} Weapon XP\n";
            }

            if (this.ArmorXpReward > 0)
            {
                tell += $"{this.ArmorXpReward} Armor XP\n";
            }
            if (this.ExtraXpReward > 0)
            {
                tell += $"{this.ExtraXpReward} Extra XP\n";
            }
            if (this.QuestReward != null)
            {
                tell += $"{this.QuestReward.Name} (new quest)\n";
            }
            if (this.DropReward != null)
            {
                tell += $"+{this.DropReward.DropAmount-this.DropReward.DropVariation}-{this.DropReward.DropAmount+this.DropReward.DropVariation}x {this.DropReward.Drop}\n ({this.DropReward.DropChance}%)";
            }
            if (tell == "")
            {
                tell = "No rewards.";
            }
            return tell;
        }
        public virtual string GenerateRewards (CharacterStructure stats)
        {
            string tell = "";
            if (this.MoneyReward > 0)
            {
                stats.Money += this.MoneyReward;
                tell += $"+{this.MoneyReward} Coins\n";
            }
            
            if (this.XpReward > 0)
            {
                stats.EXP += this.XpReward;
                tell += $"+{this.XpReward} XP\n";
            }
            
            if (this.WeaponXpReward > 0)
            {
                stats.WeaponXP += this.WeaponXpReward;
                tell += $"+{this.WeaponXpReward} Weapon XP\n";
            }

            if (this.ArmorXpReward > 0)
            {
                stats.ArmorXP += this.ArmorXpReward;
                tell += $"+{this.ArmorXpReward} Armor XP\n";
            }
            if (this.ExtraXpReward > 0)
            {
                stats.ExtraXP += this.ExtraXpReward;
                tell += $"+{this.ExtraXpReward} Extra XP\n";
            }
            if (this.QuestReward != null)
            {
                this.QuestReward.generateQuest(stats);
                tell += $"+{this.QuestReward.Name} (new quest)\n";
            }
            if (this.DropReward != null)
            {
                Dictionary<string,int> inv = Utils.InvAsDict(stats);
                string drop = this.DropReward.getDrops().Key;
                int am = this.DropReward.getDrops().Value;
                if (inv.ContainsKey(drop))
                {
                    inv[drop] += am;
                } else
                {
                    inv.Add(drop, am);
                }
                Utils.SaveInv(stats, inv);
                tell += $"+{am}x {drop}\n";
            }
            if(tell == "")
            {
                tell = "No rewards.";
            }
            return tell;
        }
        public virtual void SaveProgress (CharacterStructure stats)
        {
            throw new NotImplementedException();
        }
        public virtual object LoadProgress(CharacterStructure stats)
        {
            throw new NotImplementedException();
        }
        public virtual string ShowProgress(CharacterStructure stats)
        {
            throw new NotImplementedException();
        }
        public virtual void generateQuest(CharacterStructure stats)
        {
            throw new NotImplementedException();
        }
        public virtual bool isQuestCompleted(CharacterStructure stats)
        {
            throw new NotImplementedException();
        }

        public virtual bool isQuestExpired(CharacterStructure stats)
        {
            throw new NotImplementedException();
        }

        public virtual TimeSpan timeLeft(CharacterStructure stats)
        {
            throw new NotImplementedException();
        }
    }

    //###################
    // create quests here
    //###################
    public class KillAnyMonstersQuest : Quest
    {
        public KillAnyMonstersQuest(string name, string descriminator, string description, int progress, int progressMargin, int moneyReward, int xpReward, int weaponXpReward, int armorXpReward, int extraXpReward, Quest questReward, EnemyDrops dropReward, int completionTimeInMinutes, string obtainingDialogue, string completionDialogue, string questFailedDialogue, string questGoalDialogue, int levelLimit, QuestActivations activate) : base(name, descriminator, description, progress, progressMargin, moneyReward, xpReward, weaponXpReward, armorXpReward, extraXpReward, questReward, dropReward, completionTimeInMinutes, obtainingDialogue, completionDialogue, questFailedDialogue, questGoalDialogue, levelLimit, activate)
        {
        }
        public override void generateQuest(CharacterStructure stats)
        {
            int margin = this.Progress + Defaults.GRandom.Next(this.ProgressMargin * -1, this.ProgressMargin);
            stats.QuestData = $"{this.Descriminator};{DateTime.Now.ToString()};0;{margin.ToString()}";
        }
        public override string ShowProgress(CharacterStructure stats)
        {
            int progress = ((ValueTuple<string, DateTime, int, int>)LoadProgress(stats)).Item3;
            int goal = ((ValueTuple<string, DateTime, int, int>)LoadProgress(stats)).Item4;
            return this.QuestGoalDialogue.Replace("%current%", progress.ToString()).Replace("%goal%", goal.ToString());
        }
        public override object LoadProgress(CharacterStructure stats)
        {
            string[] data = stats.QuestData.Split(';');
            ValueTuple<string, DateTime, int, int> rdata;
            rdata.Item1 = data[0];
            rdata.Item2 = DateTime.Parse(data[1]);
            rdata.Item3 = Convert.ToInt32(data[2]);
            rdata.Item4 = Convert.ToInt32(data[3]);
            return rdata;
        }
        public void SaveProgress(CharacterStructure stats, string d, DateTime s, int p, int g)
        {
            stats.QuestData = $"{d};{s.ToString()};{p.ToString()};{g.ToString()}";
        }
        public override bool UpdateProgress(CharacterStructure stats, QuestActivations source, object param = null)
        {
            bool gainprogress = base.UpdateProgress(stats, source, param);
            if (gainprogress == false)
            {
                return false;
            } else
            {
                ValueTuple<string, DateTime, int, int> data = (ValueTuple<string, DateTime, int, int>) this.LoadProgress(stats);
                data.Item3++;
                this.SaveProgress(stats, data.Item1, data.Item2, data.Item3, data.Item4);
                return true;
            }
        }
        public override bool isQuestCompleted(CharacterStructure stats)
        {
            return (((ValueTuple<string, DateTime, int, int>) this.LoadProgress(stats)).Item3) >= (((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item4);
        }

        public override bool isQuestExpired(CharacterStructure stats)
        {
            if ((DateTime.Now - ((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item2).TotalMinutes > this.CompletionTimeInMinutes) {
                return true;
            } else
            {
                return false;
            }
        }

        public override TimeSpan timeLeft(CharacterStructure stats)
        {
            return (TimeSpan.FromMinutes(this.CompletionTimeInMinutes)) - (DateTime.Now - ((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item2);
        }
    }

     public class KillBiomeMonstersQuest : Quest
    {
        public KillBiomeMonstersQuest(string name, string descriminator, string description, int progress, int progressMargin, int moneyReward, int xpReward, int weaponXpReward, int armorXpReward, int extraXpReward, Quest questReward, EnemyDrops dropReward, int completionTimeInMinutes, string obtainingDialogue, string completionDialogue, string questFailedDialogue, string questGoalDialogue, int levelLimit, QuestActivations activate, Biome biome) : base(name, descriminator, description, progress, progressMargin, moneyReward, xpReward, weaponXpReward, armorXpReward, extraXpReward, questReward, dropReward, completionTimeInMinutes, obtainingDialogue, completionDialogue, questFailedDialogue, questGoalDialogue, levelLimit, activate)
        {
            Biome = biome;
        }
        public Biome Biome {get; set;} 

        public override void generateQuest(CharacterStructure stats)
        {
            int margin = this.Progress + Defaults.GRandom.Next(this.ProgressMargin * -1, this.ProgressMargin);
            stats.QuestData = $"{this.Descriminator};{DateTime.Now.ToString()};0;{margin.ToString()}";
        }
        public override string ShowProgress(CharacterStructure stats)
        {
            int progress = ((ValueTuple<string, DateTime, int, int>)LoadProgress(stats)).Item3;
            int goal = ((ValueTuple<string, DateTime, int, int>)LoadProgress(stats)).Item4;
            return this.QuestGoalDialogue.Replace("%current%", progress.ToString()).Replace("%goal%", goal.ToString());
        }
        public override object LoadProgress(CharacterStructure stats)
        {
            string[] data = stats.QuestData.Split(';');
            ValueTuple<string, DateTime, int, int> rdata;
            rdata.Item1 = data[0];
            rdata.Item2 = DateTime.Parse(data[1]);
            rdata.Item3 = Convert.ToInt32(data[2]);
            rdata.Item4 = Convert.ToInt32(data[3]);
            return rdata;
        }
        public void SaveProgress(CharacterStructure stats, string d, DateTime s, int p, int g)
        {
            stats.QuestData = $"{d};{s.ToString()};{p.ToString()};{g.ToString()}";
        }
        public override bool UpdateProgress(CharacterStructure stats, QuestActivations source, object param = null)
        {
            bool gainprogress = base.UpdateProgress(stats, source, param);
            if (gainprogress == false)
            {
                return false;
            } else
            {
                if (((Enemy) param).Biome == this.Biome) {
                    ValueTuple<string, DateTime, int, int> data = (ValueTuple<string, DateTime, int, int>) this.LoadProgress(stats);
                    data.Item3++;
                    this.SaveProgress(stats, data.Item1, data.Item2, data.Item3, data.Item4);
                    return true;
                }
            }
            return false;
        }
        public override bool isQuestCompleted(CharacterStructure stats)
        {
            return (((ValueTuple<string, DateTime, int, int>) this.LoadProgress(stats)).Item3) >= (((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item4);
        }

        public override bool isQuestExpired(CharacterStructure stats)
        {
            if ((DateTime.Now - ((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item2).TotalMinutes > this.CompletionTimeInMinutes) {
                return true;
            } else
            {
                return false;
            }
        }

        public override TimeSpan timeLeft(CharacterStructure stats)
        {
            return (TimeSpan.FromMinutes(this.CompletionTimeInMinutes)) - (DateTime.Now - ((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item2);
        }
    }
}