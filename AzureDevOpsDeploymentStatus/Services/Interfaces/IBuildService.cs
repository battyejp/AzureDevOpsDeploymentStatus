using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureDevOpsDeploymentStatus.Services.Interfaces
{
    public interface IBuildService
    {
        Task<Dictionary<string, string>> GetBuilds(string definitions = "52");
    }
}
