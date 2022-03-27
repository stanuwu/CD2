using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.IO;
using Discord.Commands;

namespace CD2_Bot
{
    class Program
    {
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        public async Task MainAsync()
        {
            File.Create("log.txt").Close();

            DiscordSocketConfig sconf = new DiscordSocketConfig
            {
                UseInteractionSnowflakeDate = false,
                AlwaysDownloadUsers = true,
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers,

            };

            _client = new DiscordSocketClient(sconf);
            _client.Log += Log;

            CommandServiceConfig cconf = new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                IgnoreExtraArgs = false,
            };
            CommandService _commands = new CommandService(cconf);

            string token = File.ReadAllText("token.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            _client.ButtonExecuted += ButtonHandler.HandleButtonAsync;
            _client.SelectMenuExecuted += SelectionHandler.HandleSelectionAsync;
            _client.JoinedGuild += JoinHandler.OnGuildJoin;
            _client.Connected += Init.Initialize;
            _client.SlashCommandExecuted += CommandHandler.SlashCommandHandler;
            _client.Ready += SlashCommandRegistry.Client_Ready;
            Defaults.CLIENT = _client;
            await Task.Delay(-1);
        }
        public static Task Log(LogMessage msg)
        {
            string txt = $"@{DateTime.Now} [{(msg.Severity.ToString() + "]").PadRight(9, ' ')} {msg.Source.PadRight(15, ' ').Substring(0, 15)} -> {msg.Message}";

            using (var sw = new StreamWriter("log.txt", true))
            {
                sw.WriteLine(txt);
            }
            Console.WriteLine(txt);
            return Task.CompletedTask;
        }
    }
}