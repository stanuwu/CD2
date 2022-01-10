using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace CD2_Bot
{
    public class CharacterStructure
    {
        public CharacterStructure(ulong playerID)
        {
            PlayerID = playerID;
            LastFloor = DateTime.MinValue;
            LastTrain = DateTime.MinValue;
            LastFarm = DateTime.MinValue;
        }

        public DateTime LastFloor { get; set; }
        public DateTime LastTrain { get; set; }

        public DateTime LastFarm { get; set; }

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
                NpgsqlCommand cmd2 = new NpgsqlCommand("SELECT lastheal FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd2.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                DateTime lastheal = DateTime.Parse(db.CommandString(cmd2));

                if ((DateTime.Now - lastheal).TotalSeconds > 60)
                {
                    NpgsqlCommand cmd3 = new NpgsqlCommand("UPDATE public.\"Character\" SET lastheal = @heal WHERE \"UserID\" = @id;", db.dbc);
                    cmd3.Parameters.AddWithValue("@heal", DateTime.Now.ToString());
                    cmd3.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                    db.CommandVoid(cmd3);

                    int regen = (int)(DateTime.Now - lastheal).TotalMinutes * ((MaxHP / 20) + this.Extra.Heal);

                    if(this.HP+regen > this.MaxHP)
                    {
                        this.HP = this.MaxHP;
                    }
                    else
                    {
                        this.HP += regen;
                    }
                }

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT currhp FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                return Convert.ToInt32(db.CommandString(cmd));
            } 
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET currhp = @hp WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@hp", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
            }
        }
        public Weapon Weapon {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT weapon FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                NpgsqlCommand cmd2 = new NpgsqlCommand("SELECT weaponxp FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd2.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                Weapon thisweapon = (from w in Gear.Weapons
                                        where w.Name == db.CommandString(cmd)
                                        select w).SingleOrDefault().Clone();
                thisweapon.EXP = Convert.ToInt32(db.CommandString(cmd2));
                return thisweapon;
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET weapon = @weapon WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@weapon", value.Name);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
                NpgsqlCommand cmd2 = new NpgsqlCommand("UPDATE public.\"Character\" SET weaponxp = @weaponxp WHERE \"UserID\" = @id;", db.dbc);
                cmd2.Parameters.AddWithValue("@weaponxp", value.EXP);
                cmd2.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd2);
            }
        }
        public Armor Armor {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT armor FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                NpgsqlCommand cmd2 = new NpgsqlCommand("SELECT armorxp FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd2.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                Armor thisarmor = (from w in Gear.Armors
                                     where w.Name == db.CommandString(cmd)
                                     select w).SingleOrDefault().Clone();
                thisarmor.EXP = Convert.ToInt32(db.CommandString(cmd2));
                return thisarmor;
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET armor = @armor WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@armor", value.Name);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
                NpgsqlCommand cmd2 = new NpgsqlCommand("UPDATE public.\"Character\" SET armorxp = @armorxp WHERE \"UserID\" = @id;", db.dbc);
                cmd2.Parameters.AddWithValue("@armorxp", value.EXP);
                cmd2.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd2);
            }
        }
        public Extra Extra {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT extra FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                NpgsqlCommand cmd2 = new NpgsqlCommand("SELECT extraxp FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd2.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                Extra thisextra = (from w in Gear.Extras
                                     where w.Name == db.CommandString(cmd)
                                     select w).SingleOrDefault().Clone();
                thisextra.EXP = Convert.ToInt32(db.CommandString(cmd2));
                return thisextra;
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET extra = @weapon WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@weapon", value.Name);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
                NpgsqlCommand cmd2 = new NpgsqlCommand("UPDATE public.\"Character\" SET extraxp = @extraxp WHERE \"UserID\" = @id;", db.dbc);
                cmd2.Parameters.AddWithValue("@extraxp", value.EXP);
                cmd2.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd2);
            }
        }

        public int WeaponXP
        {
            get
            {
                NpgsqlCommand cmd2 = new NpgsqlCommand("SELECT weaponxp FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd2.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                return Convert.ToInt32(db.CommandString(cmd2));
            }
            set
            {
                NpgsqlCommand cmd2 = new NpgsqlCommand("UPDATE public.\"Character\" SET weaponxp = @xp WHERE \"UserID\" = @id;", db.dbc);
                cmd2.Parameters.AddWithValue("@xp", value);
                cmd2.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd2);
            }
        }

        public int ArmorXP
        {
            get
            {
                NpgsqlCommand cmd2 = new NpgsqlCommand("SELECT armorxp FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd2.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                return Convert.ToInt32(db.CommandString(cmd2));
            }
            set
            {
                NpgsqlCommand cmd2 = new NpgsqlCommand("UPDATE public.\"Character\" SET armorxp = @xp WHERE \"UserID\" = @id;", db.dbc);
                cmd2.Parameters.AddWithValue("@xp", value);
                cmd2.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd2);
            }
        }

        public int ExtraXP
        {
            get
            {
                NpgsqlCommand cmd2 = new NpgsqlCommand("SELECT extraxp FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd2.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                return Convert.ToInt32(db.CommandString(cmd2));
            }
            set
            {
                NpgsqlCommand cmd2 = new NpgsqlCommand("UPDATE public.\"Character\" SET extraxp = @xp WHERE \"UserID\" = @id;", db.dbc);
                cmd2.Parameters.AddWithValue("@xp", value);
                cmd2.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd2);
            }
        }

        public string Inventory {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT inventory FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                string output = db.CommandString(cmd);
                return output;
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET inventory = ARRAY [" + value + "]" + " WHERE \"UserID\" = @id;", db.dbc);
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

        public bool Deleted
        {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT deleted FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                return Convert.ToBoolean(db.CommandString(cmd));
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET deleted = @deleted WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@deleted", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
            }
        }

        public string QuestData
        {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT quest FROM public.\"Character\" WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                return Convert.ToString(db.CommandString(cmd));
            }
            set
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.\"Character\" SET quest = @quest WHERE \"UserID\" = @id;", db.dbc);
                cmd.Parameters.AddWithValue("@quest", value);
                cmd.Parameters.AddWithValue("@id", (Int64)this.PlayerID);
                db.CommandVoid(cmd);
            }
        }

        public Quest Quest
        {
            get
            {
                return Quests.WhatQuest(this.QuestData);
            }
        }
        public ulong PlayerID { get; private set; }
    }
}