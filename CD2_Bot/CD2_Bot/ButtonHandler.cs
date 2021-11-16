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
            switch (btn.Data.CustomId)
            {
                case "testbtn1":
                    await BtnTest(btn);
                    break;
            }
        }

        //add functions for buttons here (try to keep it at 1 function (async task with no return) per button)
        static public async Task BtnTest(SocketMessageComponent btn)
        {
            await btn.UpdateAsync(x => x.Components = null);
        }
    }
}
