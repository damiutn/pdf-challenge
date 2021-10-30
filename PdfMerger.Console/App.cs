using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PdfMerger.Application;

namespace PdfMerger
{
    public interface IApp
    {
        Task RunAsync(string[] args);
    }

    public class App : IApp
    {
        private readonly ILogger<App> _logger;
        private readonly IPdfMergeService _pdfMergeService;

        public App(ILogger<App> logger, IPdfMergeService pdfMergeService)
        {
            //_logger = loggerFactory.CreateLogger<App>();
            _logger = logger;
            _pdfMergeService = pdfMergeService;
        }
        public async Task RunAsync(string[] args)
        {
            if (args == null || args.Length < 2)
                throw new ArgumentException("At least 2 parameters are expected");

            _logger.LogInformation("Starting process");
            var pdfMerdedCreated =await _pdfMergeService.MergePdfsAsync(args);

            _logger.LogInformation($"PDF file created {pdfMerdedCreated}");
            _logger.LogInformation("Finishing process");
        }
    }
}