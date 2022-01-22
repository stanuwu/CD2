using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public static class UserFetch
    {
        public static async void FetchUniqueUsers()
        {
            while (true)
            {
                Defaults.UUSERS = await Utils.UniqueUserCount();
                await Task.Delay(TimeSpan.FromMinutes(Defaults.FETCHEXPENSIVEDELAY));
            }
        }
    }
}
