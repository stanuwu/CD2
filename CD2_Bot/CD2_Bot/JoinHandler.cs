using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    class JoinHandler
    {
        public static async Task OnGuildJoin(SocketGuild guild)
        {
            Utils.GuildToDB(guild);
        }
    }
}
