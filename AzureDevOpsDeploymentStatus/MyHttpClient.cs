using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;

namespace AzureDevOpsDeploymentStatus
{
    public interface IMyHttpClient
    {
        string Endpoint { get; set; }
        HttpClient HttpClient { get; }

        void AddQueryParameter(string header, string value);
    }

    public class MyHttpClient : IMyHttpClient
    {
        public HttpClient HttpClient { get; private set; }

        public MyHttpClient(HttpClient client, IConfiguration config)
        {
            HttpClient = client; //new HttpClient();
            HttpClient.BaseAddress = new Uri("https://dev.azure.com");
            var byteArray = Encoding.ASCII.GetBytes($"username:{config["AzDOPAT"]}");
            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue
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
