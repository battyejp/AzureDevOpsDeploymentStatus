using AzureDevOpsDeploymentStatus.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureDevOpsDeploymentStatus.Services.Interfaces
{
    public interface IBuildService
    {
        Task GetEnvBuildResults(IDictionary<string, List<EnvBuildResult>> results, Action pipelineResultsFound = null);
    }
}
