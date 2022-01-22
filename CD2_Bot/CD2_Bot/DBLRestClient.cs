using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public static class DBLRestClient
    {
        static string auth = File.ReadAllText("dbltoken.txt");
        public static async void PostGuildCount(int count)
        {
            Uri baseUrl = new Uri("https://top.gg/api/");
            RestClient client = new RestClient(baseUrl);
            RestRequest request = new RestRequest("post", Method.Post);
            request.AddHeader("Authorization", auth);
            request.AddJsonBody(new { server_count = count }, $"bots/{Defaults.CLIENT.CurrentUser.Id}/stats");
            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Info, "DBLRestClient", $"Successfully posted guild count! ({count})"));
            } else
            {
                await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Error, "DBLRestClient", $"Posting guild count failed:\n{response.ErrorMessage}"));
            }
        }
    }
}
