using AzureDevOpsDeploymentStatus.Models;
using AzureDevOpsDeploymentStatus.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AzureDevOpsDeploymentStatus.Services
{
    public class BuildService : IBuildService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<IBuildService> logger;
        private readonly string staticEndpoint;
        private readonly string apiVersion;
        private readonly string buildDefinitionIds;

        private Builds builds;
        private string[] environments;

        public BuildService(HttpClient httpClient, IConfiguration Configuration, ILogger<IBuildService> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            staticEndpoint = $"{Configuration["AzDOOrg"]}/{Configuration["AzDOProject"]}/_apis/build/builds";
            apiVersion = Configuration["AzDOApiVersion"];
            buildDefinitionIds = Configuration["BuildDefinitionIds"];

            var byteArray = Encoding.ASCII.GetBytes($"username:{Configuration["AzDOPAT"]}");
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            environments = Configuration["Environments"].Split(',');
        }

        public async Task<Dictionary<string, StageBuildResult>> GetBuilds()
        {
            var results = new Dictionary<string, StageBuildResult>();
            var query = GetStartQueryString();
            query["definitions"] = buildDefinitionIds;
            query["branchName"] = "refs/heads/master";
            query["reasonFilter"] = "individualCI";
            logger.LogInformation($"Query string {query}");

            try
            {
                builds = await httpClient.GetFromJsonAsync<Builds>($"{staticEndpoint}?{query}");
                var orderBuilds = builds.Value.OrderByDescending(x => x.Id).ToList();

                foreach (var build in orderBuilds)
                {
                    logger.LogInformation($"Build {build.BuildNumber}");
                    var records = await httpClient.GetFromJsonAsync<Records>($"{staticEndpoint}/{build.Id}/timeline?{GetStartQueryString()}");

                    foreach (var env in environments)
                    {
                        if (results.ContainsKey(env)) continue;

                        var stageRecord = records.RecordList.Where(x => x.Type == "Stage" && x.Name == env).ToList();

                        if (stageRecord.Any() 
                            && (stageRecord.First().Result == "succeeded" ||
                                stageRecord.First().Result == "failed"))
                        {
                            logger.LogInformation($"Build {build.BuildNumber} Env: {env} Result: {stageRecord.First().Result}");
                            results.Add(env, new StageBuildResult { Build = build, Success = stageRecord.First().Result == "succeeded" });
                        }
                    }

                    if (results.Count == environments.Length) break;
                }
                return results;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occurred getting builds", ex);
                throw ex;
            }
        }

        private NameValueCollection GetStartQueryString()
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["api-version"] = apiVersion;
            return query;
        }
    }
}
