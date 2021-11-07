using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace CD2_Bot
{
    static class db
    {
        static string dbpassword = File.ReadAllText("db.txt");

        static NpgsqlConnection dbc = new NpgsqlConnection($"Host=localhost;Username=postgres;Password={dbpassword};Database=CD2");

        static public void Init()
        {
            dbc.Open();
            Utils.DebugLog(CommandString("SELECT version()"));

        }

        static public string CommandString(string cmd)
        {
            return new NpgsqlCommand(cmd, dbc).ExecuteScalar().ToString();
        }


    }
}
