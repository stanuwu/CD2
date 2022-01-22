using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public static class DBLRestClient
    {
        static string auth = File.ReadAllText("dbltoken.txt");
        public static async void PostGuildCount(int count)
        {
            HttpClient hc = new HttpClient();
            hc.DefaultRequestHeaders.Add("Authorization", auth);
            string jsoncontent = JsonConvert.SerializeObject(new { server_count = count });
            HttpContent content = new StringContent(jsoncontent);
            Utils.DebugLog((await hc.PostAsync($"https://top.gg/api/bots/{Defaults.CLIENT.CurrentUser.Id}/stats", content)).ToString());
        }
    }
}
