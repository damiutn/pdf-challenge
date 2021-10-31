using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PdfMerger.Infrastructure;

namespace PdfMerger.Domain
{
    public interface IContentExtractor
    {
        Task<string[]> GetAllPdfAsync(string[] urls, string pathToSaveTemporalPdf);
    }

    public class ContentExtractor : IContentExtractor
    {
        private readonly ILogger<ContentExtractor> _logger;
        private readonly IExternalContentRepository _externalContentRepository;

        public ContentExtractor(ILogger<ContentExtractor> logger, IExternalContentRepository externalContentRepository)
        {
            _logger = logger;
            _externalContentRepository = externalContentRepository;
        }
        public async Task<string[]> GetAllPdfAsync(string[] urls, string pathToSaveTemporalPdf)
        {
            _logger.LogDebug($"url length {urls.Length}");
            Task<byte[]>[] tasks = new Task<byte[]>[urls.Length];
            string[] contents = new string[urls.Length];
            
            //Paralelize processing
            //Implementing "for" instead of "foreach" to preserve the order of the items
            Parallel.For(0, urls.Length, index =>
            {
                _logger.LogDebug($"Processing with ThreadId {Thread.CurrentThread.ManagedThreadId}");

                var task = _externalContentRepository.GetBinaryContentFromUlrAsync(urls[index]);
                task.ContinueWith(f =>
                {
                    //After complete task it save pdf in a temp folder
                    string file = Path.Combine(pathToSaveTemporalPdf, $"pdfTemp{DateTime.Now:yyyyMMddHHmmss}_{Thread.CurrentThread.ManagedThreadId}.pdf");
                    _logger.LogDebug($"Writing content {file}");
                    File.WriteAllBytes(file, f.Result);
                    contents[index] = file;
                });
                tasks[index]=task;
            });
            
            await Task.WhenAll(tasks.ToArray());//Wait to finish all processes

            return contents;
        }
    }

    public class Content
    {
        public string File { get; set; }
    }
}
