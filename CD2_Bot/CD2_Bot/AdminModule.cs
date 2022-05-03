using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace CD2_Bot
{
    public static class AdminModule
    {
        //"guilds" command
        public static async Task GuildsAsync(SocketSlashCommand cmd)
        {
            if (!Defaults.STAFF.Contains(cmd.User.Id)) { return; }
            await cmd.RespondAsync(embed: Utils.QuickEmbedNormal("Guilds", Convert.ToString(await Utils.GuildCount())));
        }

        //"users" commands
        public static async Task UsersAsync(SocketSlashCommand cmd)
        {
            if (!Defaults.STAFF.Contains(cmd.User.Id)) { return; }
            string text = "";
            if (Defaults.UUSERS == 0)
            {
                text = "Calculating...";
            } else
            {
                text = "" + Defaults.UUSERS;
            }
            await cmd.RespondAsync(embed: Utils.QuickEmbedNormal("Users", text));
        }

        //"reload" command
        public static async Task ReloadAsync(SocketSlashCommand cmd)
        {
            if (!Defaults.STAFF.Contains(cmd.User.Id)) { return; }
            Utils.RestartClient();
            await cmd.RespondAsync(embed: Utils.QuickEmbedNormal("Reloaded!", ""));
        }

        //"status" command
        public static async Task StatusAsync(SocketSlashCommand cmd)
        {
            if (!Defaults.STAFF.Contains(cmd.User.Id)) { return; }
            if (cmd.Data.Options.Count < 1)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("Provide a status to set!"));
            }
            else
            {
                string status = (string)cmd.Data.Options.First().Value;
                Utils.UpdateStatus(status);
                await cmd.RespondAsync(embed: Utils.QuickEmbedNormal("Status updated!", status));
            }
        }

        //"broadcast" command
        public static async Task BroadcastAsync(SocketSlashCommand cmd)
        {
            if (!Defaults.STAFF.Contains(cmd.User.Id)) { return; }
            if (cmd.Data.Options.Count < 1)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("Enter a message to send out!"));
            }
            else
            {
                string broadcast = (string)cmd.Data.Options.First().Value;
                Utils.SendBroadcast(broadcast);
                await cmd.RespondAsync(embed: Utils.QuickEmbedNormal("Broadcasted!", broadcast));
            }
        }
    }
}