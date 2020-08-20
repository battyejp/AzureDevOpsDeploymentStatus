using AzureDevOpsDeploymentStatus.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AzureDevOpsDeploymentStatus
{
    public interface IBuildsHelper
    {
        Task<Dictionary<string, string>> GetBuilds(string definitions = "52");
    }

    public class BuildsHelper : IBuildsHelper
    {
        private readonly MyHttpClient myHttpClient;
        private readonly IConfiguration configuration;
        private readonly string staticEndpoint;

        private Builds builds;
        private List<string> environments;
        private Dictionary<string, string> results;

        public BuildsHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.myHttpClient = new MyHttpClient(this.configuration);
            staticEndpoint = $"{configuration["AzDOPATOrg"]}/{configuration["AzDOPATProject"]}/_apis/build/builds";

            results = new Dictionary<string, string>();
            environments = new List<string>()
            {
                "Deploy_Test",
                "Deploy_SIT",
                "Deploy_UAT", 
                "Deploy_NFT",
                "Deploy_Dev"
            };

        }

        public async Task<Dictionary<string, string>> GetBuilds(string definitions = "52")
        {
            myHttpClient.Endpoint = staticEndpoint;
            myHttpClient.AddQueryParameter("api-version", configuration["AzDOApiVersion"]);
            myHttpClient.AddQueryParameter("definitions", definitions);
            myHttpClient.AddQueryParameter("branchName", "refs/heads/master");
            myHttpClient.AddQueryParameter("reasonFilter", "individualCI");

            try
            {
                builds = await myHttpClient.GetFromJsonAsync<Builds>(myHttpClient.Endpoint);
                var orderBuilds = builds.Value.OrderByDescending(x => x.Id).ToList();

                foreach(var build in orderBuilds)
                {
                    Console.WriteLine($"Build {build.BuildNumber}");
                    myHttpClient.Endpoint = $"{staticEndpoint}/{build.Id}/timeline";
                    myHttpClient.AddQueryParameter("api-version", configuration["AzDOApiVersion"]);
                    var records = await myHttpClient.GetFromJsonAsync<Records>(myHttpClient.Endpoint);

                    foreach (var env in environments)
                    {
                        if (results.ContainsKey(env)) continue;

                        var stageRecord = records.RecordList.Where(x => x.Type == "Stage" && x.Name == env).ToList();

                        if (stageRecord.Any() && stageRecord.First().Result == "succeeded")
                        {
                            Console.WriteLine($"Build {build.BuildNumber} Env: {env}");
                            results.Add(env, build.BuildNumber);
                        }
                    }

                    if (results.Count == environments.Count) break;
                }
                return results;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
