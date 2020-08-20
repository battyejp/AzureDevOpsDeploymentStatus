using AzureDevOpsDeploymentStatus.Models;
using AzureDevOpsDeploymentStatus.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureDevOpsDeploymentStatus.Pages
{
    public partial class Index : ComponentBase
    {
        private Dictionary<string, Build> results;

        [Inject]
        public IBuildService BuildService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            results = await BuildService.GetBuilds();
            await base.OnInitializedAsync();
        }
    }
}
