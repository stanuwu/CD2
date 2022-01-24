﻿using System;
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
        public static readonly string VERSION = "1.0.0-PRE";
        public static readonly string FOOTER = $"CustomDungeons 2 - Ver. {VERSION}";
        public static readonly string BOTIMG = "https://cdn.discordapp.com/avatars/887247576297013288/ab077ae898cf9ca26c3f035c5ebf15a2.webp?size=1024";
        public static readonly int FLOORCOOLDOWN = 10;
        public static readonly int FARMINGCOOLDOWN = 10;
        public static readonly int GEARDROPCHANCE = 25;
        public static readonly int POSTGUILDCOUNTDELAY = 30;
        public static readonly int FETCHEXPENSIVEDELAY = 720;
        public static int UUSERS = 0;
        public static Random GRandom = new Random();
    }
}