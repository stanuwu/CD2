using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    static class SimulateFight
    {
        public static Embed Sim(Enemy enemy, CharacterStructure stats)
        {
            //set win default
            bool win = false;

            //create copies of weapons to apply effects to
            Weapon tweapon = stats.Weapon.Clone();
            Armor tarmor = stats.Armor.Clone();
            Extra textra = stats.Extra.Clone();

            //apply weapon effects
            tweapon.CustomEffect(stats.PlayerID, enemy);
            tarmor.CustomEffect(stats.PlayerID, enemy);
            textra.CustomEffect(stats.PlayerID, enemy);

            //calculate total damage to specific enemy
            double tdamage = Convert.ToDouble(tweapon.Damage + textra.Damage) * stats.StatMultiplier * ((double)enemy.Resistance/100)+0.01d;
            //calculate enemies damage to you with armor
            double edamage = enemy.Damage * (tarmor.Resistance / 100)+0.01d;

            //calculate player rounds to kill
            int tdpr = (int) Math.Floor(enemy.HP / tdamage);
            //calculate enemy rounds to kill
            int edpr = (int) Math.Floor(stats.HP / edamage);

            //prevent negative damage bug (hopefully)
            if (tdpr < 1) {
                tdpr = 1;
            }
            if (edpr > 1) {
                edpr = 1;
            }

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
                hptoremove = (int) Math.Floor(stats.HP - (edamage * tdpr));
                mfound = 200 + enemy.Level * 50;
                xgained = 50 + enemy.Level * 5;
                wxgained = (int)Math.Floor((double)xgained / 4);
                stats.Money += mfound;
                stats.EXP += xgained;
                stats.WeaponXP += wxgained;
                stats.ArmorXP += wxgained;
                stats.ExtraXP += wxgained;
            }
            else
            {
                win = false;
                hptoremove = stats.HP;
                enemyhp = (int) Math.Floor(enemy.HP - (tdamage * edpr));
                mlost = (500 + stats.Lvl * 50);
                if (stats.Money < mlost)
                {
                    mlost = stats.Money;
                    stats.Money = 0;
                } else
                {
                    stats.Money -= mlost;
                }
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

            //return room screen
            return embedB.Build();
        }
    }
}
