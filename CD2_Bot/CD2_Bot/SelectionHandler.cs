﻿using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    class SelectionHandler
    {
        static public async Task HandleButtonAsync(SocketMessageComponent sel)
        {
            //Register selections in this switch
            switch (sel.Data.CustomId.Split(';')[0])
            {
                case "floorroomselect":
                    await BtnTest(sel);
                    break;
            }
        }


        //add functions for buttons here (try to keep it at 1 function (async task with no return) per button)
        static public async Task BtnTest(SocketMessageComponent sel)
        {
            ulong userid = (ulong)Convert.ToInt64(sel.Data.CustomId.Split(';')[1]);
            if (userid == sel.User.Id)
            {
                await sel.UpdateAsync(x => x.Components = null);
                await sel.RespondAsync(embed: Utils.QuickEmbedNormal("Floor", $"You selected: {sel.Data.Values.First()}"));
            }
            await sel.FollowupAsync(embed:Utils.QuickEmbedError("This is not your command!"), ephemeral: true);
        }
    }
}
