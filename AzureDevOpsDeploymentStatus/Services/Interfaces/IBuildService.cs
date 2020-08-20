using AzureDevOpsDeploymentStatus.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureDevOpsDeploymentStatus.Services.Interfaces
{
    public interface IBuildService
    {
        Task<Dictionary<string, List<EnvBuildResult>>> GetEnvBuildResults();
    }
}
