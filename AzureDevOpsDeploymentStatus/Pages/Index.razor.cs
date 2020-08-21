using AzureDevOpsDeploymentStatus.Models;
using AzureDevOpsDeploymentStatus.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureDevOpsDeploymentStatus.Pages
{
    public partial class Index : ComponentBase
    {
        private Dictionary<string, List<EnvBuildResult>> buildResults;
        private string[] environments;

        [Inject]
        public IBuildService BuildService { get; set; }

        [Inject]
        public IConfigurationService ConfigurationService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            environments = ConfigurationService.Environments;
            buildResults = await BuildService.GetEnvBuildResults();

            await base.OnInitializedAsync();
        }
    }
}
