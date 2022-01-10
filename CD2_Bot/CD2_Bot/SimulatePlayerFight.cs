using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    static class SimulatePlayerFight
    {
        public static Tuple<Embed, MessageComponent> Sim(CharacterStructure p1, CharacterStructure p2, int wager)
        {
            //if player one wins
            bool win = false;

            //create copies of weapons to apply effects to
            Weapon p1weapon = p1.Weapon.Clone();
            Armor p1armor = p1.Armor.Clone();
            Extra p1extra = p1.Extra.Clone();
            Weapon p2weapon = p2.Weapon.Clone();
            Armor p2armor = p2.Armor.Clone();
            Extra p2extra = p2.Extra.Clone();

           //custom effects currently not supported because:
           //money or xp generating effects could be abused
           //current implementation of custom effects are not meant to be used on players

            //calculate total damage to specific enemy
            double p1damage = Convert.ToDouble(p1weapon.Damage + p1extra.Damage) * p1.StatMultiplier * ((double)p2armor.Resistance / 100) + 0.01;

            //calculate enemies damage to you with armor
            double p2damage = Convert.ToDouble(p2weapon.Damage + p2extra.Damage) * p2.StatMultiplier * ((double)p1armor.Resistance / 100) + 0.01;

            //calculate player rounds to kill
            int p1dpr = (int)Math.Ceiling(p2.MaxHP / p1damage);
            //calculate enemy rounds to kill
            int p2dpr = (int)Math.Ceiling(p1.MaxHP / p2damage);

            //determine win and set hp to get removed to damage recieved and how much damage the enemy took and other statics
            int p1hp = 0;
            int p2hp = 0;
            bool draw = false;
            if (p1dpr < p2dpr)
            {
                win = true;
                p1hp = (int)Math.Round(p1.MaxHP - (p2damage * p1dpr));
                p2hp = (int)Math.Round(p2.MaxHP - (p1damage * p1dpr));
            }
            else if(p1dpr != p2dpr)
            {
                win = false;
                p1hp = (int)Math.Round(p1.MaxHP - (p2damage * p2dpr));
                p2hp = (int)Math.Round(p2.MaxHP - (p1damage * p2dpr));
            } else
            {
                draw = true;
                p1hp = (int)Math.Round(p1.MaxHP - (p2damage * p1dpr));
                p2hp = (int)Math.Round(p2.MaxHP - (p1damage * p1dpr));
            }

            if ( draw == false)
            {
                if (win == true)
                {
                    p1.Money += wager;
                    p2.Money -= wager;
                }
                else
                {
                    p2.Money += wager;
                    p1.Money -= wager;
                }
            }
            
            //build pvp screen
            EmbedBuilder embedB = new EmbedBuilder
            {
                Title = "Player Fight",
                Description = $"{p1.CharacterName}[LVL {p1.Lvl}] vs. {p2.CharacterName}[LVL {p2.Lvl}]",
                Color = Color.Orange,
            };

            embedB.AddField($"{p1.CharacterName} HP", Convert.ToString(p1hp) + "/" + Convert.ToString(p1.MaxHP), true);
            embedB.AddField($"{p2.CharacterName} HP", Convert.ToString(p2hp) + "/" + Convert.ToString(p2.MaxHP), true);

            if (wager != 0 && draw == false)
            {
                if (win == true)
                {
                    embedB.AddField($"{p1.CharacterName} Wins!", $"+{wager} Coins!");
                }
                else
                {
                    embedB.AddField($"{p2.CharacterName} Wins!", $"+{wager} Coins!");
                }
            }

            if ( wager == 0)
            {
                if (draw== true)
                {
                    embedB.AddField("Draw!", $"nobody wins");
                } else
                {
                    if ( win == true)
                    {
                        embedB.AddField($"{p1.CharacterName} Wins!", "no wager");
                    } else
                    {
                        embedB.AddField($"{p2.CharacterName} Wins!", "no wager");
                    }
                }
            }

            embedB.WithFooter(Defaults.FOOTER);

            //generate extra stats button
            ComponentBuilder btnb = new ComponentBuilder().WithButton("Details", $"playerfightdetails;{p1damage};{p2damage};{p1dpr};{p2dpr};{p1.CharacterName};{p2.CharacterName};" + p1.PlayerID + ";" + p2.PlayerID, ButtonStyle.Primary);

            //return room screen
            return new Tuple<Embed, MessageComponent>(embedB.Build(), btnb.Build());
        }
    }
}
