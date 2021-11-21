using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Npgsql;

namespace CD2_Bot
{
    static class db
    {
        static string dbpassword = File.ReadAllText("db.txt");

        static public NpgsqlConnection dbc = new NpgsqlConnection($"Host=localhost;Username=postgres;Password={dbpassword};Database=CD2;Pooling=True;Minimum Pool Size=0;Maximum Pool Size=1000;Timeout=60;");

        static public async void Init()
        {
            dbc.Open();
            await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Info, "Database", "Loaded Database: " + CommandString(new NpgsqlCommand("SELECT version()", dbc))));

            using (NpgsqlDataReader ids = new NpgsqlCommand("SELECT \"UserID\" from Public.\"Character\"", dbc).ExecuteReader())
            {
                while (ids.Read())
                {
                    tempstorage.characters.Add(new CharacterStructure((ulong) ids.GetInt64(0)));
                }
                await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Info, "Database", "Loaded Characters from DB"));
            }

            using (NpgsqlDataReader gids = new NpgsqlCommand("SELECT \"ServerID\" from Public.\"Server\"", dbc).ExecuteReader())
            {
                while (gids.Read())
                {
                    tempstorage.guilds.Add(new GuildStructure((ulong)gids.GetInt64(0)));
                }
                await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Info, "Database", "Loaded Guilds from DB"));
            }

            foreach (IGuild g in await ((IDiscordClient)Defaults.CLIENT).GetGuildsAsync())
            {
                Utils.GuildToDB((SocketGuild)g);
            }
            await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Info, "Database", "Added new guilds to DB"));
        }

        static public string CommandString(NpgsqlCommand cmd)
        {
            var dbout = cmd.ExecuteScalar();
            string returns;
            if (dbout.GetType() == typeof(string[]))
            {
                returns = String.Join(",",(string[]) dbout);
            } else
            {
                returns = dbout.ToString();
            }
            return returns;
        }

        static public void CommandVoid(NpgsqlCommand cmd)
        {
            cmd.ExecuteNonQuery();
        }
    }
}