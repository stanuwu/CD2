using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CD2_Bot
{
    public static class Shop
    {
        static Calendar calendar = new CultureInfo("en-US").Calendar;

        //"shop" command
        public static async Task ShopAsync(SocketSlashCommand cmd)
        {
            StringBuilder display = new StringBuilder();
            Weapon weapon = getWeeklyWeapon();
            display.Append("🗡 **Weapon:**\n");
            display.Append(weapon.Name).Append("\n");
            display.Append($"Cost: {Prices.buy[weapon.Rarity]}\n\n");

            Armor armor = getWeeklyArmor();
            display.Append("🛡 **Armor:**\n");
            display.Append(armor.Name).Append("\n");
            display.Append($"Cost: {Prices.buy[armor.Rarity]}\n\n");

            Extra extra = getWeeklyExtra();
            display.Append("🔮 **Extra:**\n");
            display.Append(extra.Name).Append("\n");
            display.Append($"Cost: {Prices.buy[extra.Rarity]}\n\n");

            Embed guideembed = Utils.QuickEmbedNormal("Shop", display.ToString());
            ComponentBuilder guidebuttons = new ComponentBuilder()
                       .WithButton("Buy Weapon", "shop;buy;weapon", ButtonStyle.Primary, row: 0, emote: new Emoji("🗡"))
                       .WithButton("Buy Armor", "shop;buy;armor", ButtonStyle.Primary, row: 1, emote: new Emoji("🛡"))
                       .WithButton("Buy Extra", "shop;buy;extra", ButtonStyle.Primary, row: 2, emote: new Emoji("🔮"));


            MessageComponent btn = guidebuttons.Build();

            await cmd.RespondAsync(embed: guideembed, components: btn);
        }

        static public async Task ShopBuy(SocketMessageComponent btn, string type)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == btn.User.Id
                                        select user).SingleOrDefault();

            if (stats == null)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("You do not have a character."), ephemeral: true);
                return;
            }

            IGearStats gearStats = null;
            switch (type)
            {
                case "weapon":
                    gearStats = getWeeklyWeapon();
                    break;
                case "armor":
                    gearStats = getWeeklyArmor();
                    break;
                case "extra":
                    gearStats = getWeeklyExtra();
                    break;
            }
            int price = Prices.buy[gearStats.Rarity];
            if (stats.Money < price)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("You can not afford this."), ephemeral: true);
                return;
            }
            stats.Money -= price;
            Gear.RandomDrop(btn.User.Id, btn.Channel, type: type, ovr: gearStats.Name, droprarity: gearStats.Rarity);
            await btn.DeferAsync();
        }

        static int getWeekNumber()
        {
            return calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
        static Random getWeaponRandom()
        {
            return new Random(getWeekNumber() * 420 / 69);
        }

        static Random getArmorRandom()
        {
            return new Random(getWeekNumber() * 666 / 75);
        }

        static Random getExtraRandom()
        {
            return new Random(getWeekNumber() * 999 / 42);
        }

        public static Weapon getWeeklyWeapon()
        {
            List<Weapon> dropable = Gear.Weapons.FindAll(x => x.CanDrop == true);
            return dropable[getWeaponRandom().Next(0, dropable.Count-1)];
        }

        public static Armor getWeeklyArmor()
        {
            List<Armor> dropable = Gear.Armors.FindAll(x => x.CanDrop == true);
            return dropable[getArmorRandom().Next(0, dropable.Count - 1)];
        }

        public static Extra getWeeklyExtra()
        {
            List<Extra> dropable = Gear.Extras.FindAll(x => x.CanDrop == true);
            return dropable[getExtraRandom().Next(0, dropable.Count - 1)];
        }
    }
}
