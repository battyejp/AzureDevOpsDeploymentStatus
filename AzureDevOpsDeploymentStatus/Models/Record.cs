using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureDevOpsDeploymentStatus.Models
{
    public class Record
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string Result { get; set; }
    }
}
