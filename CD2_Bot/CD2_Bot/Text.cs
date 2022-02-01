using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public static class Text
    {
        public static readonly string guide_page_start = "**Welcome to CustomDungeons 2!**\n" +
            "This bot allows you to play a simple yet fun text based dungeon rpg with your friends.\n" +
            "Use `<help` to get an overview of the commands and how they work or use the buttons below to view more pages of this guide.\n" +
            "Once you have a character can alse use `<vote` to get a bit of a headstart.";

        public static readonly string guide_page_floors = "**Floors**\n" +
            "The main mechanic of this game are floors. You can use the `<floor` command to enter a new floor and you will be able to pick a room to enter. " +
            "There are a lot of different rooms that can help you on your journey. Note that floors have a cooldown and you can not start one if your HP are too low.";

        public static readonly string guide_page_fights = "**Fights**\n" +
            "If you have entered a room of encounters you will enter a fight with a monster." +
            " Fights are round based and runs until the enemy defeats you or you defeat it." +
            " You will be shown the outcome of the fight afterwards. Note that a draw will still result in a loss!";

        public static readonly string guide_page_grinding = "**Grinding**\n" +
            "There is also some more commands that you can use frequently. Currently this includes the train and farm command. " +
            "Look at the help command for more information.";

        public static readonly string guide_page_character = "**Character**\n" +
            "Using the character command you can view your character. You have some stats like HP. HP are drained in a fight and regenerate over time. " +
            "You also have 3 pieces of gear. To view more information about them use `<stats`. To view your items use `<inventory`.";

        public static readonly string guide_page_gear = "**Gear**\n" +
            "There are 3 different pieces of gear. Weapons, armor and extras. Weapons are primarily used to attack while armor determines your defense. Extras are for hp regeneration and can buff your attack. " +
            "Higher level gear can also have custom effects that can deal extra damage or have other effects. " +
            "You can obtain gear from winning a fight as a rare drop, or from buying a chest in the room of surprises as well as from crafting in the room of forging.";

        public static readonly string guide_page_quests = "**Quests**\n" +
            "Quests are another good way to earn money, xp and other rewards. You can obtain a quest by entering a room of adventures. " +
            "Once you have a quest you can see your progress with the `<quest` command. " +
            "Once you complete your quest in time return back to the room of adventures to obtain your reward.";



        public static readonly string help_page_character = "`<start [name]`\nCreate a character with the given name.\n\n" +
            "`<character <[userid]>`\nView your character (or someone elses if given a userid).\n\n" +
            "`<stats <[userid]>`\nView your gear (or someone elses if given a userid).\n\n" +
            "`<inventory`\nView your items.\n\n" +
            "`<reset`\nDelete your character.\n\n" +
            "`<rename [name]`\nChange your character name.\n\n" +
            "`<description [text]`\nChange your character description.\n\n" +
            "`<server`\nSee the stats of the current server.\n\n" +
            "`<vote`\nVote for the bot on top.gg and claim some free rewards.";

        public static readonly string help_page_stats = "`<weapon [name]`\nView the stats of the given weapon.\n\n" +
            "`<armor [name]`\nView the stats of the given armor.\n\n" +
            "`<extra [name]`\nView the stats of the given extra.\n\n" +
            "`<monster [name]`\nView the stats of the given monster.\n\n" +
            "Example: `<weapon wooden`";

        public static readonly string help_page_play = "`<floor`\nEnter a new floor and pick a door to open.\n\n" +
            "`<train <[gear type]>`\nTrain with your gear to improve and gain gear xp. You can specify a gear type to ofcus on it(weapon, armor or extra).\n\n" +
            "`<farm [activity]`\nFarm some items used for crafting and trading. Current activities are fishing, mining, foraging, harvesting and hunting.\n\n" +
            "`<quest`\nView your current quest progress.\n\n" +
            "`<pvp [@player] <[wager]>`\nChallange another player to a pvp match. You can wager coins if you want to.";

        public static readonly string help_page_money = "`<coinflip [wager] <[@opponent]>`\nCoinflip against an AI or another player for money.\n\n" +
            "`<slots [wager]`\nPlay a slots game for money.";

        public static readonly string help_page_misc = "`<lvltop`\nShowcases the top characters across all servers by level.\n\n" +
            "`<moneytop`\nShowcases the top characters across all servers by money.\n\n" +
            "`<geartop`\nShowcases the top characters across all servers by gear.\n\n" +
            "`<servertop`\nShowcases the top servers.\n\n" +
            "`<guide`\nA Guidebook to tell you how to get started.\n\n" +
            "`<help`\nHelp menu to view all commands.";

        public static readonly string help_page_rooms = "**Wealth**\nFind some coins.\n\n" +
            "**Surprises**\nGet a random chest that you can open with coins.\n\n" +
            "**Deals**\nA merchant will offer you a trade for items.\n\n" +
            "**Traps**\nJust dont go into this one...\n\n" +
            "**Adventures**\nObtain or complete quests.\n\n" +
            "**Forging**\nCraft gear in this room.\n\n" +
            "**Encounters**\nFight a random monster.\n\n" +
            "**Focused Encounters**\nFight a monster from a certain biome.\n\n" +
            "**Darkness**\nA random room.";
    }
}