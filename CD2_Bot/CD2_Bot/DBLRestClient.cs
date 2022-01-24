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
            RestClient client = new RestClient(new Uri("https://top.gg/api/"));
            RestRequest request = new RestRequest($"bots/{Defaults.CLIENT.CurrentUser.Id}/stats", Method.Post);
            request.AddHeader("Authorization", auth);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("server_count", count);
            RestResponse response = await client.ExecuteAsync(request);

            if (response.get_IsSuccessful())
            {
                await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Info, "DBLRestClient", $"Successfully posted guild count! ({count})"));
            } else
            {
                await Program.Log(new Discord.LogMessage(Discord.LogSeverity.Error, "DBLRestClient", $"Posting guild count failed: {response.ErrorException.Message}"));
            }
        }

        public static async Task<bool> UserVotedStatus(ulong uid)
        {
            RestClient client = new RestClient(new Uri("https://top.gg/api/"));
            RestRequest request = new RestRequest($"bots/{Defaults.CLIENT.CurrentUser.Id}/check", Method.Get);
            request.AddHeader("Authorization", auth);
            request.AddParameter("userId", uid);
            RestResponse response = await client.ExecuteAsync(request);

            if (response.get_IsSuccessful())
            {
                if (response.get_Content() == "{\"voted\":1}")
                {
                    return true;
                } 
            }
            return false;
        }
    }
}