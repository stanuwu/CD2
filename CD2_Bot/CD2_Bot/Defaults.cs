using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace CD2_Bot
{
    public static class Defaults
    {
        public static readonly char dPrefix = '<';

        //Log a string as Info in the Logging system.
        async public static void QuickLog(string msg)
        {
            await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Info, "Internal", msg));
        }
    }
}