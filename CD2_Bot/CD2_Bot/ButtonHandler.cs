using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    static class ButtonHandler
    {
        static public async Task HandleButtonAsync(SocketMessageComponent btn)
        {
            //Register buttons in this switch
            switch (btn.Data.CustomId.Split(';')[0])
            {
                case "testbtn1":
                    await BtnTest(btn);
                    break;
                case "dropaction":
                    await DropAction(btn);
                    break;
                case "chestopen":
                    await ChestOpen(btn);
                    break;
                case "delchar":
                    await DelChar(btn);
                    break;
                case "coinflip":
                    await DoCoinflip(btn);
                    break;
            }
        }


        //add functions for buttons here (try to keep it at 1 function (async task with no return) per button)
        static public async Task BtnTest(SocketMessageComponent btn)
        {
            ulong userid = (ulong)Convert.ToInt64(btn.Data.CustomId.Split(';')[1]);
            await btn.Message.DeleteAsync();
            await btn.RespondAsync("Hi", ephemeral: true);
        }

        static public async Task DropAction(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            ulong userid = (ulong)Convert.ToInt64(btndata[2]);
            CharacterStructure statst = (from user in tempstorage.characters
                                         where user.PlayerID == userid
                                         select user).SingleOrDefault();
            if (userid == btn.User.Id)
            {
                await btn.Message.DeleteAsync();

                if ((DateTime.Now - btn.Message.Timestamp).TotalMinutes > 15)
                {
                    await btn.RespondAsync(embed: Utils.QuickEmbedError("This drop is expired."), ephemeral: true);
                    return;
                }

                string dmsg = "";
                switch (btndata[1])
                {
                    case "claim":
                        dmsg = "Successfully Claimed!";
                        switch (btndata[3])
                        {
                            case "weapon":
                                statst.Weapon = Gear.Weapons.Find(w => w.Name == btndata[4]);
                                break;
                            case "armor":
                                statst.Armor = Gear.Armors.Find(w => w.Name == btndata[4]);
                                break;
                            case "extra":
                                statst.Extra = Gear.Extras.Find(w => w.Name == btndata[4]);
                                break;
                        }
                        break;

                    case "sell":
                        dmsg = "Successfully Sold!";
                        switch (btndata[3])
                        {
                            case "weapon":
                                statst.Money += Prices.sell[Gear.Weapons.Find(w => w.Name == btndata[4]).Rarity];
                                break;
                            case "armor":
                                statst.Money += Prices.sell[Gear.Armors.Find(w => w.Name == btndata[4]).Rarity];
                                break;
                            case "extra":
                                statst.Money += Prices.sell[Gear.Extras.Find(w => w.Name == btndata[4]).Rarity];
                                break;
                        }
                        break;

                    case "infuse":
                        dmsg = "Successfully Infused!";
                        switch (btndata[3])
                        {
                            case "weapon":
                                statst.WeaponXP += Prices.infuse[Gear.Weapons.Find(w => w.Name == btndata[4]).Rarity];
                                break;
                            case "armor":
                                statst.ArmorXP += Prices.infuse[Gear.Armors.Find(w => w.Name == btndata[4]).Rarity];
                                break;
                            case "extra":
                                statst.ExtraXP += Prices.infuse[Gear.Extras.Find(w => w.Name == btndata[4]).Rarity];
                                break;
                        }
                        break;
                }
                await btn.RespondAsync(embed: Utils.QuickEmbedNormal("Drop", dmsg), ephemeral: true);
            }
            else
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This is not your drop."), ephemeral: true);
            }
        }

        static public async Task ChestOpen(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            ulong userid = (ulong)Convert.ToInt64(btndata[1]);
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == userid
                                        select user).SingleOrDefault();
            if (userid == btn.User.Id)
            {
                if ((DateTime.Now - btn.Message.Timestamp).TotalMinutes > 15)
                {
                    await btn.Message.DeleteAsync();
                    await btn.RespondAsync(embed: Utils.QuickEmbedError("This chest is expired."), ephemeral: true);
                    return;
                }
                Rarity drarity = (Rarity)Enum.Parse(typeof(Rarity), btndata[2]);
                if (stats.Money >= Prices.buy[drarity])
                {
                    await btn.Message.DeleteAsync();
                    stats.Money -= Prices.buy[drarity];
                    Gear.RandomDrop(userid, btn.Channel, drarity, btndata[3]);
                }
                else
                {
                    await btn.RespondAsync(embed: Utils.QuickEmbedError("You can not afford this chest."), ephemeral: true);
                }
            }
            else
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This is not your chest."), ephemeral: true);
            }
        }

        static public async Task DelChar(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            ulong userid = (ulong)Convert.ToInt64(btndata[2]);
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == userid
                                        select user).SingleOrDefault();
            if (userid == btn.User.Id)
            {
                if ((DateTime.Now - btn.Message.Timestamp).TotalMinutes > 5)
                {
                    await btn.Message.DeleteAsync();
                    await btn.RespondAsync(embed: Utils.QuickEmbedError("This prompt is expired."), ephemeral: true);
                    return;
                }
                await btn.Message.DeleteAsync(); ;
                if (btndata[1] == "confirm")
                {
                    stats.Deleted = true;
                    await btn.RespondAsync(embed: Utils.QuickEmbedNormal("Success", "Your character was deleted!"), ephemeral: true);
                }
            }
            else
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This is not your prompt."), ephemeral: true);
            }
        }

        static public async Task DoCoinflip(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            ulong userid = (ulong)Convert.ToInt64(btndata[2]);
            ulong userid2 = (ulong)Convert.ToInt64(btndata[3]);

            if ((DateTime.Now - btn.Message.Timestamp).TotalMinutes > 15)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This request is expired."), ephemeral: true);
                return;
            }

            if (btndata[1] == "cancel" && (btn.User.Id == userid || btn.User.Id == userid2))
            {
                await btn.Message.DeleteAsync();
                return;
            }

            if (btn.User.Id != userid2)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("This is not your coinflip request."), ephemeral: true);
                return;
            }

            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == userid
                                        select user).SingleOrDefault();
            CharacterStructure stats2 = (from user in tempstorage.characters
                                        where user.PlayerID == userid2
                                        select user).SingleOrDefault();

            if (stats == null || stats2 == null)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("One of you does not have a character."));
                await btn.Message.DeleteAsync();
                return;
            }

            int wager = Convert.ToInt32(btndata[4]);

            if (stats.Money < wager || stats2.Money < wager)
            {
                await btn.RespondAsync(embed: Utils.QuickEmbedError("One of you can not afford this coinflip."));
                return;
            }

            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Coinflip"
            };
            embed.WithFooter(Defaults.FOOTER);
            int cfw = Defaults.GRandom.Next(0, 2);
            embed.WithColor(Color.Green);
            switch (cfw)
            {
                case 0:
                    stats.Money += wager;
                    stats2.Money -= wager;
                    embed.Description = $"<@{userid}> takes {wager} coins from <@{userid2}>!";
                    break;

                default:
                    stats.Money -= wager;
                    stats2.Money += wager;
                    embed.Description = $"<@{userid}> looses {wager} coins to <@{userid2}>!";
                    break;
            }
            await btn.Message.DeleteAsync();
            await btn.RespondAsync(embed: embed.Build());
        }
    }
}