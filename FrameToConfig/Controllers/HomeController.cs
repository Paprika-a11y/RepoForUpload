using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;
using System.Web.Mvc;

namespace FrameToConfig.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string test()
        {
            var a= string.Empty;

            //invoke func
            IConfiguration config= GetConfiguration();

            if (config != null) {
                a = config.GetConnectionString("test");
            }
            return a;
        }

        private IConfiguration GetConfiguration(string label = null)
        {
            TokenCredential credential = new DefaultAzureCredential();

            return new ConfigurationBuilder()
                .AddAzureAppConfiguration(options =>
                {
                    options = options.Connect(
                            new Uri("https://dorisconfig.azconfig.io"), credential)
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