using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;

namespace AzureDevOpsDeploymentStatus
{
    public class MyHttpClient : HttpClient
    {
        public MyHttpClient(IConfiguration config)
        {
            BaseAddress = new Uri("https://dev.azure.com");
            var byteArray = Encoding.ASCII.GetBytes($"username:{config["AzDOPAT"]}");
            DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue
                ("Basic", Convert.ToBase64String(byteArray));
        }

        public string Endpoint { get; set; }

        public void AddQueryParameter(string header, string value)
        {
            if (string.IsNullOrWhiteSpace(Endpoint)) throw new NullReferenceException("Endpoint");

            if (!Endpoint.Contains('?'))
                Endpoint += "?";
            else
                Endpoint += "&";

            Endpoint += $"{header}={value}";
        }
    }
}
