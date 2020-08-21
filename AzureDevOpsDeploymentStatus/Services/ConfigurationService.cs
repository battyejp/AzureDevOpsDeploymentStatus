using AzureDevOpsDeploymentStatus.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace AzureDevOpsDeploymentStatus.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public ConfigurationService(IConfiguration configuration, ILogger<IConfigurationService> logger)
        {
            ApiVersion = configuration["AzDOApiVersion"];
            BuildDefinitionIds = configuration["BuildDefinitionIds"];
            AzureDevOpsRestApiEndpointStart = $"{configuration["AzDOOrg"]}/{configuration["AzDOProject"]}/_apis/build/builds";

            var StageEnvMappings = new Dictionary<string, string>();
            var valuesSection = configuration.GetSection("StageEnviromentMappings");
            foreach (var section in valuesSection.GetChildren())
            {
                logger.LogInformation($"{section.GetValue<string>("Stage")} -> {section.GetValue<string>("Environment")}");
                StageEnvMappings.Add(section.GetValue<string>("Stage"), section.GetValue<string>("Environment"));
            }

            Stages = StageEnvMappings.Keys.ToArray();
            Environments = StageEnvMappings.Values.ToArray();
        }

        public string ApiVersion { get; set; }

        public string BuildDefinitionIds { get; set; }

        public string AzureDevOpsRestApiEndpointStart { get; set; }

        public Dictionary<string, string> StageEnvMappings { get; set; }

        public string[] Stages { get; set; }

        public string[] Environments { get; set; }
    }
}
