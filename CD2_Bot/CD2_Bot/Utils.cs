using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Npgsql;

namespace CD2_Bot
{
    static class Utils
    {
        public static async void DebugLog(string msg)
        {
            await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Debug, "Debugging", msg));
        }

        public static async Task<int> GuildCount()
        {
            IReadOnlyCollection<IGuild> guilds = await ((IDiscordClient) Defaults.CLIENT).GetGuildsAsync();
            return guilds.Count;

        }

        public static async Task<int> UniqueUserCount()
        {
            IReadOnlyCollection<Discord.IGuild> guilds = await ((IDiscordClient)Defaults.CLIENT).GetGuildsAsync();
            List<IUser> uusers = new List<IUser> { };

            foreach (IGuild g in guilds)
            {
                IReadOnlyCollection<Discord.IGuildUser> tusers = await g.GetUsersAsync();
                foreach (IGuildUser gu in tusers)
                {
                    if (!uusers.Any(x=> x.Id==gu.Id))
                    {
                        uusers.Add(gu);
                    }
                }
            }

            return uusers.Count;
        }

        public static async void RestartClient(ICommandContext ctx)
        {
            await ctx.Client.StopAsync();
            await ctx.Client.StartAsync();
        }

        public static async void UpdateStatus(string msg)
        {
            await Defaults.CLIENT.SetGameAsync(msg);
        }

        public static async void SendBroadcast(ICommandContext ctx, string msg)
        {
            IReadOnlyCollection<Discord.IGuild> guilds = await ctx.Client.GetGuildsAsync();
            foreach (IGuild g in guilds)
            {
                ITextChannel channel = await g.GetDefaultChannelAsync();
                await channel.SendMessageAsync(msg);
            }

        }

        public static Embed QuickEmbedNormal(string title, string description)
        {
            var embed = new EmbedBuilder
            {
                Title = title,
                Description = description
            };

            embed.WithColor(Color.Teal);
            embed.WithFooter(Defaults.FOOTER);

            return embed.Build();
        }

        public static Embed QuickEmbedError(string description)
        {
            var embed = new EmbedBuilder
            {
                Title = "Error",
                Description = description
            };

            embed.WithColor(Color.DarkRed);
            embed.WithFooter(Defaults.FOOTER);

            return embed.Build();
        }

        public static Embed QuickEmbedBotinfo()
        {
            var embed = new EmbedBuilder
            {
                Title = "CustomDungeons 2 - Info",
                Description = "This bot.",
                ThumbnailUrl = Defaults.BOTIMG
            };

            embed.WithColor(Color.Purple);
            embed.WithFooter(Defaults.FOOTER);

            return embed.Build();
        }
        public static Embed QuickEmbedMenu(Dictionary<string, string> menuitems)
        {
            var embed = new EmbedBuilder
            {
                Title = "Menu",
                Description = "Basic menu"
            };

            foreach (string k in menuitems.Keys)
            {
                embed.AddField(k, menuitems[k], true);
            }

            embed.WithColor(Color.Gold);
            embed.WithFooter(Defaults.FOOTER);

            return embed.Build();
        }

        public static Embed QuickEmbedList(List<string> listitems)
        {
            var embed = new EmbedBuilder
            {
                Title = "List"
            };

            foreach (string l in listitems)
            {
                embed.Description += $"- {l}\n";
            }

            embed.WithColor(Color.Magenta);
            embed.WithFooter(Defaults.FOOTER);

            return embed.Build();
        }

        public static Dictionary<string,int> InvAsDict(CharacterStructure stats)
        {
            Dictionary<string, int> dinv = new Dictionary<string, int> { };
            if (!String.IsNullOrEmpty(stats.Inventory))
            {
                foreach (string s in stats.Inventory.Split(','))
                {
                    dinv[s.Split(';')[0]] = Convert.ToInt32(s.Split(';')[1]);
                }
            }
            return dinv;
        }
        public static void SaveInv(CharacterStructure stats, Dictionary<string, int> dinv)
        {
            string dv = "";
            foreach (string s in dinv.Keys)
            {
                dv += "'" + s + ";" + Convert.ToString(dinv[s]) + "',";
            }
            dv = dv.Remove(dv.Length - 1);
            stats.Inventory = dv;
        }

        public static void GuildToDB(SocketGuild guild)
        {
            if (!tempstorage.guilds.Any(x => x.GuildID == guild.Id))
            {
                NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO public.\"Server\" VALUES (@id, 0, 0, 0);", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)guild.Id);
                db.CommandVoid(cmd);
                tempstorage.guilds.Add(new GuildStructure(guild.Id));
            }
        }

        public static async void SendFileAsyncFast(SocketSlashCommand cmd, byte[] ms, string filename)
        {
            MemoryStream ms2 = new MemoryStream(ms);
            ms2.Position = 0;
            await cmd.RespondWithFileAsync(ms2, filename, "", null, false, false, null, null, null, null);
        }
    }
}