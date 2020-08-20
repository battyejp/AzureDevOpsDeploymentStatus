namespace AzureDevOpsDeploymentStatus.Models
{
    public class EnvBuildResult
    {
        public string Environment { get; set; }

        public Build Build { get; set; }

        public bool Success { get; set; }
    }
}
