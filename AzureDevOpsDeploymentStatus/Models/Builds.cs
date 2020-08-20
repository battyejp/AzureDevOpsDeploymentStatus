using System.Text.RegularExpressions;

namespace AzureDevOpsDeploymentStatus.Models
{
    public class Builds
    {
        public int Count { get; set; }

        public Build[] Value { get; set; }
    }

    public class Build
    {
        public int Id { get; set; }

        public string BuildNumber { get; set; }

        public string Url { get; set; }

        public Definition Definition { get; set; }

        public string Link => $"https://dev.azure.com/IPF-International-Limited/Artemis/_build/results?buildId={Id}&view=results";

        public string Version
        {
            get 
            {
                var rgx = new Regex("[^0-9.]");
                return rgx.Replace(BuildNumber, "");
            }
        }
    }

    public class Definition
    {
        public string Name { get; set; }
    }
}
