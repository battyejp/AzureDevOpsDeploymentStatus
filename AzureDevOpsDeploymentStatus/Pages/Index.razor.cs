using AzureDevOpsDeploymentStatus.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AzureDevOpsDeploymentStatus.Pages
{
    public partial class Index : ComponentBase
    {
        private Dictionary<string, string> results;
        private Builds builds;
        private List<string> environments;
        private string staticEndpoint;

        MyHttpClient HttpClient { get; set; }

        [Inject]
        HttpClient DefaultHttpClient { get; set; }

        [Inject]
        IConfiguration Configuration { get; set; }

        [Inject]
        ILogger<Index> Logger { get; set; }

        public Index()
        {
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

        protected override async Task OnInitializedAsync()
        {
            HttpClient = new MyHttpClient(DefaultHttpClient, Configuration);
            staticEndpoint = $"{Configuration["AzDOPATOrg"]}/{Configuration["AzDOPATProject"]}/_apis/build/builds";
            Logger.LogInformation($"Some config: {Configuration["AzDOPATOrg"]}");
            results = await GetBuilds();
            await base.OnInitializedAsync();
        }

        private async Task<Dictionary<string, string>> GetBuilds(string definitions = "52")
        {
            HttpClient.Endpoint = staticEndpoint;
            HttpClient.AddQueryParameter("api-version", Configuration["AzDOApiVersion"]);
            HttpClient.AddQueryParameter("definitions", definitions);
            HttpClient.AddQueryParameter("branchName", "refs/heads/master");
            HttpClient.AddQueryParameter("reasonFilter", "individualCI");

            try
            {
                builds = await HttpClient.HttpClient.GetFromJsonAsync<Builds>(HttpClient.Endpoint);
                var orderBuilds = builds.Value.OrderByDescending(x => x.Id).ToList();

                foreach (var build in orderBuilds)
                {
                    Logger.LogInformation($"Build {build.BuildNumber}");
                    HttpClient.Endpoint = $"{staticEndpoint}/{build.Id}/timeline";
                    HttpClient.AddQueryParameter("api-version", Configuration["AzDOApiVersion"]);
                    var records = await HttpClient.HttpClient.GetFromJsonAsync<Records>(HttpClient.Endpoint);

                    foreach (var env in environments)
                    {
                        if (results.ContainsKey(env)) continue;

                        var stageRecord = records.RecordList.Where(x => x.Type == "Stage" && x.Name == env).ToList();

                        if (stageRecord.Any() && stageRecord.First().Result == "succeeded")
                        {
                            Logger.LogInformation($"Build {build.BuildNumber} Env: {env}");
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
