using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureDevOpsDeploymentStatus.Services.Interfaces
{
    public interface IConfigurationService
    {
        public string ApiVersion { get; set; }

        public string BuildDefinitionIds { get; set; }

        public string AzureDevOpsRestApiEndpointStart { get; set; }

        public Dictionary<string, string> StageEnvMappings { get; set; }

        public string[] Environments { get; set; }
    }
}
