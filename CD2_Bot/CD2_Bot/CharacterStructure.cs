﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace CD2_Bot
{
    class CharacterStructure
    {
        public CharacterStructure(ulong playerID)
        {
            PlayerID = playerID;
            LastFloor = DateTime.MinValue;
        }

        public DateTime LastFloor { get; set; }

        public string CharacterName {
            get {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT name FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64) this.PlayerID);
                return db.CommandString(cmd);
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET name = @name WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@name", value);
                cmd.Parameters.AddWithValue("@id", (Int64) this.PlayerID);
                db.CommandVoid(cmd);
            }
        }
        public int Lvl
        {
            get
            {
                return (int)Math.Floor(Math.Sqrt(EXP / 10) / 2);
            }
        }
        public string Title {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT title FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                return db.CommandString(cmd);
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET title = @title WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@title", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
            }
        }
        public string Description {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT description FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                return db.CommandString(cmd);
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET description = @description WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@description", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
            }
        }
        public string CharacterClass
        {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT class FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                return db.CommandString(cmd);
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET class = @class WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@class", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
            }
        }
        public int Money 
        {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT money FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                return Convert.ToInt32(db.CommandString(cmd));
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET money = @money WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@money", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
            }
        }
        public int EXP {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT exp FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                return Convert.ToInt32(db.CommandString(cmd));
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET exp = @exp WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@exp", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
            }
        }
        public int MaxHP {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT hp FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                int hpadded = 0;
                if( Lvl > 50)
                {
                    hpadded = 983 + 5 * (Lvl - 50); // 983 ist die HP-Anzahl, die bei Level 50 erreicht wird.
                }
                else
                {
                    hpadded = (int)Math.Floor(100 + 0.05 * Math.Pow(Lvl, 2.5));
                }
                return Convert.ToInt32(db.CommandString(cmd)) + hpadded;
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET hp = @hp WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@hp", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
            }
        }
        public int HP
        {
            get
            {
                return MaxHP-50; //Muss noch implementiert werden ::::((((
            }
        }
        public string Weapon {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT weapon FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                return db.CommandString(cmd);
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET weapon = @weapon WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@weapon", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
            }
        }
        public string Armor {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT armor FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                return db.CommandString(cmd);
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET armor = @armor WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@armor", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
            }
        }
        public string Extra {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT extra FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                return db.CommandString(cmd);
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET extra = @extra WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@extra", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
            }
        }
        public Dictionary<string, int> Inventory {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT inventory FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                string output = db.CommandString(cmd);
                Dictionary<string, int> dinv = new Dictionary<string, int> { };
                if (!String.IsNullOrEmpty(output))
                {
                    foreach (string s in output.Split(','))
                    {
                        dinv[s.Split(';')[0]] = Convert.ToInt32(s.Split(';')[1]);
                    }
                }
                return dinv;
            }
            set
            {
                string dv = "";
                foreach(string s in value.Keys)
                {
                    dv += "'" + s + ";" + Convert.ToString(value[s]) + "',";
                }
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET inventory = " + "ARRAY["+dv+"]" + " WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
            }
        }
        public double StatMultiplier {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT statmultiplier FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                return Convert.ToDouble(db.CommandString(cmd)) + 1 + 0.01*Lvl;
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET statmultiplier = @statmultiplier WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@statmultiplier", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
            }
        }
        public ulong PlayerID { get; private set; }
    }
}