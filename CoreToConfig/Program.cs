using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreToConfig
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            GetConfiguration();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        public static IConfiguration GetConfiguration(string label = null)
        {
            TokenCredential credential = new DefaultAzureCredential();

            return new ConfigurationBuilder()
                .AddAzureAppConfiguration(options =>
                {
                    options = options.Connect(
                            new Uri(Environment.GetEnvironmentVariable("APP_CONFIGURATION_ENDPOINT")), credential)
                        .ConfigureKeyVault(kv => { kv.SetCredential(credential); });
                    if (!string.IsNullOrEmpty(label))
                    {
                        options.Select(KeyFilter.Any, label);
                    }
                    else
                    {
                        options.Select(KeyFilter.Any, LabelFilter.Null);
                    }
                })
                .Build();
        }
    }
}
