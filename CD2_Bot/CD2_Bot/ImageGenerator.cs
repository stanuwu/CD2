using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using Discord;

namespace CD2_Bot
{
    public class ImageGenerator
    {
        public static async Task<MemoryStream> MakeCharacterImage(CharacterStructure stats)
        {
            string backgound = "static/characterbg1.png";

            string name = stats.CharacterName;
            int level = stats.Lvl;
            string description = stats.Description;
            string title = stats.Title;
            string cclass = stats.CharacterClass;
            int money = stats.Money;
            int exp = stats.EXP;
            int maxhp = stats.MaxHP;
            int hp = stats.HP;
            string weapon = stats.Weapon.Name;
            string armor = stats.Armor.Name;
            string extra = stats.Extra.Name;
            decimal multi = (decimal)stats.StatMultiplier;
            string imageurl = "";
            IUser founduser = Defaults.CLIENT.GetUser(stats.PlayerID);
            if (founduser != null)
            {
                imageurl = founduser.GetAvatarUrl();
            }
            else
            {
                imageurl = Defaults.CLIENT.CurrentUser.GetDefaultAvatarUrl();
            }
            using (Bitmap image = new Bitmap(backgound))
            using (Font hfont = new Font("Dubai Light", 54))
            using (Font dfont = new Font("Consolas", 28))
            using (Font gfont = new Font("Consolas", 20))
            using (Graphics g = Graphics.FromImage(image))
            using (Brush hbrush = new SolidBrush(System.Drawing.Color.FromArgb(9, 255, 252)))
            using (Brush brush = new SolidBrush(System.Drawing.Color.FromArgb(0, 210, 210)))
            using (Brush hpbrush = new SolidBrush(System.Drawing.Color.FromArgb(221, 46, 68)))
            using (Brush hpobrush = new SolidBrush(System.Drawing.Color.FromArgb(0, 0, 0)))
            using (WebClient client = new WebClient())
            using (Bitmap pfp = new Bitmap(client.OpenRead(imageurl)))
            {
                //settings for text drawing
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                StringFormat center = new StringFormat();
                center.Alignment = StringAlignment.Center;

                //draw name
                g.DrawString(name, hfont, hbrush, new PointF(170, 20));

                //draw desciption
                g.DrawString(description, dfont, brush, new Rectangle(180, 95, 390, 70));

                //draw profile picture
                g.DrawImage(pfp, 40, 40, 120, 120);

                //draw money
                g.DrawString(money.ToString("N0").Replace(",", "."), gfont, brush, new Rectangle(60, 270, 240, 34));

                //draw exp
                g.DrawString(exp.ToString("N0").Replace(",", "."), gfont, brush, new Rectangle(343, 270, 237, 34));

                //draw level
                g.DrawString("Lvl " + level, dfont, brush, new Rectangle(50, 175, 100, 35), center);

                //draw class
                g.DrawString("Class: " + cclass, gfont, brush, new Rectangle(385, 180, 195, 34));

                //draw title
                g.DrawString("Title: " + title, gfont, brush, new Rectangle(172, 180, 192, 34));

                //drawhealthbar
                int healthbarlength = (int)Math.Round(((double)512 / maxhp) * hp);
                g.FillRectangle(hpbrush, new Rectangle(69, 218, healthbarlength, 38));
                g.DrawString($"{hp}/{maxhp}", dfont, hpobrush, new Rectangle(70, 221, 510, 35), center);

                //draw weapon
                g.DrawString(weapon, gfont, brush, new Rectangle(60, 310, 235, 34));

                //draw armor
                g.DrawString(armor, gfont, brush, new Rectangle(342, 310, 235, 34));

                //draw extra
                g.DrawString(extra, gfont, brush, new Rectangle(60, 350, 235, 34));

                //draw stat multiplier
                g.DrawString("Multiplier: x" + multi, gfont, brush, new Rectangle(342, 350, 235, 34));

                //finalize
                g.Dispose();
                MemoryStream output = new MemoryStream();
                image.Save(output, System.Drawing.Imaging.ImageFormat.Png);
                image.Dispose();
                return output;
            }
        }
    }
}
