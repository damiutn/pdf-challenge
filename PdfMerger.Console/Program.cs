using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PdfMerger.Application;
using PdfMerger.Domain;
using PdfMerger.Infrastructure;
using PdfMerger.Shared;
using Serilog;

namespace PdfMerger
{
    public class Program
    {
        private static IConfigurationRoot _configuration;
        public static async Task Main(string[] args)
        {
            // Create service collection
            Log.Information("Creating service collection");
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            Log.Information("Building service provider");
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            // Initialize serilog logger
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .CreateLogger();


            try
            {
                Log.Information("Starting service");
                await serviceProvider.GetService<IApp>().RunAsync(args);
                Log.Information("Ending service");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error running service");
                throw;
            }
            
        }
        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add logging
            serviceCollection.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(dispose: true);
            }));

            serviceCollection.AddLogging();

            // Build configuration
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            #region Inversion Of Control
            serviceCollection.AddSingleton(_configuration);
            serviceCollection.AddTransient<IApp,App>();
            serviceCollection.AddTransient<IPdfMergeService, PdfMergeService>();
            serviceCollection.AddTransient<IPdf,Pdf>();
            serviceCollection.AddTransient<IExternalContentRepository,ExternalContentRepository>();
            serviceCollection.AddTransient<IContentExtractor,ContentExtractor>();
            serviceCollection.Configure<PdfMergerOptions>(_configuration.GetSection("PdfMergerConfig"));

            #endregion


            #region Http client
            //I Configured  httpClient to get pdf content
            //Notice that  I'm using polly for resilience

            serviceCollection.AddHttpClient("pdfClient", (_, c) =>
                {
                })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var handler = new HttpClientHandler
                    {
                        AllowAutoRedirect = false
                    };
                    handler.ServerCertificateCustomValidationCallback += (_, _, _, _) => true;
                    return handler;
                }).AddPolicyHandler(PollyPolicyBuilder.BuildRetryPolicy());


            #endregion
        }

    }
}
