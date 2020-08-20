using AzureDevOpsDeploymentStatus.Services;
using AzureDevOpsDeploymentStatus.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AzureDevOpsDeploymentStatus
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddHttpClient<IBuildService, BuildService>(client =>
            {
                client.BaseAddress = new Uri("https://dev.azure.com/");
            });

            await builder.Build().RunAsync();
        }
    }
}
