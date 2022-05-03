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

            try {

                List<Task> commands = new List<Task>() { };

                //"test" command
                SocketGuild guild = Defaults.CLIENT.GetGuild(Defaults.TESTGUILDID);
                SlashCommandBuilder testCommand = new SlashCommandBuilder();
                testCommand.WithName("test")
                    .WithDescription("Admin Command: Test commands.");
                commands.Add(guild.CreateApplicationCommandAsync(testCommand.Build()));


                //"guilds" command
                SlashCommandBuilder guildsCommand = new SlashCommandBuilder();
                guildCommand.WithName("guilds")
                    .WithDescription("Admin Command: Get the amount of guilds the bot is in.");
                commands.Add(guild.CreateApplicationCommandAsync(guildsCommand.Build()));


                //"users" command
                SlashCommandBuilder usersCommand = new SlashCommandBuilder();
                usersCommand.WithName("users")
                    .WithDescription("Admin Command: Get the amount of unique users the bot has access to.");
                commands.Add(guild.CreateApplicationCommandAsync(usersCommand.Build()));


                //"reload" command
                SlashCommandBuilder reloadCommand = new SlashCommandBuilder();
                reloadCommand.WithName("reload")
                    .WithDescription("Admin Command: Reload the bot.");
                commands.Add(guild.CreateApplicationCommandAsync(reloadCommand.Build()));


                //"status" command
                SlashCommandBuilder statusCommand = new SlashCommandBuilder();
                statusCommand.WithName("status")
                    .WithDescription("Admin Command: Change the status of the bot.")
                    .AddOption("utatus", ApplicationCommandOptionType.String, "The status to set.", isRequired: true);
                commands.Add(guild.CreateApplicationCommandAsync(statusCommand.Build()));


                //"broadcast" command
                SlashCommandBuilder broadcastCommand = new SlashCommandBuilder();
                broadcastCommand.WithName("broadcast")
                    .WithDescription("Admin Command: Send a message to all servers the bot is in.")
                    .AddOption("message", ApplicationCommandOptionType.String, "The message to broadcast.", isRequired: true);
                commands.Add(guild.CreateApplicationCommandAsync(broadcastCommand.Build()));



                //#############
                //real commands
                //#############

                //"character" command
                SlashCommandBuilder characterCommand = new SlashCommandBuilder()
                    .WithName("character")
                    .WithDescription("Look at your own or someone elses character.")
                    .AddOption("user", ApplicationCommandOptionType.User, "The user that you want to view the character of.", isRequired: false);
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(characterCommand.Build()));

                //"start" command
                SlashCommandBuilder startCommand = new SlashCommandBuilder()
                    .WithName("start")
                    .WithDescription("Create a character to start your journey")
                    .AddOption("name", ApplicationCommandOptionType.String, "The name you want your character to have.", isRequired: true);
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(startCommand.Build()));

                //"stats" command
                SlashCommandBuilder statsCommand = new SlashCommandBuilder()
                    .WithName("stats")
                    .WithDescription("Have a look at your or someone elses gear.")
                    .AddOption("user", ApplicationCommandOptionType.User, "The user that you want to view the character of.", isRequired: false);
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(statsCommand.Build()));

                //"server" command
                SlashCommandBuilder serverCommand = new SlashCommandBuilder()
                    .WithName("server")
                    .WithDescription("View the stats of the server you are currently in.");
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(serverCommand.Build()));

                //"inventory" command
                SlashCommandBuilder inventoryCommand = new SlashCommandBuilder()
                    .WithName("inventory")
                    .WithDescription("View your inventory.");
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(inventoryCommand.Build()));

                //"reset" command
                SlashCommandBuilder resetCommand = new SlashCommandBuilder()
                    .WithName("reset")
                    .WithDescription("Delete your character.");
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(resetCommand.Build()));

                //"rename" command
                SlashCommandBuilder renameCommand = new SlashCommandBuilder()
                    .WithName("rename")
                    .WithDescription("Change the name of your character.")
                    .AddOption("name", ApplicationCommandOptionType.String, "The name you want your character to have.", isRequired: true);
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(renameCommand.Build()));

                //"description" command
                SlashCommandBuilder descriptionCommand = new SlashCommandBuilder()
                    .WithName("description")
                    .WithDescription("Change the description of your character.")
                    .AddOption("description", ApplicationCommandOptionType.String, "Your new character description.", isRequired: true);
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(descriptionCommand.Build()));

                //"editcharacter" command
                SlashCommandBuilder editcharacterCommand = new SlashCommandBuilder()
                    .WithName("editcharacter")
                    .WithDescription("ADMIN COMMAND: Modify user data.")
                    .AddOption("userid", ApplicationCommandOptionType.String, "If of the user to edit", isRequired: true)
                    .AddOption("data", ApplicationCommandOptionType.String, "What data to edit", isRequired: true)
                    .AddOption("value", ApplicationCommandOptionType.String, "What to set the data to", isRequired: true);
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(editcharacterCommand.Build()));

                //"floor" command
                SlashCommandBuilder floorCommand = new SlashCommandBuilder()
                    .WithName("floor")
                    .WithDescription("Enter a new floor.");
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(floorCommand.Build()));

                //"coinflip" command
                SlashCommandBuilder coinflipCommand = new SlashCommandBuilder()
                    .WithName("coinflip")
                    .WithDescription("Bet on a coinflip alone or against another player.")
                    .AddOption("wager", ApplicationCommandOptionType.Integer, "How much money to bet.", isRequired: true)
                    .AddOption("opponent", ApplicationCommandOptionType.User, "Who to coinflip against.", isRequired: false);
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(coinflipCommand.Build()));

                //"slots" command
                SlashCommandBuilder slotsCommand = new SlashCommandBuilder()
                    .WithName("slots")
                    .WithDescription("Wager some money in a slot game.")
                    .AddOption("wager", ApplicationCommandOptionType.Integer, "How much money to bet.", isRequired: true);
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(slotsCommand.Build()));

                //"guide" command
                SlashCommandBuilder guideCommand = new SlashCommandBuilder()
                    .WithName("guide")
                    .WithDescription("A short guide on how to play the game.");
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(guideCommand.Build()));

                //"help" command
                SlashCommandBuilder helpCommand = new SlashCommandBuilder()
                    .WithName("help")
                    .WithDescription("A menu with all the commands and how to use them.");
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(helpCommand.Build()));

                //"weapon" command
                SlashCommandBuilder weaponCommand = new SlashCommandBuilder()
                    .WithName("weapon")
                    .WithDescription("View the stats of a weapon.")
                    .AddOption("name", ApplicationCommandOptionType.String, "Name of the weapon.", isRequired: true);
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(weaponCommand.Build()));

                //"armor" command
                SlashCommandBuilder armorCommand = new SlashCommandBuilder()
                    .WithName("armor")
                    .WithDescription("View the stats of a armor.")
                    .AddOption("name", ApplicationCommandOptionType.String, "Name of the armor.", isRequired: true);
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(armorCommand.Build()));

                //"extra" command
                SlashCommandBuilder extraCommand = new SlashCommandBuilder()
                    .WithName("extra")
                    .WithDescription("View the stats of a extra.")
                    .AddOption("extra", ApplicationCommandOptionType.String, "Name of the extra.", isRequired: true);
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(extraCommand.Build()));

                //"monster" command
                SlashCommandBuilder monsterCommand = new SlashCommandBuilder()
                    .WithName("monster")
                    .WithDescription("View the stats of a monster.")
                    .AddOption("name", ApplicationCommandOptionType.String, "Name of the monster.", isRequired: true);
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(monsterCommand.Build()));

                //"lvltop" command
                SlashCommandBuilder lvltopCommand = new SlashCommandBuilder()
                    .WithName("lvltop")
                    .WithDescription("See the level leaderboards.");
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(lvltopCommand.Build()));

                //"moneytop" command
                SlashCommandBuilder moneytopCommand = new SlashCommandBuilder()
                    .WithName("moneytop")
                    .WithDescription("See the money leaderboards.");
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(moneytopCommand.Build()));

                //"geartop" command
                SlashCommandBuilder geartopCommand = new SlashCommandBuilder()
                    .WithName("geartop")
                    .WithDescription("See the gear leaderboards.");
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(geartopCommand.Build()));

                //"servertop" command
                SlashCommandBuilder servertopCommand = new SlashCommandBuilder()
                    .WithName("servertop")
                    .WithDescription("See the server leaderboards.");
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(servertopCommand.Build()));

                //"pvp" command
                SlashCommandBuilder pvpCommand = new SlashCommandBuilder()
                    .WithName("pvp")
                    .WithDescription("Fight against another player for money.")
                    .AddOption("wager", ApplicationCommandOptionType.Integer, "How much money to bet.", isRequired: true)
                    .AddOption("opponent", ApplicationCommandOptionType.User, "Who to coinflip against.", isRequired: true);
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(pvpCommand.Build()));

                //"quest" command
                SlashCommandBuilder questCommand = new SlashCommandBuilder()
                    .WithName("quest")
                    .WithDescription("View your current quest.");
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(questCommand.Build()));

                //"vote" command
                SlashCommandBuilder voteCommand = new SlashCommandBuilder()
                    .WithName("vote")
                    .WithDescription("Cliam free rewards!");
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(voteCommand.Build()));

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
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(farmCommand.Build()));

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
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(trainCommand.Build()));


                //"shop" command
                SlashCommandBuilder shopCommand = new SlashCommandBuilder()
                    .WithName("shop")
                    .WithDescription("Buy new gear from a random weekly selection.");
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(shopCommand.Build()));


                //"class" command
                SlashCommandBuilder classCommand = new SlashCommandBuilder()
                    .WithName("class")
                    .WithDescription("Search classes by name.")
                    .AddOption("name", ApplicationCommandOptionType.String, "Name of the class.", isRequired: true);
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(classCommand.Build()));


                //"setclass" command
                SlashCommandBuilder setclassCommand = new SlashCommandBuilder()
                    .WithName("setclass")
                    .WithDescription("Set your class.");
                SlashCommandOptionBuilder setclassOptions = new SlashCommandOptionBuilder()
                        .WithName("class")
                        .WithDescription("What class do you want to use?")
                        .WithRequired(true)
                        .WithType(ApplicationCommandOptionType.String);
                foreach (Class c in Class.Classes)
                {
                    setclassOptions.AddChoice($"{c.Name} (Lvl. {c.Level})", c.Name);
                }
                setclassCommand.AddOption(setclassOptions);
                commands.Add(Defaults.CLIENT.CreateGlobalApplicationCommandAsync(setclassCommand.Build()));

                await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Info, "Command Registry", "Registering Commands!"));

                await Task.WhenAll(commands);

                await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Info, "Command Registry", "Finished registering Commands!"));
            }
            catch (Exception e) {
                await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Error, "Command Registry", e.Message));
                await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Error, "Command Registry", e.StackTrace));
            }
        }
    }
}
