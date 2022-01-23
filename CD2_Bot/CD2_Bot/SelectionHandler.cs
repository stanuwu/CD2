using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    class SelectionHandler
    {
        static public async Task HandleSelectionAsync(SocketMessageComponent sel)
        {
            //Register selections in this switch
            switch (sel.Data.CustomId.Split(';')[0])
            {
                case "floorroomselect":
                    await FloorSelect(sel);
                    break;
                case "craft":
                    await CraftingDrop(sel);
                    break;
                case "biomefight":
                    await BiomeFight(sel);
                    break;
            }
        }


        //add functions for buttons here (try to keep it at 1 function (async task with no return) per button)
        static public async Task FloorSelect(SocketMessageComponent sel)
        {
            ulong userid = (ulong)Convert.ToInt64(sel.Data.CustomId.Split(';')[1]);
            if (userid == sel.User.Id)
            {
                if ((DateTime.Now - sel.Message.Timestamp).TotalMinutes > Defaults.FLOORCOOLDOWN-1)
                {
                    await sel.Message.DeleteAsync();
                    await sel.RespondAsync(embed: Utils.QuickEmbedError("This floor is expired."), ephemeral: true);
                    return;
                }
                await sel.DeferAsync();
                ulong gid = ((SocketGuildUser)sel.User).Guild.Id;
                string selOpt = string.Join(", ", sel.Data.Values);
                Tuple<Embed, Optional<MessageComponent>> results = Rooms.ExecuteRoom(selOpt, userid, gid, sel.Channel);
                await sel.Message.DeleteAsync();
                await sel.FollowupAsync(components: results.Item2.Value, embed: results.Item1 );
            } else
            {
                await sel.RespondAsync(embed: Utils.QuickEmbedError("This is not your floor!"), ephemeral: true);
            }
        }

        static public async Task CraftingDrop(SocketMessageComponent sel)
        {
            ulong userid = (ulong)Convert.ToInt64(sel.Data.CustomId.Split(';')[1]);
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == userid
                                        select user).SingleOrDefault();

            if (userid == sel.User.Id)
            {
                if ((DateTime.Now - sel.Message.Timestamp).TotalMinutes > 15)
                {
                    await sel.Message.DeleteAsync();
                    await sel.RespondAsync(embed: Utils.QuickEmbedError("This room is expired."), ephemeral: true);
                    return;
                }

                string[] data = sel.Data.Values.FirstOrDefault().Split(';');
                string item = data[0];
                int coincost = Convert.ToInt32(data[1]);
                string type = data[2];

                Dictionary<string, int> inv = Utils.InvAsDict(stats);

                bool canafford = true;
                Tuple<string, Dictionary<string, int>, int, string> rcp = Crafting.list.Find(r => r.Item1 == item);

                foreach (string k in rcp.Item2.Keys)
                {
                    if (!(inv.ContainsKey(k) && inv[k] >= rcp.Item2[k]))
                    {
                        canafford = false;
                    }
                }

                if (canafford && stats.Money >= coincost)
                {
                    stats.Money -= coincost;
                    foreach (string k in rcp.Item2.Keys)
                    {
                        inv[k] -= rcp.Item2[k];
                        if (inv[k] < 1)
                        {
                            inv.Remove(k);
                        }
                    }
                    Utils.SaveInv(stats, inv);
                    await sel.Message.DeleteAsync();
                    Gear.RandomDrop(userid, sel.Channel, ovr: item, ovrtype: type);
                    return;
                }
                else
                {
                    await sel.RespondAsync(embed: Utils.QuickEmbedError("You can not afford to craft this!"), ephemeral: true);
                    return;
                }

            }
            else
            {
                await sel.RespondAsync(embed: Utils.QuickEmbedError("This is not your room!"), ephemeral: true);
            }
        }

        static public async Task BiomeFight(SocketMessageComponent sel)
        {
            ulong userid = (ulong)Convert.ToInt64(sel.Data.CustomId.Split(';')[1]);
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == userid
                                        select user).SingleOrDefault();

            if ((DateTime.Now - sel.Message.Timestamp).TotalMinutes > 15)
            {
                await sel.Message.DeleteAsync();
                await sel.RespondAsync(embed: Utils.QuickEmbedError("This room is expired."), ephemeral: true);
                return;
            }

            Biome sbiome = (Biome)Enum.Parse(typeof(Biome), sel.Data.Values.FirstOrDefault().Split(';')[0]);
            Enemy opponent = EnemyGen.RandomEnemy(stats.Lvl, sbiome);

            Tuple<Embed, MessageComponent> fr = SimulateFight.Sim(opponent, stats);
            Embed embed = fr.Item1;
            MessageComponent msgc = fr.Item2;

            int gearroll = Defaults.GRandom.Next(1, Defaults.GEARDROPCHANCE); ;
            //creative way to check for win (dont ask lol)
            if (embed.Color.ToString() == "#2ECC71" && gearroll == 1)
            {
                Gear.RandomDrop(stats.PlayerID, sel.Channel);
            }

            await sel.UpdateAsync(x =>
            {
                x.Embed = embed;
                x.Components = msgc;
            });
        }
    }
}
