using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.Text;
using Discord;
using Color = System.Drawing.Color;
using System.Linq;

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
                g.DrawString("LVL " + level, dfont, brush, new Rectangle(444, 120, 130, 35), center);

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

        public static async Task<MemoryStream> MakeStatsImage(CharacterStructure stats)
        {
            string backgound = "static/stats1.png";

            Weapon weaponstats = stats.Weapon;
            Armor armorstats = stats.Armor;
            Extra extrastats = stats.Extra;

            string name = stats.CharacterName;

            string weapon = weaponstats.Name;
            int wexp = weaponstats.EXP;
            int wlevel = weaponstats.Level;
            decimal wdamage = weaponstats.Damage;

            string armor = armorstats.Name;
            int aexp = armorstats.EXP;
            int alevel = armorstats.Level;
            int resistance = armorstats.Resistance;

            string extra = extrastats.Name;
            int eexp = extrastats.EXP;
            int elevel = extrastats.Level;
            decimal edamage = extrastats.Damage;
            int eheal = extrastats.Heal;

            using (Bitmap image = new Bitmap(backgound))
            using (Graphics g = Graphics.FromImage(image))
            using (Font hfont = new Font("Heebo", 40, FontStyle.Bold))
            using (Font dfont = new Font("Rubik", 30))
            using (Font gfont = new Font("Rubik", 20))
            using (Brush hbrush = new SolidBrush(Color.FromArgb(20, 20, 20)))
            using (Brush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
            using (Brush hpbrush = new SolidBrush(Color.FromArgb(221, 46, 68)))
            using (WebClient client = new WebClient())
            {
                //settings for text drawing
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                StringFormat center = new StringFormat();
                center.Alignment = StringAlignment.Center;

                //draw name
                g.DrawString(name, hfont, hbrush, new PointF(10, 13));

                //draw weapon
                g.DrawString(weapon, dfont, brush, new Rectangle(82, 100, 500, 34));
                g.DrawString($"EXP: {wexp}", gfont, brush, new Rectangle(82, 140, 500, 34));
                g.DrawString($"LVL: {wlevel}", gfont, brush, new Rectangle(82, 165, 500, 34));
                g.DrawString($"Damage: {wdamage}", gfont, brush, new Rectangle(200, 140, 500, 34));

                //draw armor
                g.DrawString(armor, dfont, brush, new Rectangle(82, 200, 500, 34));
                g.DrawString($"EXP: {aexp}", gfont, brush, new Rectangle(82, 240, 500, 34));
                g.DrawString($"LVL: {alevel}", gfont, brush, new Rectangle(82, 265, 500, 34));
                g.DrawString($"Resistance: {resistance}%", gfont, brush, new Rectangle(200, 240, 500, 34));

                //draw extra
                g.DrawString(extra, dfont, brush, new Rectangle(82, 300, 500, 34));
                g.DrawString($"EXP: {eexp}", gfont, brush, new Rectangle(82, 340, 500, 34));
                g.DrawString($"LVL: {elevel}", gfont, brush, new Rectangle(82, 365, 500, 34));
                g.DrawString($"Damage: {edamage}", gfont, brush, new Rectangle(200, 340, 500, 34));
                g.DrawString($"Heal: {eheal}", gfont, brush, new Rectangle(200, 365, 500, 34));

                //finalize
                g.Dispose();
                MemoryStream output = new MemoryStream();
                image.Save(output, System.Drawing.Imaging.ImageFormat.Png);
                image.Dispose();
                return output;
            }
        }
        
        public static async Task<MemoryStream> MakeInventoryImage(CharacterStructure player)
        {
            string name = player.CharacterName;
            Dictionary<string, int> inv = Utils.InvAsDict(player);

            StringBuilder items = new StringBuilder();
            foreach (string item in inv.Keys.Take((int)Math.Ceiling(inv.Count/2f)))
            {
                items.Append($"{inv[item]}x {item}\n");
            }
            
            StringBuilder items2 = new StringBuilder();
            foreach (string item in inv.Keys.Skip((int)Math.Ceiling(inv.Count/2f)))
            {
                items2.Append($"{inv[item]}x {item}\n");
            }

            string backgound = "static/inventory.png";
            
            using (Bitmap image = new Bitmap(backgound))
            using (Font hfont = new Font("Heebo", 23, FontStyle.Bold))
            using (Font gfont = new Font("Rubik", 100, FontStyle.Regular, GraphicsUnit.Pixel))
            using (Graphics g = Graphics.FromImage(image))
            using (Brush hbrush = new SolidBrush(Color.FromArgb(20, 20, 20)))
            using (Brush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
            {
                //settings for text drawing
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                StringFormat center = new StringFormat();
                StringFormat left = new StringFormat();
                center.Alignment = StringAlignment.Center;
                left.Alignment = StringAlignment.Near;
                

                SizeF RealSize = g.MeasureString(items.ToString(), gfont);
                float HeightScaleRatio = 270 / RealSize.Height;
                float WidthScaleRatio = 280 / RealSize.Width;

                float ScaleRatio = (HeightScaleRatio < WidthScaleRatio)
                    ? HeightScaleRatio
                    : WidthScaleRatio;

                float ScaleFontSize = gfont.Size * ScaleRatio;

                Font ScaledFont = new Font(gfont.FontFamily, (int)(ScaleFontSize*0.625));
                
                //draw name
                g.DrawString(name, hfont, hbrush, new PointF(10, 13));
                

                //draw inventory
                g.DrawString(items.ToString(), ScaledFont, brush, new Rectangle(10, 107, 280, 270), left);
                g.DrawString(items2.ToString(), ScaledFont, brush, new Rectangle(310, 107, 280, 270), left);

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
