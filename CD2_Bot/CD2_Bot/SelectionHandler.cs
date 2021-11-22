﻿using Discord;
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
            }
        }


        //add functions for buttons here (try to keep it at 1 function (async task with no return) per button)
        static public async Task FloorSelect(SocketMessageComponent sel)
        {
            ulong userid = (ulong)Convert.ToInt64(sel.Data.CustomId.Split(';')[1]);
            if (userid == sel.User.Id)
            {
                try {
                ulong gid = ((SocketGuildUser)sel.User).Guild.Id;
                string selOpt = string.Join(", ", sel.Data.Values);
                Tuple<Embed, Optional<MessageComponent>> results = Rooms.ExecuteRoom(selOpt, userid, gid, sel.Channel);
                await sel.UpdateAsync(x => { x.Components = results.Item2; x.Embed = results.Item1; });
                }
                catch (Exception r) { Utils.DebugLog(r.Message);  }
            } else
            {
                await sel.RespondAsync(embed: Utils.QuickEmbedError("This is not your floor!"), ephemeral: true);
            }
        }
    }
}
