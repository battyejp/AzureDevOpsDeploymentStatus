using AzureDevOpsDeploymentStatus.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureDevOpsDeploymentStatus.Pages
{
    public partial class Index : ComponentBase
    {
        private Dictionary<string, string> results;

        [Inject]
        public IBuildService BuildsService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            results = await BuildsService.GetBuilds();
            await base.OnInitializedAsync();
        }
    }
}
