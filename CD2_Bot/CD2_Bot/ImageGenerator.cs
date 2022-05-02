using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using Discord;
using Color = System.Drawing.Color;

namespace CD2_Bot
{
    public class ImageGenerator
    {
        public static async Task<MemoryStream> MakeCharacterImage(CharacterStructure stats)
        {
            string backgound = "static/bg1.png";

            string name = stats.CharacterName;
            int level = stats.Lvl;
            string description = stats.Description;
            string title = stats.Title;
            string cclass = stats.CharacterClass.Name;
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
            using (Font hfont = new Font("Heebo", 8, FontStyle.Bold))
            using (Font dfont = new Font("Rubik", 6))
            using (Font gfont = new Font("Rubik", 5))
            using (Graphics g = Graphics.FromImage(image))
            using (Brush hbrush = new SolidBrush(Color.FromArgb(20, 20, 20)))
            using (Brush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
            using (Brush hpbrush = new SolidBrush(Color.FromArgb(221, 46, 68)))
            using (WebClient client = new WebClient())
            using (Bitmap pfp = new Bitmap(client.OpenRead(imageurl)))
            {
                //settings for text drawing
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                StringFormat center = new StringFormat();
                center.Alignment = StringAlignment.Center;

                //draw name
                g.DrawString(name, hfont, hbrush, new PointF(186, 65));

                //draw desciption
                g.DrawString(description, gfont, brush, new Rectangle(350, 225, 219, 80));

                //draw profile picture
                g.DrawImage(pfp, 22, 37, 163, 163);

                //draw money
                g.DrawString(money.ToString("N0"), gfont, brush, new Rectangle(376, 322, 240, 34));

                //draw exp
                g.DrawString(exp.ToString("N0"), gfont, brush, new Rectangle(376, 353, 237, 34));

                //draw level
                g.DrawString("LVL " + level, dfont, brush, new Rectangle(444, 120, 100, 35), center);

                //draw class
                g.DrawString(cclass, dfont, brush, new Rectangle(233, 120, 195, 34));

                //draw title
                g.DrawString(title, dfont, brush, new Rectangle(194, 33, 195, 37));

                //drawhealthbar
                int healthbarlength = (int)Math.Round(((double)361 / maxhp) * hp);
                g.FillRectangle(hpbrush, new Rectangle(212, 172, healthbarlength, 29));
                g.DrawString($"{hp}/{maxhp}", dfont, brush, new Rectangle(335, 172, 510, 35));

                //draw weapon
                g.DrawString(weapon, gfont, brush, new Rectangle(82, 233, 235, 34));

                //draw armor
                g.DrawString(armor, gfont, brush, new Rectangle(82, 289, 235, 34));

                //draw extra
                g.DrawString(extra, gfont, brush, new Rectangle(82, 345, 360, 34));

                //draw stat multiplier
                g.DrawString(multi.ToString(), gfont, brush, new Rectangle(505, 340, 277, 34));

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
