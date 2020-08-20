namespace AzureDevOpsDeploymentStatus.Models
{
    public class Builds
    {
        public int Count { get; set; }

        public Value[] Value { get; set; }
    }

    public class Value
    {
        public int Id { get; set; }

        public string BuildNumber { get; set; }
    }
}
