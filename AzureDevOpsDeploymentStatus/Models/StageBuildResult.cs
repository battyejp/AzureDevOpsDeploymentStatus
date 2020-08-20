using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureDevOpsDeploymentStatus.Models
{
    public class StageBuildResult
    {
        public Build Build { get; set; }

        public bool Success { get; set; }
    }
}
