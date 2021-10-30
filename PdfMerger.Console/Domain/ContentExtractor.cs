using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PdfMerger.Infrastructure;

namespace PdfMerger.Domain
{
    public interface IContentExtractor
    {
        List<byte[]> GetContents(string[] urls);
    }

    public class ContentExtractor : IContentExtractor
    {
        private readonly IExternalContentRepository _externalContentRepository;

        public ContentExtractor(IExternalContentRepository externalContentRepository)
        {
            _externalContentRepository = externalContentRepository;
        }
        public List<byte[]> GetContents(string[] urls)
        {
            List<Task<byte[]>> tasks = new List<Task<byte[]>>();
            //Paralelize processing
            Parallel.ForEach(urls, arg =>
            {
                Console.WriteLine($"Processing {Thread.CurrentThread.ManagedThreadId}");

                tasks.Add(_externalContentRepository.GetBinaryContentFromUlrAsync(arg));
            });

            Task.WaitAll(tasks.ToArray());

            return tasks.Select(f => f.Result).ToList();

        }
    }
}
