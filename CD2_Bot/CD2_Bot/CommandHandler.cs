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
    public class CommandHandler
    {
        public readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly char commandPrefix = Defaults.dPrefix;

        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            _commands = commands;
            _client = client;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
            await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Info, "Command Handler", "Loaded Commands"));
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            if (!(message.HasCharPrefix(commandPrefix, ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot || message.HasStringPrefix("<@", ref argPos))
                return;

            await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Info, "Command Handler", $"Running Command: \"{message}\""));

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);
        }
    }
}
