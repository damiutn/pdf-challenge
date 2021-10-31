using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PdfMerger.Infrastructure;

namespace PdfMerger.Domain
{
    public interface IContentExtractor
    {
        List<byte[]> GetContents(string[] urls);
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
        public List<byte[]> GetContents(string[] urls)
        {
            _logger.LogDebug($"url length {urls.Length}");
            Task<byte[]>[] tasks = new Task<byte[]>[urls.Length];
            
            //Paralelize processing
            //Implementing "for" instead of "foreach" to preserve the order of the items
            Parallel.For(0, urls.Length, index =>
            {
                _logger.LogDebug($"Processing with ThreadId {Thread.CurrentThread.ManagedThreadId}");

                tasks[index]=(_externalContentRepository.GetBinaryContentFromUlrAsync(urls[index]));
            });
            
            Task.WaitAll(tasks.ToArray());//Wait to finish all processes

            var bytesList = tasks.Select(f => f.Result).ToList();
            return bytesList;

        }
    }
}
