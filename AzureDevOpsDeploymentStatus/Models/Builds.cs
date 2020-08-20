using System;
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

        public Project Project { get; set; }

        public string Link
        {
            get
            {
                var split = Project.Url.Split(new string[] { "_apis" }, StringSplitOptions.None);
                return $"{split[0]}{Project.Name}/_build/results?buildId={Id}&view=results";
            }
        }

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

        public int Id { get; set; }
    }

    public class Project
    {
        public string Name { get; set; }

        public string Url { get; set; }
    }
}
