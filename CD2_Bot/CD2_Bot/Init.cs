using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    class Init
    {
        public static bool hasbooted = false;

        public static async Task Initialize()
        {
            if (hasbooted == false)
            {
                db.Init();
                Utils.UpdateStatus($"<help | <guide | Version {Defaults.VERSION}");
                UserFetch.FetchUniqueUsers();
                GuildPostTask.StartPosting();
                hasbooted = true;
            }
        }
    }
}
