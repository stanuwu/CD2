using Discord;
using Discord.Net;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public static class SlashCommandRegistry
    {
        public static async Task Client_Ready()
        {
            //register slash commands here

            //#############
            //test commands
            //#############

            //"test" command
            SocketGuild guild = Defaults.CLIENT.GetGuild(Defaults.TESTGUILDID);
            SlashCommandBuilder testCommand = new SlashCommandBuilder();
            testCommand.WithName("test");
            testCommand.WithDescription("this is a test");
            await guild.CreateApplicationCommandAsync(testCommand.Build());

            //"character" command
            SlashCommandBuilder characterCommand = new SlashCommandBuilder()
                .WithName("character")
                .WithDescription("Look at your own or someone elses character.")
                .AddOption("user", ApplicationCommandOptionType.User, "The user that you want to view the character of.", isRequired: false);
            await guild.CreateApplicationCommandAsync(characterCommand.Build());

            //"start" command
            SlashCommandBuilder startCommand = new SlashCommandBuilder()
                .WithName("start")
                .WithDescription("Create a character to start your journey")
                .AddOption("name", ApplicationCommandOptionType.String, "The name you want your character to have.", isRequired: true);
            await guild.CreateApplicationCommandAsync(startCommand.Build());

            //"stats" command
            SlashCommandBuilder statsCommand = new SlashCommandBuilder()
                .WithName("stats")
                .WithDescription("Have a look at your or someone elses gear.")
                .AddOption("user", ApplicationCommandOptionType.User, "The user that you want to view the character of.", isRequired: false);
            await guild.CreateApplicationCommandAsync(statsCommand.Build());

            //"server" command
            SlashCommandBuilder serverCommand = new SlashCommandBuilder()
                .WithName("server")
                .WithDescription("View the stats of the server you are currently in.");
            await guild.CreateApplicationCommandAsync(statsCommand.Build());

            //"inventory" command
            SlashCommandBuilder inventoryCommand = new SlashCommandBuilder()
                .WithName("inventory")
                .WithDescription("View your inventory.");
            await guild.CreateApplicationCommandAsync(inventoryCommand.Build());

            //"reset" command
            SlashCommandBuilder resetCommand = new SlashCommandBuilder()
                .WithName("reset")
                .WithDescription("Delete your character.");
            await guild.CreateApplicationCommandAsync(resetCommand.Build());

            //"rename" command
            SlashCommandBuilder renameCommand = new SlashCommandBuilder()
                .WithName("rename")
                .WithDescription("Change the name of your character.")
                .AddOption("name", ApplicationCommandOptionType.String, "The name you want your character to have.", isRequired: true);
            await guild.CreateApplicationCommandAsync(renameCommand.Build());

            //"description" command
            SlashCommandBuilder descriptionCommand = new SlashCommandBuilder()
                .WithName("description")
                .WithDescription("Change the description of your character.")
                .AddOption("description", ApplicationCommandOptionType.String, "Your new character description.", isRequired: true);
            await guild.CreateApplicationCommandAsync(descriptionCommand.Build());

            //"editcharacter" command
            SlashCommandBuilder editcharacterCommand = new SlashCommandBuilder()
                .WithName("editcharacter")
                .WithDescription("ADMIN COMMAND: Modify user data.")
                .AddOption("userid", ApplicationCommandOptionType.String, "If of the user to edit", isRequired: true)
                .AddOption("data", ApplicationCommandOptionType.String, "What data to edit", isRequired: true)
                .AddOption("value", ApplicationCommandOptionType.String, "What to set the data to", isRequired: true);
            await guild.CreateApplicationCommandAsync(editcharacterCommand.Build());

            //"floor" command
            SlashCommandBuilder floorCommand = new SlashCommandBuilder()
                .WithName("floor")
                .WithDescription("Enter a new floor.");
            await guild.CreateApplicationCommandAsync(floorCommand.Build());

            //"coinflip" command
            SlashCommandBuilder coinflipCommand = new SlashCommandBuilder()
                .WithName("coinflip")
                .WithDescription("Bet on a coinflip alone or against another player.")
                .AddOption("wager", ApplicationCommandOptionType.Integer, "How much money to bet.", isRequired: true)
                .AddOption("opponent", ApplicationCommandOptionType.User, "Who to coinflip against.", isRequired: false);
            await guild.CreateApplicationCommandAsync(coinflipCommand.Build());

            //"slots" command
            SlashCommandBuilder slotsCommand = new SlashCommandBuilder()
                .WithName("slots")
                .WithDescription("Wager some money in a slot game.")
                .AddOption("wager", ApplicationCommandOptionType.Integer, "How much money to bet.", isRequired: true);
            await guild.CreateApplicationCommandAsync(slotsCommand.Build());

            //"guide" command
            SlashCommandBuilder guideCommand = new SlashCommandBuilder()
                .WithName("guide")
                .WithDescription("A short guide on how to play the game.");
            await guild.CreateApplicationCommandAsync(guideCommand.Build());

            //"help" command
            SlashCommandBuilder helpCommand = new SlashCommandBuilder()
                .WithName("help")
                .WithDescription("A menu with all the commands and how to use them.");
            await guild.CreateApplicationCommandAsync(helpCommand.Build());

            //"weapon" command
            SlashCommandBuilder weaponCommand = new SlashCommandBuilder()
                .WithName("weapon")
                .WithDescription("View the stats of a weapon.")
                .AddOption("name", ApplicationCommandOptionType.String, "Name of the weapon.", isRequired: true);
            await guild.CreateApplicationCommandAsync(weaponCommand.Build());

            //"armor" command
            SlashCommandBuilder armorCommand = new SlashCommandBuilder()
                .WithName("armor")
                .WithDescription("View the stats of a armor.")
                .AddOption("name", ApplicationCommandOptionType.String, "Name of the armor.", isRequired: true);
            await guild.CreateApplicationCommandAsync(armorCommand.Build());

            //"extra" command
            SlashCommandBuilder extraCommand = new SlashCommandBuilder()
                .WithName("extra")
                .WithDescription("View the stats of a extra.")
                .AddOption("extra", ApplicationCommandOptionType.String, "Name of the extra.", isRequired: true);
            await guild.CreateApplicationCommandAsync(extraCommand.Build());

            //"monster" command
            SlashCommandBuilder monsterCommand = new SlashCommandBuilder()
                .WithName("monster")
                .WithDescription("View the stats of a monster.")
                .AddOption("name", ApplicationCommandOptionType.String, "Name of the monster.", isRequired: true);
            await guild.CreateApplicationCommandAsync(monsterCommand.Build());

            //"lvltop" command
            SlashCommandBuilder lvltopCommand = new SlashCommandBuilder()
                .WithName("lvltop")
                .WithDescription("See the level leaderboards.");
            await guild.CreateApplicationCommandAsync(lvltopCommand.Build());

            //"moneytop" command
            SlashCommandBuilder moneytopCommand = new SlashCommandBuilder()
                .WithName("moneytop")
                .WithDescription("See the money leaderboards.");
            await guild.CreateApplicationCommandAsync(moneytopCommand.Build());

            //"geartop" command
            SlashCommandBuilder geartopCommand = new SlashCommandBuilder()
                .WithName("geartop")
                .WithDescription("See the gear leaderboards.");
            await guild.CreateApplicationCommandAsync(geartopCommand.Build());

            //"servertop" command
            SlashCommandBuilder servertopCommand = new SlashCommandBuilder()
                .WithName("servertop")
                .WithDescription("See the server leaderboards.");
            await guild.CreateApplicationCommandAsync(servertopCommand.Build());

            //"pvp" command
            SlashCommandBuilder pvpCommand = new SlashCommandBuilder()
                .WithName("pvp")
                .WithDescription("Fight against another player for money.")
                .AddOption("wager", ApplicationCommandOptionType.Integer, "How much money to bet.", isRequired: true)
                .AddOption("opponent", ApplicationCommandOptionType.User, "Who to coinflip against.", isRequired: true);
            await guild.CreateApplicationCommandAsync(pvpCommand.Build());

            //"quest" command
            SlashCommandBuilder questCommand = new SlashCommandBuilder()
                .WithName("quest")
                .WithDescription("View your current quest.");
            await guild.CreateApplicationCommandAsync(questCommand.Build());

            //"vote" command
            SlashCommandBuilder voteCommand = new SlashCommandBuilder()
                .WithName("vote")
                .WithDescription("Cliam free rewards!");
            await guild.CreateApplicationCommandAsync(voteCommand.Build());

            //"farm" command
            SlashCommandBuilder farmCommand = new SlashCommandBuilder()
                .WithName("farm")
                .WithDescription("Gather some materials.")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("type")
                    .WithDescription("What to farm?")
                    .WithRequired(true)
                    .AddChoice("🐟 Fishing", "fishing")
                    .AddChoice("🥔 Farming", "farming")
                    .AddChoice("🌲 Foraging", "foraging")
                    .AddChoice("⛏️ Mining", "mining")
                    .AddChoice("🍄 Collecting", "collecting")
                    .WithType(ApplicationCommandOptionType.String)
                    );
            await guild.CreateApplicationCommandAsync(farmCommand.Build());

            //"train" command
            SlashCommandBuilder trainCommand = new SlashCommandBuilder()
                .WithName("train")
                .WithDescription("Train with your gear.")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("type")
                    .WithDescription("What to train?")
                    .WithRequired(false)
                    .AddChoice("🗡 Weapon", "weapon")
                    .AddChoice("🛡 Armor", "armor")
                    .AddChoice("🔮 Extra", "extra")
                    .WithType(ApplicationCommandOptionType.String)
                    );
            await guild.CreateApplicationCommandAsync(trainCommand.Build());


            //"shop" command
            SlashCommandBuilder shopCommand = new SlashCommandBuilder()
                .WithName("shop")
                .WithDescription("Buy new gear from a random weekly selection.");
            await guild.CreateApplicationCommandAsync(shopCommand.Build());


            //#############
            //real commands
            //#############
        }
    }
}
