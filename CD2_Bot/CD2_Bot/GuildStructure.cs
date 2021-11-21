using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    class GuildStructure
    {
        public GuildStructure(ulong guildID)
        {
            GuildID = guildID;
        }

        public ulong GuildID { get; set; }

        public int BossesSlain
        {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT bossesslain FROM public.\"Server\" WHERE \"ServerID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.GuildID);
                return Convert.ToInt32(db.CommandString(cmd));
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Server\" SET bossesslain = @bosses WHERE \"ServerID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@bosses", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.GuildID);
                db.CommandVoid(cmd);
            }
        }

        public int DoorsOpened
        {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT doorskickedin FROM public.\"Server\" WHERE \"ServerID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.GuildID);
                return Convert.ToInt32(db.CommandString(cmd));
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Server\" SET doorskickedin = @doors WHERE \"ServerID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@doors", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.GuildID);
                db.CommandVoid(cmd);
            }
        }

        public int QuestsFinished
        {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT questsfinished FROM public.\"Server\" WHERE \"ServerID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.GuildID);
                return Convert.ToInt32(db.CommandString(cmd));
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Server\" SET questsfinished = @quests WHERE \"ServerID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@quests", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.GuildID);
                db.CommandVoid(cmd);
            }
        }
    }
}
