using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PdfMerger.Infrastructure;

namespace PdfMerger.Domain
{
    public class ContentExtractor
    {
        private readonly ExternalContentRepository _externalContentRepository = new();
        public List<byte[]> GetContents(string[] urls)
        {
            List<byte[]> contents = new List<byte[]>();
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
