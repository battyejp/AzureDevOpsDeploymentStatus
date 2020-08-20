using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureDevOpsDeploymentStatus.Models
{
    public class EnvBuildResult
    {
        public string Environment { get; set; }

        public Build Build { get; set; }

        public bool Success { get; set; }
    }
}
