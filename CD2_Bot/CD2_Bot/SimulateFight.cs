﻿using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    static class SimulateFight
    {
        public static Tuple<Embed,MessageComponent> Sim(Enemy enemy, CharacterStructure stats)
        {
            //set win default
            bool win = false;

            //create copies of weapons to apply effects to
            Weapon tweapon = stats.Weapon.Clone();
            Armor tarmor = stats.Armor.Clone();
            Extra textra = stats.Extra.Clone();

            //class function
            Class.beforeFight(stats, enemy, tweapon, tarmor, textra);

            //apply weapon effects
            double tdmge = tweapon.CustomEffect(stats, enemy, tarmor, textra);
            tdmge += tarmor.CustomEffect(stats, enemy, tweapon, textra);
            tdmge += textra.CustomEffect(stats, enemy, tweapon, tarmor);

            //apply enemy effects
            double edmge = enemy.CustomEffect(stats, tweapon, tarmor, textra);

            //calculate total damage to specific enemy
            double tdamage = Convert.ToDouble((tweapon.Damage + textra.Damage) * (decimal) stats.CharacterClass.DamageMultiplier) * stats.StatMultiplier * ((double)enemy.Resistance/100)+0.01;
            tdamage += tdmge;
            //calculate enemies damage to you with armor
            double edamage = enemy.Damage * ((double)(tarmor.Resistance + stats.CharacterClass.ResistanceBonus) / 100)+0.01;
            edamage += edmge;

            //calculate player rounds to kill
            int tdpr = (int) Math.Ceiling(enemy.HP / tdamage);
            //calculate enemy rounds to kill
            int edpr = (int) Math.Ceiling(stats.HP / edamage);

            //determine win and set hp to get removed to damage recieved and how much damage the enemy took and other statics
            int xgained = 0;
            int mfound = 0;
            int mlost = 0;
            int hptoremove = 0;
            int enemyhp = 0;
            int wxgained = 0;
            if (tdpr < edpr)
            {
                win = true;
                hptoremove = (int) Math.Round((edamage * tdpr));
                enemyhp = (int)Math.Round(enemy.HP - (tdamage * tdpr));
            }
            else
            {
                win = false;
                hptoremove = stats.HP;
                enemyhp = (int) Math.Round(enemy.HP - (tdamage * edpr));
            }

            if (hptoremove >= stats.HP)
            {
                win = false;
            }

            if (win == true) {
                mfound = 50 + enemy.Level * 20;
                xgained = 10 + enemy.Level * 4;
                wxgained = (int)Math.Floor((double)xgained / 2);
                stats.Money += mfound;
                stats.EXP += xgained;
                stats.WeaponXP += wxgained;
                stats.ArmorXP += wxgained;
                stats.ExtraXP += wxgained;
                //class function
                Class.winFight(stats, mfound, xgained, wxgained, enemy);
            } else
            {
                mlost = (100 + stats.Lvl * 20);
                if (stats.Money < mlost)
                {
                    mlost = stats.Money;
                    stats.Money = 0;
                }
                else
                {
                    stats.Money -= mlost;
                }
                Class.looseFight(stats, mlost, enemy);
            }

            //remove hp
            stats.HP -= hptoremove;

            //drops
            KeyValuePair<string, int> drops = new KeyValuePair<string, int> ("none", 0);
            if (enemy.Drops != null && win == true)
            {
                drops = enemy.Drops.getDrops();
            }

            //build room screen
            EmbedBuilder embedB = new EmbedBuilder
            {
                Title = "Fight",
                Description = $"{stats.CharacterName} vs. {enemy.Type}  [LVL {enemy.Level}]"
            };

            embedB.AddField("Your HP", Convert.ToString(stats.HP) + "/" + Convert.ToString(stats.MaxHP), true);
            embedB.AddField("Enemy HP", Convert.ToString(enemyhp) + "/" + Convert.ToString(enemy.HP), true);

            if (win == true)
            {
                //quest trigger
                if (stats.QuestData != "none")
                {
                    stats.Quest.UpdateProgress(stats, QuestActivations.DefeatMonster, enemy);
                }
                
                embedB.WithColor(Color.Green);
                embedB.AddField("Coins Earned", Convert.ToString(mfound));
                embedB.AddField("XP Gained", Convert.ToString(xgained));
                embedB.AddField("Gear XP Gained", Convert.ToString(wxgained));



                if (drops.Key!="none" && drops.Value > 0)
                {
                    Dictionary<string, int> inventory = Utils.InvAsDict(stats);
                    if (inventory.Keys.Any(x => x == drops.Key))
                    {
                        inventory[drops.Key] += drops.Value;
                    } else
                    {
                        inventory.Add(drops.Key, drops.Value);
                    }
                    Utils.SaveInv(stats, inventory);
                    embedB.AddField("Drops", Convert.ToString(drops.Value)+"x "+Convert.ToString(drops.Key));
                }
            } else
            {
                embedB.WithColor(Color.Red);
                embedB.AddField("Coins lost", Convert.ToString(mlost));
            }
            
            embedB.WithFooter(Defaults.FOOTER);

            //generate extra stats button
            ComponentBuilder btnb = new ComponentBuilder().WithButton("Details", $"fightdetails;{tdamage};{edamage};{tdpr};{edpr};" + stats.PlayerID, ButtonStyle.Primary);

            //return room screen
            return new Tuple<Embed, MessageComponent> ( embedB.Build(), btnb.Build());
        }
    }
}
