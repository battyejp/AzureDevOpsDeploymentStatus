using System.Text.Json.Serialization;

namespace AzureDevOpsDeploymentStatus.Models
{
    public class Records
    {
        [JsonPropertyName("Records")]
        public Record[] RecordList { get; set; }
    }
}
