using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Dal;
using CarRental.Web.Hosting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Logging;

namespace CarRental.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            (await CreateWebHostBuilder(args)
                .Build()
                .MigrateDatabase<CarRentalDbContext>())
                .Run();
            //CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging((logging) =>
            {
                logging.AddDebug();
                logging.AddConsole();
            })
            .ConfigureAppConfiguration((context, config) => 
            {
                if (context.HostingEnvironment.IsProduction())
                {
                    var builtconfig = config.Build();

                    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    var keyVaultClient = new KeyVaultClient(
                        new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider
                            .KeyVaultTokenCallback));

                    config.AddAzureKeyVault($"https://{builtconfig["KeyVaultName"]}.vault.azure.net/", keyVaultClient, new DefaultKeyVaultSecretManager());
                }
            })
            .UseStartup<Startup>();
    }
}
