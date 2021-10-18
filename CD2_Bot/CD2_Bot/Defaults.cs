using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace CD2_Bot
{
    public static class Defaults
    {
        public static readonly char dPrefix = '<';
        public static readonly List<ulong> STAFF = new List<ulong> { 159355703016947713, 255732879525412865, 623984743914012712 };
        public static DiscordSocketClient CLIENT;
    }
}