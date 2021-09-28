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

            _client = new DiscordSocketClient();
            _client.Log += Log;

            CommandServiceConfig cconf = new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                IgnoreExtraArgs = false
            };
            CommandService _commands = new CommandService(cconf);

            string token = File.ReadAllText("token.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            CommandHandler commandHandler = new CommandHandler(_client, _commands);
            await commandHandler.InstallCommandsAsync();

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