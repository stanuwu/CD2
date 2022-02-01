using System;
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

            //craft any quests
            new CraftItemsQuest("Crafting Beginner", "CRX1", "Craft any item.", 1, 0, 300, 70, 0, 0, 0, null, null, 2160, "You need to learn how to create your own gear! Enter a Room of Forging and craft any item.", "You sure have some talent! Take this.", "Sadly you failed. There is always another chance.", "You have crafted %current%/%goal% items.", 0, QuestActivations.CraftItemAny, "any"),
            new CraftItemsQuest("Advanced Forgery", "CRX2", "Craft a few items.", 3, 0, 1200, 250, 0, 0, 0, null, null, 3360, "You must improve your forging skills. Make some gear and then come back to me.", "Wonderful Quality! Have this.", "At your level you should have been able to complete this task. I can not compensate you!", "You have crafted %current%/%goal% items.", 5, QuestActivations.CraftItemAny, "any"),
            new CraftItemsQuest("Master of Creation", "CRX3", "Craft a massive amount of items.", 5, 1, 2200, 400, 0, 0, 0, null, null, 10080, "Now that you have mastered the art of forging there is many that want to learn from you. Show them how its done!", "Impeccable .", "I am gravely dissapointed in you.", "You have crafted %current%/%goal% items.", 15, QuestActivations.CraftItemAny, "any"),
            //craft weapon quests
            new CraftItemsQuest("Weapon Crafter", "CRW1", "Craft any weapon.", 1, 0, 300, 70, 50, 0, 0, null, null, 2160, "Set out to master the art of creation! Make yourself a weapon in a Room of Forging.", "You sure have some talent! Take this.", "Sadly you failed. There is always another chance.", "You have crafted %current%/%goal% weapons.", 0, QuestActivations.CraftItemAny, "weapon"),
            new CraftItemsQuest("Advanced Blacksmith", "CRW2", "Craft two weapons.", 2, 0, 700, 150, 50, 0, 0, null, null, 2360, "You must improve your forging skills. Make some weapons and then come back to me.", "Wonderful Quality! Have this.", "At your level you should have been able to complete this task. I can not compensate you!", "You have crafted %current%/%goal% weapons.", 5, QuestActivations.CraftItemAny, "weapon"),
            new CraftItemsQuest("Creator of Desctruction", "CRW3", "Craft three weapons.", 3, 0, 1700, 300, 50, 0, 0, null, null, 7080, "Now that you have mastered the art of forging there is many that want to learn from you. Show them how its done!", "Impeccable .", "I am gravely dissapointed in you.", "You have crafted %current%/%goal% weapons.", 15, QuestActivations.CraftItemAny, "weapon"),
            //craft armor quests
            new CraftItemsQuest("Armor Crafter", "CRA1", "Craft any armor.", 1, 0, 300, 70, 0, 50, 0, null, null, 2160, "Set out to master the art of creation! Make yourself any piece of armor in a Room of Forging.", "You sure have some talent! Take this.", "Sadly you failed. There is always another chance.", "You have crafted %current%/%goal% pieces of armor.", 0, QuestActivations.CraftItemAny, "armor"),
            new CraftItemsQuest("Advanced Armorer", "CRA2", "Craft two pieces of armor.", 2, 0, 900, 150, 0, 50, 0, null, null, 2360, "You must improve your forging skills. Make some armor and then come back to me.", "Wonderful Quality! Have this.", "At your level you should have been able to complete this task. I can not compensate you!", "You have crafted %current%/%goal% pieces of armor.", 5, QuestActivations.CraftItemAny, "armor"),
            new CraftItemsQuest("On the Defense", "CRA3", "Craft three pieces of armor.", 3, 0, 1700, 300, 0, 50, 0, null, null, 7080, "Now that you have mastered the art of forging there is many that want to learn from you. Show them how its done!", "Impeccable .", "I am gravely dissapointed in you.", "You have crafted %current%/%goal% pieces of armor.", 15, QuestActivations.CraftItemAny, "armor"),
            //craft extra quests
            new CraftItemsQuest("Extra Crafter", "CRE1", "Craft any extra.", 1, 0, 300, 70, 0, 0, 50, null, null, 2160, "Set out to master the art of creation! Make yourself an extra in a Room of Forging.", "You sure have some talent! Take this.", "Sadly you failed. There is always another chance.", "You have crafted %current%/%goal% extras.", 0, QuestActivations.CraftItemAny, "extra"),
            new CraftItemsQuest("Advanced Enchanter", "CRE2", "Craft two extras.", 2, 0, 900, 150, 0, 0, 50, null, null, 2360, "You must improve your forging skills. Make some extras and then come back to me.", "Wonderful Quality! Have this.", "At your level you should have been able to complete this task. I can not compensate you!", "You have crafted %current%/%goal% extras.", 5, QuestActivations.CraftItemAny, "extra"),
            new CraftItemsQuest("Darkness Forger", "CRE3", "Craft three extras.", 3, 0, 1700, 300, 0, 0, 50, null, null, 7080, "Now that you have mastered the art of forging there is many that want to learn from you. Show them how its done!", "Impeccable .", "I am gravely dissapointed in you.", "You have crafted %current%/%goal% extras.", 15, QuestActivations.CraftItemAny, "extra"),

            //trade any quests
            new CompleteTradeQuest("Rookie Trader", "TRA1", "Complete a trade.", 1, 0, 300, 70, 0, 0, 0, null, null, 2160, "If you want to make it in the world you need to be able to get what you want. Go do a trade in a Room of Deals.", "Not bad for a beginner! Trading Pays.", "Not everyone is cut out for the business lifestyle.", "You have completed %current%/%goal% trades.", 0, QuestActivations.TradeComplete),
            new CompleteTradeQuest("Yours for Mine", "TRA2", "Complete some trades.", 3, 0, 1200, 250, 0, 0, 0, null, null, 3360, "Trading can make you rich, trading can make you happy. Get out there and make some deals.", "I see you have learned a lot! Let me give you this.", "Sometimes trading comes with losses.", "You have completed %current%/%goal% trades.", 5, QuestActivations.TradeComplete),
            new CompleteTradeQuest("Master of Deals", "TRA3", "Complete a lot of trades.", 5, 1, 2200, 400, 0, 0, 0, null, null, 10080, "Show me how you dominate the market. Go make those deals!", "Now I believe miracles can happen.", "Truly fascinating loss. I can not reward you for this.", "You have completed %current%/%goal% trades.", 15, QuestActivations.TradeComplete),

            //train any quests
            new TrainQuest("Self-Improvement", "TNX1", "Train a few times.", 10, 3, 500, 50, 20, 20, 20, null, null, 2160, "The first step to winning is to look at yourself and improve your skills. Go and train with your gear.", "Do you see what I meant? It's that easy!", "You can't keep blaming your mistakes on others.", "You have trained %current%/%goal% times.", 0, QuestActivations.TrainCompleteAny, "any"),
            new TrainQuest("One with the Blade", "TNW1", "Train with your weapon a few times.", 7, 3, 500, 50, 50, 0, 0, null, null, 2160, "Your Swordsmanship needs some work. Go and train with your weapon.", "Do you see what I meant? It's that easy!", "No improvements have been made here.", "You have trained with your weapon %current%/%goal% times.", 0, QuestActivations.TrainCompleteAny, "weapon"),
            new TrainQuest("Metal Man", "TNA1", "Train with your armor a few times.", 7, 3, 500, 50, 50, 0, 0, null, null, 2160, "You do not move well in your armor. Go and train with it.", "Do you see what I meant? It's that easy!", "No improvements have been made here.", "You have trained with your armor %current%/%goal% times.", 0, QuestActivations.TrainCompleteAny, "armor"),
            new TrainQuest("Artifact Abuser", "TNE1", "Train with your extra a few times.", 7, 3, 500, 50, 50, 0, 0, null, null, 2160, "You are not using your extra its full potential. Go tain with it.", "Do you see what I meant? It's that easy!", "No improvements have been made here.", "You have trained with your extra %current%/%goal% times.", 0, QuestActivations.TrainCompleteAny, "extra"),

            //farm any quests
            new FarmQuest("Working Hard", "FXX1", "Farm a few times.", 10, 3, 600, 60, 0, 0, 0, null, null, 2160, "Some things can only be found if you look for them. Go farming.", "There you go! Here is a reward.", "You don't need to farm but then I can not offer you a reward.", "You have farmed %current%/%goal% times.", 0, QuestActivations.FarmCompleteAny, "any"),
            new FarmQuest("Big Catch", "FFI1", "Go fishing a few times.", 5, 1, 400, 50, 0, 0, 0, null, new EnemyDrops("Hydra Scale", 1, 0, 100), 2160, "Fishing is your best friend.", "Wonderful. I have a gift for you.", "I'm dissapointed in you.", "You have fished %current%/%goal% times.", 0, QuestActivations.FarmCompleteAny, "fishing"),
            new FarmQuest("Mine Diamonds", "FMI1", "Go mining a few times.", 5, 1, 400, 50, 0, 0, 0, null, new EnemyDrops("Diamond", 1, 0, 100), 2160, "Mining is your best friend.", "Wonderful. I have a gift for you.", "I'm dissapointed in you.", "You have mined %current%/%goal% times.", 0, QuestActivations.FarmCompleteAny, "mining"),
            new FarmQuest("Felling Trees", "FFO1", "Go foraging a few times.", 5, 1, 400, 50, 0, 0, 0, null, new EnemyDrops("Tropical Wood", 1, 0, 100), 2160, "Foraging is your best friend.", "Wonderful. I have a gift for you.", "I'm dissapointed in you.", "You have foraged %current%/%goal% times.", 0, QuestActivations.FarmCompleteAny, "foraging"),
            new FarmQuest("Looking and Finding", "FCO1", "Go harvesting a few times.", 5, 1, 400, 50, 0, 0, 0, null, new EnemyDrops("Rare Root", 1, 0, 100), 2160, "Harvesting is your best friend.", "Wonderful. I have a gift for you.", "I'm dissapointed in you.", "You have harvested %current%/%goal% times.", 0, QuestActivations.FarmCompleteAny, "harvesting"),
            new FarmQuest("The Game is On", "FHU1", "Go hunting a few times.", 5, 1, 400, 50, 0, 0, 0, null, new EnemyDrops("Bear Pelt", 1, 0, 100), 2160, "Hunting is your best friend.", "Wonderful. I have a gift for you.", "I'm dissapointed in you.", "You have hunted %current%/%goal% times.", 0, QuestActivations.FarmCompleteAny, "hunting"),

            //enter floor quests
            new EnterFloorQuest("Dungeon Master", "EFX1", "Explore some floors.", 3, 0, 800, 90, 0, 0, 0, null, null, 2160, "The Dungeons is calling. Go explore some new floors!", "Very brave of you.", "You must not be scard of what might happen. Go on, try again.", "You have opened %current%/%goal% floors.", 0, QuestActivations.EnterFloor),

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
        CraftItemAny,
        TradeComplete,
        TrainCompleteAny,
        FarmCompleteAny,
        EnterFloor,
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

    public class CraftItemsQuest : Quest
    {
        public string Type { get; set; }

        public CraftItemsQuest(string name, string descriminator, string description, int progress, int progressMargin, int moneyReward, int xpReward, int weaponXpReward, int armorXpReward, int extraXpReward, Quest questReward, EnemyDrops dropReward, int completionTimeInMinutes, string obtainingDialogue, string completionDialogue, string questFailedDialogue, string questGoalDialogue, int levelLimit, QuestActivations activate, string type = "any") : base(name, descriminator, description, progress, progressMargin, moneyReward, xpReward, weaponXpReward, armorXpReward, extraXpReward, questReward, dropReward, completionTimeInMinutes, obtainingDialogue, completionDialogue, questFailedDialogue, questGoalDialogue, levelLimit, activate)
        {
            Type = type;
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
            }
            else if (Type == "any" || Type == (string) param)
            {
                ValueTuple<string, DateTime, int, int> data = (ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats);
                data.Item3++;
                this.SaveProgress(stats, data.Item1, data.Item2, data.Item3, data.Item4);
                return true;
            } else
            {
                return false;
            }
        }
        public override bool isQuestCompleted(CharacterStructure stats)
        {
            return (((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item3) >= (((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item4);
        }

        public override bool isQuestExpired(CharacterStructure stats)
        {
            if ((DateTime.Now - ((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item2).TotalMinutes > this.CompletionTimeInMinutes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override TimeSpan timeLeft(CharacterStructure stats)
        {
            return (TimeSpan.FromMinutes(this.CompletionTimeInMinutes)) - (DateTime.Now - ((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item2);
        }
    }

    public class CompleteTradeQuest : Quest
    {
        public CompleteTradeQuest(string name, string descriminator, string description, int progress, int progressMargin, int moneyReward, int xpReward, int weaponXpReward, int armorXpReward, int extraXpReward, Quest questReward, EnemyDrops dropReward, int completionTimeInMinutes, string obtainingDialogue, string completionDialogue, string questFailedDialogue, string questGoalDialogue, int levelLimit, QuestActivations activate) : base(name, descriminator, description, progress, progressMargin, moneyReward, xpReward, weaponXpReward, armorXpReward, extraXpReward, questReward, dropReward, completionTimeInMinutes, obtainingDialogue, completionDialogue, questFailedDialogue, questGoalDialogue, levelLimit, activate)
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
            }
            else
            {
                ValueTuple<string, DateTime, int, int> data = (ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats);
                data.Item3++;
                this.SaveProgress(stats, data.Item1, data.Item2, data.Item3, data.Item4);
                return true;
            }
        }
        public override bool isQuestCompleted(CharacterStructure stats)
        {
            return (((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item3) >= (((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item4);
        }

        public override bool isQuestExpired(CharacterStructure stats)
        {
            if ((DateTime.Now - ((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item2).TotalMinutes > this.CompletionTimeInMinutes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override TimeSpan timeLeft(CharacterStructure stats)
        {
            return (TimeSpan.FromMinutes(this.CompletionTimeInMinutes)) - (DateTime.Now - ((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item2);
        }
    }

    public class TrainQuest : Quest
    {
        public string Type { get; set; }

        public TrainQuest(string name, string descriminator, string description, int progress, int progressMargin, int moneyReward, int xpReward, int weaponXpReward, int armorXpReward, int extraXpReward, Quest questReward, EnemyDrops dropReward, int completionTimeInMinutes, string obtainingDialogue, string completionDialogue, string questFailedDialogue, string questGoalDialogue, int levelLimit, QuestActivations activate, string type = "any") : base(name, descriminator, description, progress, progressMargin, moneyReward, xpReward, weaponXpReward, armorXpReward, extraXpReward, questReward, dropReward, completionTimeInMinutes, obtainingDialogue, completionDialogue, questFailedDialogue, questGoalDialogue, levelLimit, activate)
        {
            Type = type;
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
            }
            else if (Type == "any" || Type == (string)param)
            {
                ValueTuple<string, DateTime, int, int> data = (ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats);
                data.Item3++;
                this.SaveProgress(stats, data.Item1, data.Item2, data.Item3, data.Item4);
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool isQuestCompleted(CharacterStructure stats)
        {
            return (((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item3) >= (((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item4);
        }

        public override bool isQuestExpired(CharacterStructure stats)
        {
            if ((DateTime.Now - ((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item2).TotalMinutes > this.CompletionTimeInMinutes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override TimeSpan timeLeft(CharacterStructure stats)
        {
            return (TimeSpan.FromMinutes(this.CompletionTimeInMinutes)) - (DateTime.Now - ((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item2);
        }
    }

    public class FarmQuest : Quest
    {
        public string Type { get; set; }

        public FarmQuest(string name, string descriminator, string description, int progress, int progressMargin, int moneyReward, int xpReward, int weaponXpReward, int armorXpReward, int extraXpReward, Quest questReward, EnemyDrops dropReward, int completionTimeInMinutes, string obtainingDialogue, string completionDialogue, string questFailedDialogue, string questGoalDialogue, int levelLimit, QuestActivations activate, string type = "any") : base(name, descriminator, description, progress, progressMargin, moneyReward, xpReward, weaponXpReward, armorXpReward, extraXpReward, questReward, dropReward, completionTimeInMinutes, obtainingDialogue, completionDialogue, questFailedDialogue, questGoalDialogue, levelLimit, activate)
        {
            Type = type;
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
            }
            else if (Type == "any" || Type == (string)param)
            {
                ValueTuple<string, DateTime, int, int> data = (ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats);
                data.Item3++;
                this.SaveProgress(stats, data.Item1, data.Item2, data.Item3, data.Item4);
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool isQuestCompleted(CharacterStructure stats)
        {
            return (((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item3) >= (((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item4);
        }

        public override bool isQuestExpired(CharacterStructure stats)
        {
            if ((DateTime.Now - ((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item2).TotalMinutes > this.CompletionTimeInMinutes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override TimeSpan timeLeft(CharacterStructure stats)
        {
            return (TimeSpan.FromMinutes(this.CompletionTimeInMinutes)) - (DateTime.Now - ((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item2);
        }
    }

    public class EnterFloorQuest : Quest
    {
        public EnterFloorQuest(string name, string descriminator, string description, int progress, int progressMargin, int moneyReward, int xpReward, int weaponXpReward, int armorXpReward, int extraXpReward, Quest questReward, EnemyDrops dropReward, int completionTimeInMinutes, string obtainingDialogue, string completionDialogue, string questFailedDialogue, string questGoalDialogue, int levelLimit, QuestActivations activate) : base(name, descriminator, description, progress, progressMargin, moneyReward, xpReward, weaponXpReward, armorXpReward, extraXpReward, questReward, dropReward, completionTimeInMinutes, obtainingDialogue, completionDialogue, questFailedDialogue, questGoalDialogue, levelLimit, activate)
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
            }
            else
            {
                ValueTuple<string, DateTime, int, int> data = (ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats);
                data.Item3++;
                this.SaveProgress(stats, data.Item1, data.Item2, data.Item3, data.Item4);
                return true;
            }
        }
        public override bool isQuestCompleted(CharacterStructure stats)
        {
            return (((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item3) >= (((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item4);
        }

        public override bool isQuestExpired(CharacterStructure stats)
        {
            if ((DateTime.Now - ((ValueTuple<string, DateTime, int, int>)this.LoadProgress(stats)).Item2).TotalMinutes > this.CompletionTimeInMinutes)
            {
                return true;
            }
            else
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