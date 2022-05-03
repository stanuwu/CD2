using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace CD2_Bot
{
    public static class CommandHandler
    {
        public static async Task SlashCommandHandler(SocketSlashCommand command)
        {
            await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Info, "Slash Command Handler", $"Running Command: \"{command.CommandName}\""));

            try
            {
                if (command.IsDMInteraction)
                {
                    await command.RespondAsync("This bot can not be used in direct messages, please invite it to a server!");
                    return;
                }

                switch (command.CommandName)
                {
                    case "character":
                        await CharacterManagement.CharacterAsync(command);
                        break;
                    case "start":
                        await CharacterManagement.StartAsync(command);
                        break;
                    case "stats":
                        await CharacterManagement.StatsAsync(command);
                        break;
                    case "server":
                        await CharacterManagement.ServerAsync(command);
                        break;
                    case "inventory":
                        await CharacterManagement.InventoryAsync(command);
                        break;
                    case "reset":
                        await CharacterManagement.ResetAsync(command);
                        break;
                    case "rename":
                        await CharacterManagement.RenameAsync(command);
                        break;
                    case "description":
                        await CharacterManagement.DescriptionAsync(command);
                        break;
                    case "editcharacter":
                        await CharacterManagement.EditcharacterAsync(command);
                        break;
                    case "floor":
                        await Floor.FloorAsync(command);
                        break;
                    case "coinflip":
                        await Gambling.CoinflipAsync(command);
                        break;
                    case "slots":
                        await Gambling.SlotsAsync(command);
                        break;
                    case "guide":
                        await Guide.GuideAsync(command);
                        break;
                    case "help":
                        await Help.HelpAsync(command);
                        break;
                    case "weapon":
                        await MiscStats.ViewWeaponAsync(command);
                        break;
                    case "armor":
                        await MiscStats.ViewArmorAsync(command);
                        break;
                    case "extra":
                        await MiscStats.ViewExtraAsync(command);
                        break;
                    case "monster":
                        await MiscStats.ViewMonsterAsync(command);
                        break;
                    case "lvltop":
                        await MiscStats.ServerTopAsync(command);
                        break;
                    case "moneytop":
                        await MiscStats.MoneyTopAsync(command);
                        break;
                    case "geartop":
                        await MiscStats.GearTopAsync(command);
                        break;
                    case "servertop":
                        await MiscStats.ServerTopAsync(command);
                        break;
                    case "pvp":
                        await PlayerCommands.PlayerFightRequestAsync(command);
                        break;
                    case "quest":
                        await PlayerCommands.PlayerViewQuestAsync(command);
                        break;
                    case "vote":
                        await PlayerCommands.VoteAsync(command);
                        break;
                    case "farm":
                        await Farm.FarmAsync(command);
                        break;
                    case "train":
                        await Train.TrainAsync(command);
                        break;
                    case "shop":
                        await Shop.ShopAsync(command);
                        break;
                    case "class":
                        await MiscStats.ViewClassAsync(command);
                        break;
                    case "setclass":
                        await Class.SetClassAsync(command);
                        break;
                    case "guilds":
                        await AdminModule.GuildsAsync(command);
                        break;
                    case "users":
                        await AdminModule.UsersAsync(command);
                        break;
                    case "reload":
                        await AdminModule.ReloadAsync(command);
                        break;
                    case "status":
                        await AdminModule.StatusAsync(command);
                        break;
                    case "broadcast":
                        await AdminModule.BroadcastAsync(command);
                        break;
                    default:
                        await command.RespondAsync($"The command {command.CommandName} has not been created or linked.");
                        break;
                }
            }

            catch (Exception e)
            {
                await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Error, "Slash Command Handler", e.Message));
                await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Error, "Slash Command Handler", e.StackTrace));
            }
        }
    }
}
